open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Types
open Sql
open Suave.Xml

let html xml = 
    OK (View.index xml)
    >=> Writers.setMimeType "text/html; charset=utf-8"

//let addAddress (network:Network, address:Address) =
//    choose[
//        GET >=> warbler(fun _ -> html [text "addAddress GET"])
//        POST >=> warbler(fun _ -> html [text "addAddress POST"])
//    ]

let addEtcAddress x = 
    let valid = validateAddress Network.ETC x
    if valid then
        html [text (["valid address added: "; x] |> String.concat "")]
    else
        html [text (["invalid address, not added: "; x] |> String.concat "")]

// TODO: Handle the implicitly ignored values here at addUser and addWallet
let addUser user =
    Sql.addUser user
    html [text "added user"]

let addWallet network userId address =
    let network' = enum<Network>(int32 network)
    let user' = getUserById <| int64 userId
    match user' with
    | Some u -> Sql.addWallet network' u.UserId address
                html [text "added wallet"]
    | None -> html [text "user does not exist"]
    
let disallowGet f = choose [
    GET >=> html [text "Send request via POST"]
    POST >=> f
]

let webPart = choose [
                path Path.home >=> html View.home
                disallowGet <| pathScan Path.addEtcAddress addEtcAddress
                disallowGet <| pathScan Path.addUser addUser
                //pathScan Path.addAddress addAddress

                html View.notFound
]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig webPart
    0 // return an integer exit code
