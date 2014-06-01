// Learn more about F# at http://fsharp.net
// See the 'F# Tutorial' project for more help.


open ServiceStack.Common
open ServiceStack.WebHost.Endpoints
open ServiceStack.Logging
open ServiceStack.Logging.Support.Logging
open HelloService
open System

type AppHost() = 
    inherit AppHostHttpListenerBase("Hello F# Service", 
                                    typeof<HelloService>.Assembly)
    override this.Configure container = 
        ignore()

[<EntryPoint>]
let main argv = 
    LogManager.LogFactory <- new ConsoleLogFactory()
    printfn "%A" argv
    let host = "http://localhost:8080/"
    printfn "listening on %s ..." host
    let appHost = new AppHost()
    appHost.Init()
    appHost.Start host
    while true do
        Console.ReadLine() |> ignore
    0 // return an integer exit code
