namespace Test
{
    using System;
    using System.Diagnostics;
    using System.Windows.Forms;
    using ZetaHtmlEditControl.UI.EditControlAbsoluteBases;

    public partial class TestFormForScreenshots4 :
        Form
    {
        public TestFormForScreenshots4()
        {
            InitializeComponent();
        }

        private void TestForm_Shown(object sender, EventArgs e)
        {
            htmlEditUserControl1.HtmlEditControl.DocumentText = "hier 1.";
            htmlEditUserControl2.HtmlEditControl.DocumentText = "hier 2.";

            // Mehrfach den Fokus zwischen den einzelnen HTML-Editoren ändern,
            // lässt den Fehler verschwinden, dass mehrere Carets gleichzeitig blinken.
            htmlEditUserControl1.HtmlEditControl.Focus();
            htmlEditUserControl2.HtmlEditControl.Focus();
            htmlEditUserControl1.HtmlEditControl.Focus();
            htmlEditUserControl2.HtmlEditControl.Focus();
            htmlEditUserControl1.HtmlEditControl.Focus();
            htmlEditUserControl2.HtmlEditControl.Focus();

            tabControl1.Focus();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            MessageBox.Show(htmlEditUserControl1.HtmlEditControl.DocumentText);
        }
    }
}