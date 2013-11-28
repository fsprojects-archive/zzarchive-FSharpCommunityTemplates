namespace Nancy.Templates.FSharp.AspNetHostWithRazor

open Nancy

type IndexModule() as x =
    inherit NancyModule()
    do x.Get.["/"] <- fun _ -> box x.View.["index"]
