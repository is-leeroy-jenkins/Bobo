// ******************************************************************************************
//     Assembly:                Bobo
//     Author:                  Terry D. Eppler
//     Created:                 10-16-2024
// 
//     Last Modified By:        Terry D. Eppler
//     Last Modified On:        10-16-2024
// ******************************************************************************************
// <copyright file="LiveChatManager.cs" company="Terry D. Eppler">
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
//   LiveChatManager.cs
// </summary>
// ******************************************************************************************

namespace Bobo
{
    using Bobo;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 
    /// </summary>
    public class LiveChatManager
    {
        /// <summary>
        /// The new chat name
        /// </summary>
        private const string NEW_CHAT_NAME = "New Chat";

        /// <summary>
        /// Initializes a new instance of the <see cref="LiveChatManager"/> class.
        /// </summary>
        public LiveChatManager( )
        {
            ChatList = new List<Chat>( );
        }

        // On the left panel
        /// <summary>
        /// Gets the chat list.
        /// </summary>
        /// <value>
        /// The chat list.
        /// </value>
        public List<Chat> ChatList { get; }

        /// <summary>
        /// Creates new chatexists.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [new chat exists]; otherwise, <c>false</c>.
        /// </value>
        public bool NewChatExists
        {
            get
            {
                return ChatList.Exists( x => x.Name == NEW_CHAT_NAME );
            }
        }

        /// <summary>
        /// Determines whether [is new chat] [the specified name].
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>
        ///   <c>true</c> if [is new chat] [the specified name];
        /// otherwise, <c>false</c>.
        /// </returns>
        public bool IsNewChat( string name )
        {
            return name == NEW_CHAT_NAME;
        }

        /// <summary>
        /// Adds the new chat.
        /// </summary>
        /// <returns></returns>
        public Chat AddNewChat( )
        {
            return AddChat( NEW_CHAT_NAME );
        }

        /// <summary>
        /// Adds the chat.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public Chat AddChat( string name )
        {
            var _chat = new Chat( name );
            ChatList.Add( _chat );
            return _chat;
        }

        /// <summary>
        /// Deletes the chat.
        /// </summary>
        /// <param name="name">The name.</param>
        public void DeleteChat( string name )
        {
            ChatList.RemoveAll( x => x.Name == name );
        }

        /// <summary>
        /// Renames the new chat.
        /// </summary>
        /// <param name="newName">The new name.</param>
        public void RenameNewChat( string newName )
        {
            var _chat = ChatList.FirstOrDefault( x => x.Name.Equals( NEW_CHAT_NAME ) );
            if( _chat != null )
            {
                var _maxToDisplayInSelectedChat = 120;
                if( newName.Length <= _maxToDisplayInSelectedChat )
                {
                    _chat.Name = newName;
                }
                else
                {
                    // NOTE: lose original prompt for a short UI display
                    _chat.Name = newName.Substring( 0, _maxToDisplayInSelectedChat );
                }
            }
        }
    }
}