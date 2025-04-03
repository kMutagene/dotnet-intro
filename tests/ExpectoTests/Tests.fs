namespace XUnitTests

open Expecto
open MyLibrary

module MathFunctionsTests =

    [<Tests>]
    let expectoTests = 
        testList "ExpectoTests" [
            test "Adding 2 and 2 returns 4" {
                let result = MathFunctions.add 2 2
                Expect.equal result 4 "Result was incorrect"
            }
            test "Subtracting 2 from 4 returns 2" {
                let result = MathFunctions.subtract 4 2
                Expect.equal result 2 "Result was incorrect"
            }
        ]