_
![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/ProjectTemplate.png)
## C# WPF app that communicates with OpenAI GPT-3.5 Turbo API 

### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/system_requirements.png)System requirements

- You need [VC++ 2019 Runtime](https://aka.ms/vs/17/release/vc_redist.x64.exe) 32-bit and 64-bit versions

- You need .NET 6.

- You need to install the version of VC++ Runtime.

### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/baby.png)Getting started

- See the [Compilation Guide](docs/Compilation.md) for steps to get started.

### ![]() Basic Use
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

  
### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/documentation.png)  Documentation

- [User Guide](docs/Users.md)
- [Compilation Guide](docs/Compilation.md)
- [Configuration Guide](docs/Configuration.md)
- [Distribution Guide](docs/Distribution.md)


### ![](https://github.com/is-leeroy-jenkins/Bobo/blob/master/Resources/Assets/GitHubImages/csharp.png)  Code

- Bobo is built on NET 6
- Bobo supports AnyCPU as well as x86/x64 specific builds
- `MainWindow.cs` - main web browser UI and related functionality
- `Handlers` - various handlers that we have registered with CefSharp that enable deeper integration between us and CefSharp
- `Data/JSON.cs` - fast JSON serializer/deserializer
- `bin` - Binaries are included in the `bin` folder due to the complex CefSharp setup required. Don't empty this folder.
- `bin/storage` - HTML and JS required for downloads manager and custom error pages



