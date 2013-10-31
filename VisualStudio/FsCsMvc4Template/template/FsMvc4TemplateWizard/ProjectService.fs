[<AutoOpen>]
module ProjectService
    open EnvDTE
    open VSLangProj

    let AddProjectReference (target:Option<Project>) (projToReference:Option<Project>) =
        if ((Option.isSome target) && (Option.isSome projToReference)) then
            let vsTarget = target.Value.Object :?> VSProject
            vsTarget.References 
            |> Seq.cast<Reference> 
            |> Seq.filter(fun (reference) -> reference.Name = projToReference.Value.Name)
            |> Seq.iter(fun reference -> reference.Remove())
            vsTarget.References
                .AddProject((projToReference.Value.Object :?> VSProject).Project) 
                |> ignore

    let BuildProjectMap (projects:Projects) =
        projects 
        |> Seq.cast<Project> 
        |> Seq.map(fun project -> project.Name, project)
        |> Map.ofSeq

    let BuildProjectReferences (projects:Map<string, Project>) projectRefs =
        projectRefs 
        |> Seq.iter (fun (target,source) -> 
                        AddProjectReference (projects.TryFind target) 
                            (projects.TryFind source))

