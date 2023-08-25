using Microsoft.Playwright;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TelegramWebAutoAuth
{
    public class BrowserManager
    {
        private readonly string pathToCookies;
        private readonly string scannedChatsPath;

        public List<Browser> browsers { get; set; }
        public Browser currentBrowser { get; set; }

        public delegate void NewSessionCreated(object sender, Browser b);

        public event NewSessionCreated newSessionCreated;

        public BrowserManager(string _pathToCookies, string _scannedChatsPath)
        {
            pathToCookies = _pathToCookies;
            scannedChatsPath = _scannedChatsPath;
            browsers = new List<Browser>();
        }

        public void ParseSessions()
        {
            string[] fileNames = Directory.GetFiles(pathToCookies);

            foreach (string fileName in fileNames)
            {
                //string cookiesJson = File.ReadAllText(fileName);
                //var cookies = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Cookie>>(cookiesJson);
                Browser browser = new Browser(ExtractUsernameFromPath(fileName), pathToCookies, scannedChatsPath);
                browsers.Add(browser);
                newSessionCreated?.Invoke(this, browser);
            }
        }

        static string ExtractUsernameFromPath(string path)
        {
            int startIndex = path.LastIndexOf('/') + 1; // Find the last index of '/' and add 1 to get the start of username
            int endIndex = path.LastIndexOf(".json");   // Find the index of ".json" to get the end of username

            if (startIndex >= 0 && endIndex >= 0 && startIndex < endIndex)
            {
                string username = path.Substring(startIndex, endIndex - startIndex);
                return username;
            }
            else
            {
                return string.Empty;
            }
        }

        public async Task Start()
        {
            await currentBrowser?.Start();
        }
        public async Task ConfirmCode(string code)
        {
            await currentBrowser?.ConfirmCode(code);
            newSessionCreated?.Invoke(this, currentBrowser);
        }
        public async Task EnterPhoneNumber(string password)
        {
            await currentBrowser?.EnterPhoneNumber(password);
        }

        public async Task<List<Chat>> ParseChats()
        {
            return await currentBrowser.ParseChats();
        }
        public async Task CheckChats(List<Chat> chatsToCheck, List<string> postsToIgnore, List<string> textToIgnore)
        {
            await currentBrowser.CheckChats(chatsToCheck, postsToIgnore, textToIgnore);
        }

        public async Task<List<string>> SeachContact(string contact)
        {
            return await currentBrowser.SeachContact(contact);
        }

        public async Task CreateGroupAndAddMembers(string groupName, List<string> members)
        {
            await currentBrowser.CreateGroupAndAddMembers(groupName, members);
        }
        public async Task CloseBrowser()
        {
            if (currentBrowser != null)
            {
                await currentBrowser?.Close();
            }
        }
        public void AddBrowser(string phoneNumber)
        {
            Browser browser = new Browser(phoneNumber, pathToCookies, scannedChatsPath);
            browsers.Add(browser);
            currentBrowser = browser;
        }
        public void ChooseBrowser(string phoneNumber)
        {
            currentBrowser?.Close();
            currentBrowser = browsers?.Find(x => x.phoneNumber == phoneNumber);
            currentBrowser?.Start();
        }
        public void DeleteSave()
        {
            currentBrowser.DeleteSave();
        }
    }
}
