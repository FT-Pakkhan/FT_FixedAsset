using DevExpress.Data;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;

namespace FT_FixedAsset.Module.Web.Editors
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ListViewSumTxtEditor : ViewController<ListView>
    {
        public ListViewSumTxtEditor()
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
            ASPxGridListEditor editor = ((ListView)View).Editor as ASPxGridListEditor;
            if (editor != null)
            {
                editor.Grid.GroupSummary.Clear();
                ASPxSummaryItem groupSummary = new ASPxSummaryItem();

                groupSummary.FieldName = "CostFC";
                groupSummary.SummaryType = SummaryItemType.Sum;
                groupSummary.ShowInGroupFooterColumn = "CostFC";
                groupSummary.DisplayFormat = "{0:n2}";

            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

    }
}
