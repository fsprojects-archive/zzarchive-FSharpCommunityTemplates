module SampleUsage

open System

open Servo
open Service

[<EntryPoint>]
let main (argv: String[]) = 
    Toolbox.Runner(new $safeprojectname$Class())
    0 
