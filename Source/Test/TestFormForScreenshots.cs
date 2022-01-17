namespace Test
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using System.Globalization;
    using System.Threading;
    using System.Windows.Forms;
    using ZetaHtmlEditControl.Code.Configuration;
    using ZetaHtmlEditControl.UI.EditControlAbsoluteBases;
    using ZetaHtmlEditControl.UI.EditControlBases;

    public partial class TestFormForScreenshots :
        Form,
        IExternalInformationProvider
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
            htmlEditUserControl1.Configure(new HtmlEditControlConfiguration
            {
                AllowFontChange = false,
                ExternalInformationProvider = this,
                ReplaceNonBreakingSpaceOnGet = true
            });
            htmlEditUserControl1.IsToolbarVisible = false;

            timer1.Start();

            htmlEditUserControl1.HtmlEditControl.DocumentFocusIn += HtmlEditControlOnDocumentFocusIn;
            htmlEditUserControl1.HtmlEditControl.DocumentFocusOut += HtmlEditControlOnDocumentFocusOut;
            htmlEditUserControl1.HtmlEditControl.DocumentActivate += HtmlEditControlOnDocumentActivate;
            htmlEditUserControl1.HtmlEditControl.DocumentDeactivate += HtmlEditControlOnDocumentDeactivate;

            const string s3 = @"<p>ZetaHtmlEditControl.Code.HttpServer<br />
{<br />
<span style=""background-color: #00FF00"">using System.Globalization;<br />
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
            htmlEditUserControl1.HtmlEditControl.DocumentText = "hier 1.";
            htmlEditUserControl2.HtmlEditControl.DocumentText = "hier 2.";
            updateUI();


            updateUI();
        }

        private void HtmlEditControlOnDocumentDeactivate(object sender, ExtendedWebBrowser.MSHtmlEventArgs msHtmlEventArgs)
        {
            Trace.TraceInformation($@"DEACTIVATE in Control '{((Control)sender).GetHashCode()}'.");
        }

        private void HtmlEditControlOnDocumentFocusOut(object sender, ExtendedWebBrowser.MSHtmlEventArgs msHtmlEventArgs)
        {
            Trace.TraceInformation($@"LEAVE in Control '{((Control)sender).GetHashCode()}'.");
        }

        private void HtmlEditControlOnDocumentFocusIn(object sender, ExtendedWebBrowser.MSHtmlEventArgs msHtmlEventArgs)
        {
            Trace.TraceInformation($@"ENTER in Control '{((Control)sender).GetHashCode()}'.");
        }

        private void HtmlEditControlOnDocumentActivate(object sender, ExtendedWebBrowser.MSHtmlEventArgs msHtmlEventArgs)
        {
            Trace.TraceInformation($@"ACTIVATE in Control '{((Control)sender).GetHashCode()}'.");
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
                    //var el = htmlEditUserControl1.HtmlEditControl.GetElementAtCaret();
                    //infoTextBox.Text = el == null ? @"-" : el.tagName;
                };
        }

        System.Drawing.Font IExternalInformationProvider.Font
        {
            get { return new Font(@"Times New Roman", 20, GraphicsUnit.Pixel); }
        }

        System.Drawing.Color? IExternalInformationProvider.ForeColor
        {
            get
            {
                return Color.Green;
            }
        }

        public Color? ControlBorderColor
        {
            get { return null; }
        }

        void IExternalInformationProvider.SavePerUserPerWorkstationValue(string name, string value)
        {
        }

        string IExternalInformationProvider.RestorePerUserPerWorkstationValue(string name, string fallBackTo)
        {
            return null;
        }

        TextModuleInfo[] IExternalInformationProvider.GetTextModules()
        {
            return null;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show(this, htmlEditUserControl1.HtmlEditControl.DocumentText);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            //var c = FindFocusedControl(this);
            //Trace.TraceInformation($@"FOCUSED CONTROL: '{c?.Name}', '{c?.GetHashCode()}'.");
        }

        public static Control FindFocusedControl(Control control)
        {
            var container = control as IContainerControl;
            while (container != null)
            {
                control = container.ActiveControl;
                container = control as IContainerControl;
            }
            return control;
        }
    }
}