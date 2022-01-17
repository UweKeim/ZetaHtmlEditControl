namespace Test
{
    using ZetaHtmlEditControl.UI;
    using ZetaHtmlEditControl.UI.EditControlDerives;

    partial class TestFormForScreenshots2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestFormForScreenshots2));
            this.htmlEditUserControl1 = new ZetaHtmlEditControl.UI.EditControlDerives.HtmlEditControl();
            this.htmlEditUserControl2 = new ZetaHtmlEditControl.UI.EditControlDerives.HtmlEditControl();
            this.SuspendLayout();
            // 
            // htmlEditUserControl1
            // 
            this.htmlEditUserControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlEditUserControl1.Location = new System.Drawing.Point(12, 12);
            this.htmlEditUserControl1.Name = "htmlEditUserControl1";
            this.htmlEditUserControl1.Size = new System.Drawing.Size(294, 75);
            this.htmlEditUserControl1.TabIndex = 0;
            // 
            // htmlEditUserControl2
            // 
            this.htmlEditUserControl2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.htmlEditUserControl2.Location = new System.Drawing.Point(12, 93);
            this.htmlEditUserControl2.Name = "htmlEditUserControl2";
            this.htmlEditUserControl2.Size = new System.Drawing.Size(294, 82);
            this.htmlEditUserControl2.TabIndex = 0;
            // 
            // TestFormForScreenshots2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(318, 185);
            this.Controls.Add(this.htmlEditUserControl2);
            this.Controls.Add(this.htmlEditUserControl1);
            this.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "TestFormForScreenshots2";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Test form for the Zeta Html Edit Control";
            this.Load += new System.EventHandler(this.TestForm_Load);
            this.Shown += new System.EventHandler(this.TestForm_Shown);
            this.ResumeLayout(false);

		}

		#endregion

        private HtmlEditControl htmlEditUserControl1;
        private HtmlEditControl htmlEditUserControl2;
    }
}

