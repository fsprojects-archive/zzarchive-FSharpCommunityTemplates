open System
open MultiProjectDialog

[<EntryPoint>]
[<STAThread>]
let main argv = 
    let projects = [ "Test1", "test1", "fsharp"; "Test2", "test2", "fsharp"; "Test3", "test3", "fsharp"; "Test4", "test4", "fsharp"
                     "Test5", "test5", "fsharp"; "Test6", "test6", "fsharp"; "Test7", "test7", "fsharp"; "Test8", "test8", "fsharp"]
    let dialog = new TemplateWizardDialog(projects |> List.toSeq)
    dialog.ShowDialog()
    0 // return an integer exit code
