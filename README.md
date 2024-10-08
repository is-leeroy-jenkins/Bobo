_
![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/ProjectTemplate.png)
## C# WPF app that communicates with OpenAI GPT-3.5 Turbo API 

## System requirements

- You need [VC++ 2019 Runtime](https://aka.ms/vs/17/release/vc_redist.x64.exe) 32-bit and 64-bit versions

- You need .NET 6.

- You need to install the version of VC++ Runtime.

## Getting started

- See the [Compilation Guide](docs/Compilation.md) for steps to get started.


## Documentation

- [User Guide](docs/Users.md)
- [Compilation Guide](docs/Compilation.md)
- [Configuration Guide](docs/Configuration.md)
- [Distribution Guide](docs/Distribution.md)


## Code

- SharpBrowser uses CefSharp 106 and is built on NET 6
- SharpBrowser supports AnyCPU as well as x86/x64 specific builds
- `MainForm.cs` - main web browser UI and related functionality
- `Handlers` - various handlers that we have registered with CefSharp that enable deeper integration between us and CefSharp
- `Data/JSON.cs` - fast JSON serializer/deserializer
- `bin` - Binaries are included in the `bin` folder due to the complex CefSharp setup required. Don't empty this folder.
- `bin/storage` - HTML and JS required for downloads manager and custom error pages



