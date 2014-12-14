module XService

open NLog
open Servo
open System

type XServiceService() =
    static let ClassLogger = LogManager.GetCurrentClassLogger()

    interface IService with
        member this.OnContinue() = 
            raise <| NotImplementedException()

        member this.OnCustomCommand(command : int) = 
            raise <| NotImplementedException()

        member this.OnPause() = 
            raise <| NotImplementedException()

        member this.OnPowerEvent(powerStatus : System.ServiceProcess.PowerBroadcastStatus) = 
            raise <| NotImplementedException()

        member this.OnSessionChange(changeDescription : System.ServiceProcess.SessionChangeDescription) = 
            raise <| NotImplementedException()

        member this.OnShutdown() = 
            raise <| NotImplementedException()

        member this.OnStart(args : string[]) = 
            ClassLogger.Info("Service Started - XService")

        member this.OnStop() = 
            ClassLogger.Info("Service Stopped - XService")
