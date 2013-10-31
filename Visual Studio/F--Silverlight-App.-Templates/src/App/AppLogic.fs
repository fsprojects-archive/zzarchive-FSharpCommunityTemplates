

namespace FSharpSilverlightMvvmTemplate

    open System
    open System.Collections.Generic
    open System.Linq
    open System.Net
    open System.Windows
    open System.Windows.Controls
    open System.Windows.Documents
    open System.Windows.Input
    open System.Windows.Media
    open System.Windows.Media.Animation
    open System.Windows.Shapes

    [<AutoOpen>]
    module private Utilities = 
        /// Use this implementation of the dynamic binding operator
        /// to bind to Xaml components in code-behind, see example below
        let (?) (c:obj) (s:string) =
            match c with 
            | :? ResourceDictionary as r ->  r.[s] :?> 'T
            | :? Control as c -> c.FindName(s) :?> 'T
            | _ -> failwith "dynamic lookup failed"


    type ExpenseItHome() as this = 
        inherit UserControl()
        let approveButton : Button = this?ApproveButton
        do Application.LoadComponent(this, new System.Uri("/App;component/ExpenseItHome.xaml", System.UriKind.Relative));

    type MainPage() as this = 
        inherit UserControl()
        do Application.LoadComponent(this, new System.Uri("/App;component/MainPage.xaml", System.UriKind.Relative));

    type App() as this = 
        inherit Application()
        do Application.LoadComponent(this, new System.Uri("/App;component/App.xaml", System.UriKind.Relative));
        do this.Startup.Add(fun _ -> this.RootVisual <- new MainPage())
        do this.Exit.Add(fun _ -> ())
        do this.UnhandledException.Add(fun e -> 
            // If the this is running outside of the debugger then report the exception using
            // the browser's exception mechanism. On IE this will display it a yellow alert 
            // icon in the status bar and Firefox will display a script error.
            if not System.Diagnostics.Debugger.IsAttached then
                // NOTE: This will allow the thislication to continue running after an exception has been thrown
                // but not handled. 
                // For production thislications this error handling should be replaced with something that will 
                // report the error to the website and stop the thislication.
                e.Handled <- true;
                Deployment.Current.Dispatcher.BeginInvoke(fun _ -> 
                    try
                        let errorMsg = e.ExceptionObject.Message + e.ExceptionObject.StackTrace;
                        let errorMsg = errorMsg.Replace('"', '\'').Replace("\r\n", @"\n");

                        System.Windows.Browser.HtmlPage.Window.Eval("throw new Error(\"Unhandled Error in Silverlight Application " + errorMsg + "\");") |> ignore
                    with _ -> ()) |> ignore)
