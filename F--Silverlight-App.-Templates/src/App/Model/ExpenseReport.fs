namespace FSharpSilverlightMvvmTemplate.Model

type ExpenseReport =
    { Name : string
      Department : string
      ExpenseLineItems : seq<Expense>}
