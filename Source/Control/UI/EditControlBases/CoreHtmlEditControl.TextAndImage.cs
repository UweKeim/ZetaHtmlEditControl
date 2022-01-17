namespace ZetaHtmlEditControl.UI.EditControlBases
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Text.RegularExpressions;
    using Code.Configuration;
    using Code.Html;
    using Code.MsHtml;
    using Properties;

    public partial class CoreHtmlEditControl
    {
        private const string CssFontStyle =
            @"font-family: Segoe UI, Tahoma, Verdana, Arial; font-size: {font-size}; ";

        private string _cssFontSize;

        private string _cssText = DefaultCssText;
        private HtmlConversionHelper _htmlConversionHelper;
        private string _htmlTemplate = DefaultHtmlTemplate;

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string DefaultCssText { get; set; } =
            @"body { {font-style}; margin: 4px; {color}; }
			li { margin-bottom: 5pt; }
			table {
				border-width: 1px;
				border-style: dotted;
				border-color: #C6C6C6;
			}
			table td, table th {
				border-width: 1px;
				border-style: dotted;
				border-color: #C6C6C6;
			}
			table p {
				margin: 0;
				padding: 0;
			}";

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static string DefaultHtmlTemplate { get; set; } =
            $@"<!DOCTYPE {InternalEditingConfiguration.DocType}>
			<html style=""height:100%"">
				<head>
                    <meta http-equiv=""X-UA-Compatible"" content=""IE={InternalEditingConfiguration.IEVersion}"" />
					<meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
					<style type=""text/css"">##CSS##</style>
					<style type=""text/css"">
                        html,body {{ height:100%; }} 
                        #editor {{ height: 100%; }}
                    </style>
                    <script type=""text/javascript"">
                        var checkLoad = function() {{   
                            if(document.readyState !== ""complete"" ) {{
                                setTimeout(checkLoad, 11);
                            }} else {{
                                turnOnDesignMode();
                            }}
                        }};  

                        checkLoad(); 

                        function turnOnDesignMode() {{
                            try {{
                                try {{ window.external.NotifyDocumentReady(); }} catch ( err ) {{}}
                                document.designMode = ""on"";
                                document.body.hideFocus = true;
                            }} catch ( err2 ) {{ }}
                        }}
                    </script>
				</head>
				<body spellcheck=""true"">##BODY##</body>
			</html>";

        /// <summary>
        /// Assigns a style sheet to the HTML editor.
        /// Set<see cref="DocumentText"/>t to activate.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CssText
        {
            set { _cssText = value; }
        }

        /// <summary>
        /// Set own HTML Code.
        /// This '##BODY##' Tag will be replaced with the Body.
        /// Optional: '##CSS##'
        /// Set <see cref="DocumentText"/> to activate.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string HtmlTemplate
        {
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentNullException(
                        nameof(value),
                        Resources.SR_HtmlEditControl_HtmlTemplate_AvaluefortheHtmlTemplatemustbeprovided);
                }
                else if (!value.Contains(@"##BODY##"))
                {
                    throw new ArgumentException(
                        Resources.SR_HtmlEditControl_HtmlTemplate_MissingBODYinsidetheHtmlTemplatepropertyvalue,
                        nameof(value));
                }
                else
                {
                    _htmlTemplate = value;
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CompleteDocumentText
        {
            get { return base.DocumentText; }
            set { base.DocumentText = value; }
        }

        /// <summary>
        /// Wenn hier ein Wert drin steht, dann wird der Wert für eingefügte Links
        /// verwendet.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TargetForLinks { get; set; }

        /// <summary>
        /// Gets or sets the HTML content.
        /// </summary>
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string DocumentText
        {
            get { return PrepareDocumentTextGet(MsHtmlLegacyFromBadToGoodTranslator.Translate(base.DocumentText)); }
            set { base.DocumentText = prepareDocumentTextSet(MsHtmlLegacyFromGoodToBadTranslator.Translate(value)); }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string TextOnlyFromDocumentBody => base.DocumentText.GetBodyFromHtmlCode().GetOnlyTextFromHtmlCode();

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public string CssFontSize
        {
            get { return string.IsNullOrEmpty(_cssFontSize) ? getCssFontSizeWithUnit() : _cssFontSize; }
            set { _cssFontSize = value; }
        }

        public string CssColor
        {
            get
            {
                var color = ForeColor;
                return $@"#{color.R:X2}{color.G:X2}{color.B:X2}";
            }
        }

        private void constructCoreHtmlEditControlTextAndImage()
        {
            _htmlConversionHelper = new HtmlConversionHelper();

            TargetForLinks = @"_blank";
        }

        public string MakeFullHtmlFromBody(
            string body)
        {
            return doBuildCompleteHtml(body, DefaultHtmlTemplate, DefaultCssText);
        }

        public void SetDocumentText(
            string text,
            string externalImagesFolderPath = null,
            bool useImagesFolderPathPlaceHolder = false)
        {
            DocumentText =
                _htmlConversionHelper.ConvertSetHtml(
                    text,
                    externalImagesFolderPath,
                    useImagesFolderPathPlaceHolder ? HtmlImageHelper.ImagesFolderPathPlaceHolder : null);
        }

        public string GetDocumentText(
            string externalImagesFolderPath,
            bool useImagesFolderPathPlaceHolder = false)
        {
            var result =
                _htmlConversionHelper.ConvertGetHtml(
                    DocumentText,
                    EverInitialized ? Document?.Url : null,
                    externalImagesFolderPath,
                    useImagesFolderPathPlaceHolder ? HtmlImageHelper.ImagesFolderPathPlaceHolder : null);

            return result;
        }

        private string prepareDocumentTextSet(string html)
        {
            return buildCompleteHtml(html.GetBodyFromHtmlCode().CheckCompleteHtmlTable());
        }

        private string buildCompleteHtml(string htmlBody)
        {
            return doBuildCompleteHtml(htmlBody, _htmlTemplate, _cssText);
        }

        private string doBuildCompleteHtml(
            string htmlBody,
            string htmlTemplate,
            string cssText)
        {
            string tmpHtml;
            if (string.IsNullOrEmpty(htmlTemplate))
            {
                tmpHtml = htmlBody;
            }
            else
            {
                tmpHtml = htmlTemplate;
                tmpHtml = tmpHtml.Replace(@"##BODY##", htmlBody);
            }

            tmpHtml = tmpHtml.Replace(@"##CSS##", replaceCss(cssText));

            return tmpHtml;
        }

        private string replaceCss(string cssText)
        {
            if (!string.IsNullOrEmpty(cssText) && cssText.Contains(@"{font-style}"))
            {
                cssText = cssText.Replace(@"{font-style}", CssFontStyle);
                cssText = cssText.Replace(@"{font-size}", CssFontSize);
            }

            if (!string.IsNullOrEmpty(cssText) && cssText.Contains(@"{color}"))
            {
                cssText = cssText.Replace(@"{color}", $@"color: {CssColor}");
            }

            return cssText;
        }

        private string getCssFontSizeWithUnit()
        {
            //Console.WriteLine(getFontScaleFactor());

            // http://stackoverflow.com/questions/139655/convert-pixels-to-points
            var font = Font;

            switch (font.Unit)
            {
                case GraphicsUnit.World:
                case GraphicsUnit.Display:
                case GraphicsUnit.Inch:
                case GraphicsUnit.Document:
                case GraphicsUnit.Millimeter:
                    return $@"{font.SizeInPoints}pt";
                case GraphicsUnit.Pixel:
                    return $@"{font.Size}px";
                case GraphicsUnit.Point:
                    return $@"{font.Size}pt";
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        protected override string PrepareDocumentTextGet(string html)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;

            var result = html.GetBodyFromHtmlCode();
            result = Regex.Replace(result, @"<![^>]*>", string.Empty, RegexOptions.Singleline);

            if (Configuration.ReplaceNonBreakingSpaceOnGet) result = result.Replace(@"&nbsp;", @" ");

            result = result.MakeLinkTargets(TargetForLinks);

            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_htmlConversionHelper != null)
            {
                _htmlConversionHelper.Dispose();
                _htmlConversionHelper = null;
            }
        }
    }
}