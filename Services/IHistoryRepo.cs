namespace Bobo
{
    using System.Collections.Generic;

    public interface IHistoryRepo
    {
        public string DBConfigInfo { get; }

        public List<Chat> LoadChatList();

        public void AddChat(Chat chat);

        public void DeleteChat(Chat chat);
    }
}
