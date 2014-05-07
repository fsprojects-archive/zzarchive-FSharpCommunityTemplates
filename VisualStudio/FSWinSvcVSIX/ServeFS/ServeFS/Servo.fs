namespace Servo

open System
open System.ServiceProcess
open System.Diagnostics
open System.Collections
open System.ComponentModel
open System.Configuration.Install
open System.Reflection
open Microsoft.FSharp.Data.UnitSystems.SI.UnitSymbols

type Conf () =
    static member val ServiceName = "ServeFS" with get, set
    static member val DisplayName = "ServeFS Service" with get, set
    static member val StartType = ServiceStartMode.Automatic with get, set
    static member val Description = "ServeFS Service (Default Name)" with get, set
    static member val Timeout = 3 * 60<s> with get, set

type SvcController () =
    let _controller = new ServiceController(Conf.ServiceName)

    member x.Start () = 
        Trace.WriteLine("starting service...");

        try
            _controller.Start(Environment.GetCommandLineArgs())

            _controller.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(Conf.Timeout |> float))

            match _controller.Status with
            | ServiceControllerStatus.Running -> Trace.WriteLine("started")
            | _ -> Trace.WriteLine(String.Format("service not started with status: {0}", _controller.Status))
        with xe -> Trace.WriteLine(String.Format("could not start service. {0}", xe))

    member x.Stop () =
        Trace.WriteLine("stopping service...")
        
        try
            _controller.Stop()

            _controller.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(Conf.Timeout |> float))

            match _controller.Status with
            | ServiceControllerStatus.Stopped -> Trace.WriteLine("stopped")
            | _ -> Trace.WriteLine(String.Format("service not stopped with status: {0}", _controller.Status))
        with xe -> Trace.WriteLine(String.Format("could not stop service. {0}", xe))

[<RunInstaller(true)>]
type public SvcInstaller () =
    inherit Installer ()

    do
        let processInstaller = new ServiceProcessInstaller () 
        let serviceInstaller = new ServiceInstaller ()
        processInstaller.Account <- ServiceAccount.LocalSystem
        serviceInstaller.ServiceName <- Conf.ServiceName
        serviceInstaller.DisplayName <- Conf.DisplayName
        serviceInstaller.StartType <- Conf.StartType
        serviceInstaller.Description <- Conf.Description

        base.Installers.Add(serviceInstaller) |> ignore
        base.Installers.Add(processInstaller) |> ignore
    
    static member Install (undo: bool) (args: String[]) =
        Trace.WriteLine(if undo then "uninstalling" else "installing")

        try
            use assinst = new AssemblyInstaller(Assembly.GetAssembly(typeof<SvcInstaller>), args)
            let state = new Hashtable()
            assinst.UseNewContext <- true
            try
                if undo then
                    assinst.Uninstall(state)
                else
                    assinst.Install(state)
                    assinst.Commit(state)
            with xeInternal -> 
                try
                    assinst.Rollback(state)
                with _ -> ()
                raise xeInternal
        with xe -> Trace.WriteLine(String.Format("installer failed {0}", xe))

module Toolbox =
    let ShowHelp () =
        Console.WriteLine("-h or -help          shows this help")
        Console.WriteLine("-i or -install       installs this windows service")
        Console.WriteLine("-u or -uninstall     uninstalls this windows service")
        Console.WriteLine("-start               starts this windows service")
        Console.WriteLine("-stop                stops this windows service")

    let Runner (svc: #ServiceBase) = 
        let argv = Environment.GetCommandLineArgs()
        let argl = argv |> Seq.map (fun x -> x.ToLower()) |> Seq.skip 1 |> List.ofSeq

        match argl with
        | ["-h"] | ["-help"] -> ShowHelp ()
        | ["-i"] | ["-install"] -> SvcInstaller.Install false argv
        | ["-u"] | ["-uninstall"] -> SvcInstaller.Install true argv
        | ["-start"] -> (new SvcController()).Start()
        | ["-stop"] -> (new SvcController()).Stop()
        | _ -> svc |> ServiceBase.Run
