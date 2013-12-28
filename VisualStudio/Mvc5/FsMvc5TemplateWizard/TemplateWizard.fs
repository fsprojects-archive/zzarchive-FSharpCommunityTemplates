namespace FsMvc5TemplateWizard

open System
open System.IO
open System.Windows.Forms
open System.Collections.Generic
open EnvDTE
open EnvDTE80
open Microsoft.VisualStudio.Shell
open Microsoft.VisualStudio.TemplateWizard
open VSLangProj
open FsMvc5Dialog

type TemplateWizard() =
    [<DefaultValue>] val mutable solution : Solution2
    [<DefaultValue>] val mutable dte : DTE
    [<DefaultValue>] val mutable dte2 : DTE2
    [<DefaultValue>] val mutable destinationPath : string
    [<DefaultValue>] val mutable safeProjectName : string
    [<DefaultValue>] val mutable vsixInstallPath : string
    [<DefaultValue>] val mutable isWebApi : bool

    let mutable selectedWebProjectName = "Razor"
    interface IWizard with
        member this.RunStarted (automationObject:Object, 
                                replacementsDictionary:Dictionary<string,string>, 
                                runKind:WizardRunKind, customParams:Object[]) =
            this.vsixInstallPath <- customParams |> Seq.cast |> Seq.find(fun x -> x.Contains ".vstemplate")
            this.dte <- automationObject :?> DTE
            this.dte2 <- automationObject :?> DTE2
            this.solution <- this.dte2.Solution :?> EnvDTE80.Solution2
            this.destinationPath <- replacementsDictionary.["$destinationdirectory$"]
            this.safeProjectName <- replacementsDictionary.["$safeprojectname$"]

            let dialog = new TemplateWizardDialog()
            match dialog.ShowDialog().Value with
            | true -> 
                match dialog.SelectedProjectTypeIndex with
                | 1 -> this.isWebApi <- true
                | _ -> () 
            | _ ->
                raise (new WizardCancelledException())
        member this.ProjectFinishedGenerating project = "Not Implemented" |> ignore
        member this.ProjectItemFinishedGenerating projectItem = "Not Implemented" |> ignore
        member this.ShouldAddProjectItem filePath = true
        member this.BeforeOpeningFile projectItem = "Not Implemented" |> ignore
        member this.RunFinished() = 
            let currentCursor = Cursor.Current
            Cursor.Current <- Cursors.WaitCursor
            try
                let mutable selectedWebAppProjectName = "WebApp"

                let templatePath = this.vsixInstallPath 
                try
                    let AddProject status projectVsTemplateName projectName =
                        this.dte2.StatusBar.Text <- status
                        let path = templatePath.Replace("FsMvc5.vstemplate", projectVsTemplateName)
                        this.dte2.Solution.AddFromTemplate(path, Path.Combine(this.destinationPath, projectName), 
                            projectName, false) |> ignore
                    AddProject "Adding the F# Web project..." 
                        (Path.Combine(selectedWebProjectName, selectedWebProjectName + ".vstemplate")) webName
                    AddProject "Adding the F# Web project..." 
                        (Path.Combine(selectedWebAppProjectName, selectedWebAppProjectName + ".vstemplate")) webAppName
                with
                | ex -> failwith (sprintf "%s\n\r%s" 
                            "The project creation has failed. The actual exception message is: "
                            ex.Message)
            finally
                Cursor.Current <- currentCursor
            
