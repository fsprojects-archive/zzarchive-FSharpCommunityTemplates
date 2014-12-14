open XService
open Servo.Toolbox

[<EntryPoint>]
let main argv = 
    (new XServiceService()).Run(true)
     
    0 
