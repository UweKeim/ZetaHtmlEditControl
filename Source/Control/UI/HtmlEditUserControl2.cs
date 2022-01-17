namespace ZetaHtmlEditControl.UI
{
    using System.ComponentModel;
    using System.Windows.Forms;
    using EditControlAbsoluteBases;
    using EditControlBases;
    using EditControlDerives;

    public partial class HtmlEditUserControl2 : UserControl
    {
        public HtmlEditUserControl2()
        {
            InitializeComponent();
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ExtendedWebBrowser HtmlEditControl => htmlEditControl;
    }
}