namespace $safeprojectname$.App_Start
    open ServiceStack
    open ServiceStack.Common
    open $safeprojectname$
    open System

    
    type AppHost() = 
        inherit AppHostBase("Hello F# Service", 
                                        typeof<HelloService>.Assembly)
        override this.Configure container = 
            ignore()

        static member start() = 
            let apphost = new AppHost()
            apphost.Init() |> ignore
            
            

