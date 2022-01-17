namespace ZetaHtmlEditControl.UI.EditControlDerives
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Windows.Forms;
    using Code.Configuration;
    using Code.PInvoke;
    using EditControlBases;
    using Helper;

    /// <summary>
    /// Edit control, primarily designed to work in conjunction
    /// with the ZetaHelpdesk application.
    /// </summary>
    /// <remarks>
    /// Oleg Shilo 22.05.2013, all code related to:
    ///   - EmbeddImages
    ///   - FontSize
    ///   - FontName
    ///   - PrintPreview
    ///   - Print
    /// </remarks>
    public partial class HtmlEditControl :
        CoreHtmlEditControl
    {
        private bool _everLoadedTextModules;
        private bool _firstCreate = true;
        private int _objectID = 1;
        private TextModuleInfo[] _textModules;
        private bool _textModulesFilled;
        private Timer _timerTextChange = new Timer();
        private string _tmpCacheTextChange = string.Empty;
        private string _tmpFolderPath = string.Empty;

        public HtmlEditControl()
        {
            InitializeComponent();

            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode)
            {
                _tmpFolderPath = Path.Combine(Path.GetTempPath(), @"zhe1-" + Guid.NewGuid());
                Directory.CreateDirectory(_tmpFolderPath);

                _timerTextChange.Tick += timerTextChange_Tick;
                _timerTextChange.Interval = 200;
                _timerTextChange.Start();

                // --

                constructHtmlEditControlKeyboard();

                Configure(Configuration);
            }
        }

        // Siehe andere "Bugfix 2015-11-02"-Kommentare.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool AllowWebBrowserDrop
        {
            get { return base.AllowWebBrowserDrop; }
            set { base.AllowWebBrowserDrop = value; }
        }

        // Siehe andere "Bugfix 2015-11-02"-Kommentare.
        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool IsWebBrowserContextMenuEnabled
        {
            get { return base.IsWebBrowserContextMenuEnabled; }
            set { base.IsWebBrowserContextMenuEnabled = value; }
        }

        [Browsable(false)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int TextChangeCheckInterval
        {
            get { return _timerTextChange.Interval; }
            set
            {
                if (value < 1000) //1 min
                {
                    _timerTextChange.Interval = value;
                }
            }
        }

        public bool HasTextModules
        {
            get
            {
                checkGetTextModules();
                return _textModules != null && _textModules.Length > 0;
            }
        }

        protected override void OnCheckInitialize(EventArgs args)
        {
            base.OnCheckInitialize(args);

            // --

            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode && !EverInitialized && IsHandleCreated)
            {
                var form = FindForm();
                if (form != null)
                {
                    // Uwe Keim: Bugfix 2015-11-02:
                    // 
                    // Wenn auf Document-Instanz zu früh zu gegriffen wird, hat das den Effekt,
                    // dass bei mehr als einem Editor auf einem Form, das Caret in allen
                    // Editoren blinkt.
                    //
                    // Deshalb habe ich hierher einige Funktionsaufrufe gesetzt, die vorher im
                    // Konstruktor standen.
                    //
                    // Das alleine hat es noch nicht ausgemacht, geholfen hat auch:
                    //
                    // Mehrfach den Fokus zwischen den einzelnen HTML-Editoren ändern,
                    // lässt den Fehler verschwinden, dass mehrere Carets gleichzeitig blinken.

                    if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode)
                    {
                        AllowWebBrowserDrop = false;

                        Navigate(@"about:blank");
                    }

                    // --

                    MarkAsEverInitialized();
                }
            }
        }

        protected override void OnHandleDestroyed(EventArgs e)
        {
            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode)
            {
                UnmarkAsEverInitialized();

                if (_timerTextChange != null)
                {
                    _timerTextChange.Stop();
                    _timerTextChange.Dispose();
                    _timerTextChange = null;
                }

                if (!string.IsNullOrEmpty(_tmpFolderPath))
                {
                    if (Directory.Exists(_tmpFolderPath)) Directory.Delete(_tmpFolderPath, true);
                    _tmpFolderPath = null;
                }
            }

            base.OnHandleDestroyed(e);
        }

        public override void Configure(HtmlEditControlConfiguration configuration)
        {
            base.Configure(configuration);

            _everLoadedTextModules = false; // Reset to force reload.
            updateUI();
        }

        public new HtmlDocument Document => EverInitialized ? base.Document : null;

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);

            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode && EverInitialized)
            {
                Document?.Body?.Focus();
            }
        }

        protected override void OnNavigated(WebBrowserNavigatedEventArgs e)
        {
            base.OnNavigated(e);

            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode)
            {
                if (_firstCreate)
                {
                    _firstCreate = false;

                    // 2012-08-28, Uwe Keim: Enable gray shortcut texts.
                    contextMenuStrip.Renderer = new MyToolStripRender();
                }
            }
        }

        private void timerTextChange_Tick(
            object sender,
            EventArgs e)
        {
            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode && !IsDisposed && IsHandleCreated)
            {
                var s = DocumentText ?? string.Empty;

                if (_tmpCacheTextChange != s)
                {
                    _tmpCacheTextChange = s;

                    var h = TextChanged;
                    h?.Invoke(this, new EventArgs());
                }
            }
        }

        public new event EventHandler TextChanged;

        private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
        {
            updateUI();
        }

        private void updateUI()
        {
            // Siehe andere "Bugfix 2015-11-02"-Kommentare.

            if ( EverInitialized && Document?.DomDocument != null)
            {
                boldToolStripMenuItem.Enabled = CanBold;
                italicToolStripMenuItem.Enabled = CanItalic;
                cutToolStripMenuItem.Enabled = CanCut;
                copyToolStripMenuItem.Enabled = CanCopy;
                pasteAsTextToolStripMenuItem.Enabled = CanPaste;
                pasteToolStripMenuItem.Enabled = CanPaste;
                pasteFromMsWordToolStripItem.Enabled = CanPaste;
                deleteToolStripMenuItem.Enabled = CanDelete;
                indentToolStripMenuItem.Enabled = CanIndent;
                justifyCenterToolStripMenuItem.Enabled = CanJustifyCenter;
                justifyLeftToolStripMenuItem.Enabled = CanJustifyLeft;
                justifyRightToolStripMenuItem.Enabled = CanJustifyRight;
                numberedListToolStripMenuItem.Enabled = CanOrderedList;
                outdentToolStripMenuItem.Enabled = CanOutdent;
                bullettedListToolStripMenuItem.Enabled = CanBullettedList;
                foreColorToolStripMenuItem.Enabled = CanForeColor;
                backColorToolStripMenuItem.Enabled = CanBackColor;
                hyperLinkToolStripMenuItem.Enabled = CanInsertHyperlink;
                htmlToolStripMenuItem.Enabled = CanShowSource;
                removeFormattingToolStripMenuItem.Enabled = CanRemoveFormatting;

                // --
                // Table menu.

                insertNewTableToolStripMenuItem.Enabled = CanInsertTable;
                insertRowBeforeCurrentRowToolStripMenuItem.Enabled = CanInsertTableRow;
                insertColumnBeforeCurrentColumnToolStripMenuItem.Enabled = CanInsertTableColumn;
                addRowAfterTheLastTableRowToolStripMenuItem.Enabled = CanAddTableRow;
                addColumnAfterTheLastTableColumnToolStripMenuItem.Enabled = CanAddTableColumn;
                tablePropertiesToolStripMenuItem.Enabled = CanTableProperties;
                rowPropertiesToolStripMenuItem.Enabled = CanTableRowProperties;
                columnPropertiesToolStripMenuItem.Enabled = CanTableColumnProperties;
                cellPropertiesToolStripMenuItem.Enabled = CanTableCellProperties;
                deleteRowToolStripMenuItem.Enabled = CanTableDeleteRow;
                deleteColumnToolStripMenuItem.Enabled = CanTableDeleteColumn;
                deleteTableToolStripMenuItem.Enabled = CanTableDeleteTable;

                // --

                textModulesToolStripMenuItem.Visible = HasTextModules;
                textModulesSeparator.Visible = HasTextModules;
            }
        }

        private void boldToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteBold();
        }

        private void italicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteItalic();
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecutePaste();
        }

        private void pasteAsTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecutePasteAsText();
        }

        private void htmlToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteShowSource();
        }

        private void hyperLinkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteInsertHyperlink();
        }

        private void indentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteIndent();
        }

        private void justifyCenterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteJustifyCenter();
        }

        private void justifyLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteJustifyLeft();
        }

        private void justifyRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteJustifyRight();
        }

        private void numberedListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteNumberedList();
        }

        private void outdentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteOutdent();
        }

        private void bullettedListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteBullettedList();
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteCopy();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteCut();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteDelete();
        }

        private void foreColorNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColorNone();
        }

        private void foreColor01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor01();
        }

        private void foreColor02ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor02();
        }

        private void foreColor03ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor03();
        }

        private void foreColor04ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor04();
        }

        private void foreColor05ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor05();
        }

        private void foreColor06ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor06();
        }

        private void foreColor07ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor07();
        }

        private void foreColor08ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor08();
        }

        private void foreColor09ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor09();
        }

        private void foreColor10ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetForeColor10();
        }

        private void backColorNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetBackColorNone();
        }

        private void backColor01ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetBackColor01();
        }

        private void backColor02ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetBackColor02();
        }

        private void backColor03ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetBackColor03();
        }

        private void backColor04ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetBackColor04();
        }

        private void backColor05ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteSetBackColor05();
        }

        private void insertNewTableToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteInsertTable();
        }

        private void insertRowBeforeCurrentRowToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteInsertTableRow();
        }

        private void insertColumnBeforeCurrentColumnToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteInsertTableColumn();
        }

        private void addRowAfterTheLastTableRowToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableAddTableRow();
        }

        private void addColumnAfterTheLastTableColumnToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableAddTableColumn();
        }

        private void tablePropertiesToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableProperties();
        }

        private void rowPropertiesToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableRowProperties();
        }

        private void columnPropertiesToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableColumnProperties();
        }

        private void cellPropertiesToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableCellProperties();
        }

        private void deleteRowToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableDeleteRow();
        }

        private void deleteColumnToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableDeleteColumn();
        }

        private void deleteTableToolStripMenuItem_Click(
            object sender,
            EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteTableDeleteTable();
        }

        protected override void OnUpdateUI()
        {
            if (!EverInitialized) return;
            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode)
            {
                var h = UINeedsUpdate;
                h?.Invoke(this, EventArgs.Empty);
            }
        }

        public event EventHandler UINeedsUpdate;

        internal void FillTextModules(
            ToolStripDropDownItem textModulesToolStripItem)
        {
            checkGetTextModules();

            textModulesToolStripItem.DropDownItems.Clear();

            foreach (var textModule in _textModules)
            {
                var mi = new ToolStripMenuItem(textModule.Name) { Tag = textModule };

                mi.Click += delegate
                {
                    var tm = (TextModuleInfo)mi.Tag;
                    InsertHtmlAtCurrentSelection(tm.Html);
                };

                textModulesToolStripItem.DropDownItems.Add(mi);
            }
        }

        private void checkGetTextModules()
        {
            if (!EverInitialized) return;
            if (Configuration?.ExternalInformationProvider != null && !_everLoadedTextModules)
            {
                _everLoadedTextModules = true;
                _textModules = Configuration.ExternalInformationProvider.GetTextModules();
            }
        }

        private void textModulesToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            checkFillTextModules(textModulesToolStripMenuItem);
        }

        private void checkFillTextModules(ToolStripDropDownItem toolStripMenuItem)
        {
            if (!EverInitialized) return;
            if (!_textModulesFilled)
            {
                _textModulesFilled = true;
                FillTextModules(toolStripMenuItem);
            }
        }

        protected override bool OnNeedShowContextMenu(
            NativeMethods.ContextMenuKind contextMenuKind,
            Point position,
            NativeMethods.IUnknown queryForStatus,
            NativeMethods.IDispatch objectAtScreenCoordinates)
        {
            //if (!EverInitialized) return true;

            base.OnNeedShowContextMenu(contextMenuKind, position, queryForStatus, objectAtScreenCoordinates);

            if (!DesignMode && !HtmlEditorDesignModeManager.IsDesignMode)
            {
                if (Configuration?.ExternalInformationProvider != null)
                {
                    var font = Configuration.ExternalInformationProvider.Font;
                    contextMenuStrip.Font = font ?? Font;

                    if (Configuration.ExternalInformationProvider.ForeColor.HasValue)
                    {
                        contextMenuStrip.ForeColor = Configuration.ExternalInformationProvider.ForeColor.Value;
                    }
                }
                else
                {
                    contextMenuStrip.Font = Font;
                }

                contextMenuStrip.Show(position);
            }

            return true;
        }

        private void underlineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteUnderline();
        }

        private void pasteFromMsWordToolStripItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecutePasteFromWord();
        }

        private void removeFormattingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!EverInitialized) return;
            ExecuteRemoveFormatting();
        }
    }
}