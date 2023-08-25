
namespace TelegramWebAutoAuth
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.phoneNumberTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.codeTextBox = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.userCheckTextBox = new System.Windows.Forms.TextBox();
            this.totalUserFileLbl = new System.Windows.Forms.Label();
            this.userCheckBrowseBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.saveSkypeTextBox = new System.Windows.Forms.TextBox();
            this.saveSkypeBtn = new System.Windows.Forms.Button();
            this.userCheckLbl = new System.Windows.Forms.Label();
            this.usersCheckedLbl = new System.Windows.Forms.Label();
            this.usersGrabbedLbl = new System.Windows.Forms.Label();
            this.startBtn = new System.Windows.Forms.Button();
            this.groupNameTextBox = new System.Windows.Forms.TextBox();
            this.labelGrpName = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.skypeUserList = new System.Windows.Forms.TextBox();
            this.skypeUserBrowse = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // phoneNumberTextBox
            // 
            this.phoneNumberTextBox.Location = new System.Drawing.Point(65, 12);
            this.phoneNumberTextBox.Name = "phoneNumberTextBox";
            this.phoneNumberTextBox.Size = new System.Drawing.Size(264, 20);
            this.phoneNumberTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Email";
            // 
            // codeTextBox
            // 
            this.codeTextBox.Location = new System.Drawing.Point(65, 65);
            this.codeTextBox.Name = "codeTextBox";
            this.codeTextBox.Size = new System.Drawing.Size(264, 20);
            this.codeTextBox.TabIndex = 2;
            this.codeTextBox.UseSystemPasswordChar = true;
            // 
            // Password
            // 
            this.Password.AutoSize = true;
            this.Password.Location = new System.Drawing.Point(3, 68);
            this.Password.Name = "Password";
            this.Password.Size = new System.Drawing.Size(56, 13);
            this.Password.TabIndex = 3;
            this.Password.Text = "Password:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(146, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Login";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(254, 103);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 6;
            this.button3.Text = "Logout";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(65, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(264, 21);
            this.comboBox1.TabIndex = 10;
            this.comboBox1.SelectedValueChanged += new System.EventHandler(this.comboBox1_SelectedValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "User:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(371, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Users To Check";
            // 
            // userCheckTextBox
            // 
            this.userCheckTextBox.Location = new System.Drawing.Point(490, 12);
            this.userCheckTextBox.Name = "userCheckTextBox";
            this.userCheckTextBox.Size = new System.Drawing.Size(264, 20);
            this.userCheckTextBox.TabIndex = 13;
            // 
            // totalUserFileLbl
            // 
            this.totalUserFileLbl.AutoSize = true;
            this.totalUserFileLbl.Location = new System.Drawing.Point(487, 46);
            this.totalUserFileLbl.Name = "totalUserFileLbl";
            this.totalUserFileLbl.Size = new System.Drawing.Size(156, 13);
            this.totalUserFileLbl.TabIndex = 14;
            this.totalUserFileLbl.Text = "Stats : Total Users In The File : ";
            // 
            // userCheckBrowseBtn
            // 
            this.userCheckBrowseBtn.Location = new System.Drawing.Point(776, 10);
            this.userCheckBrowseBtn.Name = "userCheckBrowseBtn";
            this.userCheckBrowseBtn.Size = new System.Drawing.Size(75, 23);
            this.userCheckBrowseBtn.TabIndex = 15;
            this.userCheckBrowseBtn.Text = "Browse";
            this.userCheckBrowseBtn.UseMnemonic = false;
            this.userCheckBrowseBtn.UseVisualStyleBackColor = true;
            this.userCheckBrowseBtn.Click += new System.EventHandler(this.userCheckBrowseBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(371, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Save Skype Accounts";
            // 
            // saveSkypeTextBox
            // 
            this.saveSkypeTextBox.Location = new System.Drawing.Point(490, 69);
            this.saveSkypeTextBox.Name = "saveSkypeTextBox";
            this.saveSkypeTextBox.Size = new System.Drawing.Size(264, 20);
            this.saveSkypeTextBox.TabIndex = 17;
            // 
            // saveSkypeBtn
            // 
            this.saveSkypeBtn.Location = new System.Drawing.Point(776, 67);
            this.saveSkypeBtn.Name = "saveSkypeBtn";
            this.saveSkypeBtn.Size = new System.Drawing.Size(75, 23);
            this.saveSkypeBtn.TabIndex = 18;
            this.saveSkypeBtn.Text = "Browse";
            this.saveSkypeBtn.UseMnemonic = false;
            this.saveSkypeBtn.UseVisualStyleBackColor = true;
            this.saveSkypeBtn.Click += new System.EventHandler(this.saveSkypeBtn_Click);
            // 
            // userCheckLbl
            // 
            this.userCheckLbl.AutoSize = true;
            this.userCheckLbl.Location = new System.Drawing.Point(371, 113);
            this.userCheckLbl.Name = "userCheckLbl";
            this.userCheckLbl.Size = new System.Drawing.Size(150, 13);
            this.userCheckLbl.TabIndex = 19;
            this.userCheckLbl.Text = "Stats : Total Users To Check :";
            // 
            // usersCheckedLbl
            // 
            this.usersCheckedLbl.AutoSize = true;
            this.usersCheckedLbl.Location = new System.Drawing.Point(370, 160);
            this.usersCheckedLbl.Name = "usersCheckedLbl";
            this.usersCheckedLbl.Size = new System.Drawing.Size(140, 13);
            this.usersCheckedLbl.TabIndex = 20;
            this.usersCheckedLbl.Text = "Stats : Total Users Checked";
            // 
            // usersGrabbedLbl
            // 
            this.usersGrabbedLbl.AutoSize = true;
            this.usersGrabbedLbl.Location = new System.Drawing.Point(607, 160);
            this.usersGrabbedLbl.Name = "usersGrabbedLbl";
            this.usersGrabbedLbl.Size = new System.Drawing.Size(147, 13);
            this.usersGrabbedLbl.TabIndex = 21;
            this.usersGrabbedLbl.Text = "Stats : Total Skype Grabbed :";
            // 
            // startBtn
            // 
            this.startBtn.Location = new System.Drawing.Point(891, 32);
            this.startBtn.Name = "startBtn";
            this.startBtn.Size = new System.Drawing.Size(107, 40);
            this.startBtn.TabIndex = 22;
            this.startBtn.Text = "Start";
            this.startBtn.UseMnemonic = false;
            this.startBtn.UseVisualStyleBackColor = true;
            this.startBtn.Click += new System.EventHandler(this.startBtn_Click);
            // 
            // groupNameTextBox
            // 
            this.groupNameTextBox.Location = new System.Drawing.Point(490, 189);
            this.groupNameTextBox.Name = "groupNameTextBox";
            this.groupNameTextBox.Size = new System.Drawing.Size(264, 20);
            this.groupNameTextBox.TabIndex = 23;
            // 
            // labelGrpName
            // 
            this.labelGrpName.AutoSize = true;
            this.labelGrpName.Location = new System.Drawing.Point(371, 196);
            this.labelGrpName.Name = "labelGrpName";
            this.labelGrpName.Size = new System.Drawing.Size(67, 13);
            this.labelGrpName.TabIndex = 24;
            this.labelGrpName.Text = "Group Name";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(373, 272);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(381, 42);
            this.button2.TabIndex = 25;
            this.button2.Text = "Create Group";
            this.button2.UseMnemonic = false;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(371, 236);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(114, 13);
            this.label3.TabIndex = 26;
            this.label3.Text = "Select Skype User List";
            // 
            // skypeUserList
            // 
            this.skypeUserList.Location = new System.Drawing.Point(490, 229);
            this.skypeUserList.Name = "skypeUserList";
            this.skypeUserList.Size = new System.Drawing.Size(264, 20);
            this.skypeUserList.TabIndex = 27;
            // 
            // skypeUserBrowse
            // 
            this.skypeUserBrowse.Location = new System.Drawing.Point(776, 226);
            this.skypeUserBrowse.Name = "skypeUserBrowse";
            this.skypeUserBrowse.Size = new System.Drawing.Size(75, 23);
            this.skypeUserBrowse.TabIndex = 28;
            this.skypeUserBrowse.Text = "Browse";
            this.skypeUserBrowse.UseMnemonic = false;
            this.skypeUserBrowse.UseVisualStyleBackColor = true;
            this.skypeUserBrowse.Click += new System.EventHandler(this.skypeUserBrowse_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1136, 632);
            this.Controls.Add(this.skypeUserBrowse);
            this.Controls.Add(this.skypeUserList);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.labelGrpName);
            this.Controls.Add(this.groupNameTextBox);
            this.Controls.Add(this.startBtn);
            this.Controls.Add(this.usersGrabbedLbl);
            this.Controls.Add(this.usersCheckedLbl);
            this.Controls.Add(this.userCheckLbl);
            this.Controls.Add(this.saveSkypeBtn);
            this.Controls.Add(this.saveSkypeTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.userCheckBrowseBtn);
            this.Controls.Add(this.totalUserFileLbl);
            this.Controls.Add(this.userCheckTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.codeTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.phoneNumberTextBox);
            this.Name = "Form1";
            this.Text = "Skype Auto Auth";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox phoneNumberTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox codeTextBox;
        private System.Windows.Forms.Label Password;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox userCheckTextBox;
        private System.Windows.Forms.Label totalUserFileLbl;
        private System.Windows.Forms.Button userCheckBrowseBtn;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox saveSkypeTextBox;
        private System.Windows.Forms.Button saveSkypeBtn;
        private System.Windows.Forms.Label userCheckLbl;
        private System.Windows.Forms.Label usersCheckedLbl;
        private System.Windows.Forms.Label usersGrabbedLbl;
        private System.Windows.Forms.Button startBtn;
        private System.Windows.Forms.TextBox groupNameTextBox;
        private System.Windows.Forms.Label labelGrpName;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox skypeUserList;
        private System.Windows.Forms.Button skypeUserBrowse;
    }
}

