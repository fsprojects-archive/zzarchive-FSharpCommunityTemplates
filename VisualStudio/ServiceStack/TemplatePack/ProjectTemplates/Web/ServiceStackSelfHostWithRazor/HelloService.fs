namespace HelloService

open ServiceStack
open ServiceStack.Common
open ServiceStack.ServiceInterface
open ServiceStack.ServiceHost


[<CLIMutable>]
type HelloResponse = 
    { Result : string }


[<Route("/hello")>]
[<Route("/hello/{name}")>]
type Hello() = 
    interface IReturn<HelloResponse>
    member val Name = "" with get, set

[<DefaultView("Hello")>]
type HelloService() = 
    inherit Service()
    member this.Any(request : Hello) = { Result = "Hello " + request.Name }

