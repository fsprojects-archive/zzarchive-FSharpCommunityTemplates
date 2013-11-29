namespace Nancy.Templates.FSharp.AspNetHost

open Nancy

type IndexModule() as x =
    inherit NancyModule()
    do x.Get.["/"] <- fun _ -> box x.View.["index"]

