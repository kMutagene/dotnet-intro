module XUnitTests

open Xunit
open MyLibrary

module MathFunctionsTests =

    [<Fact>]
    let ``Adding 2 and 2 returns 4`` () =
        let result = MathFunctions.add 2 2
        Assert.Equal(4, result)

    [<Fact>]
    let ``Subtracting 2 from 4 returns 2`` () =
        let result = MathFunctions.subtract 4 2
        Assert.Equal(2, result)