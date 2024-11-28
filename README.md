#### _
![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/ProjectTemplate.png)
## C# WPF app that communicates with OpenAI GPT-3.5 Turbo API 

### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/system_requirements.png)    System requirements
- You need [VC++ 2019 Runtime](https://aka.ms/vs/17/release/vc_redist.x64.exe) 32-bit and 64-bit versions

- You need .NET 6.

- You need to install the version of VC++ Runtime.

### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/baby.png)    Getting started
- See the [Compilation Guide](docs/Compilation.md) for steps to get started.

### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/features.png)    Basic Use
- To use the code, you need to sign up and get your OpenAI GPT-3.5 Turbo API key (free) at: https://platform.openai.com/account/api-keys
- Put your key in App.xaml.cs. Your key could look like 'sk-Ih...WPd'.
- Bobo uses the MVVM design pattern with MainWindow class and ChatViewModel class.

    ``` 
        public ChatViewModel(WhetstoneChatGPTService chatGPTService)
        {
            _chatGPTService = chatGPTService;
            _chatHistory = new ChatHistory();
            _selectedChat = _chatHistory.AddNewChat();
            ChatList = new ObservableCollection<Chat>(_chatHistory.ChatList);
        }
    ``` 

- `ChatViewModel` contains `WhetstoneChatGPTService` wrapping the NuGet package Whetstone.
- ChatGPT that does communication with OpenAI GPT-3.5 Turbo API. 
- `ChatViewModel` also contains `ChatHistory` tracking a chat list with each chat holding the chat result. 
- ChatList binds with the chat list in XAML on the left panel.

```
        [RelayCommand]
        private async Task Send()
        {    
            if (!ValidateInput(ChatInput, out string prompt))
            {
                return;
            }

            try
            {       
                await Send(prompt, prompt);     
                PostProcessOnSend(prompt);
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }

```

- When the user clicks 'Send' button, prompt (input in the text box) will be sent to OpenAI API server. 
- The result will be displayed on the right side of the screen.

### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/DemoImages/CSharpWpfChatGPT.png)
- Explain is like a lookup, kind of a shortcut of Send. For example, with input of 'ML.NET', I will automatically send "Explain 'ML.NET'" to the GPT server. 
- This feature is helpful for finding meaning of a single word or a phrase. Translate to is just simply a Send with selected language (e.g., "Translate to Spanish 'ChatGPT'"). 
- You would get "'chatgpt' se traduce al español como 'chatgpt'."

```
        [RelayCommand]
        private async Task Explain()
        {
            await ExecutePost("Explain");
        }

        [RelayCommand]
        private async Task TranslateTo()
        {
            await ExecutePost($"Translate to {SelectedLang}");
        }

        private async Task ExecutePost(string prefix)
        {   
            try
            {     
                // 'Explain' or 'Translate to' always uses a new chat
                AddNewChatIfNotExists();

                prompt = $"{prefix} '{prompt}'";
                await Send(prompt, prompt);

                PostProcessOnSend(prompt);     
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }
```

- Speak has nothing to with GPT, but a way of finding out pronunciation of input.

```
        [RelayCommand]
        private void Speak()
        {
            try
            {
                var synthesizer = new SpeechSynthesizer()
                {
                    Volume = 100,  // 0...100
                    Rate = -2,     // -10...10
                };

                synthesizer.SelectVoiceByHints
                 (IsFemaleVoice ? VoiceGender.Female : VoiceGender.Male, VoiceAge.Adult);

                synthesizer.SpeakAsync(ChatInput);
            }
            catch (Exception ex)
            {
                StatusMessage = ex.Message;
            }
        }

```


### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/DemoImages/CSharpWpfChatGPT2.png)
-  Lastly, you can ask GPT-3.5 Turbo to create an image based on a prompt (seen above)

  
### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/documentation.png)    Documentation
- [User Guide](docs/Users.md)
- [Compilation Guide](docs/Compilation.md)
- [Configuration Guide](docs/Configuration.md)
- [Distribution Guide](docs/Distribution.md)



## ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/openai.png) Gnerative AI 

- [Federal Appropriations](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Github/Appropriations.md) - vectorized dataset of federal appropriations available for fine-tuning learning models
- [Federal Regulations](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Github/Regulations.md) - vectorized dataset of federal finance regulations available for fine-tuning learning models


### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/csharp.png)    Code
- Bobo is built on NET 6
- Bobo supports AnyCPU as well as x86/x64 specific builds
- `MainWindow.cs` - main web browser UI and related functionality


## 🙏 Acknowledgements

Bubba uses the following projects and libraries. Please consider supporting them as well (e.g., by starring their repositories):

|                                 Library                                       |             Description                                                |
| ----------------------------------------------------------------------------- | ---------------------------------------------------------------------- |
| [CefSharp.WPF.Core](https://github.com/cefsharp)                              | .NET (WPF/Windows Forms) bindings for the Chromium Embedded Framework  |
| [Epplus](https://github.com/EPPlusSoftware/EPPlus)                  		    | EPPlus-Excel spreadsheets for .NET      								 |
| [Google.Api.CustomSearchAPI.v1](https://developers.google.com/custom-search)  | Google APIs Client Library for working with Customsearch v1            |
| [LinqStatistics](https://www.nuget.org/packages/LinqStatistics)                     | Linq extensions to calculate basic statistics.                                                 |
| [System.Data.SqlServerCe](https://www.nuget.org/packages/System.Data.SqlServerCe_unofficial)                  | Unofficial package of System.Data.SqlServerCe.dll if you need it as dependency.      |
| [Microsoft.Office.Interop.Outlook 15.0.4797.1004](https://www.nuget.org/packages/Microsoft.Office.Interop.Outlook)                        | This an assembly you can use for Outlook 2013/2016/2019 COM interop, generated and signed by Microsoft. This is entirely unsupported and there is no license since it is a repackaging of Office assemblies.                                       |
| [ModernWpfUI 0.9.6](https://www.nuget.org/packages/ModernWpfUI/0.9.7-preview.2)                     | Modern styles and controls for your WPF applications.         |
| [Newtonsoft.Json 13.0.3](https://www.nuget.org/packages/Newtonsoft.Json)                                          | Json.NET is a popular high-performance JSON framework for .NET                  |
| [RestoreWindowPlace 2.1.0](https://www.nuget.org/packages/RestoreWindowPlace)                                             | Save and restore the place of the WPF window                                           |
| [SkiaSharp 2.88.9](https://github.com/mono/SkiaSharp)   | SkiaSharp is a cross-platform 2D graphics API for .NET platforms based on Google's Skia Graphics Library.                           |
| [Syncfusion.Licensing 24.1.41](https://www.nuget.org/packages/Syncfusion.Licensing)                           | Syncfusion licensing is a .NET library for validating the registered Syncfusion license in an application at runtime.         |
| [System.Data.OleDb 9.0.0](https://www.nuget.org/packages/System.Data.OleDb)  | This package implements a data provider for OLE DB data sources.                            |
| [OpenAI API](https://github.com/openai/openai-dotnet)                       | The OpenAI .NET library provides convenient access to the OpenAI REST API from .NET applications.  |
| [System.Data.SqlClient 4.9.0](https://www.nuget.org/packages/System.Data.SqlClient) | The legacy .NET Data Provider for SQL Server.                       |
| [MahApps.Metro](https://mahapps.com/)                                         | UI toolkit for WPF applications                                        |
| [System.Data.SQLite.Core](https://www.nuget.org/packages/System.Data.SQLite.Core)                        | The official SQLite database engine for both x86 and x64 along with the ADO.NET provider. |
| [System.Speech 9.0.0](https://www.nuget.org/packages/System.Speech)          | Provides APIs for speech recognition and synthesis built on the Microsoft Speech API in Windows.                               |
| [ToastNotifications.Messages.Net6 1.0.4](https://github.com/rafallopatka/ToastNotifications)          | Toast notifications for WPF allows you to create and display rich notifications in WPF applications.                              |                           |
| [Syncfusion.SfSkinManager.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.SfSkinManager.WPF)          | The Syncfusion WPF Skin Manageris a .NET UI library that contains the SfSkinManager class, which helps apply the built-in themes to the Syncfusion UI controls for WPF.                               |
| [Syncfusion.Shared.Base 24.1.41](https://www.nuget.org/packages/Syncfusion.Shared.Base)          | Syncfusion WinForms Shared Components                               |
| [Syncfusion.Shared.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Shared.WPF)          | Syncfusion WPF components                               |
| [Syncfusion.Themes.FluentDark.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Themes.FluentDark.WPF)          | The Syncfusion WPF Fluent Dark Theme for WPF contains the style resources to change the look and feel of a WPF application to be similar to the modern Windows UI style introduced in Windows 8 apps.                               |
| [Syncfusion.Tools.WPF 24.1.41](https://www.nuget.org/packages/Syncfusion.Tools.WPF)          | This package contains WPF AutoComplete, WPF DockingManager, WPF Navigation Pane, WPF Hierarchy Navigator, WPF Range Slider, WPF Ribbon, WPF TabControl, WPF Wizard, and WPF Badge components for WPF application.                               |
| [Syncfusion.UI.WPF.NET 24.1.41](https://www.nuget.org/packages/Syncfusion.UI.WPF.NET)          | Syncfusion WPF Controls is a library of 100+ WPF UI components and file formats to build modern WPF applications. 




## ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/signature.png)  Code Signing 

Bobo uses free code signing provided by [SignPath.io](https://signpath.io/) and a free code signing certificate
from [SignPath Foundation](https://signpath.org/).

The binaries and installer are built on [AppVeyor](https://ci.appveyor.com/project/is-leeroy-jenkins/networkmanager) directly from the [GitHub repository](https://github.com/is-leeroy-jenkins/Bobo/blob/main/appveyor.yml).
Build artifacts are automatically sent to [SignPath.io](https://signpath.io/) via webhook, where they are signed after manual approval by the maintainer.
The signed binaries are then uploaded to the [GitHub releases](https://github.com/is-leeroy-jenkins/Bobo/releases) page.


## ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/web.png) Privacy Policy

This program will not transfer any information to other networked systems unless specifically requested by the user or the person installing or operating it.

Bobo has integrated the following services for additional functions, which can be enabled or disabled at the first start (in the welcome dialog) or at any time in the settings:

- [api.github.com](https://docs.github.com/en/site-policy/privacy-policies/github-general-privacy-statement) (Check for program updates)
- [ipify.org](https://www.ipify.org/) (Retrieve the public IP address used by the client)
- [ip-api.com](https://ip-api.com/docs/legal) (Retrieve network information such as geo location, ISP, DNS resolver used, etc. used by the client)

## 📝 License

Bobo is published under the [MIT General Public License v3](https://github.com/is-leeroy-jenkins/Bobo/blob/main/LICENSE).

The licenses of the libraries used can be found [here](https://github.com/is-leeroy-jenkins/Bobo/tree/main/Resources/Licenses).
