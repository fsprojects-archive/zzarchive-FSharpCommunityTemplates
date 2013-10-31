namespace FSharpWpfMvvmTemplate.View

open System
open System.Windows
open System.Windows.Controls
open System.Windows.Markup

type ExpenseItHome() =
    inherit UserControl()
    member x.InitializeComponent() =
        do Application.LoadComponent(x, 
            new System.Uri("/FSharpWpfMvvmTemplate.App;component/expenseithome.xaml", 
                UriKind.Relative))

