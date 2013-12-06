namespace $safeprojectname$.Models

open Newtonsoft.Json

[<CLIMutable>]
[<JsonObject(MemberSerialization=MemberSerialization.OptOut)>]
type Car = {
    Make : string
    Model : string
}
