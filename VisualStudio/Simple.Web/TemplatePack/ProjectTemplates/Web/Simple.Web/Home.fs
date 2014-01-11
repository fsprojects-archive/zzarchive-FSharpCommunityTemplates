namespace $safeprojectname$.Home

open Simple.Web

[<UriTemplate("/")>]
type Index() =
    interface IGet with
        member __.Get() = Status 200