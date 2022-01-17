namespace Test
{
    using System;
    using System.Windows.Forms;

    public partial class TestFormForScreenshots2 :
        Form
    {
        public TestFormForScreenshots2()
        {
            InitializeComponent();
        }

        private void TestForm_Shown(object sender, EventArgs e)
        {
            htmlEditUserControl1.DocumentText = "hier 1.";
            htmlEditUserControl2.DocumentText = "hier 2.";

            //htmlEditUserControl1.Parent = this;
            //htmlEditUserControl2.Parent = this;

            //panel1.Visible = false;
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
        }
    }
}