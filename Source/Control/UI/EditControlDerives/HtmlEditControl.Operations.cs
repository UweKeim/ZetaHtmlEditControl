namespace ZetaHtmlEditControl.UI.EditControlDerives
{
    using System.Windows.Forms;
    using Code.MsHtml;
    using Code.PInvoke;
    using mshtml;
    using Properties;
    using Tools;

    public partial class HtmlEditControl
    {
        private void ExecuteSystemInfo()
        {
            var msg = $@"URL: {Url}.";

            MessageBox.Show(
                FindForm(),
                msg,
                Resources.HtmlEditControl_ExecuteSystemInfo_Information,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        private void ExecuteSelectAll()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;
                doc.execCommand(@"SelectAll", false, null);
            }
        }

        internal void ExecuteUnderline()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;
                doc.execCommand(@"Underline", false, null);
            }
        }

        private void ExecuteRedo()
        {
            if (!EverInitialized) return;

            Document?.ExecCommand(@"Redo", false, null);
        }

        internal void ExecuteUndo()
        {
            if (!EverInitialized) return;

            Document?.ExecCommand(@"Undo", false, null);
        }

        public void ExecutePrintPreview()
        {
            if (!EverInitialized) return;

            if (Document != null && Configuration.AllowPrint)
            {
                OleCommandTargetExecute(NativeMethods.IdmPrintpreview, null);
            }
        }

        public void ExecutePrint()
        {
            if (!EverInitialized) return;

            if (Document != null && Configuration.AllowPrint)
            {
                OleCommandTargetExecute(NativeMethods.IdmPrint, null);
            }
        }

        internal void ExecuteBold()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;
                doc.execCommand(@"Bold", false, null);
            }
        }

        //commands list
        //https://developer.mozilla.org/en/docs/Rich-Text_Editing_in_Mozilla
        internal void ExecuteFontSize(string newSize)
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;
                doc.execCommand(@"FontSize", false, newSize);
            }
        }

        internal void ExecuteFontName(string name)
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;
                doc.execCommand(@"FontName", false, name);
            }
        }

        internal void ExecuteItalic()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc =
                    (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"Italic", false, null);
            }
        }

        internal void ExecutePaste()
        {
            if (!EverInitialized) return;

            handlePaste(PasteMode.Normal);
        }

        internal void ExecutePasteAsText()
        {
            if (!EverInitialized) return;

            handlePaste(PasteMode.Text);
        }

        internal void ExecutePasteFromWord()
        {
            if (!EverInitialized) return;

            handlePaste(PasteMode.MsWord);
        }

        internal void ExecuteShowSource()
        {
            if (!EverInitialized) return;

            using (var form = new HtmlSourceTextEditForm(DocumentText))
            {
                form.ExternalInformationProvider = Configuration?.ExternalInformationProvider;

                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    DocumentText = form.HtmlText;
                    updateUI();
                }
            }
        }

        internal void ExecuteInsertHyperlink()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"CreateLink", true, null);
            }
        }

        internal void ExecuteIndent()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc =
                    (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"Indent", false, null);
            }
        }

        internal void ExecuteJustifyCenter()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc =
                    (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"JustifyCenter", false, null);
            }
        }

        internal void ExecuteJustifyLeft()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc =
                    (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"JustifyLeft", false, null);
            }
        }

        internal void ExecuteJustifyRight()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"JustifyRight", false, null);
            }
        }

        internal void ExecuteNumberedList()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"InsertOrderedList", false, null);
            }
        }

        internal void ExecuteOutdent()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"Outdent", false, null);
            }
        }

        internal void ExecuteBullettedList()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"InsertUnorderedList", false, null);
            }
        }

        internal void ExecuteCopy()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"Copy", false, null);
            }
        }

        internal void ExecuteCut()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"Cut", false, null);
            }
        }

        internal void ExecuteDelete()
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"Delete", false, null);
            }
        }

        public void ExecuteInsertTable()
        {
            if (!EverInitialized) return;

            using (var form = new HtmlEditorTableNewForm())
            {
                form.ExternalInformationProvider = Configuration?.ExternalInformationProvider;

                if (form.ShowDialog(FindForm()) == DialogResult.OK)
                {
                    InsertHtmlAtCurrentSelection(form.Html);
                }
            }
        }

        public void ExecuteInsertTableRow()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;

            if (table != null)
            {
                int rowIndex = CurrentSelectionTableRowIndex;

                var row =
                    HtmlEditorTableNewForm.AddTableRowsAfterRow(
                        table,
                        rowIndex,
                        1);

                // Set focus to first cell in the new line.
                if (row != null)
                {
                    var cell = row.cells.item(0, 0) as IHTMLTableCell;
                    MoveCaretToElement(cell as IHTMLElement);
                }
            }
        }

        internal void ExecuteSetBackColor05()
        {
            if (!EverInitialized) return;

            setBackColor(@"ff00ff");
        }

        public void ExecuteInsertTableColumn()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;

            if (table != null)
            {
                var columnIndex = CurrentSelectionTableColumnIndex;

                HtmlEditorTableNewForm.AddTableColumnsAfterColumn(
                    table,
                    columnIndex,
                    1);
            }
        }

        public void ExecuteTableAddTableRow()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;

            if (table != null)
            {
                var row = HtmlEditorTableNewForm.AddTableRowsAtBottom(table, 1);

                MoveCaretToElement(row.cells.item(0, 0) as IHTMLElement);
            }
        }

        public void ExecuteTableAddTableColumn()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;

            if (table != null)
            {
                HtmlEditorTableNewForm.AddTableColumnsAtRight(table, 1);
            }
        }

        public void ExecuteTableProperties()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;

            if (table != null)
            {
                using (var form = new HtmlEditorTableNewForm())
                {
                    form.ExternalInformationProvider = Configuration?.ExternalInformationProvider;

                    form.Table = table;
                    form.ShowDialog(FindForm());
                }
            }
        }

        public void ExecuteTableDeleteRow()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;
            var rowIndex = CurrentSelectionTableRowIndex;

            if (table != null && rowIndex != -1)
            {
                table.deleteRow(rowIndex);
            }
        }

        public void ExecuteTableDeleteColumn()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;
            var columnIndex = CurrentSelectionTableColumnIndex;

            if (table != null && columnIndex != -1)
            {
                var rows = table.rows;

                if (rows != null)
                {
                    for (var i = 0; i < rows.length; ++i)
                    {
                        var row = rows.item(i, i) as IHTMLTableRow;
                        row?.deleteCell(columnIndex);
                    }
                }
            }
        }

        public void ExecuteTableDeleteTable()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;
            var tableNode = table as IHTMLDOMNode;
            tableNode?.removeNode(true);
        }

        public void ExecuteTableRowProperties()
        {
            if (!EverInitialized) return;

            var row = CurrentSelectionTableRow;

            if (row != null)
            {
                using (var form = new HtmlEditorCellPropertiesForm())
                {
                    form.ExternalInformationProvider = Configuration == null
                        ? null
                        : Configuration.ExternalInformationProvider;

                    form.Initialize(row);
                    form.ShowDialog(FindForm());
                }
            }
        }

        public void ExecuteTableColumnProperties()
        {
            if (!EverInitialized) return;

            var table = CurrentSelectionTable as IHTMLTable;
            var columnIndex = CurrentSelectionTableColumnIndex;

            if (table != null && columnIndex >= 0)
            {
                using (var form = new HtmlEditorCellPropertiesForm())
                {
                    form.ExternalInformationProvider = Configuration?.ExternalInformationProvider;

                    form.Initialize(table, columnIndex);
                    form.ShowDialog(FindForm());
                }
            }
        }

        public void ExecuteTableCellProperties()
        {
            if (!EverInitialized) return;

            var cells = CurrentSelectionTableCells;

            if (cells != null && cells.Length > 0)
            {
                using (var form = new HtmlEditorCellPropertiesForm())
                {
                    form.ExternalInformationProvider = Configuration?.ExternalInformationProvider;

                    form.Initialize(cells);
                    form.ShowDialog(FindForm());
                }
            }
        }

        internal void ExecuteSetForeColorNone()
        {
            if (!EverInitialized) return;

            setForeColor(MsHtmlLegacyFromBadToGoodTranslator.NoForegroundColor);
        }

        internal void ExecuteSetForeColor01()
        {
            if (!EverInitialized) return;

            setForeColor(@"c00000");
        }

        internal void ExecuteSetForeColor02()
        {
            if (!EverInitialized) return;

            setForeColor(@"ff0000");
        }

        internal void ExecuteSetForeColor03()
        {
            if (!EverInitialized) return;

            setForeColor(@"ffc000");
        }

        internal void ExecuteSetForeColor04()
        {
            if (!EverInitialized) return;

            setForeColor(@"ffff00");
        }

        internal void ExecuteSetBackColorNone()
        {
            if (!EverInitialized) return;

            setBackColor(MsHtmlLegacyFromBadToGoodTranslator.NoBackgroundColor);
        }

        internal void ExecuteSetBackColor02()
        {
            if (!EverInitialized) return;

            setBackColor(@"00ff00");
        }

        internal void ExecuteSetBackColor03()
        {
            if (!EverInitialized) return;

            setBackColor(@"00ffff");
        }

        internal void ExecuteSetBackColor04()
        {
            if (!EverInitialized) return;

            setBackColor(@"ff0000");
        }

        internal void ExecuteSetForeColor05()
        {
            if (!EverInitialized) return;

            setForeColor(@"92d050");
        }

        internal void ExecuteSetForeColor06()
        {
            if (!EverInitialized) return;

            setForeColor(@"00b050");
        }

        internal void ExecuteSetForeColor07()
        {
            if (!EverInitialized) return;

            setForeColor(@"00b0f0");
        }

        internal void ExecuteSetForeColor08()
        {
            if (!EverInitialized) return;

            setForeColor(@"0070c0");
        }

        internal void ExecuteSetForeColor09()
        {
            if (!EverInitialized) return;

            setForeColor(@"002060");
        }

        internal void ExecuteSetForeColor10()
        {
            if (!EverInitialized) return;

            setForeColor(@"7030a0");
        }

        internal void ExecuteSetBackColor01()
        {
            if (!EverInitialized) return;

            setBackColor(@"ffff00");
        }

        private void setForeColor(
            string color)
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;

                doc.execCommand(@"ForeColor", false, @"#" + color.Trim('#'));
            }
        }

        private void setBackColor(
            string color)
        {
            if (!EverInitialized) return;

            if (Document != null)
            {
                var doc = (HTMLDocument) Document.DomDocument;
                doc.execCommand(@"BackColor", false, @"#" + color.Trim('#'));
            }
        }

        /// <summary>
        /// Entweder von Auswahl oder von allem.
        /// </summary>
        internal void ExecuteRemoveFormatting()
        {
            if (!EverInitialized) return;

            if (IsTextSelection)
            {
                var sel = CurrentSelectionText;
                sel?.execCommand(@"removeFormat", false, null);
            }
            else if (IsNoneSelection)
            {
                var range = CreateRangeOfWholeBody();
                range.execCommand(@"removeFormat", false, null);
            }
        }
    }
}