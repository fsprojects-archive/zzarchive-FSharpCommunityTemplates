namespace $safeprojectname$.StartupTasks

open System
open System.Diagnostics
open System.Threading
open System.Threading.Tasks

open Simple.Web
open Simple.Web.StructureMap

open $safeprojectname$.Communication
open $safeprojectname$.Communication.Commands

type DomainStartup() =
    inherit StructureMapStartupBase()

    let logMessage =
        DateTime.Now
        |> sprintf "%A\n%s\n\n" >> Trace.WriteLine
    
    let logError e =
        e.ToString() |> logMessage

    let eventBus =
        [ (* MyModule.createEventHandler1 *) ]  // Register your event handlers here
        |> List.map (fun handler -> handler logError)
        |> EventBusStartup.createEventStore

    let commandRouter =
        [ (* MyModule.createCommandHandler1 *) ]  // ..and your command handlers here
        |> List.map (fun handler -> handler eventBus)   // Apply the event bus
        |> List.map (fun handler -> handler logError)   // Apply the onError function
        |> CommandBus.createCommandRouter

    override __.Configure(config) =
        config.For(typedefof<IRouter<Command>>).Singleton().Use(commandRouter) |> ignore