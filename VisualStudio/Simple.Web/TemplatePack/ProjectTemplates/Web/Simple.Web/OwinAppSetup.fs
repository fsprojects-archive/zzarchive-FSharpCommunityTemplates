module OwinAppSetup

open System
open System.Collections.Generic
open System.Threading.Tasks
open Simple.Web

type UseAction = Action<Func<IDictionary<string, obj>, Func<IDictionary<string, obj>, Task>, Task>>

type OwinAppSetup() =
    static member Setup(useMethod:UseAction) =
        let run x y = Application.Run(x, y)
        run |> useMethod.Invoke

type StaticContentStartup() =
    interface IStartupTask with
        member __.Run(config, env) =
            let pf f = config.PublicFolders.Add(PublicFolder f) |> ignore
            ["/Scripts"; "/Content"; "/App"] |> List.iter pf

open Simple.Web.StructureMap
type StructureMapStartup() =
    inherit StructureMapStartupBase()
    override __.Configure(config) =
        // config.For<IMySerivce>().Use<MyService>() |> ignore
        ()