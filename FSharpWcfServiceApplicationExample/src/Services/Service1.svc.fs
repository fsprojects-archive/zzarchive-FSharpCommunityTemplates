namespace FSharpWcfServiceApplicationTemplate

open System
open FSharpWcfServiceApplicationTemplate.Contracts

type Service1() =
    interface IService1 with
        member x.GetData value =
            sprintf "%A" value
        member x.GetDataUsingDataContract composite =
            match composite.BoolValue with
            | true -> composite.StringValue <- 
                          sprintf "%A%A" composite.StringValue "Suffix"
            | _ -> "do nothing" |> ignore
            composite