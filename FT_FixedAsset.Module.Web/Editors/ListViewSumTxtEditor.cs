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

                // non-persistent lists (Import Result, widgets) cannot read General Settings - nothing to set up there
                if (!(ObjectSpace is DevExpress.ExpressApp.Xpo.XPObjectSpace)) return;

                ASPxSummaryItem groupSummary = new ASPxSummaryItem();

                groupSummary.FieldName = "CostFC";
                groupSummary.SummaryType = SummaryItemType.Sum;
                groupSummary.ShowInGroupFooterColumn = "CostFC";
                FTS11OADM oadm = ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1"));
                groupSummary.DisplayFormat = "{0:n" + (oadm == null ? FT_FixedAsset.Module.Controllers.DecimalPlaces.DefaultSumDec : oadm.U_SumDec) + "}";
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

    }
}
