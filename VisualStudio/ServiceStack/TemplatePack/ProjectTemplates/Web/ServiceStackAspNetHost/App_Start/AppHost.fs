namespace $safeprojectname$.App_Start
    open ServiceStack
    open ServiceStack.Common
    open ServiceStack.WebHost.Endpoints
    open $safeprojectname$
    open System

    //[<assembly: WebActivator.PreApplicationStartMethod(typeof<$safeprojectname$.App_Start.AppHost>, "Start")>]
    
    
    type AppHost() = 
        inherit AppHostBase("Hello F# Service", 
                                        typeof<HelloService>.Assembly)
        override this.Configure container = 
            ignore()

        static member start() = 
            let apphost = new AppHost()
            apphost.Init() 
            
            

