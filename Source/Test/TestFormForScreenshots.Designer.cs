namespace Test
{
    partial class TestFormForScreenshots
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && (components != null) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestFormForScreenshots));
            this.ToolbarVisibleCheckBox = new System.Windows.Forms.CheckBox();
            this.htmlEditUserControl1 = new ZetaHtmlEditControl.HtmlEditUserControl();
            this.SuspendLayout();
            // 
            // ToolbarVisibleCheckBox
            // 
            this.ToolbarVisibleCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ToolbarVisibleCheckBox.AutoSize = true;
            this.ToolbarVisibleCheckBox.Checked = true;
            this.ToolbarVisibleCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.ToolbarVisibleCheckBox.Location = new System.Drawing.Point(12, 531);
            this.ToolbarVisibleCheckBox.Name = "ToolbarVisibleCheckBox";
            this.ToolbarVisibleCheckBox.Size = new System.Drawing.Size(113, 21);
            this.ToolbarVisibleCheckBox.TabIndex = 1;
            this.ToolbarVisibleCheckBox.Text = "Toolbar visible";
            this.ToolbarVisibleCheckBox.UseVisualStyleBackColor = true;
            this.ToolbarVisibleCheckBox.CheckedChanged += new System.EventHandler(this.ToolbarVisibleCheckBox_CheckedChanged);
            // 
            // htmlEditUserControl1
            // 
            this.htmlEditUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlEditUserControl1.IE10RenderingMode = true;
            this.htmlEditUserControl1.IsToolbarVisible = false;
            this.htmlEditUserControl1.Location = new System.Drawing.Point(12, 12);
            this.htmlEditUserControl1.Name = "htmlEditUserControl1";
            this.htmlEditUserControl1.Size = new System.Drawing.Size(695, 513);
            this.htmlEditUserControl1.TabIndex = 0;
            // 
            // TestFormForScreenshots
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(719, 564);
            this.Controls.Add(this.ToolbarVisibleCheckBox);
            this.Controls.Add(this.htmlEditUserControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(525, 410);
            this.Name = "TestFormForScreenshots";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test form for the Zeta Html Edit Control";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.Shown += new System.EventHandler(this.TestForm_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

        private ZetaHtmlEditControl.HtmlEditUserControl htmlEditUserControl1;
        private System.Windows.Forms.CheckBox ToolbarVisibleCheckBox;
	}
}

