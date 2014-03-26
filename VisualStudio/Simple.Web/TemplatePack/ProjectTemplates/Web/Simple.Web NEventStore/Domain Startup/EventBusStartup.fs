namespace $safeprojectname$.StartupTasks

module EventBusStartup =
    
    open NEventStore
    open NEventStore.Dispatcher
    open NEventStore.Persistence.InMemoryPersistence
    open NEventStore.Persistence.SqlPersistence.SqlDialects

    [<RequireQualifiedAccess>]
    module Serializer =
        open Nessos.FsPickler
        open NEventStore.Serialization

        let createSerializer() =
            let pickler = FsPickler()

            { new ISerialize with
                member __.Serialize<'T> (output, graph: 'T) =
                    pickler.Serialize(output, graph) 
                member __.Deserialize<'T> input =
                    pickler.Deserialize<'T>(input) }

    let createDispatcher handlers =
        { new IDispatchCommits with
            member __.Dispatch commit =
                handlers |> List.iter (fun h -> h commit.StreamId commit.Events)
            member __.Dispose() = () }

    let createEventStore handlers =
        let dispatcher = createDispatcher handlers

        Wireup.Init()
            .LogToConsoleWindow()
            .UsingInMemoryPersistence()
// -------------------------------------------------------------------------------------------------
// To use SQL persistence, create a database and setup the "default" connection string in Web.Config
// and uncomment the next three lines.
//            .UsingSqlPersistence("default")
//                .WithDialect(MsSqlDialect())
//                .InitializeStorageEngine()
// -------------------------------------------------------------------------------------------------
            .UsingCustomSerialization(Serializer.createSerializer())
                .Compress()
            .UsingAsynchronousDispatchScheduler()
                .DispatchTo(dispatcher)
            .Build()