module Sql

open System
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

let ctx = sql.GetDataContext()

let users = ctx.Main.Users
let walletAddress = ctx.Main.WalletAddresses