namespace Nancy.Templates.FSharp.SelfHostWithRazor

open Nancy

type IndexModule() as x =
    inherit NancyModule()
    do x.Get.["/"] <- fun _ -> box x.View.["index"]
