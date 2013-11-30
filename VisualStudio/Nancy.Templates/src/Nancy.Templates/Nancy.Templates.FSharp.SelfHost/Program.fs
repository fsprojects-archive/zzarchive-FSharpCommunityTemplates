module Nancy.Templates.FSharp.SelfHost.Program

open System
open Nancy.Hosting.Self

[<EntryPoint>]
let main args =
    let uri = Uri("http://localhost:3579")

    use host = new NancyHost(uri)
    host.Start()

    Console.WriteLine("Your application is running on " + uri.AbsoluteUri)
    Console.WriteLine("Press any [Enter] to close the host.")
    Console.ReadLine() |> ignore
    0
