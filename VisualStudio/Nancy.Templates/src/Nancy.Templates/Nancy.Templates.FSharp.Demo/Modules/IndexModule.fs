namespace Nancy.Templates.FSharp.Demo

open Nancy

type IndexModule() as x =
    inherit NancyModule()
    do x.Get.["/"] <- fun _ -> box x.View.["index"]
