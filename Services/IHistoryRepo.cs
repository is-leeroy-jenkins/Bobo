using System.Collections.Generic;
using Bobo.Models;

namespace Bobo.Services
{
    using Models;

    public interface IHistoryRepo
    {
        public string DBConfigInfo { get; }
        public List<Chat> LoadChatList();
        public void AddChat(Chat chat);
        public void DeleteChat(Chat chat);
    }
}
