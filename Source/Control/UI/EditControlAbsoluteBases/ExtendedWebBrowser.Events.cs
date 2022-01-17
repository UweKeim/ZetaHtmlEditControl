namespace ZetaHtmlEditControl.UI.EditControlAbsoluteBases
{
    using System;
    using System.Runtime.InteropServices.ComTypes;
    using mshtml;

    public partial class ExtendedWebBrowser
    {
        private IConnectionPoint _icp;
        private int _eventsCookie = -1;

        private void bindEvents()
        {
            // --
            // http://codinglight.blogspot.de/2009/07/webbrowser-implementation-that-actually.html

            var icpc = (IConnectionPointContainer) Document?.DomDocument;
            var guid = typeof (HTMLDocumentEvents2).GUID;
            icpc?.FindConnectionPoint(ref guid, out _icp);
            _icp.Advise(new HandleWebBrowserDhtmlEvents(this), out _eventsCookie);
        }

        private void unbindEvents(bool disposing)
        {
            if (disposing)
            {
                if (-1 != _eventsCookie) _icp.Unadvise(_eventsCookie);
                _eventsCookie = -1;
            }
        }

        public class MSHtmlEventArgs :
            EventArgs
        {
            public MSHtmlEventArgs(IHTMLEventObj obj)
            {
                EventObj = obj;
            }

            public IHTMLEventObj EventObj { get; private set; }
        }

        public event EventHandler<MSHtmlEventArgs> DocumentActivate;
        public event EventHandler<MSHtmlEventArgs> DocumentAfterUpdate;
        public event EventHandler<MSHtmlEventArgs> DocumentBeforeActivate;
        public event EventHandler<MSHtmlEventArgs> DocumentBeforeDeactivate;
        public event EventHandler<MSHtmlEventArgs> DocumentBeforeEditFocus;
        public event EventHandler<MSHtmlEventArgs> DocumentBeforeUpdate;
        public event EventHandler<MSHtmlEventArgs> DocumentCellChange;
        public event EventHandler<MSHtmlEventArgs> DocumentClick;
        public event EventHandler<MSHtmlEventArgs> DocumentContextMenu;
        public event EventHandler<MSHtmlEventArgs> DocumentControlSelect;
        public event EventHandler<MSHtmlEventArgs> DocumentDataAvailable;
        public event EventHandler<MSHtmlEventArgs> DocumentDataSetChanged;
        public event EventHandler<MSHtmlEventArgs> DocumentDataSetComplete;
        public event EventHandler<MSHtmlEventArgs> DocumentDoubleClick;
        public event EventHandler<MSHtmlEventArgs> DocumentDeactivate;
        public event EventHandler<MSHtmlEventArgs> DocumentDragStart;
        public event EventHandler<MSHtmlEventArgs> DocumentErrorUpdate;
        public event EventHandler<MSHtmlEventArgs> DocumentFocusIn;
        public event EventHandler<MSHtmlEventArgs> DocumentFocusOut;
        public event EventHandler<MSHtmlEventArgs> DocumentHelp;
        public event EventHandler<MSHtmlEventArgs> DocumentKeyDown;
        public event EventHandler<MSHtmlEventArgs> DocumentKeyPress;
        public event EventHandler<MSHtmlEventArgs> DocumentKeyUp;
        public event EventHandler<MSHtmlEventArgs> DocumentMouseDown;
        public event EventHandler<MSHtmlEventArgs> DocumentMouseMove;
        public event EventHandler<MSHtmlEventArgs> DocumentMouseUp;
        public event EventHandler<MSHtmlEventArgs> DocumentMouseOut;
        public event EventHandler<MSHtmlEventArgs> DocumentMouseOver;
        public event EventHandler<MSHtmlEventArgs> DocumentMouseWheel;
        public event EventHandler<MSHtmlEventArgs> DocumentPropertyChange;
        public event EventHandler<MSHtmlEventArgs> DocumentReadyStateChange;
        public event EventHandler<MSHtmlEventArgs> DocumentRowEnter;
        public event EventHandler<MSHtmlEventArgs> DocumentRowExit;
        public event EventHandler<MSHtmlEventArgs> DocumentRowsDelete;
        public event EventHandler<MSHtmlEventArgs> DocumentRowsInserted;
        public event EventHandler<MSHtmlEventArgs> DocumentSelectionChange;
        public event EventHandler<MSHtmlEventArgs> DocumentSelectStart;
        public event EventHandler<MSHtmlEventArgs> DocumentStop;

        private class HandleWebBrowserDhtmlEvents : 
            HTMLDocumentEvents2
        {
            private readonly ExtendedWebBrowser _webBrowser;

            public HandleWebBrowserDhtmlEvents(ExtendedWebBrowser webBrowser)
            {
                _webBrowser = webBrowser;
            }

            public void onactivate(IHTMLEventObj e)
            {
                _webBrowser.DocumentActivate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onafterupdate(IHTMLEventObj e)
            {
                _webBrowser.DocumentAfterUpdate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onbeforeactivate(IHTMLEventObj e)
            {
                _webBrowser.DocumentBeforeActivate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public bool onbeforedeactivate(IHTMLEventObj e)
            {
                _webBrowser.DocumentBeforeDeactivate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void onbeforeeditfocus(IHTMLEventObj e)
            {
                _webBrowser.DocumentBeforeEditFocus?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onbeforeupdate(IHTMLEventObj e)
            {
                _webBrowser.DocumentBeforeUpdate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void oncellchange(IHTMLEventObj e)
            {
                _webBrowser.DocumentCellChange?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onclick(IHTMLEventObj e)
            {
                _webBrowser.DocumentClick?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public bool oncontextmenu(IHTMLEventObj e)
            {
                _webBrowser.DocumentContextMenu?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public bool oncontrolselect(IHTMLEventObj e)
            {
                _webBrowser.DocumentControlSelect?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void ondataavailable(IHTMLEventObj e)
            {
                _webBrowser.DocumentDataAvailable?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void ondatasetchanged(IHTMLEventObj e)
            {
                _webBrowser.DocumentDataSetChanged?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void ondatasetcomplete(IHTMLEventObj e)
            {
                _webBrowser.DocumentDataSetComplete?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool ondblclick(IHTMLEventObj e)
            {
                _webBrowser.DocumentDoubleClick?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void ondeactivate(IHTMLEventObj e)
            {
                _webBrowser.DocumentDeactivate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool ondragstart(IHTMLEventObj e)
            {
                _webBrowser.DocumentDragStart?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public bool onerrorupdate(IHTMLEventObj e)
            {
                _webBrowser.DocumentErrorUpdate?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void onfocusin(IHTMLEventObj e)
            {
                _webBrowser.removeAcceptAndCancelButtons();
                //_webBrowser.Document?.Body?.Focus();

                _webBrowser.DocumentFocusIn?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onfocusout(IHTMLEventObj e)
            {
                _webBrowser.DocumentFocusOut?.Invoke(_webBrowser, new MSHtmlEventArgs(e));

                _webBrowser.restoreAcceptAndCancelButtons();
            }

            public bool onhelp(IHTMLEventObj e)
            {
                _webBrowser.DocumentHelp?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void onkeydown(IHTMLEventObj e)
            {
                _webBrowser.DocumentKeyDown?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onkeypress(IHTMLEventObj e)
            {
                _webBrowser.DocumentKeyPress?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void onkeyup(IHTMLEventObj e)
            {
                _webBrowser.DocumentKeyUp?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onmousedown(IHTMLEventObj e)
            {
                _webBrowser.DocumentMouseDown?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onmousemove(IHTMLEventObj e)
            {
                _webBrowser.DocumentMouseMove?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onmouseout(IHTMLEventObj e)
            {
                _webBrowser.DocumentMouseOut?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onmouseover(IHTMLEventObj e)
            {
                _webBrowser.DocumentMouseOver?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onmouseup(IHTMLEventObj e)
            {
                _webBrowser.DocumentMouseUp?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onmousewheel(IHTMLEventObj e)
            {
                _webBrowser.DocumentMouseWheel?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void onpropertychange(IHTMLEventObj e)
            {
                _webBrowser.DocumentPropertyChange?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onreadystatechange(IHTMLEventObj e)
            {
                _webBrowser.DocumentReadyStateChange?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onrowenter(IHTMLEventObj e)
            {
                _webBrowser.DocumentRowEnter?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onrowexit(IHTMLEventObj e)
            {
                _webBrowser.DocumentRowExit?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public void onrowsdelete(IHTMLEventObj e)
            {
                _webBrowser.DocumentRowsDelete?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onrowsinserted(IHTMLEventObj e)
            {
                _webBrowser.DocumentRowsInserted?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public void onselectionchange(IHTMLEventObj e)
            {
                _webBrowser.DocumentSelectionChange?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
            }

            public bool onselectstart(IHTMLEventObj e)
            {
                _webBrowser.DocumentSelectStart?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }

            public bool onstop(IHTMLEventObj e)
            {
                _webBrowser.DocumentStop?.Invoke(_webBrowser, new MSHtmlEventArgs(e));
                return true;
            }
        }
    }
}