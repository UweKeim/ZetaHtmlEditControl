namespace ZetaHtmlEditControl.UI.EditControlAbsoluteBases
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Windows.Forms;
    using Code.HttpServer;
    using Code.MsHtml;
    using Microsoft.VisualBasic.CompilerServices;

    public partial class ExtendedWebBrowser :
        WebBrowser
    {
        private static readonly object TypeLock = new object();
        private static IExternalWebServer _webServer;
        private static IExternalWebServer _externalWebServer;
        private int _documentCompletedCount;
        private int _documentSetCount;
        private string _textToSet = string.Empty;

        [ComVisible(true)]
        // ReSharper disable once MemberCanBePrivate.Global
        public sealed class WebBrowserObjectForScripting
        {
            private readonly ExtendedWebBrowser _owner;

            public WebBrowserObjectForScripting(ExtendedWebBrowser owner)
            {
                _owner = owner;
            }

            // ReSharper disable once UnusedMember.Global
            public void Log(object text)
            {
                Trace.TraceInformation(text?.ToString() ?? string.Empty);
            }

            // ReSharper disable once UnusedMember.Global
            public void NotifyDocumentReady()
            {
                Trace.TraceInformation("Document Ready received from JavaScript.");
                //_owner.turnWebBrowserDesignModeOn();
                _owner.MarkAsEverInitialized();
            }
        }

        private bool _wasOn;

        public ExtendedWebBrowser()
        {
            if (!HtmlEditorDesignModeManager.IsDesignMode)
            {
                bindObjectForScripting();
            }
        }

        internal void EnsureHtmlDesignMode()
        {
            if (!EverInitialized)
            {
                // 2015-11-11, Uwe Keim:
                //
                // Achtung, nur aufrufen wenn schon alles initialisiert wurde.
                // Kann verwendet werden, um z.B. bei manchmal "vergessener" Aktivierung
                // (z.B. durch Timing-Issues) nochmal aufzurufen, z.B. im Vorfeld, wenn
                // User den Fokus ins Control setzt.

                // 2015-11-11, Uwe Keim:
                // Bisher nur als Code, noch nicht aufgerufen und noch nicht getestet.

                Document?.InvokeScript(@"turnOnDesignMode");
            }
        }

        private void bindObjectForScripting()
        {
            ObjectForScripting = new WebBrowserObjectForScripting(this);
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public override string Text
        {
            get { return DocumentText; }
            set { DocumentText = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new string DocumentText
        {
            get
            {
                if (DesignMode)
                {
                    return string.Empty;
                }
                else
                {
                    // --
                    // 2013-02-23, Uwe Keim:
                    //
                    // The idea here is that a condition could occur where a text was set by code 
                    // and immediately read back again by code, but the browser had not enough time
                    // to actually navigate to the text (i.e. the end-user does not see the text yet).
                    //
                    // In such a case, the reading of the DocumentText property would return the
                    // previously loaded text, which is a blank text when the control was just initialized.
                    // 
                    // To avoid this, we keep track of when setting the text and when actually finished
                    // loading that text. We only return the text from the HTML editor when it was finished
                    // loading. Otherwise, we just return the text that was programmatically set to
                    // the control.

                    // --
                    // 2013-03-07, Uwe Keim:
                    //
                    // I've noticed that in Zeta Helpdesk where I included this control, in some cases
                    // the control is not in edit mode and I have to re-open the dialog again. I'm not
                    // sure whether the change hier was the reason, but I've never seen the behaviour
                    // prior to making this change.

                    if (_documentCompletedCount > 0 && _documentSetCount > 0)
                    {
                        return Document?.Body?.InnerHtml;
                        //return Document?.GetElementById(@"editor")?.InnerHtml;
                        //return base.DocumentText;
                    }
                    else
                    {
                        return _textToSet;
                    }
                }
            }
            set
            {
                if (!DesignMode)
                {
                    // 2015-11-11, Uwe Keim: Da wir inzwischen über contentEditable im HTML arbeiten,
                    // können wir auch direkt ins DOM schreiben, wenn es einmal geladen wurde.
                    if (_documentCompletedCount > 0 && _documentSetCount > 0 && Document?.Body != null)
                    {
                        Document.Body.InnerHtml = PrepareDocumentTextGet(MsHtmlLegacyFromBadToGoodTranslator.Translate(value));
                    }
                    else
                    {
                        _documentCompletedCount = 0;
                        _documentSetCount++;
                        _textToSet = value;

                        Navigate(WebServer.SetDocumentText(this, value));
                    }
                }
            }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public static IExternalWebServer ExternalWebServer
        {
            get
            {
                lock (TypeLock)
                {
                    return _externalWebServer;
                }
            }
            set
            {
                lock (TypeLock)
                {
                    _externalWebServer = value;
                }
            }
        }

        private static IExternalWebServer WebServer
        {
            get
            {
                lock (TypeLock)
                {
                    if (_externalWebServer == null)
                    {
                        if (_webServer == null)
                        {
                            var ws = new WebServer();
                            ws.Initialize();
                            _webServer = ws;
                        }

                        return _webServer;
                    }
                    else
                    {
                        return _externalWebServer;
                    }
                }
            }
        }

        public new void Navigate(string url)
        {
            // This Application.DoEvents() is necessary, 
            // otherwise the webbrowser gets a 
            // AccessViolationException, whyever.
            Application.DoEvents();

            bindObjectForScripting();

            // Turn off before navigating to get rid of the "Document was modified" message box.
            // http://social.msdn.microsoft.com/Forums/en/winforms/thread/4928c061-951a-43cc-aad2-8844084c148d
            turnWebBrowserDesignModeOff();

            // This Application.DoEvents() is necessary, 
            // otherwise the webbrowser gets a 
            // AccessViolationException, whyever.
            Application.DoEvents();

            base.Navigate(url);
        }

        protected override void OnDocumentCompleted(
            WebBrowserDocumentCompletedEventArgs e)
        {
            bindObjectForScripting();

            base.OnDocumentCompleted(e);

            //turnWebBrowserDesignModeOn();

            // This Application.DoEvents() is necessary, 
            // otherwise the webbrowser gets a 
            // AccessViolationException, whyever.
            Application.DoEvents();

            _documentCompletedCount++;

            bindEvents();
        }

        protected override void Dispose(bool disposing)
        {
            unbindEvents(disposing);

            base.Dispose(disposing);
        }

        private void turnWebBrowserDesignModeOn()
        {
            return;

            var axInstance = ActiveXInstance;
            if (axInstance != null)
            {
                var instance =
                    NewLateBinding.LateGet(
                        axInstance,
                        null,
                        @"Document",
                        new object[0],
                        null,
                        null,
                        null);

                if (instance != null)
                {
                    NewLateBinding.LateSetComplex(
                        instance,
                        null,
                        @"designMode",
                        new object[] { @"On" },
                        null,
                        null,
                        false,
                        true);

                    _wasOn = true;
                }
            }
        }

        private void turnWebBrowserDesignModeOff()
        {
            return;

            if (_wasOn)
            {
                var axInstance = ActiveXInstance;
                if (axInstance != null)
                {
                    var instance =
                        NewLateBinding.LateGet(
                            axInstance,
                            null,
                            @"Document",
                            new object[0],
                            null,
                            null,
                            null);

                    if (instance != null)
                    {
                        NewLateBinding.LateSetComplex(
                            instance,
                            null,
                            @"designMode",
                            new object[] { @"Off" },
                            null,
                            null,
                            false,
                            true);
                    }
                }
            }
        }

        protected virtual string PrepareDocumentTextGet(string html)
        {
            throw new System.NotImplementedException();
        }
    }
}