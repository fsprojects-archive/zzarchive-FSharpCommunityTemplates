namespace FsMvc5TemplateWizard

open System
open System.IO
open System.Windows.Forms
open System.Collections.Generic
open EnvDTE
open EnvDTE80
open Microsoft.VisualStudio.TemplateWizard
open FsMvc5Dialog

type TemplateWizard() =
    [<DefaultValue>] val mutable solution : Solution2
    [<DefaultValue>] val mutable dte : DTE
    [<DefaultValue>] val mutable dte2 : DTE2
    [<DefaultValue>] val mutable destinationPath : string
    [<DefaultValue>] val mutable vsixInstallPath : string
    [<DefaultValue>] val mutable selectedProjectName : string
    [<DefaultValue>] val mutable safeProjectName : string

    let projectNames = [| "FsMvc5"; "WebApi" |]

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
            try
                match dialog.ShowDialog().Value with
                | true -> 
                    this.selectedProjectName <- projectNames.[dialog.SelectedProjectTypeIndex]
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
