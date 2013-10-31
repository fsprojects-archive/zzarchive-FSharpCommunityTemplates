namespace FSSLOnlyTemplateWizard

open System
open System.IO
open System.Collections.Generic
open EnvDTE
open Microsoft.VisualStudio.TemplateWizard
open FSSLOnlyDialog

type TemplateWizard() =
    interface IWizard with
        member x.RunStarted (automationObject:Object, replacementsDictionary:Dictionary<string,string>, 
                             runKind:WizardRunKind, customParams:Object[]) =
            let x86Path = 
                match Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86) with
                | progFiles when String.IsNullOrEmpty progFiles -> 
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)
                | _ -> 
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)
            let sl4Exists = Directory.Exists(Path.Combine(x86Path, @"Microsoft F#\Silverlight\Libraries\Client\v4.0"))
            let sl5Exists = Directory.Exists(Path.Combine(x86Path, @"Microsoft F#\Silverlight\Libraries\Client\v5.0"))
            match sl4Exists, sl5Exists with
            | true, true ->
                let dialog = new TemplateWizardDialog()
                match dialog.ShowDialog().Value with
                | true -> 
                    replacementsDictionary.["$targetframeworkversion$"] <- dialog.SelectedSilverlightVersion
                | _ ->
                    raise (new WizardCancelledException())
            | false, false ->
                raise(new ApplicationException("Please install version 4 and/or 5 of the F# Silverlight Developer Tools - see http://blogs.msdn.com/b/fsharpteam/archive/2011/04/22/update-to-the-f-2-0-free-tools-release-corresponding-to-visual-studio-2010-sp1-april-2011-ctp.aspx."))
            | true, false -> replacementsDictionary.["$targetframeworkversion$"] <- "4.0"
            | false, true -> replacementsDictionary.["$targetframeworkversion$"] <- "5.0"
        member x.ProjectFinishedGenerating (project:Project) = "Do Nothing" |> ignore
        member x.ProjectItemFinishedGenerating projectItem = "Do Nothing" |> ignore
        member x.ShouldAddProjectItem filePath = true
        member x.BeforeOpeningFile projectItem = "Do Nothing" |> ignore
        member x.RunFinished() = "Do Nothing" |> ignore
