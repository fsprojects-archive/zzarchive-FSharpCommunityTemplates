namespace FSharpWeb2.Models

open Newtonsoft.Json

[<CLIMutable>]
[<JsonObject(MemberSerialization=MemberSerialization.OptOut)>]
type Car = {
    Make : string
    Model : string
}
