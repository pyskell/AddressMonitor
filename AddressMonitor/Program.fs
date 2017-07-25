open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Types
open Suave.Xml

let html xml = 
    OK (View.index xml)
    >=> Writers.setMimeType "text/html; charset=utf-8"

let addAddress (network:Network, address:Address) =
    choose[
        GET >=> warbler(fun _ -> html [text "addAddress GET"])
        POST >=> warbler(fun _ -> html [text "addAddress POST"])
    ]

let addEtcAddress x = 
    let valid = validateAddress Network.ETC x
    if valid then
        html [text (["valid address added: "; x] |> String.concat "")]
    else
        html [text (["invalid address, not added: "; x] |> String.concat "")]

let webPart = choose [
                path Path.home >=> html View.home
                pathScan Path.addEtcAddress addEtcAddress
                //pathScan Path.addAddress addAddress

                html View.notFound
]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig webPart
    0 // return an integer exit code
