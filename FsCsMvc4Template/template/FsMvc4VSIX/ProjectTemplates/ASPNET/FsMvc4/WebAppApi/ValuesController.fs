namespace FsWeb.Controllers

open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http

type ValuesController() =
    inherit ApiController()

    // GET /api/values
    member x.Get() = [| "value1"; "value2" |] |> Array.toSeq
    // GET /api/values/5
    member x.Get (id:int) = "value"
    // POST /api/values
    member x.Post ([<FromBody>] value:string) = ()
    // PUT /api/values/5
    member x.Put (id:int) ([<FromBody>] value:string) = ()
    // DELETE /api/values/5
    member x.Delete (id:int) = ()
