namespace ZetaHtmlEditControl.UI.EditControlDerives
{
    using mshtml;

    /// <summary>
    /// Zentrale Stelle für grundlegende kann/kann-nicht und aktuelle Zustände abfragen.
    /// </summary>
    public partial class HtmlEditControl
    {
        public bool IsBold => EverInitialized && (bool) DomDocument.queryCommandValue(@"Bold");

        public bool IsItalic => EverInitialized && (bool) DomDocument.queryCommandValue(@"Italic");

        public bool IsUnderline => EverInitialized && (bool) DomDocument.queryCommandValue(@"Underline");

        public bool IsOrderedList => EverInitialized && (bool) DomDocument.queryCommandValue(@"InsertOrderedList");

        public bool IsBullettedList => EverInitialized && (bool) DomDocument.queryCommandValue(@"InsertUnorderedList");

        public int FontSize => EverInitialized ? DomDocument.queryCommandValue(@"FontSize") as int? ?? 0 : 0;

        public string FontName => EverInitialized ? (string) DomDocument.queryCommandValue(@"FontName") : string.Empty;

        public bool IsJustifyLeft => EverInitialized && (bool) DomDocument.queryCommandValue(@"JustifyLeft");

        public bool IsJustifyCenter => EverInitialized && (bool) DomDocument.queryCommandValue(@"JustifyCenter");

        public bool IsJustifyRight => EverInitialized && (bool) DomDocument.queryCommandValue(@"JustifyRight");

        internal bool CanOutdent => EverInitialized && Document != null && (Enabled &&
                                                                            ((HTMLDocument) Document.DomDocument)
                                                                                .queryCommandEnabled(
                                                                                    @"Outdent"));

        internal bool CanOrderedList => EverInitialized && Document != null && (Enabled &&
                                                                                ((HTMLDocument) Document.DomDocument)
                                                                                    .queryCommandEnabled(
                                                                                        @"InsertOrderedList"));

        public bool CanUndo => EverInitialized && DomDocument.queryCommandEnabled(@"Undo");

        internal bool CanJustifyRight => EverInitialized && Document != null && (Enabled &&
                                                                                 ((HTMLDocument) Document.DomDocument)
                                                                                     .queryCommandEnabled(
                                                                                         @"JustifyRight"));

        internal bool CanRemoveFormatting
            => EverInitialized && Document != null && Enabled && (IsTextSelection || IsNoneSelection);

        internal bool CanJustifyLeft => EverInitialized && Document != null && (Enabled &&
                                                                                ((HTMLDocument) Document.DomDocument)
                                                                                    .queryCommandEnabled(
                                                                                        @"JustifyLeft"));

        internal bool CanJustifyCenter => EverInitialized && Document != null && (Enabled &&
                                                                                  ((HTMLDocument) Document.DomDocument)
                                                                                      .queryCommandEnabled
                                                                                      (@"JustifyCenter"));

        internal bool CanIndent => EverInitialized && Document != null && (Enabled &&
                                                                           ((HTMLDocument) Document.DomDocument)
                                                                               .queryCommandEnabled(
                                                                                   @"Indent"));

        internal bool CanDelete => EverInitialized && Document != null && (Enabled &&
                                                                           ((HTMLDocument) Document.DomDocument)
                                                                               .queryCommandEnabled(
                                                                                   @"Delete"));

        internal bool CanPaste => EverInitialized && Document != null && (Enabled &&
                                                                          ((HTMLDocument) Document.DomDocument)
                                                                              .queryCommandEnabled(
                                                                                  @"Paste"));

        internal bool CanCopy
            => EverInitialized && Document != null && ((HTMLDocument) Document.DomDocument).queryCommandEnabled(@"Copy")
            ;

        private bool CanCut => EverInitialized && Document != null && (Enabled &&
                                                                       ((HTMLDocument) Document.DomDocument)
                                                                           .queryCommandEnabled(@"Cut"));

        internal bool CanItalic => EverInitialized && Document != null && (Enabled &&
                                                                           ((HTMLDocument) Document.DomDocument)
                                                                               .queryCommandEnabled(
                                                                                   @"Italic"));

        internal bool CanUnderline => EverInitialized && Document != null && (Enabled &&
                                                                              ((HTMLDocument) Document.DomDocument)
                                                                                  .queryCommandEnabled(
                                                                                      @"Underline"));

        internal bool CanBold => EverInitialized && Document != null && (Enabled &&
                                                                         ((HTMLDocument) Document.DomDocument)
                                                                             .queryCommandEnabled(@"Bold"))
            ;

        internal bool CanChangeFont => EverInitialized && Document != null && Enabled;

        internal bool CanBullettedList => EverInitialized && Document != null && (Enabled &&
                                                                                  ((HTMLDocument) Document.DomDocument)
                                                                                      .queryCommandEnabled
                                                                                      (@"InsertUnorderedList"));

        internal bool CanForeColor => EverInitialized && Document != null && (Enabled &&
                                                                              ((HTMLDocument) Document.DomDocument)
                                                                                  .queryCommandEnabled(
                                                                                      @"ForeColor"));

        internal bool CanBackColor => EverInitialized && Document != null && (Enabled &&
                                                                              ((HTMLDocument) Document.DomDocument)
                                                                                  .queryCommandEnabled(
                                                                                      @"BackColor"));

        internal bool CanInsertHyperlink => EverInitialized && Document != null && (Enabled &&
                                                                                    ((HTMLDocument) Document.DomDocument)
                                                                                        .queryCommandEnabled(
                                                                                            @"CreateLink"));

        internal bool CanShowSource => EverInitialized;

        public bool CanTableProperties => EverInitialized && IsTableCurrentSelectionInsideTable;

        public bool CanAddTableRow => EverInitialized && IsTableCurrentSelectionInsideTable;

        public bool CanAddTableColumn => EverInitialized && IsTableCurrentSelectionInsideTable;

        public bool CanInsertTable => EverInitialized;

        public bool CanInsertTableRow => EverInitialized && IsTableCurrentSelectionInsideTable;

        public bool CanInsertTableColumn => EverInitialized && IsTableCurrentSelectionInsideTable;

        public bool CanTableDeleteRow => EverInitialized && CurrentSelectionTableCell != null;

        public bool CanTableDeleteColumn => EverInitialized && CurrentSelectionTableCell != null;

        public bool CanTableDeleteTable => EverInitialized && CurrentSelectionTable != null;

        public bool CanTableRowProperties => EverInitialized && CurrentSelectionTableRow != null;

        public bool CanTableColumnProperties => EverInitialized && CurrentSelectionTableCell != null;

        public bool CanTableCellProperties
        {
            get
            {
                if (!EverInitialized) return false;
                var cells = CurrentSelectionTableCells;
                return cells != null && cells.Length > 0;
            }
        }
    }
}