using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramWebAutoAuth
{
    public class Chat
    {
        public string chatName { get; set; }
        public string id { get; set; }
        public ChatType chatType { get; set; }

        public Chat(string _chatName, string _id, ChatType _chatType)
        {
            chatName = _chatName;
            id = _id;
            chatType = _chatType;
        }
    }
    public enum ChatType
    {
        Group, Channel
    }
}
