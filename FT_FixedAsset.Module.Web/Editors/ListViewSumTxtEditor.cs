using DevExpress.Data;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Web.Editors
{
    public partial class ListViewSumTxtEditor : ViewController<ListView>
    {
        public ListViewSumTxtEditor()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
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
                groupSummary.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

    }
}
