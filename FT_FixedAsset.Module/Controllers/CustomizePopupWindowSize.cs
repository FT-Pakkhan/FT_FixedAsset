using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web;
using System.Web.UI.WebControls;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CustomizePopupWindowSize : WindowController
    {
        public CustomizePopupWindowSize()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
            this.TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ((WebApplication)Application).PopupWindowManager.PopupShowing += PopupWindowManager_PopupShowing;
        }

        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void PopupWindowManager_PopupShowing(object sender, PopupShowingEventArgs e)
        {
            e.PopupControl.CustomizePopupWindowSize += XafPopupWindowControl_CustomizePopupWindowSize;
        }
        private void XafPopupWindowControl_CustomizePopupWindowSize(object sender, DevExpress.ExpressApp.Web.Controls.CustomizePopupWindowSizeEventArgs e)
        {
            e.Width = Unit.Percentage(80);
            e.Height = Unit.Percentage(65);
            e.ShowPopupMode = DevExpress.ExpressApp.Web.Controls.ShowPopupMode.Centered;
            e.Handled = true;
        }
    }
}
