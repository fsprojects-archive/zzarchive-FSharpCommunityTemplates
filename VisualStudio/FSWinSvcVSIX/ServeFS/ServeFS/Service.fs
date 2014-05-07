/// this file contains the implementation of the Windows Service
/// Servo would take care of installing/uninstalling and starting/stopping the service

module Service

open System
open System.Threading
open System.Diagnostics
open System.ServiceProcess

open Servo

Conf.ServiceName <- "$safeprojectname$"
Conf.DisplayName <- "$safeprojectname$ Display Name"
Conf.Description <- "$safeprojectname$ Description"

/// separating implementations lets us debugging it as a console application
type $safeprojectname$Logic () =
    let stopped = new ManualResetEvent(false)

    member x.OnStart (args: String[]) = 
        Trace.WriteLine("$safeprojectname$ Implementation: OnStart (running...)")
        // this does nothing but waiting for OnStop triggered
        async {
            stopped.WaitOne() |> ignore
        } |> Async.Start

    member x.OnStop () = 
        Trace.WriteLine("$safeprojectname$ Implementation: OnStop")
        stopped.Set() |> ignore
        Trace.Flush()

type $safeprojectname$Class () =
    inherit ServiceBase()

    let imp = new $safeprojectname$Logic()
    do
        base.ServiceName <- Conf.ServiceName
        base.CanHandlePowerEvent <- false
        base.CanHandleSessionChangeEvent <- false
        base.CanPauseAndContinue <- false
        base.CanShutdown <- false
        base.CanStop <- true
        base.AutoLog <- true

    override x.OnStart (args: String[]) =
        //additional technical things like
        //base.RequestAdditionalTime(1000 * 60 * 3)
        imp.OnStart (args)

    override x.OnStop() =
        //base.RequestAdditionalTime(...)
        imp.OnStop()
