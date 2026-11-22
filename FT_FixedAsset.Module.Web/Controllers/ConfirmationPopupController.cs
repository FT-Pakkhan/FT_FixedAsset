using System.Collections.Generic;
using System.ComponentModel;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Web.Controllers
{
    /// <summary>
    /// Replaces the browser confirm box ("localhost says ... OK / Cancel") of every button that has a
    /// ConfirmationMessage (Run Depreciation, Post, Delete, ...) with the same centred Yes / No popup
    /// used by Cancel Assets / Cancel Trans. The message texts stay where they are defined.
    /// Yes runs the button exactly as before; No does nothing.
    /// </summary>
    public class ConfirmationPopupController : ViewController
    {
        private readonly Dictionary<SimpleAction, string> messages = new Dictionary<SimpleAction, string>();
        private readonly List<SimpleAction> hooked = new List<SimpleAction>();
        private SimpleAction confirmedAction;   // the button being run after "Yes"

        protected override void OnActivated()
        {
            base.OnActivated();
            CaptureMessages();
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            CaptureMessages();   // model-defined messages (e.g. Delete) may be applied after activation
        }

        protected override void OnDeactivated()
        {
            foreach (SimpleAction a in hooked) a.Executing -= Action_Executing;
            hooked.Clear();
            foreach (var pair in messages) pair.Key.ConfirmationMessage = pair.Value;   // leave the actions as we found them
            messages.Clear();
            base.OnDeactivated();
        }

        private void CaptureMessages()
        {
            if (Frame == null) return;
            foreach (Controller c in Frame.Controllers)
            {
                foreach (ActionBase action in c.Actions)
                {
                    var a = action as SimpleAction;   // only plain buttons; other action kinds keep their own behaviour
                    if (a == null || string.IsNullOrEmpty(a.ConfirmationMessage)) continue;
                    messages[a] = a.ConfirmationMessage;
                    a.ConfirmationMessage = null;     // no browser confirm box
                    if (!hooked.Contains(a))
                    {
                        a.Executing += Action_Executing;
                        hooked.Add(a);
                    }
                }
            }
        }

        private void Action_Executing(object sender, CancelEventArgs e)
        {
            var a = (SimpleAction)sender;
            if (ReferenceEquals(a, confirmedAction)) return;   // "Yes" was clicked: run normally
            string message;
            if (!messages.TryGetValue(a, out message)) return;

            e.Cancel = true;   // ask first
            ShowConfirmation(a, Formatted(a, message));
        }

        // fills placeholders such as {0} (e.g. the view name in "Reset View Settings") the same way XAF does for its own confirm box
        private static string Formatted(SimpleAction a, string message)
        {
            if (message.IndexOf('{') < 0) return message;
            try
            {
                a.ConfirmationMessage = message;
                return a.GetFormattedConfirmationMessage() ?? message;
            }
            catch
            {
                return message;
            }
            finally
            {
                a.ConfirmationMessage = null;
            }
        }

        private void ShowConfirmation(SimpleAction a, string message)
        {
            IObjectSpace os = Application.CreateObjectSpace(typeof(CancelConfirmation));
            CancelConfirmation confirm = os.CreateObject<CancelConfirmation>();
            confirm.Message = message;

            DetailView dv = Application.CreateDetailView(os, confirm, false);
            dv.ViewEditMode = ViewEditMode.View;
            dv.Caption = string.IsNullOrEmpty(a.Caption) ? "Confirm" : a.Caption.Replace("&", "");
            if (dv.FindItem("MessageText") is StaticText messageText) messageText.Text = message;

            Application.ShowViewStrategy.ShowViewInPopupWindow(dv, () =>
            {
                confirmedAction = a;
                try { a.DoExecute(); }
                finally { confirmedAction = null; }
            }, null, "Yes", "No");
        }
    }
}
