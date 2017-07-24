module Path

open Types

// TODO: Figure this out.
type AddressPath = PrintfFormat<(Network -> Address -> string), unit, string, string, (Network * Address)>

let home = "/"
let addAddress : AddressPath = "/add/%A/%A"