using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AI_PlugIn.DataStructures;

namespace AI_PlugIn.Utilities
{
    internal static class OutlookUtilities
    {

        internal static Result<string> GetTextFromSelectedMailItem()
        {
            Result<string> result = new Result<string>();
            try
            {
                (MailItem mail, Inspector inspector) = OutlookUtilities.TryGetCurrentMailItem();

                if (mail == null)
                {
                    result.ErrorEncountered = true;
                    result.Message = "No mail item is selected or open.";
                    return result;
                }

                if (inspector != null)
                {
                    string selected_text = OutlookUtilities.GetSelectedText(inspector);
                    if (selected_text.Length > 5) //Selection must be > 6 chartecters to prevent selection false positives.
                    {
                        result.Value = selected_text;
                        return result;
                    }
                }

                string subject = mail.Subject ?? "";
                string body = mail.Body ?? "";
                string htmlBody = mail.HTMLBody ?? "";

                result.Value = $"Subject: {subject}\n\nBody preview:\n{body.Substring(0, Math.Min(body.Length, 300))}";
                return result;
            }
            catch (System.Exception ex)
            {
                result.ErrorEncountered = true;
                result.Message = "Failed to read email content:\n" + ex.Message;
                return result;
            }
        }

        /// <summary>
        /// Works in all cases:
        /// - Inspector (popped-out read or compose)
        /// - Explorer inline compose (reply/forward in reading pane)
        /// - Explorer selection (reading pane)
        /// Returns null if nothing applies.
        /// </summary>
        private static (MailItem mailItem, Inspector inspector) TryGetCurrentMailItem()
        {
            var app = Globals.ThisAddIn.Application;
            object window = app.ActiveWindow();

            if (window is Explorer explorer)
            {
                // Inline compose (reply/forward in reading pane)
                try
                {
                    if (explorer.ActiveInlineResponse is MailItem inline) { return (inline, null); }
                }
                catch { /* some profiles can throw; ignore */ }

                // Selection in reading pane
                var selection = explorer.Selection;
                if (selection != null && selection.Count > 0 && selection[1] is MailItem selected) { return (selected, null); }

                return (null, null);
            }

            // If an Inspector is the active window (you clicked a popped-out mail’s ribbon button)
            if (window is Inspector inspector && inspector.CurrentItem is MailItem mail) { return (mail, inspector); }

            return (null, null);
        }

        /// <summary>
        /// Gets selected text from mail item inspector.
        /// </summary>
        /// <param name="inspector"></param>
        /// <returns></returns>
        private static string GetSelectedText(Inspector inspector)
        {
            string selectedText = string.Empty;
            if (inspector != null && inspector.EditorType == OlEditorType.olEditorWord)
            {
                Document wordDoc = (Document)inspector.WordEditor;
                Microsoft.Office.Interop.Word.Application wordApp = wordDoc.Application;
                Microsoft.Office.Interop.Word.Selection selection = wordApp.Selection;

                selectedText = selection.Text;
            }
            return selectedText;
        }
    }
}
