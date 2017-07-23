module Types

type Network = ETC | ETH | BTC

type Address = {
    Network: Network;
    Address: string;
}

type User = {
    Address: Address[];
    Email: string;
}

// Stubs, need to implement
let validateEthereumAddress (network:Network) (x:string) : Address option = Some {Network=network; Address=x}

let validateBitcoinAddress = validateEthereumAddress

let validateAddress (network:Network) (x:string) : Address option =
    match network with
    | Network.ETC -> validateEthereumAddress network x
    | Network.ETH -> validateEthereumAddress network x
    | Network.BTC -> validateBitcoinAddress network x
