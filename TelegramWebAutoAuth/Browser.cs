using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Playwright;
using System.Globalization;
using System.Threading;
using Newtonsoft.Json;
using System.Drawing;

namespace TelegramWebAutoAuth
{
    public class Browser
    {
        public IPlaywright playwright;
        public IBrowser browser;
        public IPage page;

        public List<Chat> chats = new List<Chat>();
        public List<Chat> monitoringChats = new List<Chat>();

        private List<Cookie> cookies;
        public string phoneNumber;
        private string sessionPath;
        private readonly string scannedChatsPath;
        private Random rnd = new Random();

        private object messageCheckerLocker = new object();
        private readonly string dateTimeFormat = "yyyy-MM-dd HH-mm-ss.fff";

        public delegate void PostUploadedHandler(bool isSuccessful, DateTime time);
        public event PostUploadedHandler postUploaded;

        public Browser(string _phoneNumber, string _sessionPath, string _scannedChatsPath, List<Cookie> _cookies)
        {
            phoneNumber = _phoneNumber;
            sessionPath = _sessionPath;
            scannedChatsPath = _scannedChatsPath;
            cookies = _cookies;
        }
        public Browser(string _phoneNumber, string _sessionPath, string _scannedChatsPath)
        {
            phoneNumber = _phoneNumber;
            sessionPath = _sessionPath;
            scannedChatsPath = _scannedChatsPath;
        }

        public async Task Start()
        {
            playwright = await Playwright.CreateAsync();
            if (cookies != null && cookies.Count > 0)
            {
                await page.Context.AddCookiesAsync(cookies);
            }
            browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
            {
                Headless = false
            });
            if (File.Exists(sessionPath + phoneNumber + ".json"))
            {
                page = await browser.NewPageAsync(new BrowserNewPageOptions() { StorageStatePath = sessionPath + phoneNumber + ".json", AcceptDownloads = true });
            }
            else
            {
                page = await browser.NewPageAsync();
            }

            await page.GotoAsync("https://web.skype.com/?openPstnPage=true");
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            await page.WaitForLoadStateAsync(LoadState.Load);
            await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

            await Task.Delay(10000);

            try
            {
                // Find the element by attribute
                var element = await page.QuerySelectorAsync("[data-text-as-pseudo-element='Got it!']");

                if (element != null)
                {
                    await element.ClickAsync();
                }
            }
            catch (Exception)
            {

            }
        }

        public async Task EnterPhoneNumber(string password)
        {
            await page.GotoAsync("https://web.skype.com/?openPstnPage=true");

            var emailInput = await page.QuerySelectorAsync("input[type='email']");

            // Fill in the email input field
            await emailInput.FillAsync(phoneNumber); // Replace with your email

            // Delay to observe the filled input
            await Task.Delay(3000);

            // Find the "Next" button using a selector
            var nextButton = await page.QuerySelectorAsync("input[type='submit'][value='Next']");

            // Click the "Next" button
            await nextButton.ClickAsync();

            // Delay to observe the click action
            await Task.Delay(2000);

            // Find the password input field using a selector
            var passwordInput = await page.QuerySelectorAsync("input[type='password']");

            // Fill in the password field
            await passwordInput.TypeAsync(password); // Replace with the actual password

            // Delay to observe the password being filled
            await Task.Delay(2000);

            // Find the "Sign in" button using a selector
            var signInButton = await page.QuerySelectorAsync("input[value='Sign in']");

            // Click the "Sign in" button
            await signInButton.ClickAsync();

            // Delay to observe the click action
            await Task.Delay(2000);


            try
            {
                // Find the "Yes" button using a selector
                var yesButton = await page.QuerySelectorAsync("input[value='Yes']");

                // Click the "Yes" button
                await yesButton.ClickAsync();

                // Delay to observe the click action
                await Task.Delay(4000);
            }
            catch (Exception)
            {

            }

            try
            {
                // Find the <div> element with the specified text
                var divElement = await page.QuerySelectorAsync("div[data-text-as-pseudo-element='Skip']");

                if (divElement != null)
                {
                    // Click the <div> element
                    await divElement.ClickAsync();
                    await Task.Delay(4000);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                // Find the <div> element with the specified text
                var divElement = await page.QuerySelectorAsync("div[data-text-as-pseudo-element='No, do not contribute']");

                if (divElement != null)
                {
                    // Click the <div> element
                    await divElement.ClickAsync();
                    await Task.Delay(4000);
                }
            }
            catch (Exception)
            {
            }

            try
            {
                // Find the element by attribute
                var element = await page.QuerySelectorAsync("[data-text-as-pseudo-element='Got it!']");

                if (element != null)
                {
                    await element.ClickAsync();
                }
            }
            catch (Exception)
            {

            }

            await Task.Delay(10000);
            SaveCookies();
        }

        public async Task CreateGroupAndAddMembers(string groupName,List<string> members)
        {
            // Find the <div> element with the specified text
            var divElement = await page.QuerySelectorAsync("div[data-text-as-pseudo-element='New Chat']");

            if (divElement != null)
            {
                // Click the <div> element
                await divElement.ClickAsync();
                await Task.Delay(2000);
            }

            // Find the <button> element with the specified text
            var buttonElement = await page.QuerySelectorAsync("button[aria-label='New Group Chat']");

            if (buttonElement != null)
            {
                // Click the <button> element
                await buttonElement.ClickAsync();
                await Task.Delay(2000);
            }

            // Find the <input> element with the specified attributes
            var inputElement = await page.QuerySelectorAsync("input[aria-label='Group Name: ']");

            if (inputElement != null)
            {
                // Type the text "test" into the <input> element
                await inputElement.TypeAsync(groupName);
                await Task.Delay(2000);
            }

            // Find the <button> element with the specified attributes
            var buttonElementNext = await page.QuerySelectorAsync("button[aria-label='Next']");

            if (buttonElementNext != null)
            {
                // Click the <button> element
                await buttonElementNext.ClickAsync();
                await Task.Delay(2000);
            }

            int firstSearch = 0;
            foreach (var memberSkypeId in members)
            {
                if(firstSearch != 0)
                {
                    // Find the <button> element with the specified attributes
                    var buttonElementClose = await page.QuerySelectorAsync("button[aria-label='Close search']");

                    if (buttonElementClose != null)
                    {
                        // Click the <button> element
                        await buttonElementClose.ClickAsync();
                        await Task.Delay(2000);
                    }

                    // Find the <button> element with the specified attributes
                    var buttonElementAgainSearch = await page.QuerySelectorAsync("button[aria-label='Search people and bots']");

                    if (buttonElementAgainSearch != null)
                    {
                        // Click the <button> element
                        await buttonElementAgainSearch.ClickAsync();
                        await Task.Delay(2000);
                    }
                }
                
                // Find the <input> element with the specified attributes
                var inputElementGroupName = await page.QuerySelectorAsync("input[aria-label='Search']");

                if (inputElementGroupName != null)
                {
                    // Type "test" into the <input> element
                    await inputElementGroupName.TypeAsync(memberSkypeId);
                    await Task.Delay(6000);
                }

                // Find the <div> element with the specified attributes
                var divElementSelect = await page.QuerySelectorAsync("div[role='listitem']");

                if (divElementSelect != null)
                {
                    // Click the <div> element
                    await divElementSelect.ClickAsync();
                    await Task.Delay(2000);
                }

                try
                {
                    //If present in more people
                    // Find the parent <div> element with specific role and aria-label
                    var parentDiv = await page.QuerySelectorAsync("div[role='group'][aria-label='More people']");

                    if (parentDiv != null)
                    {
                        // Find all <div> elements with the specified role attribute within the parent <div>
                        var divElements = await parentDiv.QuerySelectorAllAsync("div[role='listitem']");

                        foreach (var divElementName in divElements)
                        {
                            // Click each <div> element
                            await divElementName.ClickAsync();
                            await Task.Delay(2000);
                        }
                    }


                }
                catch (Exception)
                {
                }

                firstSearch++;
            }

            // Find the <button> element with specific attributes
            var buttonElementDone = await page.QuerySelectorAsync("button[role='button'][title='Done'][aria-label='Done']");

            if (buttonElement != null)
            {
                // Click the <button> element
                await buttonElementDone.ClickAsync();
                await Task.Delay(2000);
            }

            var test = 234;

        }

        public async Task<List<string>> SeachContact(string contact)
        {
            List<string> userNamesFound = new List<string>();

            // Find the element by its aria-label attribute
            var element = await page.QuerySelectorAsync("[aria-label='People, groups, messages']");

            if (element != null)
            {
                await element.ClickAsync();
            }


            // Find the input element by its aria-label attribute
            var inputElement = await page.QuerySelectorAsync("[aria-label='Search Skype']");

            if (inputElement != null)
            {
                // Type "test" into the input element
                await inputElement.TypeAsync(contact);
                Console.WriteLine("Typed 'test' into the input element.");
            }

            await Task.Delay(9000);

            // Locate the div element and click it
            var divElementMore = await page.QuerySelectorAsync("div[data-text-as-pseudo-element='More']");
            int maxCount = 0;

            while (divElementMore != null && maxCount <= 5)
            {
                await divElementMore.ClickAsync();
                await Task.Delay(9000);
                divElementMore = await page.QuerySelectorAsync("div[data-text-as-pseudo-element='More']");
                maxCount = maxCount + 1;
            }



            // Find all div elements with role="listitem"
            var divElements = await page.QuerySelectorAllAsync("div[role='listitem']");

            if (divElements != null && divElements.Count > 0)
            {
                foreach (var divElement in divElements)
                {
                    // Get the aria-label attribute value of each div element
                    string ariaLabel = await divElement.GetAttributeAsync("aria-label");

                    // Check if the aria-label contains "skypename"
                    if (ariaLabel != null && ariaLabel.Contains("Skype Name:"))
                    {
                        int startIndex = ariaLabel.IndexOf("Skype Name:") + "Skype Name:".Length;
                        int endIndex = ariaLabel.IndexOf(",", startIndex);

                        if (startIndex != -1 && endIndex != -1)
                        {
                            string skypeName = ariaLabel.Substring(startIndex, endIndex - startIndex).Trim();
                            userNamesFound.Add(skypeName);
                        }

                        //// Extract the Skype name using string manipulation
                        //int startIndex = ariaLabel.IndexOf("Skype Name:") + "Skype Name:".Length;
                        //string skypeName = ariaLabel.Substring(startIndex).Trim();

                        //// Remove the trailing comma if present
                        //if (skypeName.EndsWith(","))
                        //{
                        //    skypeName = skypeName.Remove(skypeName.Length - 1);
                        //    userNamesFound.Add(skypeName);
                        //}
                    }
                }
            }

            // Find the button element using its title attribute
            var button = await page.QuerySelectorAsync("button[title='Close search']");

            if (button != null)
            {
                await button.ClickAsync();
                await Task.Delay(3000);
            }


            return userNamesFound;
        }

        public async Task CheckChats(List<Chat> chatsToCheck, List<string> postsToIgnore, List<string> textToIgnore)
        {
            List<string> checkedMessages = new List<string>();

            for (int i = 0; i < chatsToCheck.Count; i++)
            {
                string directoryPath = $"{scannedChatsPath}{chatsToCheck[i].chatType}/{chatsToCheck[i].chatName}";
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                string lastSavedMessage = GetLastSavedMessage(directoryPath);

                await page.GotoAsync($"https://web.telegram.org/k/#{chatsToCheck[i].id}");

                try
                {
                    var scrollBtn = await page.WaitForSelectorAsync(".active button.tgico-arrow_down", new PageWaitForSelectorOptions() { Timeout = 700 });
                    await scrollBtn.ClickAsync();
                    Task.Delay(2000).Wait();
                }
                catch (Exception ex)
                {
                    await page.EvalOnSelectorAsync(".bubbles .scrollable", "element => element.scrollTop = element.scrollHeight");
                }

                Task.Delay(1000).Wait();

                int index = 0;

                while (true)
                {
                    var messages = (await page.QuerySelectorAllAsync(".bubbles div[data-mid]")).ToList();
                    messages.Reverse();
                    int messagesCount = messages.Count;

                    //var message = messages[messagesCount - 1 - index];
                    var currentTime = DateTime.UtcNow;
                    var message = messages.Find(x => !checkedMessages.Contains(x.GetAttributeAsync("data-timestamp").Result));
                    while (message == null && DateTime.UtcNow - currentTime < TimeSpan.FromSeconds(5))
                    {
                        Task.Delay(1000).Wait();
                        message = messages.Find(x => !checkedMessages.Contains(x.GetAttributeAsync("data-timestamp").Result));
                    }
                    if (message == null)
                    {
                        break;
                    }

                    var id = await message.GetAttributeAsync("data-timestamp");
                    if (id == lastSavedMessage)
                    {
                        break;
                    }

                    if (!checkedMessages.Contains(id))
                    {
                        await message.ScrollIntoViewIfNeededAsync();
                        index = 0;
                        string folder = Path.Combine(directoryPath, $"{DateTime.UtcNow.ToString(dateTimeFormat)}-{id}");
                        Directory.CreateDirectory(folder);
                        checkedMessages.Add(id);

                        EventHandler<IDownload> downloader = null;
                        page.Download += downloader = (sender, e) =>
                        {
                            lock (messageCheckerLocker)
                            {
                                var filePath = Path.Combine($"{folder}", e.SuggestedFilename);
                                e.SaveAsAsync(filePath);
                                page.Download -= downloader;
                            }
                        };

                        var voiceMessage = await message.QuerySelectorAsync("audio-element");
                        int tries = 0;
                        if (voiceMessage == null)
                        {
                            var poolElement = await message.QuerySelectorAsync("poll-element");
                            if (poolElement == null)
                            {
                                try
                                {
                                    var excessiveText = message.QuerySelectorAsync(".message span").Result.TextContentAsync().Result;
                                    var text = message.QuerySelectorAsync(".message").Result.TextContentAsync().Result.Replace(excessiveText, "");
                                    if (ShouldIgnore(text, postsToIgnore))
                                    {
                                        continue;
                                    }
                                    for (int q = 0; q < textToIgnore.Count; q++)
                                    {
                                        text = text.Replace(textToIgnore[q], "");
                                    }

                                    File.WriteAllText(Path.Combine(folder, "message.txt"), text);
                                }
                                catch
                                {

                                }
                            }
                        }
                        else
                        {
                            try
                            {
                                await voiceMessage.ScrollIntoViewIfNeededAsync();
                                await voiceMessage.ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                await page.Locator(".tgico-download").ClickAsync();
                            }
                            catch
                            {
                                continue;
                            }
                        }

                        tries = 0;
                        var attachmentElement = await message.QuerySelectorAsync(".bubble-content .attachment");
                        if (attachmentElement != null)
                        {
                            var attachments = await attachmentElement.QuerySelectorAllAsync(".grouped-item");
                            if (attachments.Count > 0)
                            {
                                for (int t = 0; t < attachments.Count; t++)
                                {
                                attachmentStart:
                                    tries++;
                                    try
                                    {
                                        await attachments[t].ScrollIntoViewIfNeededAsync();
                                        await attachments[t].ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                        await page.Locator(".tgico-download").ClickAsync();
                                    }
                                    catch
                                    {
                                        await page.Mouse.DownAsync();
                                        if (tries - t > 3)
                                        {
                                            break;
                                        }
                                        Task.Delay(1000).Wait();
                                        goto attachmentStart;
                                    }
                                }
                            }
                            else
                            {
                                var videos = await attachmentElement.QuerySelectorAllAsync("video");
                                if (videos.Count > 0)
                                {
                                    for (int t = 0; t < videos.Count; t++)
                                    {
                                    videoStart:
                                        tries++;
                                        //var boundingBox = await videos[t].BoundingBoxAsync();

                                        //var x = boundingBox.X + boundingBox.Width / 2;
                                        //var y = boundingBox.Y + boundingBox.Height / 2;

                                        //await page.Mouse.MoveAsync(x, y);

                                        //await page.Mouse.DownAsync(new MouseDownOptions() { Button = MouseButton.Right });
                                        try
                                        {
                                            await videos[t].ScrollIntoViewIfNeededAsync();
                                            await videos[t].ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                            await page.Locator(".tgico-download").ClickAsync();
                                        }
                                        catch
                                        {
                                            await page.Mouse.DownAsync();
                                            if (tries - t > 3)
                                            {
                                                break;
                                            }
                                            Task.Delay(1000).Wait();
                                            goto videoStart;
                                        }
                                    }
                                }
                                else
                                {
                                    var imgs = await attachmentElement.QuerySelectorAllAsync("img");
                                    if (imgs.Count > 0)
                                    {
                                        for (int t = 0; t < imgs.Count; t++)
                                        {
                                        imgStart:
                                            tries++;
                                            try
                                            {
                                                await imgs[t].ScrollIntoViewIfNeededAsync();
                                                await imgs[t].ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                                await page.Locator(".tgico-download").ClickAsync();
                                            }
                                            catch
                                            {
                                                await page.Mouse.DownAsync();
                                                if (tries - t > 3)
                                                {
                                                    break;
                                                }
                                                Task.Delay(1000).Wait();
                                                goto imgStart;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        page.Download -= downloader;
                                    }
                                }
                            }
                        }
                        else
                        {
                            page.Download -= downloader;
                        }

                        Task.Delay(500).Wait();
                        lock (messageCheckerLocker) { }
                    }
                    else
                    {
                        index++;
                    }


                }
            }
        }
        public async Task StartMonitoringChats(List<Chat> chatsToCheck, List<string> postsToIgnore, List<string> textToIgnore)
        {
            while (page == null)
            {
                await Task.Delay(1000);
            }

            for (int i = 0; i < chatsToCheck.Count; i++)
            {
                if (monitoringChats.Find(x => x.id == chatsToCheck[i].id) == null)
                {
                    monitoringChats.Add(chatsToCheck[i]);
                    var newPage = await browser.NewPageAsync(new BrowserNewPageOptions() { StorageStatePath = sessionPath + phoneNumber + ".json", AcceptDownloads = true });
                    await newPage.GotoAsync($"https://web.telegram.org/k/#{chatsToCheck[i].id}");
                    Task.Delay(5000).Wait();
                    await newPage.ReloadAsync();
                    Task.Delay(5000).Wait();
                    await newPage.GotoAsync($"https://web.telegram.org/k/#{chatsToCheck[i].id}");
                    Chat chat = chatsToCheck[i];
                    Task.Run(() => MonitorChat(newPage, chat, postsToIgnore, textToIgnore, i));
                    Logger.LogAdd($"Monitoring on https://web.telegram.org/k/#{chatsToCheck[i].id} has started");
                }
            }
        }

        public async Task UploadPost(Chat chat, string folder, int threadId)
        {
            Logger.LogAdd($"[{threadId}] Started uploading new post");
            string content = File.ReadAllText(Path.Combine(folder, "message.txt"));

            var newPage = await browser.NewPageAsync();
            await newPage.GotoAsync($"https://shortnews.co.il/admin/remote/telegram/postnews?hash=e75b2e3999df3096da8232051883d91e");

            try
            {
                var chatOption = newPage.Locator($"xpath=//option[text()='{chat.chatName}']");
                string value = await chatOption.GetAttributeAsync("value");
                await newPage.Locator("select").SelectOptionAsync(value);
            }
            catch (Exception ex)
            {
                await newPage.CloseAsync();
                Logger.LogAdd($"[{threadId}] ERROR: {ex.Message}\nStackTrace: {ex.StackTrace}", Color.Red);
                postUploaded?.Invoke(false, DateTime.Now);
                return;
            }

            await newPage.Locator("textarea").TypeAsync(content);

            string[] files = Directory.GetFiles(folder);
            List<string> relevantFiles = new List<string>();
            foreach (string filePath in files)
            {
                var data = filePath.Split('\\');
                string fileName = data[data.Length - 1];
                if (fileName != "message.txt")
                {
                    relevantFiles.Add(filePath);
                }
            }
            if (relevantFiles.Count > 0)
            {
                await newPage.Locator("#formFileMultiple").SetInputFilesAsync(relevantFiles);
            }

            await newPage.Locator("[type='submit']").ClickAsync();

            var element = await WaitForElementAsync(newPage, ".container [role='alert']", 120000);
            await newPage.ScreenshotAsync(new PageScreenshotOptions() { FullPage = true, Path = "screenshot.png" });
            await newPage.CloseAsync();

            if (element != null)
            {
                Logger.LogAdd($"[{threadId}] New post has been uploaded", Color.Green);
                postUploaded?.Invoke(true, DateTime.Now);
            }
            else
            {
                Logger.LogAdd($"[{threadId}] Error during uploading new post", Color.Red);
                postUploaded?.Invoke(false, DateTime.Now);
            }
        }

        static async Task<IElementHandle> WaitForElementAsync(IPage currentPage, string selector, int timeoutInMilliseconds)
        {
            var timeoutTask = Task.Delay(timeoutInMilliseconds);
            var elementTask = currentPage.WaitForSelectorAsync(selector);

            await Task.WhenAny(timeoutTask, elementTask);

            return elementTask.Status == TaskStatus.RanToCompletion ? await elementTask : null;
        }

        public async Task MonitorChat(IPage currentPage, Chat chat, List<string> postsToIgnore, List<string> textToIgnore, int threadId)
        {
            string directoryPath = $"{scannedChatsPath}{chat.chatType}/{chat.chatName}";

            try
            {
                var scrollBtn = await currentPage.WaitForSelectorAsync(".active button.tgico-arrow_down", new PageWaitForSelectorOptions() { Timeout = 700 });
                await scrollBtn.ClickAsync();
                Task.Delay(2000).Wait();
            }
            catch (Exception ex)
            {
                await currentPage.EvalOnSelectorAsync(".bubbles .scrollable", "element => element.scrollTop = element.scrollHeight");
            }

            Task.Delay(1000).Wait();

            var messages = (await currentPage.QuerySelectorAllAsync(".bubbles div[data-mid]")).ToList();
            messages.Reverse();
            var lastMessage = await messages[0].GetAttributeAsync("data-timestamp");

            Logger.LogAdd($"[{threadId}] Starting the monitoring cycle..");

            while (true)
            {
                //Logger.LogAdd($"[{threadId}] New cycle has started");
                Task.Delay(5000).Wait();

                await currentPage.EvalOnSelectorAsync(".bubbles .scrollable", "element => element.scrollTop = element.scrollHeight");

                messages = (await currentPage.QuerySelectorAllAsync(".bubbles div[data-mid]")).ToList();
                messages.Reverse();

                var currentLastMessage = await messages[0].GetAttributeAsync("data-timestamp");
                //Logger.LogAdd($"[{threadId}] Checking {lastMessage} == {currentLastMessage}");
                if (lastMessage != currentLastMessage)
                {
                    Logger.LogAdd($"[{threadId}] lastMessage != currentLastMessage ({lastMessage} != {currentLastMessage})", Color.Green);
                    lastMessage = currentLastMessage;

                    #region Save
                    var message = messages[0];
                    await message.ScrollIntoViewIfNeededAsync();
                    string folder = Path.Combine(directoryPath, $"{DateTime.UtcNow.ToString(dateTimeFormat)}-{currentLastMessage}");
                    Directory.CreateDirectory(folder);

                    EventHandler<IDownload> downloader = null;
                    currentPage.Download += downloader = (sender, e) =>
                    {
                        Logger.LogAdd($"[{threadId}] Downloading of a new file has begun");
                        lock (messageCheckerLocker)
                        {
                            Logger.LogAdd($"[{threadId}] Entered messageCheckerLocker");
                            var filePath = Path.Combine($"{folder}", e.SuggestedFilename);
                            e.SaveAsAsync(filePath);
                            currentPage.Download -= downloader;

                            //page.WaitForDownloadAsync(new PageWaitForDownloadOptions { Timeout = 300000 }).Wait();
                        }
                    };

                    var voiceMessage = await message.QuerySelectorAsync("audio-element");
                    int tries = 0;
                    if (voiceMessage == null)
                    {
                        var poolElement = await message.QuerySelectorAsync("poll-element");
                        if (poolElement == null)
                        {
                            try
                            {
                                var excessiveText = message.QuerySelectorAsync(".message span").Result.TextContentAsync().Result;
                                var text = message.QuerySelectorAsync(".message").Result.TextContentAsync().Result.Replace(excessiveText, "");
                                if (ShouldIgnore(text, postsToIgnore))
                                {
                                    continue;
                                }
                                for (int q = 0; q < textToIgnore.Count; q++)
                                {
                                    text = text.Replace(textToIgnore[q], "");
                                }

                                File.WriteAllText(Path.Combine(folder, "message.txt"), text);
                            }
                            catch
                            {

                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            await voiceMessage.ScrollIntoViewIfNeededAsync();
                            await voiceMessage.ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                            await currentPage.Locator(".tgico-download").ClickAsync();
                        }
                        catch
                        {
                            continue;
                        }
                    }

                    tries = 0;
                    var attachmentElement = await message.QuerySelectorAsync(".bubble-content .attachment");
                    if (attachmentElement != null)
                    {
                        var attachments = await attachmentElement.QuerySelectorAllAsync(".grouped-item");
                        if (attachments.Count > 0)
                        {
                            for (int t = 0; t < attachments.Count; t++)
                            {
                            attachmentStart:
                                tries++;
                                try
                                {
                                    await attachments[t].ScrollIntoViewIfNeededAsync();
                                    await attachments[t].ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                    await currentPage.Locator(".tgico-download").ClickAsync();
                                }
                                catch
                                {
                                    await currentPage.Mouse.DownAsync();
                                    if (tries - t > 3)
                                    {
                                        break;
                                    }
                                    Task.Delay(1000).Wait();
                                    goto attachmentStart;
                                }
                            }
                        }
                        else
                        {
                            var videos = await attachmentElement.QuerySelectorAllAsync("video");
                            if (videos.Count > 0)
                            {
                                for (int t = 0; t < videos.Count; t++)
                                {
                                videoStart:
                                    tries++;
                                    //var boundingBox = await videos[t].BoundingBoxAsync();

                                    //var x = boundingBox.X + boundingBox.Width / 2;
                                    //var y = boundingBox.Y + boundingBox.Height / 2;

                                    //await currentPage.Mouse.MoveAsync(x, y);

                                    //await currentPage.Mouse.DownAsync(new MouseDownOptions() { Button = MouseButton.Right });
                                    try
                                    {
                                        await videos[t].ScrollIntoViewIfNeededAsync();
                                        await videos[t].ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                        await currentPage.Locator(".tgico-download").ClickAsync();
                                    }
                                    catch
                                    {
                                        await currentPage.Mouse.DownAsync();
                                        if (tries - t > 3)
                                        {
                                            break;
                                        }
                                        Task.Delay(1000).Wait();
                                        goto videoStart;
                                    }
                                }
                            }
                            else
                            {
                                var imgs = await attachmentElement.QuerySelectorAllAsync("img");
                                if (imgs.Count > 0)
                                {
                                    for (int t = 0; t < imgs.Count; t++)
                                    {
                                    imgStart:
                                        tries++;
                                        try
                                        {
                                            await imgs[t].ScrollIntoViewIfNeededAsync();
                                            await imgs[t].ClickAsync(new ElementHandleClickOptions() { Button = MouseButton.Right });
                                            await currentPage.Locator(".tgico-download").ClickAsync();
                                        }
                                        catch
                                        {
                                            await currentPage.Mouse.DownAsync();
                                            if (tries - t > 3)
                                            {
                                                break;
                                            }
                                            Task.Delay(1000).Wait();
                                            goto imgStart;
                                        }
                                    }
                                }
                                else
                                {
                                    currentPage.Download -= downloader;
                                }
                            }
                        }
                    }
                    else
                    {
                        currentPage.Download -= downloader;
                    }
                    #endregion
                    Task.Delay(500).Wait();
                    Logger.LogAdd($"[{threadId}] Waiting for the end of cycle (messageCheckerLocker)");
                    lock (messageCheckerLocker) { }
                    await UploadPost(chat, folder, threadId);
                }

            }
        }
        private bool ShouldIgnore(string text, List<string> postsToIgnore)
        {
            for (int i = 0; i < postsToIgnore.Count; i++)
            {
                if (text.Contains(postsToIgnore[i]))
                {
                    return true;
                }
            }

            return false;
        }

        public string GetLastSavedMessage(string path)
        {
            //string[] folders = Directory.GetDirectories(path);
            //List<DateTime> dateTimes = new List<DateTime>();
            //DateTime closestDateTime = DateTime.MaxValue;
            //var now = DateTime.UtcNow;

            //foreach (string folder in folders)
            //{
            //    var data = folder.Split('/')[2].Split('\\')[1];
            //    DateTime.TryParseExact(data, dateTimeFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dateTime);
            //    dateTimes.Add(dateTime);
            //}

            //foreach (DateTime dateTime in dateTimes)
            //{
            //    TimeSpan difference = dateTime - now;

            //    if (difference < TimeSpan.Zero)
            //        difference = -difference; // Calculate absolute difference

            //    if (difference < (closestDateTime - now))
            //        closestDateTime = dateTime;
            //}

            //string finalPath = $"{path}/{closestDateTime.ToString(dateTimeFormat)}";
            //if(!Directory.Exists(finalPath))
            //{
            //    return string.Empty;
            //}

            //string[] files = Directory.GetFiles(finalPath);

            //foreach (string file in files)
            //{
            //    if(file.EndsWith(".id"))
            //    {
            //        var r = file.Split('/')[3].Split('\\')[1];
            //        return r.Replace(".id", "");
            //    }
            //}

            //return string.Empty;

            List<string> ids = new List<string>();
            string[] folders = Directory.GetDirectories(path);

            foreach (string folder in folders)
            {
                var data = folder.Split('/')[2];
                string[] id = data.Split('-');
                ids.Add(id[5]);
            }

            ids.Sort();
            return ids.Count > 0 ? ids[ids.Count - 1] : "0";
        }
        public async Task ConfirmCode(string code)
        {
            await page.Locator("input[type='tel']").TypeAsync(code);
            await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
            //var c = await browser.Contexts[0].CookiesAsync();
            //cookies = c.Cast<Cookie>().ToList();
            SaveCookies();
        }

        public async Task<List<Chat>> ParseChats()
        {
            chats = new List<Chat>();
            List<string> checkedChats = new List<string>();
            int index = 0;

            while (true)
            {
                ILocator chatsElements = page.Locator(".chatlist a");
                //int chatsCount = await chats.CountAsync();

                if (index >= (await chatsElements.CountAsync()))
                {
                    break;
                }

                var chatElement = chatsElements.Nth(index);
                var id = await chatElement.GetAttributeAsync("data-peer-id");

                if (!checkedChats.Contains(id))
                {
                    index = 0;
                    checkedChats.Add(id);

                    string oldUrl = page.Url;
                    await chatElement.ClickAsync();
                    while (page.Url == oldUrl)
                    {
                        Task.Delay(100).Wait();
                    }

                    //.chat-info .peer-title
                    await page.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                    await page.WaitForLoadStateAsync(LoadState.Load);
                    await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

                    string statusText = await page.Locator(".chat-info .bottom").TextContentAsync();
                    while (string.IsNullOrEmpty(statusText))
                    {
                        Task.Delay(100).Wait();
                        statusText = await page.Locator(".chat-info .bottom").TextContentAsync();
                    }
                    if (statusText.Contains("member"))
                    {
                        Chat chat = new Chat(await page.Locator(".chat-info .peer-title").TextContentAsync(), id, ChatType.Channel);
                        chats.Add(chat);
                    }
                    else if (statusText.Contains("subscriber"))
                    {
                        Chat chat = new Chat(await page.Locator(".chat-info .peer-title").TextContentAsync(), id, ChatType.Group);
                        chats.Add(chat);
                    }

                    //await page.Locator("button.btn-menu-toggle").Last.ClickAsync();
                    //var text = (await page.Locator("button.btn-menu-toggle .danger span").TextContentAsync());
                    //if (text.ToLower().Contains("group"))
                    //{
                    //    Chat chat = new Chat(await page.Locator(".chat-info .peer-title").TextContentAsync(), id, ChatType.Group);
                    //    chats.Add(chat);
                    //}
                    //else if (text.ToLower().Contains("channel"))
                    //{
                    //    Chat chat = new Chat(await page.Locator(".chat-info .peer-title").TextContentAsync(), id, ChatType.Channel);
                    //    chats.Add(chat);
                    //}
                    //await page.Locator("button.btn-menu-toggle").Last.ClickAsync();

                    //try
                    //{
                    //var input = await page.WaitForSelectorAsync(".chat-input", new PageWaitForSelectorOptions() { Timeout = 500 });
                    //var className = await input.GetAttributeAsync("class");
                    //if (input != null && !className.Contains(" is-hidden"))
                    //{
                    //    var chatTypeElement = await page.WaitForSelectorAsync(".info span", new PageWaitForSelectorOptions() { Timeout = 500 });

                    //    if (chatTypeElement != null && !(await chatTypeElement.TextContentAsync()).Contains("bot"))
                    //    {
                    //        Chat group = new Chat(await page.Locator(".chat-info .peer-title").TextContentAsync());
                    //        chats.Add(group);
                    //    }
                    //}
                    //}
                    //catch { }
                }
                else
                {
                    index++;
                }

                //await page.Mouse.WheelAsync(0, 15000);
            }

            return chats;
        }

        public async Task Close()
        {
            if (playwright == null) return;
            await browser?.CloseAsync();
        }

        public async void SaveCookies()
        {
            string path = sessionPath + phoneNumber + ".json";
            //var cookiesJson = Newtonsoft.Json.JsonConvert.SerializeObject(cookies);
            //File.WriteAllText(path, cookiesJson);
            await page.Context.StorageStateAsync(new BrowserContextStorageStateOptions() { Path = path });
        }
        public void DeleteSave()
        {
            string path = sessionPath + phoneNumber + ".json";
            File.Delete(path);
        }
    }

}
