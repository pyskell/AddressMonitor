// This file is licensed under the terms of the MIT license. Copyright (c) 2017 pyskell
module Path

// TODO: This should be generalizable to an StringPath where both Network and Address are derived from their underlying types
// Also maybe string -> string -> string to string[3]?
type StringPath = PrintfFormat<(string -> string), unit, string, string, string>
type WalletPath = PrintfFormat<(string -> string -> string -> string), unit, string, string>

// The idea:
//type AddressPath = PrintfFormat<(Network -> Address -> string), unit, string, string, (Network * Address)>
//let addAddress : AddressPath = "/add/%A/%A" // or %a/%a
// Link: https://stackoverflow.com/questions/42737079/is-it-possible-to-create-a-custom-printf-textwriterformat

let home = "/"
let addAddress = "/add/address"
let addUser = "/add/user"
let addWallet : WalletPath = "add/wallet/%s/%s/%s"