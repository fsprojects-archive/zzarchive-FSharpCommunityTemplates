
"*********** Adding template-builder" | Write-Host

# TODO: These should be passed in, not declared here
$importLabel = "TemplateBuilder"
$targetsPropertyName = "TemplateBuilderTargets"
$targetsFileToAddImport = "ligershark.templates.targets";

# When this package is installed we need to add a property
# to the current project, which points to the
# .targets file in the packages folder

function RemoveExistingKnownPropertyGroups($projectRootElement){
    # if there are any PropertyGroups with a label of "$importLabel" they will be removed here
    $pgsToRemove = @()
    foreach($pg in $projectRootElement.PropertyGroups){
        if($pg.Label -and [string]::Compare($importLabel,$pg.Label,$true) -eq 0) {
            # remove this property group
            $pgsToRemove += $pg
        }
    }

    foreach($pg in $pgsToRemove){
        $pg.Parent.RemoveChild($pg)
    }
}

# TODO: Revisit this later, it was causing some exceptions
function CheckoutProjFileIfUnderScc(){
    param(
        $project = (Get-Project)
    ) 
    CheckoutIfUnderScc -filePath $project.FullName
}

function CheckoutIfUnderScc(){
    param(
        [Parameter(Mandatory=$true)]
        [string]
        $filePath,

        $project = (Get-Project)
    )
    "`tChecking if file is under source control, [{0}]" -f $filePath| Write-Verbose
    # http://daltskin.blogspot.com/2012/05/nuget-powershell-and-tfs.html
    $sourceControl = Get-Interface $project.DTE.SourceControl ([EnvDTE80.SourceControl2])
    if($sourceControl.IsItemUnderSCC($filePath) -and $sourceControl.IsItemCheckedOut($filePath)){
        "`tChecking out file [{0}]" -f $filePath | Write-Host
        $sourceControl.CheckOutItem($filePath)
    }
}

function EnsureProjectFileIsWriteable(){
    param(
        $project = (Get-Project)
    )
    $projItem = Get-ChildItem $project.FullName
    if($projItem.IsReadOnly) {
        "The project file is read-only. Please checkout the project file and re-install this package" | Write-Host -ForegroundColor Red
        throw;
    }
}

function ComputeRelativePathToTargetsFile(){
    param($startPath,$targetPath)   

    # we need to compute the relative path
    $startLocation = Get-Location

    Set-Location $startPath.Directory | Out-Null
    $relativePath = Resolve-Path -Relative $targetPath.FullName

    # reset the location
    Set-Location $startLocation | Out-Null

    return $relativePath
}

function GetSolutionDirFromProj{
    param($msbuildProject)

    if(!$msbuildProject){
        throw "msbuildProject is null"
    }

    $result = $null
    $solutionElement = $null
    foreach($pg in $msbuildProject.PropertyGroups){
        foreach($prop in $pg.Properties){
            if([string]::Compare("SolutionDir",$prop.Name,$true) -eq 0){
                $solutionElement = $prop
                break
            }
        }
    }

    if($solutionElement){
        $result = $solutionElement.Value
    }

    return $result
}

function AddImportElementIfNotExists(){
    param($projectRootElement)

    $foundImport = $false
    $importsToRemove = @()
    foreach($import in $projectRootElement.Imports){
        $importStr = $import.Project
        if(!$importStr){
            $importStr = ""
        }

        $currentLabel = $import.Label
        if(!$currentLabel){
            $currentLabel = ""
        }

        if([string]::Compare($importLabel,$currentLabel.Trim(),$true) -eq 0){
            # found the import no need to continue
            $foundImport = $true
            break
        }
    }

    if(!$foundImport){
        # the import is not in the project, add it
        # <Import Project="$(VsixCompressImport)" Condition="Exists('$(VsizCompressTargets)')" Label="VsixCompress" />
        $importToAdd = $projectRootElement.AddImport("`$($targetsPropertyName)");
        $importToAdd.Condition = "Exists('`$($targetsPropertyName)')"
        $importToAdd.Label = $importLabel 
    }        
}

function UpdateVsixManifest(){
    param(
        $project = (Get-Project)
    )
    # we will look for any file in the project which ends with .vsixmanifest and add
    # <Assets>
    #   <Asset Type="Microsoft.VisualStudio.ItemTemplate" Path="Output\ItemTemplates"/>
    # </Assets>

    $vsixManifestFiles = @()
    # search for any file in the project which ends with .vsixmanifest
    foreach ($projItem in $project.ProjectItems){ 
        if( ($projItem -and $projItem.Name -and $projItem.Name.EndsWith('.vsixmanifest'))) {
            "`tFound manifest [{0}], getting fullpath" -f $projItem.Name | Write-Verbose
            $vsixManifestFiles += $projItem.Properties.Item("FullPath").Value
        }
    }

    foreach($vsixManifestFile in $vsixManifestFiles){
        AddItemTemplateAssetTagToVisxManfiestIfNotExists -vsixFilePathToUpdate $vsixManifestFile
        AddProjectTemplateAssetTagToVisxManfiestIfNotExists -vsixFilePathToUpdate $vsixManifestFile
    }
}

function AddItemTemplateAssetTagToVisxManfiestIfNotExists(){
    param(
        [Parameter(Mandatory=$true)]
        $vsixFilePathToUpdate
    )
    
    if(!(Test-Path $vsixFilePathToUpdate)){
        ".vsixmanifest file not found at [{0}]" -f $vsixFilePathToUpdate | Write-Error
        return;
    }
    
    [xml]$vsixXml = (Get-Content $vsixFilePathToUpdate)
    if( ($vsixXml.PackageManifest.Assets.Asset | Where-Object {$_.Path -eq 'Output\ItemTemplates'}) ){
        # if the asset is already there just skip it
        "`t.vsixmanifest not modified because the 'Output\ItemTemplates' element is already in that file" | Write-Host
    }
    else{
        "`tAdding item template asset tag to .vsixmanifest file {0}" -f $vsixFilePathToUpdate | Write-Host
        CheckoutIfUnderScc -filePath $vsixFilePathToUpdate
        # create the element here
        $newElement = $vsixXml.CreateElement('Asset', $vsixXml.DocumentElement.NamespaceURI)
        $newElement.SetAttribute('Type', 'Microsoft.VisualStudio.ItemTemplate')
        $newElement.SetAttribute('Path', 'Output\ItemTemplates')
        $vsixXml.PackageManifest.Assets.AppendChild($newElement)
        $vsixXml.Save($vsixFilePathToUpdate)
    }
}
function AddProjectTemplateAssetTagToVisxManfiestIfNotExists(){
    param(
        [Parameter(Mandatory=$true)]
        $vsixFilePathToUpdate
    )
    
    if(!(Test-Path $vsixFilePathToUpdate)){
        ".vsixmanifest file not found at [{0}]" -f $vsixFilePathToUpdate | Write-Error
        return;
    }
    
    [xml]$vsixXml = (Get-Content $vsixFilePathToUpdate)
    if( ($vsixXml.PackageManifest.Assets.Asset | Where-Object {$_.Path -eq 'Output\ProjectTemplates'}) ){
        # if the asset is already there just skip it
        "`t.vsixmanifest not modified because the 'Output\ProjectTemplates' element is already in that file" | Write-Host
    }
    else{
        "`tAdding project template asset tag to .vsixmanifest file {0}" -f $vsixFilePathToUpdate | Write-Host
        CheckoutIfUnderScc -filePath $vsixFilePathToUpdate
        # create the element here
        $newElement = $vsixXml.CreateElement('Asset', $vsixXml.DocumentElement.NamespaceURI)
        $newElement.SetAttribute('Type', 'Microsoft.VisualStudio.ProjectTemplate')
        $newElement.SetAttribute('Path', 'Output\ProjectTemplates')
        $vsixXml.PackageManifest.Assets.AppendChild($newElement)
        $vsixXml.Save($vsixFilePathToUpdate)
    }
}

function UpdateProjectTemplateFilesToNone(){
    param(
        $projet = (Get-Project)
    )

    $projTemplatesItem = ($project.ProjectItems | Where-Object { $_.Name.Contains("ProjectTemplates") })
    if(!($projTemplatesItem)){
        return
    }

    # we need to loop through each sub item and mark all files as None
    MarkAllChildrenAsNone -$projTemplatesItem
}

function MarkAllChildrenAsNone(){
    param(
    [Parameter(Mandatory=$true)]
    $projectItem
    )

    foreach($pItem in $projectItem.ProjectItems){
        # mark the item as None and recurse
        MarkItemAsNone -projectItem $pItem

        if($pItem.ProjectItems) {
            MarkAllChildrenAsNone $pItem.Collection
        }
    }
}

function MarkItemAsNone(){
    param(
    [Parameter(Mandatory=$true)]
    $projectItem
    )

    # we need to see if the $projectItem has a BuildAction property and if so set it
    
    # we will supress the error message as it's by design, but we need to restore the $ErrorActionPreference value
    $prevErrorAction = $ErrorActionPreference 
    $ErrorActionPreference= 'silentlycontinue'

    $buildAction = ($projectItem.Properties.Item("BuildAction"))
    $ErrorActionPreference= $prevErrorAction
    if($buildAction){
        # if the property is not null then it exists on the projectitem, update it to None
        # http://stackoverflow.com/questions/7423564/specify-build-action-of-content-nuget
        $projectItem.Properties.Item("BuildAction").Value = [int]0
    }
}


Export-ModuleMember -function UpdateProjectTemplateFilesToNone

# just for debugging, should be removed
Export-ModuleMember -function *



