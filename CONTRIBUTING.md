# Contributing to aspnet-openapi-examples

We welcome participation in this project, either by opening issues to report problems or request features
or submitting pull requests to fix known issues.

## Issues

- You are welcome to [submit an issue](https://github.com/mikekistler/aspnet-openapi-examples/issues) with a bug report or a feature request.
- If you are reporting a bug, please indicate which version of the package you are using and provide steps to reproduce the problem.
- If you are submitting a feature request, please indicate if you are willing or able to submit a PR for it.

## Coding Style / Conventions

### C#

C# files in this project should follow the [Common C# Code Conventions] documented on MS Learn.
The [.editorconfig](./editorconfig) file in this repo should automatically enforce these
guidelines in Visual Studio and VS Code.

[Common C# Code Conventions]: https://learn.microsoft.com/dotnet/csharp/fundamentals/coding-style/coding-conventions

### Markdown

Markdown files in this project should follow the style enforced by the [markdownlint tool][],
as configured by the `.markdownlint.json` file in the root of the project.

[markdownlint tool]: https://github.com/DavidAnson/markdownlint

## Building and Testing

To build and test the project locally, clone the repo and issue the following commands
from the root directory of the project.

```sh
dotnet restore
dotnet build
```

This will generate an OpenAPI v3.0 document for the project in the root directory:

```sh
>ls -l aspnet*.json
-rw-r--r--  1 mikekistler  staff  47411 Jul 23 21:33 aspnet-openapi-examples.json
```

Check that the generated OpenAPI document with the [Spectral linter]:

```sh
>spectral lint aspnet-openapi-examples.json
No results with a severity of 'error' found!
```

## Code of Conduct

This project's code of conduct can be found in the
[CODE_OF_CONDUCT.md file](https://github.com/Azure/azure-api-style-guide/blob/main/CODE_OF_CONDUCT.md)
(v1.4.0 of the [CoC](https://contributor-covenant.org/)).
