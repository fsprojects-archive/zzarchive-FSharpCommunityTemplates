namespace $safeprojectname$

open System
open Owin
open System.Net.Http
open System.Web
open System.Web.Http
open System.Web.Mvc
open System.Web.Routing
open System.Web.Optimization

type ApiRoute = { id : RouteParameter }

/// Startup class for OWIN applications
type Startup() =
    member x.Configuration(app: IAppBuilder) =
        // Wire up Web API
        let config = new HttpConfiguration()
        // Use attribute routing
        // config.MapHttpAttributeRoutes()
        config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}", { id = RouteParameter.Optional } ) |> ignore

        app.UseWebApi(config) |> ignore

type BundleConfig() =
    static member RegisterBundles (bundles:BundleCollection) =
        bundles.Add(ScriptBundle("~/bundles/jquery").Include([|"~/Scripts/jquery-{version}.js"|]))

        // Use the development version of Modernizr to develop with and learn from. Then, when you're
        // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
        bundles.Add(ScriptBundle("~/bundles/modernizr").Include([|"~/Scripts/modernizr-*"|]))

        bundles.Add(ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"))

        bundles.Add(StyleBundle("~/Content/css").Include(
                        "~/Content/bootstrap.css",
                        "~/Content/site.css"))

/// Route for ASP.NET MVC applications
type Route = { 
    controller : string
    action : string
    id : UrlParameter }

type Global() =
    inherit System.Web.HttpApplication() 

    static member RegisterRoutes(routes:RouteCollection) =
        routes.IgnoreRoute("{resource}.axd/{*pathInfo}")
        routes.MapRoute(
            "Default", // Route name
            "{controller}/{action}/{id}", // URL with parameters
            { controller = "Home"; action = "Index"; id = UrlParameter.Optional } // Parameter defaults
        )

    member x.Application_Start() =
        AreaRegistration.RegisterAllAreas()
        Global.RegisterRoutes RouteTable.Routes |> ignore
        BundleConfig.RegisterBundles BundleTable.Bundles
