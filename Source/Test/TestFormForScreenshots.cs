namespace Test
{
    using System;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using ZetaHtmlEditControl.Code.Configuration;

    public partial class TestFormForScreenshots :
        Form
    {
        private bool _ignore;

        public TestFormForScreenshots()
        {
            InitializeComponent();

            Thread.CurrentThread.CurrentCulture = new CultureInfo(@"en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(@"en-US");
        }

        private void TestForm_Shown(object sender, EventArgs e)
        {
            htmlEditUserControl1.Configure(new HtmlEditControlConfiguration{AllowFontChange = false});
            htmlEditUserControl1.IsToolbarVisible = true;

            const string s3 = @"<p>ZetaHtmlEditControl.Code.HttpServer<br />
{<br />
<span style=""background-color: rgb(0, 255, 0);"">using System.Globalization;<br />
using System.Diagnostics;<br />
using System.Configuration;<br />
using System.Collections.Generic;<br />
using System;<br />
</span>using System.Net;<br />
using System.Net.NetworkInformation;<br />
using System.Net.Sockets;<br />
using System.Text;<br />
using global::HttpServer;<br />
using global::HttpServer.HttpModules;<br />
using global::HttpServer.Sessions;<br />
using Helper;<br />
using Properties;</p>
";
            //const string s3 = @"<P><b>Some tests</b></P><p>Random content. <font color=green>Please edit</font>.</p><p>Use right-click for options.</p>";
            htmlEditUserControl1.HtmlEditControl.DocumentText = s3;
            updateUI();


            updateUI();
        }

        private void updateUI()
        {
        }

        private void ToolbarVisibleCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (!_ignore)
            {
                _ignore = true;
                htmlEditUserControl1.IsToolbarVisible = ToolbarVisibleCheckBox.Checked;
                _ignore = false;
            }
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            htmlEditUserControl1.HtmlEditControl.WantCloseDialogWithOK += delegate { MessageBox.Show("Close."); };

            htmlEditUserControl1.IsToolbarVisible = true;
            ToolbarVisibleCheckBox.Checked = htmlEditUserControl1.IsToolbarVisible;

            htmlEditUserControl1.HtmlEditControl.UINeedsUpdate +=
                delegate
                {
                    var el = htmlEditUserControl1.HtmlEditControl.GetElementAtCaret();
                    infoTextBox.Text = el == null ? @"-" : el.tagName;
                };
        }
    }
}