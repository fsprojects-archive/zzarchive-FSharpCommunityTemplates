namespace MultiProjectTemplateWizard

open System
open System.IO
open System.Xml
open System.Windows.Forms
open System.Collections.Generic
open EnvDTE
open EnvDTE80
open Microsoft.VisualStudio.TemplateWizard
open MultiProjectDialog

type TemplateWizard() =
    [<DefaultValue>] val mutable dte2 : DTE2
    [<DefaultValue>] val mutable destinationPath : string
    [<DefaultValue>] val mutable vsixInstallPath : string
    [<DefaultValue>] val mutable selectedProjectName : string
    [<DefaultValue>] val mutable safeProjectName : string

    let addProjects (templatePath:string) (dte2:DTE2) destinationPath selectedProjectName safeProjectName =
        let currentCursor = Cursor.Current
        Cursor.Current <- Cursors.WaitCursor
        try
            try
                let AddProject status projectVsTemplateName projectName =
                    dte2.StatusBar.Text <- status
                    let path = templatePath.Replace("Web.fsharp.vstemplate", projectVsTemplateName)
                    dte2.Solution.AddFromTemplate(path, destinationPath, projectName, false) |> ignore
                AddProject "Adding the F# project..." 
                    (Path.Combine(selectedProjectName, "MyTemplate.vstemplate")) safeProjectName
            with
            | ex -> 
                failwith (sprintf "%s\n\r%s" "The project creation has failed. The actual exception message is: " ex.Message)
        finally
            Cursor.Current <- currentCursor

    let getProjectInfo (vsTemplateFilePath:string) =
        try
            let xmlDoc = XmlDocument() in xmlDoc.Load vsTemplateFilePath
            let nodes = xmlDoc.DocumentElement.GetElementsByTagName("ProjectInfo")
            nodes
            |> Seq.cast<XmlNode>
            |> Seq.map(fun node -> node.Attributes.["folderName"].Value, node.Attributes.["displayText"].Value, node.Attributes.["icon"].Value)
            |> Seq.toArray
        with
        | ex -> failwith (sprintf "%s\n\r%s" "The project creation has failed while reading the Template file settings. The actual exception message is: " ex.Message)

    interface IWizard with
        member this.RunStarted (automationObject:Object, 
                                replacementsDictionary:Dictionary<string,string>, 
                                runKind:WizardRunKind, customParams:Object[]) =
            try
                this.vsixInstallPath <- customParams |> Seq.cast |> Seq.find(fun x -> x.Contains ".vstemplate")

                let projects = getProjectInfo(this.vsixInstallPath)

                if (projects.Length = 0) then 
                    raise (new WizardBackoutException("No project information has been provided. Please add Projects/ProjectInfo elements to the WizardData element in the vstemplate file."))
                
                this.dte2 <- automationObject :?> DTE2
                this.destinationPath <- replacementsDictionary.["$destinationdirectory$"]
                this.safeProjectName <- replacementsDictionary.["$safeprojectname$"]

                let dialog = new TemplateWizardDialog(projects |> Array.toSeq)
                match dialog.ShowDialog().Value with
                | true -> 
                    let folderName, _, _ = projects.[dialog.SelectedProjectTypeIndex]
                    this.selectedProjectName <- folderName
                | _ ->
                    raise (new WizardCancelledException())
            with
            | ex -> failwith (sprintf "%s\n\r%s" "The project creation has failed. The actual exception message is: " ex.Message)
        member this.ProjectFinishedGenerating project = "Not Implemented" |> ignore
        member this.ProjectItemFinishedGenerating projectItem = "Not Implemented" |> ignore
        member this.ShouldAddProjectItem filePath = true
        member this.BeforeOpeningFile projectItem = "Not Implemented" |> ignore
        member this.RunFinished() = 
            addProjects this.vsixInstallPath this.dte2 this.destinationPath this.selectedProjectName this.safeProjectName
