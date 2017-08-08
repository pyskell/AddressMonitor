// This file is licensed under the terms of the MIT license. Copyright (c) 2017 pyskell
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

let addAddress (request : Address) = 
    // TODO: Need to handle userId here. Need to get userId from a session variable.
    let newWallet = Sql.addAddress request.Network 1L request.Address

    match newWallet with
    | Some w -> html [text (["valid address added: "; request.Address] |> String.concat "")]
    | None -> html [text (["invalid address, not added: "; request.Address] |> String.concat "")]
        

// TODO: Handle the implicitly ignored values here at addUser and addWallet
let addUser (request : AddUserRequest) =
    let password = request.Password
    Sql.addUser request.Email password
    html [text "added user"]

let webPart : WebPart = choose [
    GET >=> choose [
        path Path.home >=> html View.home
    ]
    POST >=> choose [
        path Path.addUser >=> mapJson addUser
//        path Path.addAddress >=> mapJson addAddress
        path Path.addAddress >=> mapJson addAddress
    ]

    html View.notFound
]

[<EntryPoint>]
let main argv = 
    startWebServer defaultConfig webPart
    0 // return an integer exit code
