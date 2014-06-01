namespace ServiceStackAspNetHost

open System

type Global() = 
    inherit System.Web.HttpApplication()

    member x.Application_Start (sender:Object, e:EventArgs) = 
          App_Start.AppHost.start()
 

