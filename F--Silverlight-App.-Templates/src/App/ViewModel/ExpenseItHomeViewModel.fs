namespace FSharpSilverlightMvvmTemplate.ViewModel

open System
open System.Windows
open System.Windows.Data
open System.Windows.Input
open System.ComponentModel
open System.Collections.ObjectModel
open FSharpSilverlightMvvmTemplate.Model
open FSharpSilverlightMvvmTemplate.RemoteFacade

type ExpenseItHomeViewModel(expenseReportServiceFacade : ExpenseReportServiceFacade)  =  
    inherit ViewModelBase()
    let mutable selectedExpenseReport = 
        {Name=""; Department=""; ExpenseLineItems = []}
    new () = ExpenseItHomeViewModel(new ExpenseReportServiceFacade())
    member x.ExpenseReports = 
        new ObservableCollection<ExpenseReport>(
            expenseReportServiceFacade.GetAllExpenseReports())
    member x.ApproveExpenseReportCommand = 
        new RelayCommand ((fun canExecute -> true), (fun action -> x.ApproveExpenseReport)) 
    member x.SelectedExpenseReport 
        with get () = selectedExpenseReport
        and set value = 
            selectedExpenseReport <- value
            x.OnPropertyChanged "SelectedExpenseReport"
    member x.ApproveExpenseReport = 
        "Do something to the expense report" |> ignore

