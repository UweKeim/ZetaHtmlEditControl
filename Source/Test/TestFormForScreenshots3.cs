namespace Test
{
    using System;
    using System.Windows.Forms;

    public partial class TestFormForScreenshots3 :
        Form
    {
        public TestFormForScreenshots3()
        {
            InitializeComponent();
        }

        private void TestForm_Shown(object sender, EventArgs e)
        {
            htmlEditUserControl1.HtmlEditControl.DocumentText = "hier 1.";
            htmlEditUserControl2.HtmlEditControl.DocumentText = "hier 2.";
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
        }
    }
}