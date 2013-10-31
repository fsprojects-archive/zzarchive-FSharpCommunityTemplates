namespace FsWeb.Controllers

open System.Collections.Generic
open System.Web
open System.Web.Mvc
open System.Net.Http
open System.Web.Http
open FsWeb.Models

type ContactsController() =
    inherit ApiController()

    // This is for demonstration purposes only. 
    let contacts = seq { yield Contact(FirstName = "John", LastName = "Doe", Phone = "123-123-1233")
                         yield Contact(FirstName = "Jane", LastName = "Doe", Phone = "123-111-9876") }

    // GET /api/contacts
    member x.Get() = 
        // TODO: Replace with your code to retrieve the contacts list
        contacts
    // POST /api/contacts
    member x.Post ([<FromBody>] contact:Contact) = 
        // TODO: Replace with your code to persiste the contact information
        contacts |> Seq.append [ contact ] 