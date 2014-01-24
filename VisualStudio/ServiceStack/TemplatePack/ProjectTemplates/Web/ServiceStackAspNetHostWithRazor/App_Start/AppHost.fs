namespace ServiceStackAspNetHost.App_Start
    open ServiceStack
    open ServiceStack.Common
    open ServiceStack.WebHost.Endpoints
    open ServiceStackAspNetHost
    open ServiceStack.Razor
    open System

    //[<assembly: WebActivator.PreApplicationStartMethod(typeof<ServiceStackAspNetHost.App_Start.AppHost>, "Start")>]
    
    
    type AppHost() = 
        inherit AppHostBase("Hello F# Service", 
                                        typeof<HelloService>.Assembly)
        override this.Configure container =
            this.Plugins.Add(new RazorFormat())
//            EndpointHostConfig.RazorNamespaces.Add("ServiceStack.Razor")|> ignore
//            EndpointHostConfig.RazorNamespaces.Add("ServiceStackAspNetHost") |> ignore
            let newEndPointConfig = new EndpointHostConfig()
            newEndPointConfig.DebugMode <- true
            this.SetConfig(newEndPointConfig)
            ignore()

       
        static member start() = 
            let apphost = new AppHost()
            apphost.Init()
            
            

