namespace ZetaHtmlEditControl.UI.EditControlBases
{
    using System;
    using Code.Configuration;

    public partial class CoreHtmlEditControl
    {
        public HtmlEditControlConfiguration Configuration { get; private set; } = new HtmlEditControlConfiguration();

        public virtual void Configure(HtmlEditControlConfiguration configuration)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }
    }
}