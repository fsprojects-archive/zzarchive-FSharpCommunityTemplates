[<AutoOpen>]
module NuGetService
    open System
    open System.IO
    open EnvDTE
    open Microsoft.Win32
    open Microsoft.VisualStudio.ComponentModelHost
    open Microsoft.VisualStudio.Shell.Interop
    open NuGet.VisualStudio

    let InstallPackages (serviceProvider:IServiceProvider) nuGetPackagePath (project:Project) packages =
        let componentModel = 
            serviceProvider.GetService(typeof<SComponentModel>) :?> IComponentModel
        let installer = componentModel.GetService<IVsPackageInstaller>()
        packages 
        |> Seq.iter (fun (packageId, version:string) -> 
                         installer.InstallPackage(Path.Combine(nuGetPackagePath, "NuGetPackages"), 
                             project, packageId, Version(version), false))  

