﻿// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="LiveChatUserControl.xaml.cs" company="Terry D. Eppler">
//    A windows presentation foundation (WPF) app to communicate with the Chat GPT-3.5 Turbo API
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
//   LiveChatUserControl.xaml.cs
// </summary>
// ******************************************************************************************


namespace Bobo
{
    using System.Collections.Specialized;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System;
    using ModernWpf.Controls;
    using Syncfusion.SfSkinManager;
    using ToastNotifications;
    using ToastNotifications.Lifetime;
    using ToastNotifications.Position;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:System.Windows.Controls.UserControl" />
    /// <seealso cref="T:System.Windows.Markup.IComponentConnector" />
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    [ SuppressMessage( "ReSharper", "RedundantExtendsListEntry" ) ]
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "MemberCanBePrivate.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Global" ) ]
    public partial class LiveChatUserControl : UserControl
    {
        /// <summary>
        /// The is disposed
        /// </summary>
        private protected bool _isDisposed;

        /// <summary>
        /// The busy
        /// </summary>
        private protected bool _busy;

        /// <summary>
        /// The path
        /// </summary>
        private protected object _entry = new object( );

        /// <summary>
        /// The time
        /// </summary>
        private protected int _time;

        /// <summary>
        /// The timer
        /// </summary>
        private protected Timer _timer;

        /// <summary>
        /// The timer
        /// </summary>
        private protected TimerCallback _timerCallback;

        /// <summary>
        /// The status update
        /// </summary>
        private protected Action _statusUpdate;

        /// <summary>
        /// The copy message
        /// </summary>
        private const string COPY_MESSAGE = "Copy";

        /// <summary>
        /// The already loaded
        /// </summary>
        private bool _alreadyLoaded;

        /// <summary>
        /// The chat ListView scroll viewer
        /// </summary>
        private ScrollViewer _chatListViewScrollViewer;

        /// <summary>
        /// The message ListView scroll viewer
        /// </summary>
        private ScrollViewer _messageListViewScrollViewer;

        /// <summary>
        /// The message context menu
        /// </summary>
        private ContextMenu _messageContextMenu;

        /// <summary>
        /// The live chat view model
        /// </summary>
        private protected LiveChatViewModel _liveChatViewModel;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="T:Bobo.UI.Views.LiveChatUserControl" /> class.
        /// </summary>
        public LiveChatUserControl( )
        {
            InitializeComponent( );
            _messageContextMenu = new ContextMenu( );
            _messageContextMenu.AddHandler( MenuItem.ClickEvent,
                new RoutedEventHandler( OnMessageMenuClick ) );

            // Wire Events
            PreviewKeyDown += OnLiveChatUserControlPreviewKeyDown;
            MessageListView.PreviewMouseRightButtonUp += OnMessageListViewPreviewMouseRightButtonUp;
            Loaded += OnLiveChatUserControlLoaded;
        }

        /// <summary>
        /// Gets or sets the live chat view model.
        /// </summary>
        /// <value>
        /// The live chat view model.
        /// </value>
        public LiveChatViewModel LiveChatViewModel
        {
            get
            {
                return _liveChatViewModel;
            }
            set
            {
                _liveChatViewModel = value;
            }
        }

        /// <summary>
        /// Initializes the timer.
        /// </summary>
        private void InitializeTimer( )
        {
            try
            {
                _timerCallback += UpdateStatus;
                _timer = new Timer( _timerCallback, null, 0, 260 );
            }
            catch( Exception ex )
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Fades the in asynchronously.
        /// </summary>
        /// <param name="form">The o.</param>
        /// <param name="interval">The interval.</param>
        private async void FadeInAsync( Window form, int interval = 80 )
        {
            try
            {
                ThrowIf.Null(form, nameof( form ) );
                while(form.Opacity < 1.0 )
                {
                    await Task.Delay(interval);
                    form.Opacity += 0.05;
                }

                form.Opacity = 1;
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Fades the out asynchronously.
        /// </summary>
        /// <param name="form">The o.</param>
        /// <param name="interval">The interval.</param>
        private async void FadeOutAsync(Window form, int interval = 80)
        {
            try
            {
                ThrowIf.Null(form, nameof(form));
                while(form.Opacity > 0.0)
                {
                    await Task.Delay(interval);
                    form.Opacity -= 0.05;
                }

                form.Opacity = 0;
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Creates a notifier.
        /// </summary>
        /// <returns>
        /// Notifier
        /// </returns>
        private Notifier CreateNotifier( )
        {
            try
            {
                var _position = new PrimaryScreenPositionProvider(Corner.BottomRight, 10, 10);
                var _lifeTime = new TimeAndCountBasedLifetimeSupervisor(TimeSpan.FromSeconds(5),
                    MaximumNotificationCount.UnlimitedNotifications());

                return new Notifier(cfg =>
                {
                    cfg.LifetimeSupervisor = _lifeTime;
                    cfg.PositionProvider = _position;
                    cfg.Dispatcher = Dispatcher;
                });
            }
            catch(Exception ex)
            {
                Fail(ex);
                return default(Notifier);
            }
        }

        /// <summary>
        /// Sends the message.
        /// </summary>
        /// <param name="message">The message.</param>
        private void SendMessage(string message)
        {
            try
            {
                ThrowIf.Null(message, nameof(message));
                var _splashMessage = new SplashMessage(message);
                _splashMessage.Show();
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Notifies this instance.
        /// </summary>
        private void SendNotification()
        {
            try
            {
                var _message = "THIS IS NOT YET IMPLEMENTED!";
                var _notify = new Notification(_message);
                _notify.Show();
            }
            catch(Exception ex)
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Invokes if needed.
        /// </summary>
        /// <param name="action">The action.</param>
        public void InvokeIf(Action action)
        {
            try
            {
                ThrowIf.Null(action, nameof(action));
                if(Dispatcher.CheckAccess())
                {
                    action?.Invoke();
                }
                else
                {
                    Dispatcher.BeginInvoke(action);
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Invokes if.
        /// </summary>
        /// <param name="action">The action.</param>
        public void InvokeIf(Action<object> action)
        {
            try
            {
                ThrowIf.Null(action, nameof(action));
                if(Dispatcher.CheckAccess())
                {
                    action?.Invoke(null);
                }
                else
                {
                    Dispatcher.BeginInvoke(action);
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Begins the initialize.
        /// </summary>
        private void Busy( )
        {
            try
            {
                lock( _entry )
                {
                    _busy = true;
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Ends the initialize.
        /// </summary>
        private void Chill( )
        {
            try
            {
                lock(_entry)
                {
                    _busy = false;
                }
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        // Update UI from ChatViewModel
        /// <summary>
        /// Updates the UI.
        /// </summary>
        /// <param name="interfaceUpdate">The interface update.</param>
        private void UpdateUI( InterfaceUpdate interfaceUpdate )
        {
            switch( interfaceUpdate )
            {
                case InterfaceUpdate.SetFocusToChatInput:
                    ChatInputTextBox.Focus( );
                    break;
                case InterfaceUpdate.SetupMessageListViewScrollViewer:
                    SetupMessageListViewScrollViewer( );
                    break;
                case InterfaceUpdate.MessageListViewScrollToBottom:
                    _messageListViewScrollViewer?.ScrollToBottom( );
                    break;
            }
        }

        // From: https://stackoverflow.com/a/41136328
        // This is part of implementing the "automatically scroll
        // to the bottom" functionality.
        /// <summary>
        /// Gets the scroll viewer.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        private ScrollViewer GetScrollViewer( UIElement element )
        {
            ScrollViewer _scrollViewer = null;
            if( element != null )
            {
                for(var _i = 0; _i < VisualTreeHelper.GetChildrenCount( element ) && _scrollViewer == null; _i++ )
                {
                    if(VisualTreeHelper.GetChild( element, _i ) is ScrollViewer )
                    {
                        _scrollViewer = (ScrollViewer)VisualTreeHelper.GetChild( element, _i );
                    }
                    else
                    {
                        _scrollViewer =
                            GetScrollViewer( VisualTreeHelper.GetChild( element, _i ) as UIElement );
                    }
                }
            }

            return _scrollViewer;
        }

        /// <summary>
        /// Setups the chat ListView scroll viewer.
        /// </summary>
        private void SetupChatListViewScrollViewer( )
        {
            // Get the ScrollViewer from the ListView.
            // We'll need that in order to reliably
            // implement "automatically scroll to the bottom
            // when new items are added" functionality.            
            _chatListViewScrollViewer = GetScrollViewer( ChatListView );

            // Based on: https://stackoverflow.com/a/1426312	
            var _notifyCollectionChanged = ChatListView.ItemsSource as INotifyCollectionChanged;
            if(_notifyCollectionChanged != null )
            {
                _notifyCollectionChanged.CollectionChanged += (sender, e) =>
                {
                    _chatListViewScrollViewer?.ScrollToBottom();
                };
            }
        }

        /// <summary>
        /// Shows the message context menu.
        /// </summary>
        /// <param name="message">The message.</param>
        public void ShowMessageContextMenu( Message message )
        {
            _messageContextMenu.Tag = message;
            _messageContextMenu.Items.Clear( );

            // Message header
            _messageContextMenu.Items.Add(new MenuItem
            {
                Header = "Message",
                IsHitTestVisible = false,
                FontSize = 20,
                FontWeight = FontWeights.SemiBold
            });

            ;
            _messageContextMenu.Items.Add( new Separator( ) );

            // Copy to clipboard
            _messageContextMenu.Items.Add( new MenuItem
            {
                Header = COPY_MESSAGE,
                FontSize = 20,
                Icon = new FontIcon
                {
                    Glyph = "\uE16F"
                }
            });

            _messageContextMenu.IsOpen = true;
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( )
        {
            try
            {
                //StatusLabel.Content = DateTime.Now.ToLongTimeString();
                //DateLabel.Content = DateTime.Now.ToShortDateString();
            }
            catch( Exception ex )
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Updates the status.
        /// </summary>
        private void UpdateStatus( object state )
        {
            try
            {
                Dispatcher.BeginInvoke( _statusUpdate );
            }
            catch(Exception ex)
            {
                Fail(ex);
            }
        }

        /// <summary>
        /// Handles the Loaded event of the LiveChatUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLiveChatUserControlLoaded( object sender, RoutedEventArgs e )
        {
            if( !_alreadyLoaded )
            {
                _alreadyLoaded = true;
                _liveChatViewModel = ( LiveChatViewModel )DataContext;
                SetupChatListViewScrollViewer( );
                _messageListViewScrollViewer = GetScrollViewer( MessageListView );
                SetupMessageListViewScrollViewer( );
            }
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of the LiveChatUserControl control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="KeyEventArgs"/>
        /// instance containing the event data.</param>
        private void OnLiveChatUserControlPreviewKeyDown( object sender, KeyEventArgs e )
        {
            if( e.Key == Key.Enter
                && Keyboard.Modifiers == ModifierKeys.Control )
            {
                // Ctrl+Enter for input of multiple lines
                var _liveChatUserControl = sender as LiveChatUserControl;
                if( _liveChatUserControl != null )
                {
                    // ChatGPT mostly answered this!
                    var _textBox = _liveChatUserControl.ChatInputTextBox;
                    var _caretIndex = _textBox.CaretIndex;
                    _textBox.Text = _textBox.Text.Insert( _caretIndex, Environment.NewLine );
                    _textBox.CaretIndex = _caretIndex + Environment.NewLine.Length;
                    e.Handled = true;
                }
            }
            else if( ( e.Key == Key.Up || e.Key == Key.Down )
                && ( e.KeyboardDevice.Modifiers & ModifierKeys.Control ) != 0 )
            {
                // Use CTRL+Up/Down to allow Up/Down alone for multiple lines in ChatInputTextBox
                var _inputTextBox = Keyboard.FocusedElement as TextBox;
                if( _inputTextBox?.Name == "ChatInputTextBox" )
                {
                    _liveChatViewModel?.PrevNextChatInput( e.Key == Key.Up );
                }
            }
        }

        // Needs to re-setup because MessageListView.ItemsSource
        // resets with SelectedChat.MessageList
        // Note: technically there could be a leak without
        // doing 'CollectionChanged -='
        /// <summary>
        /// Setups the message ListView scroll viewer.
        /// </summary>
        private void SetupMessageListViewScrollViewer( )
        {
            var _notifyCollectionChanged = MessageListView.ItemsSource as INotifyCollectionChanged;
            if( _notifyCollectionChanged != null )
            {
                _notifyCollectionChanged.CollectionChanged += ( sender, e ) =>
                {
                    _messageListViewScrollViewer?.ScrollToBottom( );
                };
            }
        }

        /// <summary>
        /// Handles the PreviewMouseRightButtonUp event
        /// of the MessageListView control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="MouseButtonEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMessageListViewPreviewMouseRightButtonUp( object sender,
            MouseButtonEventArgs e )
        {
            // Note: target could be System.Windows.Controls.TextBoxView (in .NET 6)
            //          but it's internal (seen in debugger) and not accessible, so use string
            var _target = e.Device.Target?.ToString( );

            // Hit test for image, text, blank space below text (Border)
            if( e.Device.Target is Grid
                || _target == "System.Windows.Controls.TextBoxView"
                || e.Device.Target is TextBlock )
            {
                var _message = ( e.Device.Target as FrameworkElement )?.DataContext as Message;
                if( _message != null )
                {
                    ShowMessageContextMenu( _message );
                    e.Handled = true;
                }
            }
        }

        /// <summary>
        /// Messages the menu on click.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="RoutedEventArgs"/>
        /// instance containing the event data.</param>
        private void OnMessageMenuClick( object sender, RoutedEventArgs args )
        {
            var _mi = args.Source as MenuItem;
            var _message = _messageContextMenu.Tag as Message;
            if( _mi != null
                && _message != null
                && _liveChatViewModel != null )
            {
                switch( _mi.Header as string )
                {
                    case COPY_MESSAGE:
                        _liveChatViewModel.CopyMessage( _message );
                        break;
                }
            }
        }

        /// <summary>
        /// Called when [calculator menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnCalculatorMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _calculator = new CalculatorWindow
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _calculator.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [file menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnFileMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [folder menu option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnFolderMenuOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                var _fileBrowser = new FileBrowser
                {
                    WindowStartupLocation = WindowStartupLocation.CenterScreen,
                    Topmost = true
                };

                _fileBrowser.Show( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [control panel option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnControlPanelOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchControlPanel( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [task manager option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private void OnTaskManagerOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WinMinion.LaunchTaskManager( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [close option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private protected void OnCloseOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                SfSkinManager.Dispose( this );
                Environment.Exit( 0 );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [chrome option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// containing the event data.</param>
        private protected void OnChromeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunChrome( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [edge option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// instance containing the event data.</param>
        private protected void OnEdgeOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunEdge( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <summary>
        /// Called when [firefox option click].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="EventArgs" />
        /// containing the event data.</param>
        private protected void OnFirefoxOptionClick( object sender, RoutedEventArgs e )
        {
            try
            {
                WebMinion.RunFirefox( );
            }
            catch( Exception ex )
            {
                Fail( ex );
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Performs application-defined tasks
        /// associated with freeing, releasing,
        /// or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c>
        /// to release both managed
        /// and unmanaged resources;
        /// <c>false</c> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if(disposing)
            {
                SfSkinManager.Dispose(this);
                _timer?.Dispose();
            }
        }

        /// <summary>
        /// Fails the specified ex.
        /// </summary>
        /// <param name="ex">The ex.</param>
        private protected static void Fail(Exception ex)
        {
            using var _error = new ErrorWindow(ex);
            _error?.SetText();
            _error?.ShowDialog();
        }
    }
}