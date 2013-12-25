namespace $safeprojectname$.Home

open Simple.Web

[<UriTemplate("/")>]
type Index() as this =
    member __.Get() = Status 200

    interface IGet with
        member __.Get() = this.Get()