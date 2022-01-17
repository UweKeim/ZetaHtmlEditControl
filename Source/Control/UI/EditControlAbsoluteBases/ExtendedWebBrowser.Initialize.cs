namespace ZetaHtmlEditControl.UI.EditControlAbsoluteBases
{
    using System;
    using System.Windows.Forms;

    public partial class ExtendedWebBrowser
    {
        private bool _everInitialized;

        public bool EverInitialized => _everInitialized && IsHandleCreated;

        protected void MarkAsEverInitialized()
        {
            if (!_everInitialized)
            {
                _everInitialized = true;
                OnDidEverInitialize(EventArgs.Empty);
            }
        }

        protected void UnmarkAsEverInitialized()
        {
            _everInitialized = false;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            checkInitialize();
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);

            checkInitialize();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (_documentCompletedCount > 0)
            {
                checkInitialize();
            }
        }

        private void checkInitialize()
        {
            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode && !EverInitialized && IsHandleCreated)
            {
                OnCheckInitialize(EventArgs.Empty);
            }
        }

        protected virtual void OnCheckInitialize(EventArgs args)
        {
            var h = CheckInitialize;
            h?.Invoke(this, args);
        }

        protected virtual void OnDidEverInitialize(EventArgs args)
        {
            var h = DidEverInitialize;
            h?.Invoke(this, args);
        }

        public event EventHandler CheckInitialize;
        public event EventHandler DidEverInitialize;
    }
}