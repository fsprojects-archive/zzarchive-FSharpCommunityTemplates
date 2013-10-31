namespace FsMvc4TemplateWizard

open System
open System.IO
open System.Windows.Forms
open System.Collections.Generic
open EnvDTE
open EnvDTE80
open Microsoft.VisualStudio.Shell
open Microsoft.VisualStudio.TemplateWizard
open VSLangProj
open NuGet.VisualStudio
open FsCsMvc4Dialog

type TemplateWizard() =
    [<DefaultValue>] val mutable solution : Solution2
    [<DefaultValue>] val mutable dte : DTE
    [<DefaultValue>] val mutable dte2 : DTE2
    [<DefaultValue>] val mutable serviceProvider : IServiceProvider
    [<DefaultValue>] val mutable destinationPath : string
    [<DefaultValue>] val mutable safeProjectName : string
    [<DefaultValue>] val mutable includeTestProject : bool
    [<DefaultValue>] val mutable vsixInstallPath : string
    [<DefaultValue>] val mutable isWebApi : bool
    [<DefaultValue>] val mutable isSpa : bool
    [<DefaultValue>] val mutable selectedJsFramework : string

    let mutable selectedWebProjectName = "Razor"
    interface IWizard with
        member this.RunStarted (automationObject:Object, 
                                replacementsDictionary:Dictionary<string,string>, 
                                runKind:WizardRunKind, customParams:Object[]) =
            this.vsixInstallPath <- customParams |> Seq.cast |> Seq.find(fun x -> x.Contains ".vstemplate")
            this.dte <- automationObject :?> DTE
            this.dte2 <- automationObject :?> DTE2
            this.solution <- this.dte2.Solution :?> EnvDTE80.Solution2
            this.serviceProvider <- new ServiceProvider(automationObject :?> 
                                     Microsoft.VisualStudio.OLE.Interop.IServiceProvider)
            this.destinationPath <- replacementsDictionary.["$destinationdirectory$"]
            this.safeProjectName <- replacementsDictionary.["$safeprojectname$"]

            let dialog = new TemplateWizardDialog()
            match dialog.ShowDialog().Value with
            | true -> 
                this.includeTestProject <- dialog.IncludeTestsProject
                selectedWebProjectName <- dialog.SelectedViewEngine
                match dialog.SelectedProjectTypeIndex with
                | 1 -> this.isWebApi <- true
                | 2 -> 
                    this.selectedJsFramework <- dialog.SelectedJsFramework
                    this.isSpa <- true
                | _ -> () 
            | _ ->
                raise (new WizardCancelledException())
        member this.ProjectFinishedGenerating project = "Not Implemented" |> ignore
        member this.ProjectItemFinishedGenerating projectItem = "Not Implemented" |> ignore
        member this.ShouldAddProjectItem filePath = true
        member this.BeforeOpeningFile projectItem = "Not Implemented" |> ignore
        member this.RunFinished() = 
            let currentCursor = Cursor.Current
            Cursor.Current <- Cursors.WaitCursor
            try
                let mutable selectedWebAppProjectName = "WebApp"
                let webName = match this.isWebApi, this.isSpa with
                              | true, false -> 
                                  selectedWebProjectName <- "WebApi"
                                  this.safeProjectName + "WebApi"
                              | false, true -> 
                                  selectedWebProjectName <- "SpaRazor"
                                  this.safeProjectName + "WebSpa"
                              | _ -> this.safeProjectName + "Web"
                let webAppName = match this.isWebApi, this.isSpa with
                                 | true, false -> 
                                     selectedWebAppProjectName <- "WebAppApi"
                                     this.safeProjectName + "WebAppApi"
                                 | false, true -> 
                                     selectedWebAppProjectName <- "WebAppSpa"
                                     this.safeProjectName + "WebAppSpa"
                                 | _ -> this.safeProjectName + "WebApp"
                let webAppTestsName = this.safeProjectName + "WebAppTests"

                let templatePath = this.vsixInstallPath 
                try
                    let AddProject status projectVsTemplateName projectName =
                        this.dte2.StatusBar.Text <- status
                        let path = templatePath.Replace("FsMvc4.vstemplate", projectVsTemplateName)
                        this.dte2.Solution.AddFromTemplate(path, Path.Combine(this.destinationPath, projectName), 
                            projectName, false) |> ignore
                    AddProject "Installing the C# Web project..." 
                        (Path.Combine(selectedWebProjectName, selectedWebProjectName + ".vstemplate")) webName
                    AddProject "Adding the F# Web App project..." 
                        (Path.Combine(selectedWebAppProjectName, selectedWebAppProjectName + ".vstemplate")) webAppName
                    if this.includeTestProject then
                        AddProject "Adding the F# Web App Tests project..." 
                            (Path.Combine("WebAppTests", "WebAppTests.vstemplate")) webAppTestsName

                    let projects = BuildProjectMap (this.dte.Solution.Projects)

                    this.dte2.StatusBar.Text <- "Adding NuGet packages..."
                    try

                        let baseNuGetPackages = 
                            [("EntityFramework", "5.0.0"); ("jQuery.UI.Combined", "1.8.11"); ("jQuery", "1.6.4"); ("jQuery.Validation", "1.9.0.1")
                             ("knockoutjs", "2.1.0"); ("Microsoft.AspNet.Mvc", "4.0.20710.0"); ("Microsoft.AspNet.WebApi.Core", "4.0.20710.0"); 
                             ("Microsoft.AspNet.Providers.Core", "1.1"); ("Microsoft.AspNet.Razor", "2.0.20710.0"); ("Microsoft.AspNet.Web.Optimization", "1.0.0") 
                             ("Microsoft.AspNet.WebApi", "4.0.20710.0"); ("Microsoft.AspNet.WebApi.Client", "4.0.20710.0"); ("Microsoft.AspNet.WebApi.WebHost", "4.0.20710.0")
                             ("Microsoft.AspNet.WebPages", "2.0.20710.0"); ("Microsoft.jQuery.Unobtrusive.Ajax", "2.0.20710.0"); ("Microsoft.jQuery.Unobtrusive.Validation", "2.0.20710.0")
                             ("Microsoft.Net.Http", "2.0.20710.0"); ("Microsoft.Web.Infrastructure", "1.0.0.0"); ("Modernizr", "2.5.3");
                             ("Newtonsoft.Json", "4.5.6"); ("WebGrease", "1.1.0"); ("Microsoft.AspNet.Providers.LocalDb", "1.1"); ("Microsoft.AspNet.Web.Optimization", "1.0.0");]
    
                        let appNuGetPackages = 
                            [("EntityFramework", "5.0.0"); ("Microsoft.AspNet.Mvc", "4.0.20710.0"); ("Microsoft.AspNet.WebApi.Core", "4.0.20710.0"); 
                             ("Microsoft.AspNet.Providers.Core", "1.1"); ("Microsoft.AspNet.Web.Optimization", "1.0.0") 
                             ("Microsoft.AspNet.WebApi", "4.0.20710.0"); ("Microsoft.AspNet.WebApi.Client", "4.0.20710.0"); ("Microsoft.AspNet.WebApi.WebHost", "4.0.20710.0")
                             ("Microsoft.Net.Http", "2.0.20710.0"); ("Microsoft.Web.Infrastructure", "1.0.0.0"); ("Newtonsoft.Json", "4.5.6")]

                        let appTestNuGetPackages = 
                            [("EntityFramework", "5.0.0"); ("Microsoft.AspNet.Mvc", "4.0.20710.0")]

                        let nugetPackages = 
                            match this.isSpa, this.selectedJsFramework with
                            | true, "None" -> baseNuGetPackages
                            | true, jsFramework -> 
                                (projects.TryFind webAppName).Value |> InstallPackages this.serviceProvider (templatePath.Replace("FsMvc4.vstemplate", "")) 
                                <| [(jsFramework + "-Bundler", "0.1.1.0")]
                                if jsFramework = "FsSpa-Angular" then
                                    baseNuGetPackages @ [(jsFramework, "0.1.1.0"); ("angularjs", "1.0.7")]
                                else 
                                    baseNuGetPackages @ [(jsFramework, "0.1.1.0")]
                            | _ -> baseNuGetPackages

                        this.dte2.StatusBar.Text <- "Adding NuGet packages to the web project..."
                        (projects.TryFind webName).Value |> InstallPackages this.serviceProvider (templatePath.Replace("FsMvc4.vstemplate", ""))
                        <| nugetPackages

// Unfortunately, if we install the packages the right way into the 2 F# projects, the references do not get setup correctly. This causes issues when deploying to Azure.
// Because of this, I've hacked a solution that allows the project to think that NuGet installed the package, but that really was done through the template. 
//                        this.dte2.StatusBar.Text <- "Adding NuGet packages to the web application project..."
//                        (projects.TryFind webAppName).Value |> InstallPackages this.serviceProvider (templatePath.Replace("FsMvc4.vstemplate", ""))
//                        <| appNuGetPackages                   
     
//                        if this.includeTestProject then               
//                            this.dte2.StatusBar.Text <- "Adding NuGet packages to the unit test project..."
//                            (projects.TryFind webAppTestsName).Value |> InstallPackages this.serviceProvider (templatePath.Replace("FsMvc4.vstemplate", ""))
//                            <| appTestNuGetPackages
                    with
                    | ex -> failwith (sprintf "%s\n\r%s\n\r%s\n\r%s\n\r%s" 
                                "The NuGet installation process failed."
                                "Ensure that you have installed at least the release candidate of ASP.NET MVC 4." 
                                "See http://asp.net/mvc/mvc4 for more information."
                                "The actual exception message is: "
                                ex.Message)

                    this.dte2.StatusBar.Text <- "Updating project references..."
                    [(webName, webAppName); (webAppTestsName, webAppName)]
                    |> BuildProjectReferences projects 
                with
                | ex -> failwith (sprintf "%s\n\r%s\n\r%s\n\r%s\n\r%s" 
                            "The project creation has failed."
                            "Ensure that you have installed at least the release candidate of ASP.NET MVC 4." 
                            "See http://asp.net/mvc/mvc4 for more information."
                            "The actual exception message is: "
                            ex.Message)
            finally
                Cursor.Current <- currentCursor
            
