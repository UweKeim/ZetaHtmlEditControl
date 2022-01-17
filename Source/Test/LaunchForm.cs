using System;
using System.Windows.Forms;

namespace Test
{
    public partial class LaunchForm : Form
    {
        public LaunchForm()
        {
            InitializeComponent();
        }

        private void LaunchForm_Shown(object sender, EventArgs e)
        {
            using (var form = new TestFormForScreenshots4())
            //using (var form = new TestFormForScreenshots5())
            {
                form.ShowDialog(this);
                Close();
            }
        }
    }
}
