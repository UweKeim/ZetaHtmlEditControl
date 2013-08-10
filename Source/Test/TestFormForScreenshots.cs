namespace Test
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using ZetaHtmlEditControl;

    public partial class TestFormForScreenshots :
        Form
    {
        public TestFormForScreenshots()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(@"en-US");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            const string s = @"<P><b>Some tests</b></P><p>Random content. <font color=green>Please edit</font>.</p><p>Use right-click for options.</p>";
            //htmlEditControl1.DocumentText = s;
            updateUI();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string s = @"<P><b>Some tests</b></P><p>Random content. <font color=red>Also editable</font>.</p>";
            //htmlEditControl1.DocumentText = s;
            updateUI();
        }

        private void TestForm_Shown(object sender, EventArgs e)
        {
            const string s = @"<P>Click the buttons below to set different texts. German Umlaute: Ä Ö Ü ä ö ü ß.</p>";
            //htmlEditControl1.SetDocumentText(s, @"C:\", true);

            const string s2 = @"<P></p>";
            htmlEditUserControl1.HtmlEditControl.SetDocumentText(s2, @"C:\", true);

            const string s3 = @"<P><b>Some tests</b></P><p>Random content. <font color=green>Please edit</font>.</p><p>Use right-click for options.</p>";
            htmlEditUserControl1.HtmlEditControl.DocumentText = s3;
            updateUI();


            updateUI();
        }

        private void updateUI()
        {
        }

        private void ToolbarVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            htmlEditUserControl1.IsToolbarVisible = ToolbarVisibleCheckBox.Checked;
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            //			WebBrowserHelper.SafeSwitchToHighestInternetExplorerVersionAsync();

            const string html =
                @"<P>Mit Bild:</P>
				<P><IMG 
				src=""http://pseudo-image-folder-path/d0906191-5a75-4568-97d4-924ee727426d""></P>
				<P>Yes!</P>";

            var images = HtmlEditControl.GetContainedImageFileNames(html);
            foreach (var image in images)
            {
                Console.WriteLine(image);
            }

            htmlEditUserControl1.HtmlEditControl.WantCloseDialogWithOK += delegate { MessageBox.Show("Close."); };
        }

        private void button3_Click(object sender, EventArgs e)
        {
            htmlEditUserControl1.HtmlEditControl.DocumentText = "lalala";
            MessageBox.Show(htmlEditUserControl1.HtmlEditControl.DocumentText);
        }
    }
}