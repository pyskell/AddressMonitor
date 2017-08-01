// This file is licensed under the terms of the MIT license. Copyright (c) 2017 pyskell
module Sql

open System
open Types
open FSharp.Data.Sql
open System.Security.Cryptography
open System.Text

[<Literal>]
let connectionString = 
    "Data Source=" + 
    __SOURCE_DIRECTORY__ + @"/database/database.db;" + 
    "Version=3;foreign keys=true"

// TODO: /interop/ folder needs additional dlls to work on Linux
// As described here: https://fsprojects.github.io/SQLProvider/core/sqlite.html
[<Literal>]
let resolutionPath = __SOURCE_DIRECTORY__ + @"/interop/"

type sql = SqlDataProvider<
                Common.DatabaseProviderTypes.SQLITE, 
                SQLiteLibrary = Common.SQLiteLibrary.SystemDataSQLite,
                ConnectionString = connectionString, 
                ResolutionPath = resolutionPath, 
                CaseSensitivityChange = Common.CaseSensitivityChange.ORIGINAL>

type DbContext = sql.dataContext
type User = DbContext.``main.UsersEntity``
type WalletAddress = DbContext.``main.WalletAddressesEntity``

let ctx = sql.GetDataContext()

let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

let Users = ctx.Main.Users
let WalletAddresses = ctx.Main.WalletAddresses

let getUserById (userId : int64) : User option = firstOrNone <|
    query {
        for user in Users do
            where (user.UserId = userId)
            select user
    }

let getUserByEmail (email : string) : User option = firstOrNone <|
    query{
        for user in Users do
            where (user.Email = email)
            select user
    }
    
let getUserWallets (user : User) : WalletAddress list = Seq.toList <|
    query {
        for wallet in WalletAddresses do
            where (wallet.UserId = user.UserId)
            select wallet
    }

let hashString (password : string) : string =
    Convert.ToBase64String(SHA256Managed.Create().ComputeHash(Encoding.ASCII.GetBytes(password)))

let verifyUser (user : User) (password : string) : bool =
    let user' = getUserByEmail user.Email
    match user' with
    | Some u -> u.Password.Equals(hashString password)
    | None -> false

// TODO: These don't inform the user of whether or not the user exists.
// May want to reconsider how we handle these functions and notify the user.
let addUser (email : string) (password : string) : User option =
    let user = getUserByEmail email
    match user with
    | Some u -> None // User already exists
    | None ->   let password' = hashString password
                let u' = Users.Create(email, password')
                ctx.SubmitUpdates()
                Some u'

// The use of validAddress here feels more procedural than functional
let addWallet (network : Network) (userId : int64) (address : string) : WalletAddress option =
    let user = getUserById userId

    let validAddress = validateAddress network address
    if validAddress then
        match user with
        | Some _ -> let wallet = WalletAddresses.Create(int64 network, userId, address)
                    ctx.SubmitUpdates()
                    Some wallet
        | None -> None
    else
        None