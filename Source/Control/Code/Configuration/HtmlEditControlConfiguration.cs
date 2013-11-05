namespace ZetaHtmlEditControl.Code.Configuration
{
    public sealed class HtmlEditControlConfiguration
    {
        public IExternalInformationProvider ExternalInformationProvider { get; set; }

        public bool AllowFontChange { get; set; }
        public bool AllowPrint { get; set; }
        public bool AllowEmbeddedImages { get; set; }
    }
}