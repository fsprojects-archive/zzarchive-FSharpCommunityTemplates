namespace $safeprojectname$.Controllers
open System
open System.Collections.Generic
open System.Linq
open System.Net.Http
open System.Web.Http

/// Retrieves values.
// You can use attribute based routing if desired as well. Make sure to uncomment "config.MapHttpAttributeRoutes()" in the global.asas.fs file if you want to use this new feature.
//[<RoutePrefix("api2/values")>]
type ValuesController() =
    inherit ApiController()
    let values = [|"value1";"value2"|]

    /// Gets all values.
    //[<Route("")>]
    member x.Get() = values

    /// Gets the value with index id.
    //[<Route("{id:int}")>]
    member x.Get(id) = sprintf "The id was %i" id
