using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using FirebirdSql.Data.FirebirdClient;
using Microsoft.Playwright;
using Newtonsoft.Json;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace TelegramWebAutoAuth
{
    public partial class Form1 : Form
    {
        public BrowserManager manager;
        public string sessionPath = "Sessions/";
        public string scannedChatsPath = "ScannedChannelsAndGroups/";
        public SaveData savedData = new SaveData();
        private readonly object writeToFileLockObject = new object();
        List<string> userFindList = new List<string>();

        public Form1()
        {
            InitializeComponent();
            panel1.Visible = true;
            panel2.Visible = true;

            //logBox.HideSelection = false;
            //Logger.logBox = logBox;

            var exitCode = Microsoft.Playwright.Program.Main(new[] { "install" });
            if (exitCode != 0)
            {
                throw new Exception($"Playwright exited with code {exitCode}");
            }

            if (!Directory.Exists(sessionPath))
            {
                Directory.CreateDirectory(sessionPath);
            }
            if (!Directory.Exists(scannedChatsPath))
            {
                Directory.CreateDirectory(scannedChatsPath);
            }
            if (!Directory.Exists(scannedChatsPath + "Group/"))
            {
                Directory.CreateDirectory(scannedChatsPath + "Group/");
            }
            if (!Directory.Exists(scannedChatsPath + "Channel/"))
            {
                Directory.CreateDirectory(scannedChatsPath + "Channel/");
            }

            if (File.Exists("savedData.json"))
            {
                savedData = JsonConvert.DeserializeObject<SaveData>(File.ReadAllText("savedData.json"));
            }

            manager = new BrowserManager(sessionPath, scannedChatsPath);
            manager.newSessionCreated += OnNewSessionCreated;
            manager.ParseSessions();

            if (savedData.isWorking)
            {
                manager.ChooseBrowser(savedData.currentBrowser);
                UpdateChatsList(manager.currentBrowser.chats);
                StartMonitoring(savedData.chatsToCheck, savedData.postsToIgnore, savedData.textToIgnore);
            }
        }

        private void OnNewSessionCreated(object sender, Browser b)
        {
            if (!comboBox1.Items.Contains(b.phoneNumber))
            {
                comboBox1.Items.Add(b.phoneNumber);
            }
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(phoneNumberTextBox.Text))
            {
                //chatList.Rows.Clear();
                await manager.CloseBrowser();
                manager.AddBrowser(phoneNumberTextBox.Text);
                await manager.Start();
                await manager.EnterPhoneNumber(codeTextBox.Text);
            }
            else
            {
                await manager.Start();
            }
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            await manager.ConfirmCode(codeTextBox.Text);
        }

        private async void button3_Click(object sender, EventArgs e)
        {
            await manager.CloseBrowser();
            manager.DeleteSave();
            savedData.isWorking = false;
            comboBox1.Items.Remove(comboBox1.SelectedItem);
            //chatList.Rows.Clear();
            SaveData();
        }

        private async void button4_Click(object sender, EventArgs e)
        {
            var chats = await manager.ParseChats();
            UpdateChatsList(chats);
        }
        public void UpdateChatsList(List<Chat> chats)
        {
            //chatList.Rows.Clear();
            // for (int i = 0; i < chats.Count; i++)
            //  {
            //      chatList.Rows.Add($"{chats[i].chatName}{Environment.NewLine}", false);
            //   }
        }

        //public List<Chat> FindCheckedChats(List<Chat> allChats)
        //{
        //    //List<string> chatTitles = new List<string>();
        //    //foreach (DataGridViewRow row in chatList.Rows)
        //    //{
        //    //    DataGridViewCheckBoxCell checkBoxCell = row.Cells[1] as DataGridViewCheckBoxCell;
        //    //    if (Convert.ToBoolean(checkBoxCell?.Value) == true)
        //    //    {
        //    //        chatTitles.Add(Convert.ToString(row.Cells[0].Value).Replace("\r", "").Replace("\n", ""));
        //    //    }
        //    //}

        //    //return allChats.FindAll(x => chatTitles.Contains(x.chatName));
        //}

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            manager.ChooseBrowser(comboBox1.Text);
            //UpdateChatsList(manager.currentBrowser.chats);
        }

        //private async void button5_Click(object sender, EventArgs e)
        //{
        //   // List<Chat> chatsToCheck = FindCheckedChats(manager.currentBrowser.chats);
        //    List<string> postsToIgnore = GetPostsToIgnore().ToList();
        //    List<string> textToIgnore = GetTextToIgnore().ToList();
        //    //await manager.CheckChats(chatsToCheck, postsToIgnore, textToIgnore);
        //}
        //public IEnumerable<string> GetPostsToIgnore()
        //{
        //    return ignorePostsTextBox.Lines;
        //}
        //public IEnumerable<string> GetTextToIgnore()
        //{
        //    return ignoreTextTextBox.Lines;
        //}

        //private void button6_Click(object sender, EventArgs e)
        //{
        //    List<Chat> chatsToCheck = FindCheckedChats(manager.currentBrowser.chats);
        //    List<string> postsToIgnore = GetPostsToIgnore().ToList();
        //    List<string> textToIgnore = GetTextToIgnore().ToList();

        //    StartMonitoring(chatsToCheck, postsToIgnore, textToIgnore);

        //    savedData.chatsToCheck = chatsToCheck;
        //    savedData.postsToIgnore = postsToIgnore;
        //    savedData.textToIgnore = textToIgnore;
        //    savedData.currentBrowser = manager.currentBrowser.phoneNumber;
        //    savedData.isWorking = true;

        //    SaveData();
        //}

        public void StartMonitoring(List<Chat> chatsToCheck, List<string> postsToIgnore, List<string> textToIgnore)
        {
            // manager.currentBrowser.postUploaded += OnPostUploaded;
            manager.currentBrowser.StartMonitoringChats(chatsToCheck, postsToIgnore, textToIgnore);
        }

        public void SaveData()
        {
            string text = JsonConvert.SerializeObject(savedData);
            File.WriteAllText("savedData.json", text);
        }

        public void userCheckBrowseBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the selected text file and display its content in the TextBox
                        string filePath = openFileDialog.FileName;
                        string fileContent = File.ReadAllText(filePath);
                        userCheckTextBox.Text = filePath;

                        userFindList = ReadFileAndCountRows(filePath);

                        // Update the label with the number of selected files
                        totalUserFileLbl.Text = $"Stats : Total Users In The File : {userFindList.Count} ";
                        userCheckLbl.Text = $"Stats : Total Users To Check : {userFindList.Count} ";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private static List<string> ReadFileAndCountRows(string filePath)
        {
            List<string> rows = new List<string>();

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    while (!reader.EndOfStream)
                    {
                        string row = reader.ReadLine();
                        rows.Add(row);
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine($"An error occurred: {ex.Message}");
            }

            return rows;
        }

        public void ConnectToDataBase()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyFirebirdConnection"].ConnectionString;

            using (FbConnection connection = new FbConnection(connectionString))
            {
                connection.Open();

                using (FbCommand command = new FbCommand("SELECT ID, Username FROM Usernames", connection))
                {
                    using (FbDataAdapter adapter = new FbDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        foreach (DataRow row in dataTable.Rows)
                        {
                            int id = Convert.ToInt32(row["ID"]);
                            string username = row["Username"].ToString();
                            Console.WriteLine($"ID: {id}, Username: {username}");
                        }
                    }
                }
            }
        }

        public void InsertDataInDataBase(List<string> userNames)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyFirebirdConnection"].ConnectionString;

            using (FbConnection connection = new FbConnection(connectionString))
            {
                connection.Open();

                foreach (string username in userNames)
                {
                    using (FbTransaction transaction = connection.BeginTransaction())
                    {
                        using (FbCommand selectCommand = new FbCommand("SELECT ID FROM Usernames WHERE Username = @Username", connection, transaction))
                        {
                            selectCommand.Parameters.AddWithValue("@Username", username);

                            object existingId = selectCommand.ExecuteScalar();

                            if (existingId == null)
                            {
                                using (FbCommand insertCommand = new FbCommand("INSERT INTO Usernames (Username) VALUES (@Username) RETURNING ID", connection, transaction))
                                {
                                    insertCommand.Parameters.AddWithValue("@Username", username);

                                    try
                                    {
                                        int id = Convert.ToInt32(insertCommand.ExecuteScalar());
                                        transaction.Commit();
                                    }
                                    catch (Exception ex)
                                    {
                                        transaction.Rollback();
                                    }
                                }
                            }
                            else
                            {
                                transaction.Rollback();
                            }
                        }
                    }
                }
            }
        }

        public async void startBtn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(userCheckTextBox.Text))
            {
                MessageBox.Show("Please select a users to check txt file before processing");
                return;
            }

            if (string.IsNullOrEmpty(saveSkypeTextBox.Text))
            {
                MessageBox.Show("Please select a save skype accounts text file before processing");
                return;
            }
            // Declare the lock object at the beginning of your class


            //try
            //{
            //    int userNameProcessedCount = 0;
            //    int userNameFound = 0;
            //    // Read all lines from the file
            //    string[] lines = File.ReadAllLines(userCheckTextBox.Text);

            //    // Display each line
            //    foreach (string line in lines)
            //    {
            //        var userNameList = await manager.currentBrowser.SeachContact(line.Trim());
            //        userNameFound = userNameFound + userNameList.Count;
            //        WriteLinesToFile(saveSkypeTextBox.Text, userNameList);
            //        usersGrabbedLbl.Text = $"Stats : Total Skype Grabbed: {userNameFound} ";
            //        userNameProcessedCount++;
            //        usersCheckedLbl.Text = $"Stats : Total Users Checked: {userNameProcessedCount} ";  
            //    }
            //}
            //catch (Exception)
            //{
            //}

            try
            {
                var stopWatch = new Stopwatch();
                stopWatch.Start();

                int userNameProcessedCount = 0;
                int userNameFound = 0;
                int maxConcurrentThreads = 10; // You can adjust this number

                int threadCount = 3; /*noOfThreadsTextBox.Text != string.Empty ? Convert.ToInt32(noOfThreadsTextBox.Text) : 0;*/

                string[] lines = File.ReadAllLines(userCheckTextBox.Text);

                HashSet<string> uniqueCombinations = new HashSet<string>();




                foreach (string line in userFindList)
                {
                    var combinations = new List<string>();

                    for (int length = 1; length <= line.Length; length++)
                    {
                        string substring = line.Substring(0, length);
                        combinations.Add(substring);
                    }

                    foreach (string combination in combinations)
                    {
                        if (!string.IsNullOrEmpty(combination))
                        {
                            uniqueCombinations.Add(combination);
                        }
                    }
                }

                userCheckLbl.Text = $"Stats : Total Users To Check : {uniqueCombinations.Count} ";
                stopWatch.Stop();

                int linesPerThread = uniqueCombinations.Count / threadCount;

                var email = comboBox1.Text;

                List<string> combinationsList = new List<string>(uniqueCombinations);

                List<Task> tasks = new List<Task>();

                for (int i = 0; i < threadCount; i++)
                {
                    int startIndex = i * linesPerThread;
                    int endIndex = (i == threadCount - 1) ? lines.Length : (i + 1) * linesPerThread;

                    var task = Task.Run(async () =>
                    {
                        var playwrightOutLook = await Playwright.CreateAsync();

                        var browserSkype = await playwrightOutLook.Chromium.LaunchAsync(new BrowserTypeLaunchOptions()
                        {
                            Headless = false
                        });

                        IPage pageSkype;

                        if (File.Exists(sessionPath + email + ".json"))
                        {
                            pageSkype = await browserSkype.NewPageAsync(new BrowserNewPageOptions() { StorageStatePath = sessionPath + email + ".json", AcceptDownloads = true });
                        }
                        else
                        {
                            pageSkype = await browserSkype.NewPageAsync();
                        }


                        await pageSkype.GotoAsync("https://web.skype.com/?openPstnPage=true");
                        await pageSkype.WaitForLoadStateAsync(LoadState.DOMContentLoaded);
                        await pageSkype.WaitForLoadStateAsync(LoadState.Load);
                        await pageSkype.WaitForLoadStateAsync(LoadState.NetworkIdle);

                        await Task.Delay(5000);

                        try
                        {
                            // Find the element by attribute
                            var element = await pageSkype.QuerySelectorAsync("[data-text-as-pseudo-element='Got it!']");

                            if (element != null)
                            {
                                await element.ClickAsync();
                            }
                        }
                        catch (Exception)
                        {

                        }

                        for (int j = startIndex; j < endIndex; j++)
                        {
                            string line = combinationsList[j];
                            var listOfContact = await SeachContact(line.Trim(), pageSkype);

                            // Update UI from the main thread
                            this.Invoke(new Action(() =>
                            {
                                WriteLinesToFile(saveSkypeTextBox.Text, listOfContact);

                                InsertDataInDataBase(listOfContact);

                                userNameProcessedCount++;
                                userNameFound = userNameFound + listOfContact.Count;

                                UpdateUI(userNameProcessedCount, userNameFound);
                            }));
                        }

                        await pageSkype.CloseAsync();
                        await browserSkype.CloseAsync();
                    });

                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }

        }

        public async Task<List<string>> SeachContact(string contact, IPage page)
        {
            try
            {


                await Task.Delay(1000);
                // Find the element by attribute
                int i = 0;
                var elementGotIt = await page.QuerySelectorAsync("[data-text-as-pseudo-element='Got it!']");
                while (elementGotIt == null && i > 3)
                {
                    elementGotIt = await page.QuerySelectorAsync("[data-text-as-pseudo-element='Got it!']");
                    await Task.Delay(1000);
                    i++;
                }

                if (elementGotIt != null)
                {
                    await elementGotIt.ClickAsync();
                }
            }
            catch (Exception)
            {

            }


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


        private void UpdateUI(int userNameProcessedCount, int userNameFound)
        {
            // This method should be implemented based on your UI framework
            // Here's a basic example using WinForms:

            // Check if the method is being called from a non-UI thread
            if (InvokeRequired)
            {
                // Call the method on the UI thread
                BeginInvoke(new Action(() => UpdateUI(userNameFound, userNameProcessedCount)));
                return;
            }

            // Update your UI elements with the latest statistics
            usersGrabbedLbl.Text = $"Stats : Total Skype Grabbed: {userNameFound}";
            usersCheckedLbl.Text = $"Stats : Total Users Checked: {userNameProcessedCount}";
        }

        private void WriteLinesToFile(string filePath, List<string> usersList)
        {
            try
            {
                lock (writeToFileLockObject)
                {
                    // Write each string with a line break to the file
                    using (StreamWriter writer = File.AppendText(filePath))
                    {
                        foreach (string item in usersList)
                        {
                            writer.WriteLine(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions
            }
        }

        private void saveSkypeBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the selected text file and display its content in the TextBox
                        string filePath = openFileDialog.FileName;
                        string fileContent = File.ReadAllText(filePath);
                        saveSkypeTextBox.Text = filePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private async void button2_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(groupNameTextBox.Text))
            {
                MessageBox.Show("Group Name Cannot Be Empty.");
                return;

            }

            if (string.IsNullOrEmpty(skypeUserList.Text))
            {
                MessageBox.Show("Skype User List Cannot Be Empty.");
                return;

            }

            //Create group
            List<string> members = new List<string>();
            // Read all lines from the file
            string[] lines = File.ReadAllLines(skypeUserList.Text);

            members = lines.ToList();


            await manager.currentBrowser.CreateGroupAndAddMembers(groupNameTextBox.Text, members);
        }

        private void skypeUserBrowse_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // Read the selected text file and display its content in the TextBox
                        string filePath = openFileDialog.FileName;
                        string fileContent = File.ReadAllText(filePath);
                        skypeUserList.Text = filePath;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error reading the file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void TestPanel_Click(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = false;

            panel1.Visible = true;
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            panel2.Visible = true;
            panel1.Visible = true;

            panel2.Visible = true;
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        //private void OnPostUploaded(bool isSuccessful, DateTime time)
        //{
        //    statsLabel.Text = $"Last posted: {time} | Success = {isSuccessful}";
        //}
    }
}
