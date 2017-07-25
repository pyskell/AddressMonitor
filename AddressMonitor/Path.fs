module Path

open Types


// TODO: This should be generalizable to an AddressPath where both Network and Address are derived from their underlying types
type EtcAddressPath = PrintfFormat<(string -> string), unit, string, string, string>

// The idea:
//type AddressPath = PrintfFormat<(Network -> Address -> string), unit, string, string, (Network * Address)>
//let addAddress : AddressPath = "/add/%A/%A" // or %a/%a
// Link: https://stackoverflow.com/questions/42737079/is-it-possible-to-create-a-custom-printf-textwriterformat

let home = "/"
let addEtcAddress : EtcAddressPath = "/add/ETC/%s"