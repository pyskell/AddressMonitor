// Learn more about F# at http://fsharp.org
// See the 'F# Tutorial' project for more help.

open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors

let html interior = 
    OK (View.index interior)
    >=> Writers.setMimeType "text/html; charset=utf-8"

let webPart = choose [
                path Path.home >=> html View.home
]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig webPart
    //printfn "%A" argv
    0 // return an integer exit code
