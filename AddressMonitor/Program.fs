open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Types
open Suave.Xml

let html interior = 
    OK (View.index interior)
    >=> Writers.setMimeType "text/html; charset=utf-8"

let addAddress (network:Network, address:Address) =
    warbler(fun _ -> html [text "Added album"])

let webPart = choose [
                path Path.home >=> html View.home
                pathScan Path.addAddress addAddress
]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig webPart
    0 // return an integer exit code
