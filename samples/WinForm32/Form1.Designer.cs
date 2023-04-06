namespace WinFormSample
{
  partial class Form1
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
      btnSelect = new System.Windows.Forms.Button();
      label1 = new System.Windows.Forms.Label();
      lblDefault = new System.Windows.Forms.Label();
      label2 = new System.Windows.Forms.Label();
      lblCurrent = new System.Windows.Forms.Label();
      btnEnumSources = new System.Windows.Forms.Button();
      listSources = new System.Windows.Forms.ListBox();
      btnSetDef = new System.Windows.Forms.Button();
      btnOpen = new System.Windows.Forms.Button();
      splitContainer1 = new System.Windows.Forms.SplitContainer();
      lblState = new System.Windows.Forms.Label();
      label3 = new System.Windows.Forms.Label();
      btnOpenDef = new System.Windows.Forms.Button();
      listCaps = new System.Windows.Forms.ListBox();
      label4 = new System.Windows.Forms.Label();
      btnStart = new System.Windows.Forms.Button();
      btnShowSettings = new System.Windows.Forms.Button();
      btnClose = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
      splitContainer1.Panel1.SuspendLayout();
      splitContainer1.Panel2.SuspendLayout();
      splitContainer1.SuspendLayout();
      SuspendLayout();
      // 
      // btnSelect
      // 
      btnSelect.Location = new System.Drawing.Point(12, 58);
      btnSelect.Name = "btnSelect";
      btnSelect.Size = new System.Drawing.Size(151, 23);
      btnSelect.TabIndex = 0;
      btnSelect.Text = "Choose default source";
      btnSelect.UseVisualStyleBackColor = true;
      btnSelect.Click += btnSelect_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(13, 36);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(87, 15);
      label1.TabIndex = 1;
      label1.Text = "Default Source:";
      // 
      // lblDefault
      // 
      lblDefault.AutoSize = true;
      lblDefault.Location = new System.Drawing.Point(106, 36);
      lblDefault.Name = "lblDefault";
      lblDefault.Size = new System.Drawing.Size(24, 15);
      lblDefault.TabIndex = 2;
      lblDefault.Text = "NA";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(13, 14);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(89, 15);
      label2.TabIndex = 3;
      label2.Text = "Current Source:";
      // 
      // lblCurrent
      // 
      lblCurrent.AutoSize = true;
      lblCurrent.Location = new System.Drawing.Point(108, 14);
      lblCurrent.Name = "lblCurrent";
      lblCurrent.Size = new System.Drawing.Size(24, 15);
      lblCurrent.TabIndex = 4;
      lblCurrent.Text = "NA";
      // 
      // btnEnumSources
      // 
      btnEnumSources.Location = new System.Drawing.Point(13, 87);
      btnEnumSources.Name = "btnEnumSources";
      btnEnumSources.Size = new System.Drawing.Size(151, 23);
      btnEnumSources.TabIndex = 5;
      btnEnumSources.Text = "Enumerate sources";
      btnEnumSources.UseVisualStyleBackColor = true;
      btnEnumSources.Click += btnEnumSources_Click;
      // 
      // listSources
      // 
      listSources.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
      listSources.FormattingEnabled = true;
      listSources.ItemHeight = 15;
      listSources.Location = new System.Drawing.Point(13, 170);
      listSources.Name = "listSources";
      listSources.Size = new System.Drawing.Size(297, 379);
      listSources.TabIndex = 6;
      // 
      // btnSetDef
      // 
      btnSetDef.Location = new System.Drawing.Point(13, 131);
      btnSetDef.Name = "btnSetDef";
      btnSetDef.Size = new System.Drawing.Size(169, 23);
      btnSetDef.TabIndex = 7;
      btnSetDef.Text = "Set selected as default";
      btnSetDef.UseVisualStyleBackColor = true;
      btnSetDef.Click += btnSetDef_Click;
      // 
      // btnOpen
      // 
      btnOpen.Location = new System.Drawing.Point(188, 131);
      btnOpen.Name = "btnOpen";
      btnOpen.Size = new System.Drawing.Size(114, 23);
      btnOpen.TabIndex = 8;
      btnOpen.Text = "Open selected";
      btnOpen.UseVisualStyleBackColor = true;
      btnOpen.Click += btnOpen_Click;
      // 
      // splitContainer1
      // 
      splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      splitContainer1.Location = new System.Drawing.Point(0, 0);
      splitContainer1.Name = "splitContainer1";
      // 
      // splitContainer1.Panel1
      // 
      splitContainer1.Panel1.Controls.Add(lblState);
      splitContainer1.Panel1.Controls.Add(label3);
      splitContainer1.Panel1.Controls.Add(btnOpenDef);
      splitContainer1.Panel1.Controls.Add(btnSelect);
      splitContainer1.Panel1.Controls.Add(btnOpen);
      splitContainer1.Panel1.Controls.Add(label1);
      splitContainer1.Panel1.Controls.Add(btnSetDef);
      splitContainer1.Panel1.Controls.Add(lblDefault);
      splitContainer1.Panel1.Controls.Add(listSources);
      splitContainer1.Panel1.Controls.Add(btnEnumSources);
      // 
      // splitContainer1.Panel2
      // 
      splitContainer1.Panel2.Controls.Add(listCaps);
      splitContainer1.Panel2.Controls.Add(label4);
      splitContainer1.Panel2.Controls.Add(btnStart);
      splitContainer1.Panel2.Controls.Add(btnShowSettings);
      splitContainer1.Panel2.Controls.Add(btnClose);
      splitContainer1.Panel2.Controls.Add(label2);
      splitContainer1.Panel2.Controls.Add(lblCurrent);
      splitContainer1.Size = new System.Drawing.Size(1023, 564);
      splitContainer1.SplitterDistance = 325;
      splitContainer1.TabIndex = 9;
      // 
      // lblState
      // 
      lblState.AutoSize = true;
      lblState.Location = new System.Drawing.Point(54, 14);
      lblState.Name = "lblState";
      lblState.Size = new System.Drawing.Size(13, 15);
      lblState.TabIndex = 10;
      lblState.Text = "0";
      // 
      // label3
      // 
      label3.AutoSize = true;
      label3.Location = new System.Drawing.Point(13, 14);
      label3.Name = "label3";
      label3.Size = new System.Drawing.Size(36, 15);
      label3.TabIndex = 9;
      label3.Text = "State:";
      // 
      // btnOpenDef
      // 
      btnOpenDef.Location = new System.Drawing.Point(169, 58);
      btnOpenDef.Name = "btnOpenDef";
      btnOpenDef.Size = new System.Drawing.Size(104, 23);
      btnOpenDef.TabIndex = 0;
      btnOpenDef.Text = "Open default";
      btnOpenDef.UseVisualStyleBackColor = true;
      btnOpenDef.Click += btnOpenDef_Click;
      // 
      // listCaps
      // 
      listCaps.FormattingEnabled = true;
      listCaps.ItemHeight = 15;
      listCaps.Location = new System.Drawing.Point(13, 87);
      listCaps.Name = "listCaps";
      listCaps.Size = new System.Drawing.Size(234, 469);
      listCaps.TabIndex = 8;
      // 
      // label4
      // 
      label4.AutoSize = true;
      label4.Location = new System.Drawing.Point(13, 62);
      label4.Name = "label4";
      label4.Size = new System.Drawing.Size(91, 15);
      label4.TabIndex = 7;
      label4.Text = "Supported Caps";
      // 
      // btnStart
      // 
      btnStart.Location = new System.Drawing.Point(299, 32);
      btnStart.Name = "btnStart";
      btnStart.Size = new System.Drawing.Size(132, 23);
      btnStart.TabIndex = 6;
      btnStart.Text = "Start acquisition";
      btnStart.UseVisualStyleBackColor = true;
      btnStart.Click += btnStart_Click;
      // 
      // btnShowSettings
      // 
      btnShowSettings.Location = new System.Drawing.Point(161, 32);
      btnShowSettings.Name = "btnShowSettings";
      btnShowSettings.Size = new System.Drawing.Size(132, 23);
      btnShowSettings.TabIndex = 5;
      btnShowSettings.Text = "Show settings only";
      btnShowSettings.UseVisualStyleBackColor = true;
      btnShowSettings.Click += btnShowSettings_Click;
      // 
      // btnClose
      // 
      btnClose.Location = new System.Drawing.Point(13, 32);
      btnClose.Name = "btnClose";
      btnClose.Size = new System.Drawing.Size(142, 23);
      btnClose.TabIndex = 0;
      btnClose.Text = "Close current source";
      btnClose.UseVisualStyleBackColor = true;
      btnClose.Click += btnClose_Click;
      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(1023, 564);
      Controls.Add(splitContainer1);
      Name = "Form1";
      Text = "TWAIN Test";
      splitContainer1.Panel1.ResumeLayout(false);
      splitContainer1.Panel1.PerformLayout();
      splitContainer1.Panel2.ResumeLayout(false);
      splitContainer1.Panel2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
      splitContainer1.ResumeLayout(false);
      ResumeLayout(false);
    }

    #endregion

    private System.Windows.Forms.Button btnSelect;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblDefault;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblCurrent;
    private System.Windows.Forms.Button btnEnumSources;
    private System.Windows.Forms.ListBox listSources;
    private System.Windows.Forms.Button btnSetDef;
    private System.Windows.Forms.Button btnOpen;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Button btnOpenDef;
    private System.Windows.Forms.Label lblState;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Button btnShowSettings;
    private System.Windows.Forms.Button btnStart;
    private System.Windows.Forms.ListBox listCaps;
    private System.Windows.Forms.Label label4;
  }
}