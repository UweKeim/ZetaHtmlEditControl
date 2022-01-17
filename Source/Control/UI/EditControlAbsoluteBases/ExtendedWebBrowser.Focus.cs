namespace ZetaHtmlEditControl.UI.EditControlAbsoluteBases
{
    using System.Windows.Forms;

    public partial class ExtendedWebBrowser
    {
        // 2015-11-10, Uwe Keim:
        //
        // Seit meinem Fix mit den mehreren blinkenden Carets bei mehreren HTML-Editoren
        // auf einem Formular, habe ich das Phänomen, dass sich der Editor wie ein
        // einzeiliges Textfeld verhält, und zwar sobald er einen AcceptButton und CancelButton
        // zugeordnet bekommen hat.
        //
        // Also Tasten wie TAB, RETURN, ESC, usw. bewirken nicht wie bisher ein Agieren
        // _innerhalb_ des Editors, sondern werden an den Dialog weitergegeben.
        //
        // Die Ursache ist mir noch unklar; bis ich das sauber hinbekomme, werde ich eine
        // Lösung wie in http://stackoverflow.com/a/3761156/107625 beschrieben umsetzen:
        // Beim Fokus-ENTER den Dialog um die Button-Zuordnungen entfernen und beim Fokus-LOST
        // entsprechend wieder zurücksetzen auf den gemerkten Wert vor dem Entfernen.

        private IButtonControl _acceptButton;
        private IButtonControl _cancelButton;

        private void removeAcceptAndCancelButtons()
        {
            var form = FindForm();
            if (form != null)
            {
                if (_acceptButton == null) _acceptButton = form.AcceptButton;
                if (_cancelButton == null) _cancelButton = form.CancelButton;

                form.AcceptButton = null;
                form.CancelButton = null;
            }
        }

        private void restoreAcceptAndCancelButtons()
        {
            var form = FindForm();
            if (form != null)
            {
                form.AcceptButton = _acceptButton;
                form.CancelButton = _cancelButton;
            }
        }
    }
}