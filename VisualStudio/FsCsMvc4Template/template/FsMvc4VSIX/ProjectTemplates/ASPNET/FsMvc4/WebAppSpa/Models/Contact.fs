namespace FsWeb.Models

type Contact() = 
    let mutable firstName = ""
    let mutable lastName = ""
    let mutable phone = ""
    member x.FirstName with get() = firstName and set v = firstName <- v
    member x.LastName with get() = lastName and set v = lastName <- v
    member x.Phone with get() = phone and set v = phone <- v
