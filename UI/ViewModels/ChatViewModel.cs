// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 11-02-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        11-02-2024
// ******************************************************************************************
// <copyright file="ChatViewModel.cs" company="Terry D. Eppler">
//   Bocifus is an open source windows (wpf) application that interacts with OpenAI GPT-3.5 Turbo API
//   based on NET6 and written in C-Sharp.
// 
//    Copyright ©  2020-2024 Terry D. Eppler
// 
//    Permission is hereby granted, free of charge, to any person obtaining a copy
//    of this software and associated documentation files (the “Software”),
//    to deal in the Software without restriction,
//    including without limitation the rights to use,
//    copy, modify, merge, publish, distribute, sublicense,
//    and/or sell copies of the Software,
//    and to permit persons to whom the Software is furnished to do so,
//    subject to the following conditions:
// 
//    The above copyright notice and this permission notice shall be included in all
//    copies or substantial portions of the Software.
// 
//    THE SOFTWARE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
//    INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//    FITNESS FOR A PARTICULAR PURPOSE AND NON-INFRINGEMENT.
//    IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
//    DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
//    ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
//    DEALINGS IN THE SOFTWARE.
// 
//    You can contact me at:  terryeppler@gmail.com or eppler.terry@epa.gov
// </copyright>
// <summary>
//   ChatViewModel.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System;
    using System.Collections.ObjectModel;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;
    using System.Speech.Synthesis;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Messaging;
    using Whetstone.ChatGPT;

    // C# .NET 6 / 8 WPF, Whetstone ChatGPT, CommunityToolkit MVVM, ModernWpfUI, RestoreWindowPlace
    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:CommunityToolkit.Mvvm.ComponentModel.ObservableObject" />
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    public partial class ChatViewModel : ObservableObject
    {
        /// <summary>
        /// The history repo
        /// </summary>
        private protected IHistoryRepo _historyRepo;

        /// <summary>
        /// The chat GPT service
        /// </summary>
        private protected ChatGptService _chatGptService;

        /// <summary>
        /// The live chat manager
        /// </summary>
        private protected LiveChatManager _liveChatManager = new LiveChatManager( );

        /// <summary>
        /// The chat input list
        /// </summary>
        private protected List<string> _chatInputList = new List<string>( );

        /// <summary>
        /// The chat input list index
        /// </summary>
        private protected int _chatInputListIndex = -1;

        /// <summary>
        /// The chat list
        /// </summary>
        private protected ObservableCollection<Chat> _chatList;

        /// <summary>
        /// The is command busy
        /// </summary>
        [ ObservableProperty ]
        private protected bool _isCommandBusy;

        /// <summary>
        /// The selected chat
        /// </summary>
        [ ObservableProperty ]
        private protected Chat _selectedChat;

        /// <summary>
        /// The selected language
        /// </summary>
        private protected string _selectedLang;

        /// <summary>
        /// The chat input
        /// </summary>
        [ ObservableProperty ]
        private protected string _chatInput;

        /// <summary>
        /// The image pane visibility
        /// </summary>
        [ ObservableProperty ]
        private protected Visibility _imagePaneVisibility = Visibility.Collapsed;

        /// <summary>
        /// The image input
        /// </summary>
        [ ObservableProperty ]
        private protected string _imageInput = "A tennis court";

        /// <summary>
        /// The is streaming mode
        /// </summary>
        [ ObservableProperty ]
        private protected bool _isStreamingMode = true;

        /// <summary>
        /// The is female voice
        /// </summary>
        [ ObservableProperty ]
        private protected bool _isFemaleVoice = true;

        /// <summary>
        /// The status message
        /// </summary>
        [ ObservableProperty ]
        private protected string _statusMessage =
            "Ctrl+Enter for input of multiple lines. Enter-Key to send."
            + " Ctrl+UpArrow | DownArrow to navigate previous input lines";

        /// <summary>
        /// The result image
        /// </summary>
        private protected byte[ ] _resultImage;

        /// <summary>
        /// The add to history button enabled
        /// </summary>
        private protected bool _addToHistoryButtonEnabled;

        /// <summary>
        /// The update delegate
        /// </summary>
        private protected Action<InterfaceUpdate> _updateDelegate;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="ChatViewModel"/> class.
        /// </summary>
        /// <param name="historyRepo">The history repo.</param>
        /// <param name="chatGptService">The chat GPT service.</param>
        public ChatViewModel( IHistoryRepo historyRepo, ChatGptService chatGptService )
        {
            _historyRepo = historyRepo;
            _chatGptService = chatGptService;
            _selectedChat = _liveChatManager.AddNewChat( );
            _chatList = new ObservableCollection<Chat>( _liveChatManager.ChatList );
            _chatInput = "Please list top 5 ChatGPT prompts";

            // Uncomment this to insert testing data
            // DevDebugInitialize();
        }

        /// <summary>
        /// Initializes the delegates.
        /// </summary>
        private protected void InitializeDelegates( )
        {
            try
            {
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Gets or sets the update UI action.
        /// </summary>
        /// <value>
        /// The update UI action.
        /// </value>
        public Action<InterfaceUpdate> UpdateDelegate
        {
            get
            {
                return _updateDelegate;
            }
            set
            {
                _updateDelegate = value;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is command not busy.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is command not busy;
        /// otherwise, <c>false</c>.
        /// </value>
        public bool IsCommandNotBusy
        {
            get
            {
                return !IsCommandBusy;
            }
        }

        /// <summary>
        /// Gets the chat list.
        /// </summary>
        /// <value>
        /// The chat list.
        /// </value>
        public ObservableCollection<Chat> ChatList
        {
            get
            {
                return _chatList;
            }
            set
            {
                _chatList = value;
            }
        }

        /// <summary>
        /// The result image
        /// </summary>
        public byte[ ] ResultImage
        {
            get
            {
                return _resultImage;
            }
            set
            {
                _resultImage = value;
            }
        }

        /// <summary>
        /// The add to history button enabled
        /// </summary>
        public bool AddToHistoryButtonEnabled
        {
            get
            {
                return _addToHistoryButtonEnabled;
            }
            set
            {
                _addToHistoryButtonEnabled = value;
            }
        }

        /// <summary>
        /// Gets the language list.
        /// </summary>
        /// <value>
        /// The language list.
        /// </value>
        public string[ ] LangList { get; } =
        {
            "English",
            "Chinese",
            "Hindi",
            "Spanish"
        };

        /// <summary>
        /// The selected language
        /// </summary>
        public string SelectedLang
        {
            get
            {
                return _selectedLang;
            }
            set
            {
                _selectedLang = value;
            }
        }

        // Also RelayCommand from AppBar
        /// <summary>
        /// Creates new chat.
        /// </summary>
        [ RelayCommand ]
        private void NewChat( )
        {
            try
            {
                if( !AddNewChatIfNotExists( ) )
                {
                    _statusMessage = "'New Chat' already exists";
                    return;
                }

                _updateDelegate?.Invoke( InterfaceUpdate.SetFocusToChatInput );
                _statusMessage = "'New Chat' has been added and selected";
            }
            catch( Exception ex )
            {
                _statusMessage = ex.Message;
            }
        }

        /// <summary>
        /// Devs the debug initialize.
        /// </summary>
        private void DevDebugInitialize( )
        {
            var _prompt = "TestPrompt1";
            var _promptDisplay = _prompt;
            var _newMessage = new Message( "Me", _promptDisplay );
            _selectedChat.AddMessage( _newMessage );
            var _result = "TestPrompt1 result";
            _selectedChat.AddMessage( "Bot", _result.Replace( "Bot: ", string.Empty ) );
            PostProcessOnSend( _prompt );
            var _newChat = _liveChatManager.AddNewChat( );
            _chatList.Add( _newChat );
            _selectedChat = _newChat;
            _prompt = "TestPrompt2";
            _promptDisplay = _prompt;
            _newMessage = new Message( "Me", _promptDisplay );
            _selectedChat.AddMessage( _newMessage );
            _result = "TestPrompt2 result";
            _selectedChat.AddMessage( "Bot", _result.Replace( "Bot: ", string.Empty ) );
            PostProcessOnSend( _prompt );
        }

        /// <summary>
        /// Adds the new chat if not exists.
        /// </summary>
        /// <returns></returns>
        private bool AddNewChatIfNotExists( )
        {
            if( _liveChatManager.NewChatExists )
            {
                return false;
            }

            // Note: 'New Chat' will be renamed after it's used
            var _newChat = _liveChatManager.AddNewChat( );
            _chatList.Add( _newChat );
            _selectedChat = _newChat;
            return true;
        }

        // Up/Previous or down/next chat input in the chat input list
        /// <summary>
        /// Previouses the next chat input.
        /// </summary>
        /// <param name="isUp">if set to <c>true</c> [is up].</param>
        public void PrevNextChatInput( bool isUp )
        {
            if( _chatInputList.IsNotEmpty( ) )
            {
                if( _chatInput.IsBlank( ) )
                {
                    // Pick the just used last one
                    _chatInput = _chatInputList[ _chatInputList.Count - 1 ];
                    _chatInputListIndex = _chatInputList.Count - 1;
                }
                else
                {
                    if( isUp )
                    {
                        if( _chatInputListIndex <= 0 )
                        {
                            _chatInputListIndex = _chatInputList.Count - 1;
                        }
                        else
                        {
                            _chatInputListIndex--;
                        }
                    }
                    else
                    {
                        if( _chatInputListIndex >= _chatInputList.Count - 1 )
                        {
                            _chatInputListIndex = 0;
                        }
                        else
                        {
                            _chatInputListIndex++;
                        }
                    }
                }

                // Bind ChatInput
                if( !_chatInput.Equals( _chatInputList[ _chatInputListIndex ] ) )
                {
                    _chatInput = _chatInputList[ _chatInputListIndex ];
                }
            }
        }

        /// <summary>
        /// Adds to history.
        /// </summary>
        /// <param name="chat">The chat.</param>
        [ RelayCommand ]
        private void AddToHistory( Chat chat )
        {
            _historyRepo.AddChat( chat );
            FixupChatId( chat );

            // Add chat to HistoryViewModel
            WeakReferenceMessenger.Default.Send( new AddChatMessage( chat ) );
            _statusMessage = $"'{chat.Name}' (PK: {chat.Id}) added to History tab";
        }

        // If DB is configured, chat.Id will be PK (i.e. DB insert already called)    
        /// <summary>
        /// Fixups the chat identifier.
        /// </summary>
        /// <param name="chat">The chat.</param>
        private void FixupChatId( Chat chat )
        {
            if( chat.Id == 0 )
            {
                // DB not configured, assign a max + 1
                chat.Id = _chatList.Count == 0
                    ? 1
                    : _chatList.Max( x => x.Id ) + 1;
            }
        }

        // Both XAML (important DataContext in DataContext.CopyMessageCommand)
        //  binding and menu item
        /// <summary>
        /// Copies the message.
        /// </summary>
        /// <param name="message">The message.</param>
        [ RelayCommand ]
        public void CopyMessage( Message message )
        {
            var _meIndex = _selectedChat.MessageList.IndexOf( message ) - 1;
            if( _meIndex >= 0 )
            {
                var _msg = $"Me: {_selectedChat.MessageList[ _meIndex ].Text}\n\n{message.Text}";
                Clipboard.SetText( _msg );
                _statusMessage = "Both Me and Bot messages copied to clipboard";
            }
            else
            {
                Clipboard.SetText( $"Me: {message.Text}" );
                _statusMessage = "Me message copied to clipboard";
            }
        }

        /// <summary>
        /// Sends this instance.
        /// </summary>
        [ RelayCommand ]
        private async Task Send( )
        {
            if( _isCommandBusy )
            {
                return;
            }

            if( !ValidateInput( _chatInput, out var _prompt ) )
            {
                return;
            }

            try
            {
                SetCommandBusy( true, true );
                var _previousPrompts = BuildPreviousPrompts( );
                if( !string.IsNullOrEmpty( _previousPrompts ) )
                {
                    await Send( $"{_previousPrompts}\nMe: {_prompt}", _prompt );
                }
                else
                {
                    await Send( _prompt, _prompt );
                }

                PostProcessOnSend( _prompt );
                _statusMessage = "Ready";
            }
            catch( Exception ex )
            {
                _statusMessage = ex.Message;
            }

            SetCommandBusy( false, true );

            // Always set focus to ChatInput after Send()
            _updateDelegate?.Invoke( InterfaceUpdate.SetFocusToChatInput );

            // Always ScrollToBottom
            _updateDelegate?.Invoke( InterfaceUpdate.MessageListViewScrollToBottom );
        }

        /// <summary>
        /// Explains this instance.
        /// </summary>
        [ RelayCommand ]
        private async Task Explain( )
        {
            await ExecutePost( "Explain" );
        }

        /// <summary>
        /// Translates to.
        /// </summary>
        [ RelayCommand ]
        private async Task TranslateTo( )
        {
            await ExecutePost( $"Translate to {_selectedLang}" );
        }

        /// <summary>
        /// Executes the post.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        private async Task ExecutePost( string prefix )
        {
            if( _isCommandBusy )
            {
                return;
            }

            if( !ValidateInput( _chatInput, out var _prompt ) )
            {
                return;
            }

            try
            {
                SetCommandBusy( true );

                // 'Explain' or 'Translate to' always uses a new chat
                AddNewChatIfNotExists( );
                _prompt = $"{prefix} '{_prompt}'";
                await Send( _prompt, _prompt );
                PostProcessOnSend( _prompt );

                // Ensure this is marked for logic in BuildPreviousPrompts()
                _selectedChat.IsSend = false;
                _statusMessage = "Ready";
            }
            catch( Exception ex )
            {
                _statusMessage = ex.Message;
            }

            SetCommandBusy( false );

            // Always set focus to ChatInput after Send()
            _updateDelegate?.Invoke( InterfaceUpdate.SetFocusToChatInput );

            // Always ScrollToBottom
            _updateDelegate?.Invoke( InterfaceUpdate.MessageListViewScrollToBottom );
        }

        /// <summary>
        /// Speaks this instance.
        /// </summary>
        [ RelayCommand ]
        private void Speak( )
        {
            try
            {
                SetCommandBusy( true );

                // Note: need to have voices installedL: Win-Key + I, Time & language -> Speech
                var _synthesizer = new SpeechSynthesizer( )
                {
                    Volume = 100,// 0...100
                    Rate = -2    // -10...10                    
                };

                _synthesizer.SelectVoiceByHints( IsFemaleVoice
                    ? VoiceGender.Female
                    : VoiceGender.Male, VoiceAge.Adult );

                // Asynchronous / Synchronous
                _synthesizer.SpeakAsync( _chatInput );

                //synthesizer.Speak(ChatInput);
                _statusMessage = "Done";
            }
            catch( Exception ex )
            {
                _statusMessage = ex.Message;
            }

            SetCommandBusy( false );

            // Always set focus to ChatInput
            _updateDelegate?.Invoke( InterfaceUpdate.SetFocusToChatInput );
        }

        /// <summary>
        /// Validates the input.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        private bool ValidateInput( string input, out string prompt )
        {
            prompt = input.Trim( );
            if( prompt.Length < 2 )
            {
                _statusMessage = "Prompt must be at least 2 characters";
                return false;
            }

            return true;
        }

        // Build 'context' for ChatGPT
        /// <summary>
        /// Builds the previous prompts.
        /// </summary>
        /// <returns></returns>
        private string BuildPreviousPrompts( )
        {
            var _previousPrompts = string.Empty;
            if( !_selectedChat.IsSend )
            {
                // We are on 'Explain' or 'Translate to', so auto-create a new chat
                AddNewChatIfNotExists( );
            }
            else if( _selectedChat.MessageList.IsNotEmpty( ) )
            {
                // Continue with previous chat by sending previousPrompts
                foreach( var _message in _selectedChat.MessageList )
                {
                    _previousPrompts += $"{_message.Sender}: {_message.Text}";
                }
            }

            return _previousPrompts;
        }

        /// <summary>
        /// Sends the specified prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="promptDisplay">The prompt display.</param>
        private async Task Send( string prompt, string promptDisplay )
        {
            var _newMessage = new Message( "Me", promptDisplay );
            _selectedChat.AddMessage( _newMessage );
            _statusMessage = "Talking to ChatGPT API...please wait";
            if( IsStreamingMode )
            {
                await SendStreamingMode( prompt );
            }
            else
            {
                var _result = await DoSend( prompt );
                _selectedChat.AddMessage( "Bot", _result.Replace( "Bot: ", string.Empty ) );
            }

            // Clear the ChatInput field
            _chatInput = string.Empty;
        }

        /// <summary>
        /// Does the send.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <returns></returns>
        private async Task<string> DoSend( string prompt )
        {
            // GPT-3.5
            var _completionResponse = await _chatGptService.CreateChatCompletionAsync( prompt );
            var _message = _completionResponse?.GetMessage( );
            return _message?.Content ?? string.Empty;
        }

        /// <summary>
        /// Sends the streaming mode.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        private async Task SendStreamingMode( string prompt )
        {
            // Append with message.Text below
            var _message = _selectedChat.AddMessage( "Bot", string.Empty );

            // GPT-3.5
            await foreach( var _response in _chatGptService.StreamChatCompletionAsync( prompt )
                .ConfigureAwait( false ) )
            {
                if( _response is not null )
                {
                    var _responseText = _response.GetCompletionText( );
                    _message.Text = _message.Text + _responseText;
                }
            }
        }

        /// <summary>
        /// Posts the process on send.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        private void PostProcessOnSend( string prompt )
        {
            // Handle new chat
            if( _liveChatManager.IsNewChat( _selectedChat.Name ) )
            {
                // After this call, SelectedChat.Name updated on the
                // left panel because SelectedChat is/wraps the new chat                
                _liveChatManager.RenameNewChat( prompt );
                UpdateAddToHistoryButton( _selectedChat );
            }

            // Handle chat input list
            if( !_chatInputList.Any( x => x.Equals( prompt ) ) )
            {
                _chatInputList.Add( prompt );
                _chatInputListIndex = _chatInputList.Count - 1;
            }

            // Handle user input list
            if( !_chatInputList.Any( x => x.Equals( prompt ) ) )
            {
                _chatInputList.Add( prompt );
                _chatInputListIndex = _chatInputList.Count - 1;
            }
        }

        /// <summary>
        /// Expands the or collapse image pane.
        /// </summary>
        [ RelayCommand ]
        private void ExpandOrCollapseImagePane( )
        {
            ImagePaneVisibility = ImagePaneVisibility == Visibility.Visible
                ? Visibility.Collapsed
                : Visibility.Visible;
        }

        /// <summary>
        /// Creates the image.
        /// </summary>
        [ RelayCommand ]
        private async Task CreateImage( )
        {
            if( !ValidateInput( _imageInput, out var _prompt ) )
            {
                return;
            }

            try
            {
                SetCommandBusy( true );
                _statusMessage = "Creating an image...please wait";

                // Will reject query of an image of a real person
                _resultImage = await _chatGptService.CreateImageAsync( _prompt );
                _statusMessage = $"Processed image request for '{_prompt}'";
            }
            catch( Exception ex )
            {
                _statusMessage = ex.Message;
            }

            SetCommandBusy( false );
        }

        // ESC key maps to ClearChatInputCommand
        /// <summary>
        /// Clears the chat input.
        /// </summary>
        [ RelayCommand ]
        private void ClearChatInput( )
        {
            _chatInput = string.Empty;
        }

        // ESC key maps to ClearImageInputCommand
        /// <summary>
        /// Clears the image input.
        /// </summary>
        [ RelayCommand ]
        private void ClearImageInput( )
        {
            _imageInput = string.Empty;
        }

        /// <summary>
        /// Sets the command busy.
        /// </summary>
        /// <param name="isCommandBusy">if set to <c>true</c> [is command busy].</param>
        /// <param name="isSendCommand">if set to <c>true</c> [is send command].</param>
        private void SetCommandBusy( bool isCommandBusy, bool isSendCommand = false )
        {
            _isCommandBusy = isCommandBusy;
            OnPropertyChanged( nameof( IsCommandNotBusy ) );
            if( !isSendCommand )
            {
                Mouse.OverrideCursor = _isCommandBusy
                    ? Cursors.Wait
                    : null;
            }
        }

        // partial method (CommunityToolkit MVVM)
        /// <summary>
        /// Called when [selected chat changed].
        /// </summary>
        /// <param name="value">The value.</param>
        partial void OnSelectedChatChanged( Chat value )
        {
            UpdateAddToHistoryButton( value );
            if( value != null )
            {
                // Re-setup on selected chat changed
                _updateDelegate?.Invoke( InterfaceUpdate.SetupMessageListViewScrollViewer );
            }
        }

        /// <summary>
        /// Updates the add to history button.
        /// </summary>
        /// <param name="value">The value.</param>
        private void UpdateAddToHistoryButton( Chat value )
        {
            _addToHistoryButtonEnabled = value != null && !_liveChatManager.IsNewChat( value.Name );
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected static void Fail( Exception ex )
        {
            using var _error = new ErrorWindow( ex );
            _error?.SetText( );
            _error?.ShowDialog( );
        }
    }
}