# dotnet-intro

This repo aims to be a reference of setting up multiple .NET F# projects.

Due to the nature of these things, it is a snapshot, created in 04/2025, where the lates .NET LTS version was 8.0.
Versions and tooling can and will change in the future.

That said, this repo is organized to show the timeline of setup steps via commits.
Commits are grouped via their description to tasks, with the same description as the sections in this document:

**Table of contents**:

- [Preface](#preface)
- [Library](#library)
  - [Create a solution](#create-a-solution)
  - [Create the library project](#create-the-library-project)
  - [Build the project](#build-the-project)
  - [Write Some Library Code](#write-some-library-code)
  - [Add a new source file](#add-a-new-source-file)
  - [Use your modules](#use-your-modules)
  - [Pack the project](#pack-the-project)
  - [Publish the project to NuGet](#publish-the-project-to-nuget)
- [Console Application](#console-application)
  - [Create the console project](#create-the-console-project)
  - [Reference the library project](#reference-the-library-project)
  - [Write some application code](#write-some-application-code)
  - [Run the console project](#run-the-console-project)
  - [Pack the console project as a .NET tool](#pack-the-console-project-as-a-net-tool)
- [Testing](#testing)
  - [Create a test project](#create-a-test-project)
  - [Create a unit test](#create-a-unit-test)
  - [Run tests](#run-tests)
  - [Integrate with CI/CD](#integrate-with-cicd)

## Preface

This guide assumes that the user has a version of the [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) installed.  
No assumptions regarding choice of IDE (e.g., VSCode, Visual Studio, or Rider) are made.  
Where possible, pure .NET CLI commands are used.  
Note that most commands can also be done via the IDEs.
**All CLI commands are run from the project root folder if not specified otherwise.**

## Library

This section describes how to create a new F# library project and publish it to [NuGet](https://www.nuget.org/).  
Libraries are collections of reusable code that can be consumed by other projects.

### Create a solution

**Solution files** are used to organize multiple projects within a single repository.

Use the `.NET CLI` command:

```sh
dotnet new sln -n MySolution
```

* `dotnet new sln` creates a new solution file.
* `-n` specifies the name of the solution.

### Create the library project

Create an F# library project within the solution folder.

Usually, a `src` folder is created that will contain all projects of the solution.

```sh
dotnet new classlib -lang F# -n MyLibrary -o ./src/MyLibrary -f netstandard2.0
```

* `dotnet new classlib` creates a new library project.
* `-lang F#` specifies the language to be used.
* `-n` specifies the name of the project.
* `-o` specifies the output directory for the project.
* `-f` specifies the target framework for the project. Libraries are usually built against `netstandard` to be compatible with multiple .NET runtimes.

Then, add it to the solution:

```sh
dotnet sln ./MySolution.sln add ./src/MyLibrary/MyLibrary.fsproj
```

* `dotnet sln add` adds the project to a solution.
* `./MySolution` is the path to the solution file.
* `./src/MyLibrary/MyLibrary.fsproj` is the path to the project file generated in the previous step.

### Build the project

Building a project or solution compiles the source code, resolving dependencies and generating the necessary output, such as executables for applications or binaries for libraries.

You can build single projects:

```sh
dotnet build ./src/MyLibrary/MyLibrary.fsproj
```

or the entire solution:

```sh
dotnet build ./MySolution.sln
```

you can also just run `dotnet build` in the solution folder, and it will find the solution file.

### Write Some Library Code

The [classlib template](#create-the-library-project) used in a previous step already created a `MyLibrary.fs` file.
Time to add some code.

In F#, code is organized using namespaces and modules to structure functionality and avoid naming conflicts.

A namespace groups related modules and types, while modules encapsulate related functions and values:

```fsharp
namespace MyLibrary

module MathFunctions =
    let add x y = x + y
    let subtract x y = x - y

module StringUtils =
    let toUpper (s: string) = s.ToUpper()
    let toLower (s: string) = s.ToLower()
```

### Add a new source file

There are no CLI commands to add a new source file to a project, it has to be done manually.

First, create a new file in the project folder, e.g., `NewLibrary.fs`, containing an empty namespace:

```fsharp
namespace NewLibrary
```

Then, add the following code to the `MyLibrary.fsproj`:

```xml
<Compile Include="NewLibrary.fs" />
```

This tells the project to include the new file in the compilation process.

Most IDEs have a `Add file` functionality that will do this for you.

### Use your modules

You can use modules defined in other files in the same project.
Keep in mind that an F# project **is compiled in order`**, so the order of the files in the project matters:

```xml
<Compile Include="Library.fs" />
<Compile Include="NewLibrary.fs" />
```

Means you will be able to use code defined in `Library.fs` in `NewLibrary.fs`, but not the other way around.

To use the modules defined in `MyLibrary.fs`, you can use the `open` keyword to import them into your code:

```fsharp
namespace NewLibrary

module Results =

    let result1 = MyLibrary.MathFunctions.add 1 2
```

### Pack the project

Nuget packages are a way to distribute reusable code libraries and tools in the .NET ecosystem.

Generate a NuGet package from the library project using:

```sh
dotnet pack ./src/MyLibrary/MyLibrary.fsproj -o ./nupkg
```

* `dotnet pack` creates a NuGet package from the project.
* `./src/MyLibrary/MyLibrary.fsproj` is the path to the project file.
* `-o` specifies the output directory for the package.

This creates a `.nupkg` file in the `./nupkg` folder.

You can also pack the entire solution using:

```sh
dotnet pack ./MySolution.sln -o ./nupkg
```

### Publish the project to NuGet

[NuGet](https://www.nuget.org/) is a package registry for .NET.

After creating an account, you can upload `.nupkg` packages manually or via the CLI.

To publish the package using the CLI, you need to set up an API key on your account page.

Then, run the following command:

```sh
dotnet nuget push ./nupkg/MyLibrary.1.0.0.nupkg -k YOUR_API_KEY_HERE
```

* `dotnet nuget push` uploads the package to NuGet.
* `./nupkg/MyLibrary.1.0.0.nupkg` is the path to the package file.
* `-k` specifies the API key for authentication.

## Console Application

This section describes how to create a new F# console application project and wrap it as a .NET tool.
Console applications are standalone programs that can be executed from the command line.

### Create the console project

Create an F# console application using:

```sh
dotnet new console -lang F# -n MyConsoleApp -o ./src/MyConsoleApp -f net8.0
```

* `dotnet new console` creates a new console application project.
* `-lang F#` specifies the language to be used.
* `-n` specifies the name of the project.
* `-o` specifies the output directory for the project.
* `-f` specifies the target framework for the project. For applications, it is usually best to target the latest version of .NET., as you get performance improvements and new features.

Then, add it to the solution:

```sh
dotnet sln ./MySolution.sln add ./src/MyConsoleApp/MyConsoleApp.fsproj
```

### Reference the library project

To use the library in the console application, add a project reference:

```sh
dotnet add ./src/MyConsoleApp/MyConsoleApp.fsproj reference ./src/MyLibrary/MyLibrary.fsproj
```

* `dotnet add reference` adds a project reference to the console application project.
* `./src/MyConsoleApp/MyConsoleApp.fsproj` is the path to the console application project file.
* `./src/MyLibrary/MyLibrary.fsproj` is the path to the library project file.

This will create this reference in the `MyConsoleApp.fsproj`:

```xml
<ProjectReference Include="..\MyLibrary\MyLibrary.fsproj">
```

You could also add a reference to the library project via NuGet, but that would require publishing the library first:

```sh
dotnet add ./src/MyConsoleApp/MyConsoleApp.fsproj package MyLibrary --version 1.0.0
```

* `dotnet add package` adds a NuGet package reference to the console application project.
* `MyLibrary` is the name of the package.

### Write some application code

Applications must end with the main entry point, meaning the last file in the project must contain a function that is executed when the application is run.
The file created by the `dotnet new console` command is called `Program.fs`.
When this file simply ends with a function that returns unit, it implicitly becomes the main entry point:

```fsharp
printfn "Hello from F#"
```

This will print `Hello from F#` to the console when the application is run.

You can also define the entry point explicitly using the `main` function. This has several advantages for more complex programs, such as allowing you to specify command-line arguments and return values.

```fsharp
[<EntryPoint>]
let main argv =
    printfn $"""Hello from F#. You provided the following arguments: {argv |> String.concat ", "}""" // access command-line arguments from argv
    0 // return an integer exit code
```

### Run the console project

Run the application with:

```sh
dotnet run --project ./src/MyConsoleApp/MyConsoleApp.fsproj
```

you can also build the project and run the executable directly:

```sh
dotnet build --project ./src/MyConsoleApp/MyConsoleApp.fsproj
./src/MyConsoleApp/bin/Debug/net8.0/MyConsoleApp.exe
```

To pass command-line arguments to the application, use:

```sh
dotnet run --project ./src/MyConsoleApp/MyConsoleApp.fsproj -- arg1 arg2 arg3
```

or when running the executable directly:

```sh
dotnet build --project ./src/MyConsoleApp/MyConsoleApp.fsproj

./src/MyConsoleApp/bin/Debug/net8.0/MyConsoleApp.exe arg1 arg2 arg3
```

### Pack the console project as a .NET tool

.NET tools are command-line applications that can be installed and executed locally or globally via the .NET CLI.
For this you will need to add tool metadata to the project file:

```xml
<PackAsTool>true</PackAsTool>
<ToolCommandName>mytool</ToolCommandName>
```

Then, Package the console application:

```sh
dotnet pack ./src/MyConsoleApp/MyConsoleApp.fsproj -o ./nupkg
```

You can test the tool locally by installing it from the package:

```sh
dotnet tool install -g --source .\nupkg MyConsoleApp

```sh
* `dotnet tool install` installs a .NET tool.
* `-g` specifies that the tool should be installed globally.
* `--source` specifies the source of the package. In this case, it is the local `nupkg` folder.
* `MyConsoleApp` is the name of the tool.
```

and then running it:

```sh
dotnet mytool arg1 arg2 arg3
```

If this nuget package is published to NuGet, you will be able to install it via the CLI:

```sh
dotnet tool install --global MyConsoleApp --version 1.0.0
```

## Testing

Tests are essential for ensuring the correctness and reliability of your code.
_Unit tests_ are small, isolated tests that aim to verify the behavior of individual components or functions.

This section describes how to test the library and console application projects and perform them in a CI/CD pipeline.

there exist multiple testing frameworks for .NET, such as `xUnit`, or `Expecto`.
`xUnit` is a more general .NET testing framework which isa heavily used in C# projects, while `Expecto` is more focused on F# and functional programming.
This written guide will explicitly use `xUnit`, but the repository contains an equivalent example of using `Expecto` as well.

### Create a test project

test projects are special console applications that are used to run tests.

Create an F# xUnit test project using:

```sh
dotnet new xunit -lang F# -n XUnitTests -o ./tests/XUnitTests -f net8.0
```

Then, add it to the solution:

```sh
dotnet sln ./MySolution.sln add ./tests/XUnitTests/XUnitTests.fsproj
```

Reference the project under test:

```sh
dotnet add ./tests/XUnitTests/XUnitTests.fsproj reference ./src/MyLibrary/MyLibrary.fsproj
```

### Create a unit test

Unit tests in .NET are usually organized in classes that show the compiler that they are containing tests.
These classes then contain methods that each represent a single unit test.
Usually, these methods names are verbose to indicate what the test should verify.
In xUnit, you can use the `Fact` attribute to mark a method as a test case.
Inside these methods, you can use `Assert` methods to verify the actual behavior of your code against the expected behavior.

```fsharp
namespace XUnitTests

open Xunit
open MyLibrary

module MathFunctionsTests =

    [<Fact>]
    let ``Adding 2 and 2 returns 4`` () =
        let result = MathFunctions.add 2 2
        Assert.Equal(4, result)

    [<Fact>]
    let ``Subtracting 2.1 from 4.1 returns 2.0`` () =
        let result = MathFunctions.subtract 4.1 2.1
        Assert.Equal(2.0, result)
```

### Run tests

Execute all tests in a solution using:

```sh
dotnet test
```

or run tests in a specific project using:

```sh
dotnet test ./tests/XUnitTests/XUnitTests.fsproj
```

### Integrate with CI/CD

You can set up GitHub to run tests automatically when you push code to the repository or create a pull request.

A basic GitHub Actions workflow can be set up in `.github/workflows/build.yml` to run:

```yaml
name: Build and Test
on:
  push:
    branches: [ main ]
  pull_request:
    branches: [ main ]

- name: Run tests
  run: dotnet test
```

This workflow will run the tests every time you push code to the `main` branch or create a pull request against it.
