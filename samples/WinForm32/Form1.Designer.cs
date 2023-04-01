namespace WinForm32
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
      SuspendLayout();
      // 
      // btnSelect
      // 
      btnSelect.Location = new System.Drawing.Point(12, 12);
      btnSelect.Name = "btnSelect";
      btnSelect.Size = new System.Drawing.Size(151, 23);
      btnSelect.TabIndex = 0;
      btnSelect.Text = "Select default source";
      btnSelect.UseVisualStyleBackColor = true;
      btnSelect.Click += btnSelect_Click;
      // 
      // label1
      // 
      label1.AutoSize = true;
      label1.Location = new System.Drawing.Point(12, 51);
      label1.Name = "label1";
      label1.Size = new System.Drawing.Size(98, 15);
      label1.TabIndex = 1;
      label1.Text = "Default Source = ";
      // 
      // lblDefault
      // 
      lblDefault.AutoSize = true;
      lblDefault.Location = new System.Drawing.Point(115, 51);
      lblDefault.Name = "lblDefault";
      lblDefault.Size = new System.Drawing.Size(24, 15);
      lblDefault.TabIndex = 2;
      lblDefault.Text = "NA";
      // 
      // label2
      // 
      label2.AutoSize = true;
      label2.Location = new System.Drawing.Point(12, 81);
      label2.Name = "label2";
      label2.Size = new System.Drawing.Size(97, 15);
      label2.TabIndex = 3;
      label2.Text = "Current Source =";
      // 
      // lblCurrent
      // 
      lblCurrent.AutoSize = true;
      lblCurrent.Location = new System.Drawing.Point(115, 81);
      lblCurrent.Name = "lblCurrent";
      lblCurrent.Size = new System.Drawing.Size(24, 15);
      lblCurrent.TabIndex = 4;
      lblCurrent.Text = "NA";
      // 
      // btnEnumSources
      // 
      btnEnumSources.Location = new System.Drawing.Point(12, 114);
      btnEnumSources.Name = "btnEnumSources";
      btnEnumSources.Size = new System.Drawing.Size(151, 23);
      btnEnumSources.TabIndex = 5;
      btnEnumSources.Text = "Enumerate sources";
      btnEnumSources.UseVisualStyleBackColor = true;
      btnEnumSources.Click += btnEnumSources_Click;
      // 
      // listSources
      // 
      listSources.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
      listSources.FormattingEnabled = true;
      listSources.ItemHeight = 15;
      listSources.Location = new System.Drawing.Point(12, 143);
      listSources.Name = "listSources";
      listSources.Size = new System.Drawing.Size(279, 289);
      listSources.TabIndex = 6;
      // 
      // Form1
      // 
      AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
      AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      ClientSize = new System.Drawing.Size(800, 450);
      Controls.Add(listSources);
      Controls.Add(btnEnumSources);
      Controls.Add(lblCurrent);
      Controls.Add(label2);
      Controls.Add(lblDefault);
      Controls.Add(label1);
      Controls.Add(btnSelect);
      Name = "Form1";
      Text = "TWAIN Test";
      ResumeLayout(false);
      PerformLayout();
    }

    #endregion

    private System.Windows.Forms.Button btnSelect;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label lblDefault;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label lblCurrent;
    private System.Windows.Forms.Button btnEnumSources;
    private System.Windows.Forms.ListBox listSources;
  }
}