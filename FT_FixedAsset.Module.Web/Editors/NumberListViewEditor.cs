using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Web.Editors
{
    public partial class NumberListViewEditor : ViewController<ListView>
    {
        public NumberListViewEditor()
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

            ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
            if (gridListEditor != null)
            {
                gridListEditor.Grid.CustomColumnDisplayText += GridView_CustomColumnDisplayText;
            }
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
        void GridView_CustomColumnDisplayText(object sender, ASPxGridColumnDisplayTextEventArgs e)
        {
            if (e.GetType().Name == "Double")
            {
                e.DisplayText = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
            }
        }
    }
}
