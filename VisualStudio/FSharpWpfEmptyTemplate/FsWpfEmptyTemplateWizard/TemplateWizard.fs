namespace FsWpfEmptyTemplateWizard

open System
open System.IO
open System.Windows.Forms
open System.Collections.Generic
open EnvDTE
open EnvDTE80
open Microsoft.VisualStudio.Shell
open Microsoft.VisualStudio.TemplateWizard
open VSLangProj

type TemplateWizard() =
    [<DefaultValue>] val mutable solution : Solution2
    [<DefaultValue>] val mutable dte : DTE
    [<DefaultValue>] val mutable dte2 : DTE2
    [<DefaultValue>] val mutable serviceProvider : IServiceProvider
    [<DefaultValue>] val mutable destinationPath : string
    [<DefaultValue>] val mutable safeProjectName : string
    [<DefaultValue>] val mutable targetFramework : float
    interface IWizard with
        member this.RunStarted (automationObject:Object, 
                                replacementsDictionary:Dictionary<string,string>, 
                                runKind:WizardRunKind, customParams:Object[]) =
            this.dte <- automationObject :?> DTE
            this.dte2 <- automationObject :?> DTE2
            this.solution <- this.dte2.Solution :?> Solution2
            this.serviceProvider <- new ServiceProvider(automationObject :?> 
                                     Microsoft.VisualStudio.OLE.Interop.IServiceProvider)
            this.destinationPath <- replacementsDictionary.["$destinationdirectory$"]
            this.safeProjectName <- replacementsDictionary.["$safeprojectname$"]
            this.targetFramework <- Double.Parse replacementsDictionary.["$targetframeworkversion$"]
        member this.ProjectFinishedGenerating project = "Not Implemented" |> ignore
        member this.ProjectItemFinishedGenerating projectItem = "Not Implemented" |> ignore
        member this.ShouldAddProjectItem filePath = true
        member this.BeforeOpeningFile projectItem = "Not Implemented" |> ignore
        member this.RunFinished() = 
            let currentCursor = Cursor.Current
            Cursor.Current <- Cursors.WaitCursor
            try
                let templateName = "FSharpWpfEmpty"
                let templatePath = this.solution.GetProjectTemplate(templateName + ".zip", "FSharp")
                try
                    let AddProject status projectVsTemplateName projectName =
                        this.dte2.StatusBar.Text <- status
                        let path = templatePath.Replace(templateName + ".vstemplate", projectVsTemplateName)
                        this.solution.AddFromTemplate(path, Path.Combine(this.destinationPath, projectName), 
                            projectName, false) |> ignore
                
                    match (this.targetFramework > 4.), templatePath.ToLower().Contains(@"\visualstudio\11") with
                    | true, _ | _, true -> 
                        AddProject "Adding the F# project..." 
                            (Path.Combine("App", "App.vstemplate")) this.safeProjectName
                        let projects = BuildProjectMap (this.dte.Solution.Projects)
                        try
                            this.dte2.StatusBar.Text <- "Adding NuGet packages..."
                            (projects.TryFind this.safeProjectName).Value |> InstallPackages this.serviceProvider (templatePath.Replace(templateName + ".vstemplate", ""))
                            <| [("FSharpx.TypeProviders.Xaml", "1.8.41")]
                        with
                        | ex -> let msg = ex
                                ()// do nothing...
                    | _ -> 
                        AddProject "Adding the F# project..." 
                            (Path.Combine("App2", "App.vstemplate")) this.safeProjectName
                        
                with
                | ex -> failwith (sprintf "%s\n\r%s\n\r%s" 
                            "The project creation has failed."
                            "The actual exception message is: "
                            ex.Message)
            finally
                Cursor.Current <- currentCursor
            
