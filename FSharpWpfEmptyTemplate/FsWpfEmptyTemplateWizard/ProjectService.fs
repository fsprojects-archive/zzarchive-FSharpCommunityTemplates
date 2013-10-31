[<AutoOpen>]
module ProjectService
    open EnvDTE
    open VSLangProj

    let BuildProjectMap (projects:Projects) =
        projects 
        |> Seq.cast<Project> 
        |> Seq.map(fun project -> project.Name, project)
        |> Map.ofSeq


