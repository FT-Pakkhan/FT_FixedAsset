using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DisableButtonController : ViewController
    {
        public DisableButtonController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.

            switch (View.Id)
            {
                case "FTS11OFAM_FTS11_vw_FTS11JDT1_ListView":
                case "FTS11OFAM_FTS11_vw_FTS11JDT1A_ListView":
                case "FTS11OFAM_FTS11_vw_Component_ListView":
                case "FTS11_vw_NNM1_LookupListView":
                case "FTS11_vw_OACT_LookupListView":
                case "FTS11_vw_OACT_OCRD_LookupListView":
                case "FTS11_vw_OCRN_LookupListView":
                case "FTS11_vw_OPRC_LookupListView":
                case "FTS11_vw_OPRJ_LookupListView":
                case "FTS11OFAM_LookupListView":
                case "FTS11OFAC_LookupListView":
                case "FTS11OFAG_LookupListView":
                case "FTS11OLOC_LookupListView":
                case "FTS11OIND_LookupListView":
                case "FTS11T0201_ListView":
                case "FTS11T0202_ListView":
                case "FTS11T0203_ListView":
                case "FTS11T0204_ListView":
                case "FTS11T0205_ListView":
                case "FTS11T0301_ListView":
                case "FTS11T0302_ListView":
                case "FTS11T0303_ListView":
                case "FTS11T0304_ListView":
                case "FTS11T0305_ListView":
                case "FTS11T0201_FTS11T0201D_ListView":
                case "FTS11T0202_FTS11T0202D_ListView":
                case "FTS11T0203_FTS11T0203D_ListView":
                case "FTS11T0204_FTS11T0204D_ListView":
                case "FTS11T0205_FTS11T0205D_ListView":
                case "FTS11T0301_FTS11T0301D_ListView":
                case "FTS11T0302_FTS11T0302D_ListView":
                case "FTS11T0303_FTS11T0303D_ListView":
                case "FTS11T0304_FTS11T0304D_ListView":
                case "FTS11T0305_FTS11T0305D_ListView":
                    ASPxGridListEditor gridListEditor = ((ListView)View).Editor as ASPxGridListEditor;
                    if (gridListEditor != null)
                    {
                        ((ASPxGridViewContextMenu)gridListEditor.ContextMenuTemplate).ControlsCreated += HideEditColumnController_ControlsCreated;
                    }
                    break;
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        void HideEditColumnController_ControlsCreated(object sender, EventArgs e)
        {
            ASPxGridViewContextMenu contextMenu = (ASPxGridViewContextMenu)sender;
            contextMenu.ControlsCreated -= HideEditColumnController_ControlsCreated;
            foreach (GridViewColumn column in contextMenu.Editor.Grid.Columns)
            {
                if (column is GridViewDataActionColumn && ((GridViewDataActionColumn)column).Action.Id == "Edit")
                {
                    column.Visible = false;
                }
            }
        }
    }
}
