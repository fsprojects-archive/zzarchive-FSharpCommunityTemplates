namespace FSharpWcfTemplateWizard

open System
open System.Collections.Generic
open System.Collections
open EnvDTE
open Microsoft.VisualStudio.TemplateWizard
open VSLangProj

[<AutoOpen>]
module TemplateWizardMod =
    let AddProjectReference (target:Option<Project>) (projectToReference:Option<Project>) =
        if ((Option.isSome target) && (Option.isSome projectToReference)) then
            let vsControllerProject = target.Value.Object :?> VSProject
            let enumerator = vsControllerProject.References.GetEnumerator() 
            enumerator.Reset()
            let rec buildProjectReferences() = 
                match enumerator.MoveNext() with
                | true -> 
                    let reference = enumerator.Current :?> Reference
                    if reference.Name = projectToReference.Value.Name then reference.Remove()
                    vsControllerProject.References.AddProject(projectToReference.Value) |> ignore
                    buildProjectReferences()
                | _ -> "End it" |> ignore
            buildProjectReferences()

    let BuildProjectMap (projectEnumerator:IEnumerator) =
        let rec buildProjects (projectMap:Map<string,Project>) = 
            match projectEnumerator.MoveNext() with
            | true -> let project = projectEnumerator.Current :?> Project
                      projectMap 
                      |> Map.add project.Name project
                      |> buildProjects 
            | _ -> projectMap
        buildProjects Map.empty

type TemplateWizard() =
    let projectRefs = [("Services", "Contracts"); ("Web", "Contracts"); ("Web", "Services")]
    [<DefaultValue>] val mutable Dte : DTE
    interface IWizard with
        member x.RunStarted (automationObject:Object, replacementsDictionary:Dictionary<string,string>, 
                             runKind:WizardRunKind, customParams:Object[]) =
            x.Dte <- automationObject :?> DTE
        member x.ProjectFinishedGenerating (project:Project) =
            try
                let projects = BuildProjectMap (x.Dte.Solution.Projects.GetEnumerator())
                projectRefs 
                |> Seq.iter (fun (target,source) -> 
                             do AddProjectReference (projects.TryFind target) (projects.TryFind source))
            with 
            | _ -> "Do Nothing" |> ignore
        member x.ProjectItemFinishedGenerating projectItem = "Do Nothing" |> ignore
        member x.ShouldAddProjectItem filePath = true
        member x.BeforeOpeningFile projectItem = "Do Nothing" |> ignore
        member x.RunFinished() = "Do Nothing" |> ignore
