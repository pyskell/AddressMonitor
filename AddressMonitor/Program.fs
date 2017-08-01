open Suave
open Suave.Successful
open Suave.Filters
open Suave.Operators
open Suave.RequestErrors
open Types
open Sql
open Suave.Xml
open Suave.Json
open System.Runtime.Serialization
open System.Text

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
let addUser (request : AddUserRequest) =
    let password = Encoding.ASCII.GetBytes(request.Password)
    Sql.addUser request.Email password
    html [text "added user"]

let addWallet network userId address =
    let network' = enum<Network>(int32 network)
    let user' = getUserById <| int64 userId
    match user' with
    | Some u -> Sql.addWallet network' u.UserId address
                html [text "added wallet"]
    | None -> html [text "user does not exist"]
    
let postOnly f = choose [
    GET >=> html [text "Send request via POST"]
    POST >=> f
]

let webPart = choose [
                path Path.home >=> html View.home
                path Path.addUser >=> mapJson addUser
                postOnly <| pathScan Path.addEtcAddress addEtcAddress
                //pathScan Path.addAddress addAddress

                html View.notFound
]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig webPart
    0 // return an integer exit code
