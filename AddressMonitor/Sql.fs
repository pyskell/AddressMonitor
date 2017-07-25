module Sql

open System
open Types
open FSharp.Data.Sql

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

let ctx() = sql.GetDataContext()

let firstOrNone s = s |> Seq.tryFind (fun _ -> true)

let Users = ctx().Main.Users
let WalletAddresses = ctx().Main.WalletAddresses

let addUser (email : string) =
    Users.Create(email) |> ignore

let addWallet (network : Network) (userId : int64) (address : string) =
    WalletAddresses.Create(int64 network, userId, address) |> ignore

let getUser (userId : int64) : User option = firstOrNone <|
    query {
        for user in Users do
            where (user.UserId = userId)
            select user
    }

let getUserWallets (user:User) : WalletAddress list = Seq.toList <|
    query {
        for row in WalletAddresses do
            where (row.UserId = user.UserId)
            select row
    }