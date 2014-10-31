namespace $safeprojectname$.Controllers

open System.Net
open System.Net.Http
open System.Web.Http
open $safeprojectname$.Models

/// Retrieves values.
[<RoutePrefix("api")>]
type CarsController() =
    inherit ApiController()

    let values = [| { Make = "Ford"; Model = "Mustang" }; { Make = "Nissan"; Model = "Titan" } |]

    /// Gets all values.
    [<Route("cars")>]
    member x.Get() = values

    /// Gets a single value at the specified index.
    [<Route("cars/{id}")>]
    member x.Get(request: HttpRequestMessage, id: int) =
        if id >= 0 && values.Length > id then
            request.CreateResponse(values.[id])
        else 
            request.CreateResponse(HttpStatusCode.NotFound)

