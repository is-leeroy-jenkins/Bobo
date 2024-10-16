﻿// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="EmptyHistoryRepo.cs" company="Terry D. Eppler">
//    A windows presentation foundation (WPF) application for communication
//    with the Chat GPT-3.5 Turbo API and Chat GPT-4
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
//   EmptyHistoryRepo.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    /// <inheritdoc />
    /// <summary>
    /// </summary>
    /// <seealso cref="T:Bobo.IHistoryRepo" />
    [ SuppressMessage( "ReSharper", "ClassCanBeSealed.Global" ) ]
    [ SuppressMessage( "ReSharper", "FieldCanBeMadeReadOnly.Local" ) ]
    public class EmptyHistoryRepo : IHistoryRepo
    {
        /// <summary>
        /// The chat list
        /// </summary>
        private List<Chat> _chatList;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="EmptyHistoryRepo"/> class.
        /// </summary>
        public EmptyHistoryRepo( )
        {
            _chatList = new List<Chat>( );
        }

        /// <inheritdoc />
        /// <summary>
        /// Gets the database configuration information.
        /// </summary>
        /// <value>
        /// The database configuration information.
        /// </value>
        public string DbConfigInfo
        {
            get
            {
                return "Database not configured (list below not saved)";
            }
        }

        /// <inheritdoc />
        /// <summary>
        /// Loads the chat list.
        /// </summary>
        /// <returns></returns>
        public List<Chat> LoadChatList( )
        {
            // Uncomment this to insert testing data
            //DevDebugInitializeChatList();
            return _chatList;
        }

        // chat.Id remains as 0
        /// <inheritdoc />
        /// <summary>
        /// Adds the chat.
        /// </summary>
        /// <param name="chat">The chat.</param>
        public void AddChat( Chat chat )
        {
        }

        /// <inheritdoc />
        /// <summary>
        /// Deletes the chat.
        /// </summary>
        /// <param name="chat">The chat.</param>
        public void DeleteChat( Chat chat )
        {
        }

        /// <summary>
        /// Devs the debug initialize chat list.
        /// </summary>
        private void DevDebugInitializeChatList( )
        {
            var _prompt = "TestPrompt1";
            var _promptDisplay = _prompt;
            var _newMessage = new Message( "Me", _promptDisplay );
            var _chat = new Chat( _prompt );
            _chat.AddMessage( _newMessage );

            //string result = "TestPrompt1 result";
            _chat.AddMessage( "Bot",
                "TestPrompt1 result" );//.Replace("Bot: ", string.Empty));            

            _chatList.Add( _chat );
            _prompt = "TestPrompt2";
            _promptDisplay = _prompt;
            _newMessage = new Message( "Me", _promptDisplay );
            _chat = new Chat( _prompt );
            _chat.AddMessage( _newMessage );
            _chat.AddMessage( "Bot", "TestPrompt2 result" );
            _chatList.Add( _chat );
        }
    }
}