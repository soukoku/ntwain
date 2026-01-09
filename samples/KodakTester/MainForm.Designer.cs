namespace KodakTester
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            btnSelectScanner = new Button();
            groupSettings = new GroupBox();
            listFormat = new ComboBox();
            label7 = new Label();
            boxNamePrefix = new TextBox();
            label6 = new Label();
            btnOpenFolder = new Button();
            btnBrowseFolder = new Button();
            boxFolder = new TextBox();
            label5 = new Label();
            boxLimit = new NumericUpDown();
            label4 = new Label();
            listDpi = new ComboBox();
            label3 = new Label();
            lblCurScanner = new Label();
            groupTest = new GroupBox();
            boxLog = new RichTextBox();
            label2 = new Label();
            ckShowUI = new CheckBox();
            btnStop = new Button();
            btnTransfer = new Button();
            btnDriverOnly = new Button();
            groupSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)boxLimit).BeginInit();
            groupTest.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(10, 7);
            label1.Margin = new Padding(2, 0, 2, 0);
            label1.Name = "label1";
            label1.Size = new Size(418, 20);
            label1.TabIndex = 0;
            label1.Text = "Use this tool to test special features with Kodak ixxxx scanners";
            // 
            // btnSelectScanner
            // 
            btnSelectScanner.Location = new Point(10, 37);
            btnSelectScanner.Margin = new Padding(2, 2, 2, 2);
            btnSelectScanner.Name = "btnSelectScanner";
            btnSelectScanner.Size = new Size(190, 27);
            btnSelectScanner.TabIndex = 1;
            btnSelectScanner.Text = "Choose Scanner...";
            btnSelectScanner.UseVisualStyleBackColor = true;
            btnSelectScanner.Click += btnSelectScanner_Click;
            // 
            // groupSettings
            // 
            groupSettings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            groupSettings.Controls.Add(listFormat);
            groupSettings.Controls.Add(label7);
            groupSettings.Controls.Add(boxNamePrefix);
            groupSettings.Controls.Add(label6);
            groupSettings.Controls.Add(btnOpenFolder);
            groupSettings.Controls.Add(btnBrowseFolder);
            groupSettings.Controls.Add(boxFolder);
            groupSettings.Controls.Add(label5);
            groupSettings.Controls.Add(boxLimit);
            groupSettings.Controls.Add(label4);
            groupSettings.Controls.Add(listDpi);
            groupSettings.Controls.Add(label3);
            groupSettings.Location = new Point(10, 79);
            groupSettings.Margin = new Padding(2, 2, 2, 2);
            groupSettings.Name = "groupSettings";
            groupSettings.Padding = new Padding(2, 2, 2, 2);
            groupSettings.Size = new Size(594, 746);
            groupSettings.TabIndex = 2;
            groupSettings.TabStop = false;
            groupSettings.Text = "Settings";
            // 
            // listFormat
            // 
            listFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            listFormat.FormattingEnabled = true;
            listFormat.Location = new Point(126, 194);
            listFormat.Margin = new Padding(2, 2, 2, 2);
            listFormat.Name = "listFormat";
            listFormat.Size = new Size(146, 28);
            listFormat.TabIndex = 10;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(38, 197);
            label7.Margin = new Padding(2, 0, 2, 0);
            label7.Name = "label7";
            label7.Size = new Size(83, 20);
            label7.TabIndex = 9;
            label7.Text = "File Format";
            // 
            // boxNamePrefix
            // 
            boxNamePrefix.Location = new Point(126, 166);
            boxNamePrefix.Margin = new Padding(2, 2, 2, 2);
            boxNamePrefix.Name = "boxNamePrefix";
            boxNamePrefix.Size = new Size(146, 27);
            boxNamePrefix.TabIndex = 8;
            boxNamePrefix.Text = "Capture_";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(10, 168);
            label6.Margin = new Padding(2, 0, 2, 0);
            label6.Name = "label6";
            label6.Size = new Size(117, 20);
            label6.TabIndex = 7;
            label6.Text = "File Name Prefix";
            // 
            // btnOpenFolder
            // 
            btnOpenFolder.Location = new Point(220, 134);
            btnOpenFolder.Margin = new Padding(2, 2, 2, 2);
            btnOpenFolder.Name = "btnOpenFolder";
            btnOpenFolder.Size = new Size(90, 27);
            btnOpenFolder.TabIndex = 6;
            btnOpenFolder.Text = "Open";
            btnOpenFolder.UseVisualStyleBackColor = true;
            btnOpenFolder.Click += btnOpenFolder_Click;
            // 
            // btnBrowseFolder
            // 
            btnBrowseFolder.Location = new Point(126, 134);
            btnBrowseFolder.Margin = new Padding(2, 2, 2, 2);
            btnBrowseFolder.Name = "btnBrowseFolder";
            btnBrowseFolder.Size = new Size(90, 27);
            btnBrowseFolder.TabIndex = 6;
            btnBrowseFolder.Text = "Browse...";
            btnBrowseFolder.UseVisualStyleBackColor = true;
            btnBrowseFolder.Click += btnBrowseFolder_Click;
            // 
            // boxFolder
            // 
            boxFolder.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            boxFolder.Location = new Point(126, 104);
            boxFolder.Margin = new Padding(2, 2, 2, 2);
            boxFolder.Name = "boxFolder";
            boxFolder.Size = new Size(446, 27);
            boxFolder.TabIndex = 5;
            boxFolder.Text = "Images";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(38, 106);
            label5.Margin = new Padding(2, 0, 2, 0);
            label5.Name = "label5";
            label5.Size = new Size(86, 20);
            label5.TabIndex = 4;
            label5.Text = "Save Folder";
            // 
            // boxLimit
            // 
            boxLimit.Location = new Point(126, 74);
            boxLimit.Margin = new Padding(2, 2, 2, 2);
            boxLimit.Minimum = new decimal(new int[] { 1, 0, 0, int.MinValue });
            boxLimit.Name = "boxLimit";
            boxLimit.Size = new Size(146, 27);
            boxLimit.TabIndex = 3;
            boxLimit.Value = new decimal(new int[] { 1, 0, 0, int.MinValue });
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(28, 76);
            label4.Margin = new Padding(2, 0, 2, 0);
            label4.Name = "label4";
            label4.Size = new Size(98, 20);
            label4.TabIndex = 2;
            label4.Text = "Transfer Limit";
            // 
            // listDpi
            // 
            listDpi.DropDownStyle = ComboBoxStyle.DropDownList;
            listDpi.FormattingEnabled = true;
            listDpi.Location = new Point(126, 43);
            listDpi.Margin = new Padding(2, 2, 2, 2);
            listDpi.Name = "listDpi";
            listDpi.Size = new Size(146, 28);
            listDpi.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(89, 46);
            label3.Margin = new Padding(2, 0, 2, 0);
            label3.Name = "label3";
            label3.Size = new Size(32, 20);
            label3.TabIndex = 0;
            label3.Text = "DPI";
            // 
            // lblCurScanner
            // 
            lblCurScanner.AutoSize = true;
            lblCurScanner.Location = new Point(205, 41);
            lblCurScanner.Margin = new Padding(2, 0, 2, 0);
            lblCurScanner.Name = "lblCurScanner";
            lblCurScanner.Size = new Size(104, 20);
            lblCurScanner.TabIndex = 3;
            lblCurScanner.Text = "None selected";
            // 
            // groupTest
            // 
            groupTest.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupTest.Controls.Add(boxLog);
            groupTest.Controls.Add(label2);
            groupTest.Controls.Add(ckShowUI);
            groupTest.Controls.Add(btnStop);
            groupTest.Controls.Add(btnTransfer);
            groupTest.Controls.Add(btnDriverOnly);
            groupTest.Location = new Point(608, 79);
            groupTest.Margin = new Padding(2, 2, 2, 2);
            groupTest.Name = "groupTest";
            groupTest.Padding = new Padding(2, 2, 2, 2);
            groupTest.Size = new Size(830, 746);
            groupTest.TabIndex = 4;
            groupTest.TabStop = false;
            groupTest.Text = "Test";
            // 
            // boxLog
            // 
            boxLog.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            boxLog.Location = new Point(18, 94);
            boxLog.Margin = new Padding(2, 2, 2, 2);
            boxLog.Name = "boxLog";
            boxLog.Size = new Size(800, 638);
            boxLog.TabIndex = 4;
            boxLog.Text = "";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(18, 72);
            label2.Margin = new Padding(2, 0, 2, 0);
            label2.Name = "label2";
            label2.Size = new Size(40, 20);
            label2.TabIndex = 3;
            label2.Text = "Logs";
            // 
            // ckShowUI
            // 
            ckShowUI.AutoSize = true;
            ckShowUI.Location = new Point(361, 34);
            ckShowUI.Margin = new Padding(2, 2, 2, 2);
            ckShowUI.Name = "ckShowUI";
            ckShowUI.Size = new Size(210, 24);
            ckShowUI.TabIndex = 2;
            ckShowUI.Text = "Show driver during capture";
            ckShowUI.UseVisualStyleBackColor = true;
            // 
            // btnStop
            // 
            btnStop.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnStop.Location = new Point(718, 31);
            btnStop.Margin = new Padding(2, 2, 2, 2);
            btnStop.Name = "btnStop";
            btnStop.Size = new Size(99, 27);
            btnStop.TabIndex = 2;
            btnStop.Text = "Stop";
            btnStop.UseVisualStyleBackColor = true;
            btnStop.Click += btnStop_Click;
            // 
            // btnTransfer
            // 
            btnTransfer.Location = new Point(191, 31);
            btnTransfer.Margin = new Padding(2, 2, 2, 2);
            btnTransfer.Name = "btnTransfer";
            btnTransfer.Size = new Size(165, 27);
            btnTransfer.TabIndex = 1;
            btnTransfer.Text = "Start Capture";
            btnTransfer.UseVisualStyleBackColor = true;
            btnTransfer.Click += btnTransfer_Click;
            // 
            // btnDriverOnly
            // 
            btnDriverOnly.Location = new Point(18, 31);
            btnDriverOnly.Margin = new Padding(2, 2, 2, 2);
            btnDriverOnly.Name = "btnDriverOnly";
            btnDriverOnly.Size = new Size(165, 27);
            btnDriverOnly.TabIndex = 0;
            btnDriverOnly.Text = "Open Driver UI";
            btnDriverOnly.UseVisualStyleBackColor = true;
            btnDriverOnly.Click += btnDriverOnly_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1448, 834);
            Controls.Add(groupTest);
            Controls.Add(lblCurScanner);
            Controls.Add(groupSettings);
            Controls.Add(btnSelectScanner);
            Controls.Add(label1);
            Margin = new Padding(2, 2, 2, 2);
            Name = "MainForm";
            SizeGripStyle = SizeGripStyle.Show;
            Text = "Kodak Tester Utility";
            groupSettings.ResumeLayout(false);
            groupSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)boxLimit).EndInit();
            groupTest.ResumeLayout(false);
            groupTest.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Button btnSelectScanner;
        private GroupBox groupSettings;
        private Label lblCurScanner;
        private GroupBox groupTest;
        private CheckBox ckShowUI;
        private Button btnTransfer;
        private Button btnDriverOnly;
        private RichTextBox boxLog;
        private Label label2;
        private Button btnStop;
        private ComboBox listDpi;
        private Label label3;
        private Label label4;
        private NumericUpDown boxLimit;
        private Label label5;
        private Button btnOpenFolder;
        private Button btnBrowseFolder;
        private TextBox boxFolder;
        private TextBox boxNamePrefix;
        private Label label6;
        private ComboBox listFormat;
        private Label label7;
    }
}
