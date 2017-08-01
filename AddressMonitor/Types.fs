module Types

open Microsoft.FSharp.Core
open System.Runtime.Serialization

[<DataContract>]
type Network =
    | ETC = 0 
    | ETH = 1 
    | BTC = 2

[<DataContract>]
type Address = {
    [<field: DataMember(Name = "Network")>]
    Network: Network;
    [<field: DataMember(Name = "Address")>]
    Address: string;
}

[<DataContract>]
type User = {
    [<field: DataMember(Name = "Address")>]
    Address: Address[];
    [<field: DataMember(Name = "Email")>]
    Email: string;
}

[<DataContract>]
type AddUserRequest = {
    [<field: DataMember(Name = "Email")>]
    Email: string;
    [<field: DataMember(Name = "Password")>]
    Password: string;
}

// TODO: See if validation can be better, such as hash checking capital letters if present in the string.
let validateEthereumAddress (x:string) : bool = 
    let x' =
        if String.startsWith "0x" x then
            x.[2..]
        else
            x

    let len = String.length x'
    match len with
    | 40 -> true
    | _ -> false

let validateBitcoinAddress (x:string) : bool =
    let len = String.length x
    let first = x.Chars(0)
    match (len, first) with
    | (l,f) when l > 24 && l < 35 && f = '1' -> true
    | _,_ -> false

let validateAddress (network:Network) (x:string) : bool =
    match network with
    | Network.ETC -> validateEthereumAddress x
    | Network.ETH -> validateEthereumAddress x
    | Network.BTC -> validateBitcoinAddress x

// TODO: Improve email validation
let validateEmail (email:string) : bool =
    let containsAt = Seq.exists (fun a -> a = '@') email
    containsAt

// TODO: I think there's a way to put this inside the Address type and use it as the sole constructor
let makeAddress (network:Network) (x:string) : Address option =
    if validateAddress network x then
        Some {Network=network; Address=x}
    else
        None