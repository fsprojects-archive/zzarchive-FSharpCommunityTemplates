open System
open MultiProjectDialog

[<EntryPoint>]
[<STAThread>]
let main argv = 
    let projects = [ "Test1", "MVC 5 and Web API", "fsharp"; "Test2", "Web API", "FSharp"; "Test3", "My Template", "Generic"]
    let dialog = new TemplateWizardDialog(projects |> List.toSeq)
    dialog.ShowDialog()
    0 // return an integer exit code
