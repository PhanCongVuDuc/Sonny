# Sonny

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)
[![.NET](https://img.shields.io/badge/.NET-4.8%20%7C%208.0--windows-blue.svg)](https://dotnet.microsoft.com/)
[![Revit](https://img.shields.io/badge/Revit-2021--2026-orange.svg)](https://www.autodesk.com/products/revit)
[![Clean Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blueviolet.svg)](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
[![Build Status](https://img.shields.io/github/actions/workflow/status/PhanCongVuDuc/Sonny/Compile.yml?branch=master)](https://github.com/PhanCongVuDuc/Sonny/actions)

Autodesk Revit plugin project organized into multiple solution files that target versions 2021 - 2026.

## Introduction

Sonny is an **open source** project built with the following goals:

- 📚 **Learning**: An educational project for collaborative research and development of Revit API programming skills
- 🤝 **Community**: Together we develop, share knowledge and experiences
- 🆓 **Free**: Create free tools for everyone to use

This project focuses on developing useful tools for the Revit community, with open source code so that everyone can
learn, contribute, and use freely.

## Table of content

<!-- TOC -->

* [Introduction](#introduction)
* [Features](#features)
* [Architecture](#architecture)
* [What You Can Learn](#what-you-can-learn)
* [Videos](#videos)
* [Prerequisites](#prerequisites)
* [Cloning the Repository](#cloning-the-repository)
* [Build & Test](#build--test)
* [Solution Structure](#solution-structure)
* [Learn More](#learn-more)
* [Dependencies](#dependencies)
* [Contributing](#contributing)
* [License](#license)
* [Security](#security)
* [Acknowledgments](#acknowledgments)

<!-- TOC -->

## Features

Tools the add-in adds to the Revit ribbon:

| Tool | What it does |
|------|--------------|
| **AutoColumnDimension** | Creates dimension lines between every structural column in the active view and its nearest grid, in one click |
| **ColumnFromCad** | Reads column outlines from a linked AutoCAD drawing and creates real Revit structural columns from them |
| **Settings** | Display unit and interface language (English / Vietnamese) |
| **Login** | Licence activation and machine registration |

Behaviour documentation for each feature — business rules, edge cases, and what the tests prove — lives in
[docs/](docs/README.md).

## Architecture

This project follows **Clean Architecture** principles, organizing code into layers with clear separation of concerns:

![Clean Architecture](assets/images/clean-architecture.png)

The architecture consists of four main layers:

- **Domain Layer** - Core business entities and interfaces, framework-agnostic
- **UseCases Layer** - Application business rules and use case implementations
- **Infrastructure Layer** - Framework implementations (Revit API, external services)
- **Presentation Layer** - UI components, ViewModels, and user interface

Dependencies flow inward, ensuring that core business logic remains independent of frameworks and external concerns.

## What You Can Learn

- **Dependency Injection** - Using Microsoft.Extensions.DependencyInjection for IoC
- **Unit Testing** - Writing tests with NUnit and NSubstitute
- **MVVM Pattern** - Implementing MVVM with CommunityToolkit.Mvvm
- **Resource Management / Localization** - Multi-language support with ResourceDictionary and WPFLocalizeExtension
- **Multi-version Support** - Supporting multiple Revit versions (2021-2026)
- **Multiple Units Support** - Handling different measurement units (feet, meters, inches, etc.)
- **Async Programming** - Using async/await with Revit API
- **Build Automation** - Using Nuke for build automation
- **MSBuild Customization** - Custom MSBuild targets and tasks

## Videos

### Settings

- [Login Demo](https://youtu.be/H_OkdKYkkwk)
- [Units & Languages Support](https://www.youtube.com/watch?v=DQbC8JEZ5BM)

### Dimension Tools

- [AutoColumnDimension Demo](https://www.youtube.com/watch?v=ue2QgaLX7fE)

### Model from CAD

- [ColumnFromCad Demo](https://www.youtube.com/watch?v=D6tkBJeo9uo)

### Unit Testing

- [Unit Testing by Console](https://www.youtube.com/watch?v=L4UvIr6Km7g)
- [Unit Testing by Visual Studio](https://www.youtube.com/watch?v=exr4cEYcHmk)

## Prerequisites

Before you can build this project, you need to install .NET and IDE.
If you haven't already installed these, you can do so by visiting the following:

- [.NET Framework 4.8 Developer Pack](https://dotnet.microsoft.com/download/dotnet-framework/net48) — targeted by Revit
  2021–2024
- [.NET SDK 9.0](https://dotnet.microsoft.com/en-us/download/dotnet) — builds the `net8.0-windows` targets used by Revit
  2025–2026
- [JetBrains Rider](https://www.jetbrains.com/rider/) or [Visual Studio](https://visualstudio.microsoft.com/)

| Revit version | Configuration suffix | TargetFramework  |
|---------------|----------------------|------------------|
| 2021–2024     | `R21`–`R24`          | `net48`          |
| 2025–2026     | `R25`, `R26`         | `net8.0-windows` |

After installation, clone this repository to your local machine and navigate to the project directory.

## Cloning the Repository

This project uses **Git Submodule** to manage dependencies. The following libraries are included as submodules from
their own repositories:

- **Revit.Async** - Async utilities for Revit API
- **Sonny.EasyRibbon** - Attribute-based framework for creating Revit Ribbon UI
- **Sonny.RevitExtensions** - Revit API extension methods and utilities library
- **Sonny.Keygen** - License management library using Keygen API with Auth0 authentication

### Clone with Submodules

When cloning this repository, you need to initialize and update submodules to get all dependencies:

```bash
# Clone with submodules (recommended)
git clone --recursive <repository-url>

# Or if you already cloned without --recursive
git submodule update --init --recursive
```

### Updating Submodules

To update submodules to their latest commits:

```bash
# Update submodules to latest commits
git submodule update --remote

# Commit the submodule reference update on a feature branch
git checkout -b chore/update-submodules
git add source/Revit.Async source/Sonny.EasyRibbon source/Sonny.RevitExtensions source/Sonny.Keygen
git commit -m "Update: submodules to latest version"
git push origin chore/update-submodules
```

> [!IMPORTANT]
> This project follows Git Flow: open the pull request against `develop`, never against `master`. `master` is
> release-only — tags on it trigger the publish workflow.

### Working with Submodules

This project includes the following submodules:

#### Revit.Async

- **Submodule location**: `source/Revit.Async`
- **Submodule repository**: [Revit.Async](https://github.com/PhanCongVuDuc/Revit.Async)
- The submodule tracks a specific commit from the Revit.Async repository
- Changes to Revit.Async should be committed in its own repository, not in Sonny

#### Sonny.EasyRibbon

- **Submodule location**: `source/Sonny.EasyRibbon`
- **Submodule repository**: [Sonny.EasyRibbon](https://github.com/PhanCongVuDuc/Sonny.EasyRibbon)
- The submodule tracks a specific commit from the Sonny.EasyRibbon repository
- Changes to Sonny.EasyRibbon should be committed in its own repository, not in Sonny

#### Sonny.RevitExtensions

- **Submodule location**: `source/Sonny.RevitExtensions`
- **Submodule repository**: [Sonny.RevitExtensions](https://github.com/PhanCongVuDuc/Sonny.RevitExtensions)
- The submodule tracks a specific commit from the Sonny.RevitExtensions repository
- Changes to Sonny.RevitExtensions should be committed in its own repository, not in Sonny

#### Sonny.Keygen

- **Submodule location**: `source/Sonny.Keygen`
- **Submodule repository**: [Sonny.Keygen](https://github.com/PhanCongVuDuc/Sonny.Keygen)
- The submodule tracks a specific commit from the Sonny.Keygen repository
- Changes to Sonny.Keygen should be committed in its own repository, not in Sonny

> [!NOTE]
> If you see empty submodule folders after cloning, you need to initialize submodules using
`git submodule update --init --recursive`

## Build & Test

> [!IMPORTANT]
> Configuration names contain a **space** — `Debug R25`, `Release R21`. Always quote them on the command line, or the
> build will fail to resolve the configuration.

```powershell
# Full pipeline (default target = Compile)
./.nuke/build.cmd

# Named targets
./.nuke/build.cmd CreateBundle      # -> output/Sonny.Application.bundle.zip
./.nuke/build.cmd CreateInstaller   # -> .msi via install/Installer.csproj

# Build a single Revit version directly (faster inner loop)
dotnet build source/Sonny.Application/Sonny.Application.csproj -c "Debug R25"
```

A successful build deploys the add-in to your local Revit add-ins folder automatically
(`Nice3point.Revit.Build.Tasks`), so building is enough to try the tool in Revit.

### Running tests

Tests launch a real Revit process through `ricaun.RevitTest.TestAdapter`, so the matching Revit year must be installed.

```powershell
dotnet test source/Sonny.Application.Tests/Sonny.Application.Tests.csproj -c "Debug R23"

# A single test
dotnet test source/Sonny.Application.Tests/Sonny.Application.Tests.csproj -c "Debug R23" `
  --filter "FullyQualifiedName~ColumnFromCad_CreateColumns_Test"
```

Integration tests open `.rvt` fixtures from `Resources/RevitFiles` and assert exact element counts against hard-coded
`UniqueId`s, so they are pinned to those documents. Unit tests under `Core/UnitTests` and `ResourceManager/UnitTests`
use NUnit + NSubstitute and need no Revit document.

> [!NOTE]
> CI builds `Release*` configurations only and skips any project whose name contains "Tests" — tests are not run by CI.
> Run them locally.

## Solution Structure

| Folder  | Description                                                                |
|---------|----------------------------------------------------------------------------|
| build   | Nuke build system. Used to automate project builds                         |
| install | Add-in installer, called implicitly by the Nuke build                      |
| source  | Project source code folder. Contains all solution projects                 |
| docs    | Behaviour documentation — intent, business rules, edge cases               |
| assets  | Images used by the documentation                                           |
| output  | Folder of generated files by the build system, such as bundles, installers |

## Learn More

- [docs/](docs/README.md) — behaviour documentation for this project: how a command reaches its interactor, the
  UIDocument lifetime rule, and per-feature business rules and edge cases
- [RevitTemplates Wiki](https://github.com/Nice3point/RevitTemplates/wiki) — publishing, CI/CD, conditional
  compilation, and API references for the underlying template

## Dependencies

This project uses the following libraries and tools:

- **[RevitTemplates](https://github.com/Nice3point/RevitTemplates)** - Project templates and build system for Revit
  plugins
- **[Revit.Async](https://github.com/PhanCongVuDuc/Revit.Async)** - Async utilities for Revit API
- **[RevitTest](https://github.com/ricaun-io/RevitTest)** - Testing framework for Revit applications
- **[Sonny.EasyRibbon](https://github.com/PhanCongVuDuc/Sonny.EasyRibbon)** - Attribute-based framework for creating
  Revit Ribbon UI
- **[Sonny.RevitExtensions](https://github.com/PhanCongVuDuc/Sonny.RevitExtensions)** - Revit API extension methods and
  utilities library
- **[Sonny.Keygen](https://github.com/PhanCongVuDuc/Sonny.Keygen)** - License management library using Keygen API with
  Auth0 authentication

## Contributing

We welcome contributions! Please see our [Contributing Guide](CONTRIBUTING.md) for details on:

- How to report bugs
- How to suggest features
- How to submit pull requests
- Code style guidelines

Please read our [Code of Conduct](CODE_OF_CONDUCT.md) before contributing.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Security

If you discover a security vulnerability, please see our [Security Policy](SECURITY.md) for information on how to report
it.

## Acknowledgments

We would like to express our gratitude to the creators of the following open-source libraries:

- **[RevitTemplates](https://github.com/Nice3point/RevitTemplates)** by [Nice3point](https://github.com/Nice3point) -
  Project templates and build system for Revit plugins
- **[Revit.Async](https://github.com/PhanCongVuDuc/Revit.Async)** by [KennanChan](https://github.com/KennanChan) - Async
  utilities for Revit API
- **[RevitTest](https://github.com/ricaun-io/RevitTest)** by [ricaun-io](https://github.com/ricaun-io) - Testing
  framework for Revit applications
