﻿namespace ZetaHtmlEditControl.UI.EditControlBases
{
    using System;
    using System.Drawing;
    using System.Runtime.InteropServices;
    using System.Runtime.InteropServices.ComTypes;
    using System.Windows.Forms;
    using Code.PInvoke;
    using EditControlAbsoluteBases;
    using mshtml;
    using IDataObject = System.Runtime.InteropServices.ComTypes.IDataObject;

    public partial class CoreHtmlEditControl :
        ExtendedWebBrowser,
        UnsafeNativeMethods.IDocHostUIHandler,
        UnsafeNativeMethods.IServiceProvider,
        UnsafeNativeMethods.IOleClientSite
    {
        public const int ENointerface = unchecked((int)0x80004002);
        public const int ENotimpl = unchecked((int)0x80004001);
        public const int Sok = 0;
        private static readonly Guid IidIhtmlEditHost = new Guid(@"3050f6a0-98b5-11cf-bb82-00aa00bdce0b");
        private static readonly Guid SidShtmlEditHost = new Guid(@"3050f6a0-98b5-11cf-bb82-00aa00bdce0b");
        private readonly HtmlEditHost _editHost = new HtmlEditHost();

        private bool _customDocUIHandlerSet;
        private bool _clientSiteSet;

        private void checkConnect()
        {
            // 2005-09-02: Can be null if showing a full-window PDF-viewer.
            if (!_customDocUIHandlerSet /*EverInitialized &&*/)
            {
                var doc = Document?.DomDocument;
                if (doc != null)
                {
                    _customDocUIHandlerSet = true;

                    var cd = (UnsafeNativeMethods.ICustomDoc) doc;

                    // Set the IDocHostUIHandler.
                    cd.SetUIHandler(this);
                }
            }

            // --

            // 2005-09-02: Can be null if showing a full-window PDF-viewer.
            if (!_clientSiteSet /*EverInitialized &&*/ )
            {
                var axInstance = ActiveXInstance;
                if (axInstance != null)
                {
                    _clientSiteSet = true;

                    var oe = (UnsafeNativeMethods.IOleObject) axInstance;

                    // 2013-05-19, Uwe Keim:
                    // Hier wird konfiguriert, dass diese Klasse IServiceProvider-Anfragen
                    // erhalten kann.
                    oe.SetClientSite(this);
                }
            }
        }

        protected override void OnDidEverInitialize(EventArgs args)
        {
            base.OnDidEverInitialize(args);

            //checkConnect();
        }

        public CoreHtmlEditControl()
        {
            constructCoreHtmlEditControlBase();
            constructCoreHtmlEditControlTextAndImage();
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            _htmlConversionHelper?.Dispose();

            base.OnHandleDestroyed(e);
        }

        public bool IsDocumentLoaded { get; private set; }

        // Siehe andere "Bugfix 2015-11-02"-Kommentare.
        public IHTMLDocument2 DomDocument => EverInitialized ? Document?.DomDocument as IHTMLDocument2 : null;

        public int ShowContextMenu(
            int dwID,
            NativeMethods.POINT pt,
            object pcmdtReserved,
            object pdispReserved)
        {
            var kind =
                NativeMethods.ContextMenuKind.CONTEXT_MENU_DEFAULT;

            if (dwID == 0x02)
            {
                kind = NativeMethods.ContextMenuKind.CONTEXT_MENU_DEFAULT;
            }
            else if (dwID == 0x04)
            {
                kind = NativeMethods.ContextMenuKind.CONTEXT_MENU_CONTROL;
            }
            else if (dwID == 0x08)
            {
                kind = NativeMethods.ContextMenuKind.CONTEXT_MENU_TABLE;
            }
            else if (dwID == 0x10)
            {
                kind = NativeMethods.ContextMenuKind.CONTEXT_MENU_TEXTSELECT;
            }
            else if (dwID == 0x30)
            {
                kind = NativeMethods.ContextMenuKind.CONTEXT_MENU_ANCHOR;
            }
            else if (dwID == 0x20)
            {
                kind = NativeMethods.ContextMenuKind.CONTEXT_MENU_UNKNOWN;
            }

            var queryForStatus = pcmdtReserved as NativeMethods.IUnknown;
            var objectAtScreenCoordinates = pdispReserved as NativeMethods.IDispatch;

            if (OnNeedShowContextMenu(
                kind,
                new Point(pt.x, pt.y),
                queryForStatus,
                objectAtScreenCoordinates))
            {
                // Don't show MSHTML context menu but the one that will be attached
                // in a derived class.
                return NativeMethods.SRESULTS.S_OK;
            }
            else
            {
                // Let MSHTML show the context menu.
                return NativeMethods.SRESULTS.S_FALSE;
            }
        }

        public int GetHostInfo(
            NativeMethods.DOCHOSTUIINFO info)
        {
            info.cbSize = Marshal.SizeOf(typeof(NativeMethods.DOCHOSTUIINFO));
            info.dwFlags = (int)(
                NativeMethods.DOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DOUTERBORDER |
                NativeMethods.DOCHOSTUIFLAG.DOCHOSTUIFLAG_NO3DBORDER |

                // http://msdn.microsoft.com/library/default.asp?url=/workshop/browser/hosting/reference/enum/dochostuiflag.asp
                // set the DOCHOSTUIFLAG_THEME if you want your buttons to have the XP look.
                NativeMethods.DOCHOSTUIFLAG.DOCHOSTUIFLAG_THEME);

            // default indicates we don't have info.
            return NativeMethods.SRESULTS.S_OK;
        }

        public int ShowUI(
            int dwID,
            UnsafeNativeMethods.IOleInPlaceActiveObject activeObject,
            NativeMethods.IOleCommandTarget commandTarget,
            UnsafeNativeMethods.IOleInPlaceFrame frame,
            UnsafeNativeMethods.IOleInPlaceUIWindow doc)
        {
            // default means we don't have any UI, and control should show its UI
            return NativeMethods.SRESULTS.S_FALSE;
        }

        public int HideUI()
        {
            // we don't have UI by default, so just pretend we hid it
            return NativeMethods.SRESULTS.S_OK;
        }

        public virtual int UpdateUI()
        {
            if (IsDocumentLoaded && EverInitialized)
            {
                OnUpdateUI();
            }

            return NativeMethods.SRESULTS.S_OK;
        }

        public int EnableModeless(
            bool fEnable)
        {
            // We don't have any UI by default, so pretend we updated it.
            return NativeMethods.SRESULTS.S_OK;
        }

        public int OnDocWindowActivate(
            bool fActivate)
        {
            // We don't have any UI by default, so pretend we updated it.
            return NativeMethods.SRESULTS.S_OK;
        }

        public int OnFrameWindowActivate(
            bool fActivate)
        {
            // We don't have any UI by default, so pretend we updated it.
            return NativeMethods.SRESULTS.S_OK;
        }

        public int ResizeBorder(
            NativeMethods.COMRECT rect,
            UnsafeNativeMethods.IOleInPlaceUIWindow doc,
            bool fFrameWindow)
        {
            // We don't have any UI by default, so pretend we updated it.
            return NativeMethods.SRESULTS.S_OK;
        }

        public int TranslateAccelerator(
            ref NativeMethods.MSG msg,
            ref Guid group,
            int nCmdID)
        {
            // No translation here.
            return NativeMethods.SRESULTS.S_FALSE;
        }

        public int GetOptionKeyPath(
            string[] pbstrKey,
            int dw)
        {
            // No replacement option key.
            return NativeMethods.SRESULTS.S_FALSE;
        }

        public int GetDropTarget(
            UnsafeNativeMethods.IOleDropTarget pDropTarget,
            out UnsafeNativeMethods.IOleDropTarget ppDropTarget)
        {
            // no additional drop target
            ppDropTarget = pDropTarget;
            return NativeMethods.SRESULTS.S_FALSE;
        }

        /// <summary>
        /// Gets the external.
        /// </summary>
        /// <param name="ppDispatch">The pp dispatch.</param>
        /// <returns></returns>
        public int GetExternal(
            out object ppDispatch)
        {
            // window.external from JavaScript.

            ppDispatch = this;
            return NativeMethods.SRESULTS.S_OK;
        }

        public int TranslateUrl(
            int dwTranslate,
            string strUrlIn,
            out string pstrUrlOut)
        {
            // no translation happens by default
            pstrUrlOut = strUrlIn;
            return NativeMethods.SRESULTS.S_FALSE;
        }

        public int FilterDataObject(
            IDataObject pDo,
            out IDataObject ppDoRet)
        {
            // no data object by default
            ppDoRet = pDo;
            return NativeMethods.SRESULTS.S_FALSE;
        }

        int UnsafeNativeMethods.IOleClientSite.SaveObject()
        {
            return ENotimpl;
        }

        int UnsafeNativeMethods.IOleClientSite.GetMoniker(uint dwAssign, uint dwWhichMoniker, out IMoniker ppmk)
        {
            ppmk = null;
            return ENotimpl;
        }

        int UnsafeNativeMethods.IOleClientSite.ShowObject()
        {
            return Sok;
        }

        int UnsafeNativeMethods.IOleClientSite.OnShowWindow(bool fShow)
        {
            return ENotimpl;
        }

        int UnsafeNativeMethods.IOleClientSite.RequestNewObjectLayout()
        {
            return ENotimpl;
        }

        int UnsafeNativeMethods.IServiceProvider.QueryService(ref Guid guidService, ref Guid riid, out IntPtr ppvObject)
        {
            if (guidService.Equals(SidShtmlEditHost) && riid.Equals(IidIhtmlEditHost))
            {
                ppvObject = Marshal.GetComInterfaceForObject(_editHost, typeof(IHTMLEditHost));
                return Sok;
            }
            else
            {
                ppvObject = IntPtr.Zero;
                return ENointerface;
            }
        }

        private void constructCoreHtmlEditControlBase()
        {
            // Nichts bisher.
        }

        public object OleCommandTargetExecute(int cmdId, params object[] arguments)
        {
            if (EverInitialized && Document != null)
            {
                var cmdt = (NativeMethods.IOleCommandTarget)Document.DomDocument;
                var cgidMshtml = new Guid(@"DE4BA900-59CA-11CF-9592-444553540000");
                var retVal = new object[] { null };

                cmdt.Exec(ref cgidMshtml, cmdId, 0, arguments, retVal);

                return retVal[0];
            }
            else
            {
                return null;
            }
        }

        protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
        {
            base.OnNavigated(e);

            IsDocumentLoaded = true;

            //checkConnect();
        }

        protected override void OnDocumentCompleted(WebBrowserDocumentCompletedEventArgs e)
        {
            base.OnDocumentCompleted(e);

            checkConnect();
        }

        protected virtual void OnUpdateUI()
        {
        }

        protected virtual bool OnNeedShowContextMenu(
            NativeMethods.ContextMenuKind contextMenuKind,
            Point position,
            NativeMethods.IUnknown queryForStatus,
            NativeMethods.IDispatch objectAtScreenCoordinates)
        {
            return true;
        }
    }
}