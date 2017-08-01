namespace AddressMonitorTests.Tests

module ``Address Monitor Sql Tests`` =
    open NUnit.Framework
    open FsUnit
    open AddressMonitor

    [<Test>]
    let ``hashString should return the same hash for the same input`` () =
        let string' = "test"
        Sql.hashString string' = Sql.hashString string' |> should be True


