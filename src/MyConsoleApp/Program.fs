[<EntryPoint>]
let main argv =
    printfn $"""Hello from F#. You provided the following arguments: {argv |> String.concat ", "}""" // access command-line arguments from argv
    0 // return an integer exit code