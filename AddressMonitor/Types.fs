module Types

open Microsoft.FSharp.Core

type Network = ETC | ETH | BTC

type Address = {
    Network: Network;
    Address: string;
}

type User = {
    Address: Address[];
    Email: string;
}

let validateEthereumAddress (network:Network) (x:string) : bool = 
    let len = String.length x
    match len with
    | 40 -> true
    | _ -> false

let validateBitcoinAddress (network:Network) (x:string) : bool =
    let len = String.length x
    let first = x.Chars(0)
    match (len, first) with
    | (l,f) when l > 24 && l < 35 && f = '1' -> true
    | _,_ -> false

let validateAddress (network:Network) (x:string) : bool =
    match network with
    | Network.ETC -> validateEthereumAddress network x
    | Network.ETH -> validateEthereumAddress network x
    | Network.BTC -> validateBitcoinAddress network x
