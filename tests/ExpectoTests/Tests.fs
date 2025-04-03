module Tests

open System
open Expecto

[<Tests>]
let expectoTests = 
    testList "ExpectoTests" [
        test "Yes" {
            Expect.isTrue true "No"
        }
    ]