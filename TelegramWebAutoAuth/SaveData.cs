using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramWebAutoAuth
{
    public class SaveData
    {
        public string currentBrowser;
        public List<Chat> chatsToCheck = new List<Chat>();
        public List<string> postsToIgnore = new List<string>();
        public List<string> textToIgnore = new List<string>();
        public bool isWorking = false;
    }
}
