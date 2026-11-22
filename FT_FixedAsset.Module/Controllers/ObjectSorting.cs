using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ObjectSorting : ViewController<ListView>
    {
        public ObjectSorting()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.

            foreach (IModelColumn column in View.Model.Columns)
            {
                if (View.Id == "FTS11STRN_LookupListView" || View.Id == "FTS11STRN_ListView")
                {

                    if (column.PropertyName == "FAGrpCod")
                    {
                        column.GroupIndex = 0;
                        column.SortIndex = 0;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }

                    if (column.PropertyName == "Code")
                    {
                        column.SortIndex = 1;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }

                }

                if (View.Id == "SE_FADeprnBy_LookupListView" || View.Id == "SE_FADeprnBy_ListView")
                {
                    if (column.PropertyName == "Code")
                    {
                        column.SortIndex = 0;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                    }
                }

                if (View.Id == "SE_FADeprnTyp_LookupListView" || View.Id == "SE_FADeprnTyp_ListView")
                {
                    if (column.PropertyName == "Code")
                    {
                        column.SortIndex = 0;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                    }
                }

                if (View.Id == "SE_FAProrate_LookupListView" || View.Id == "SE_FAProrate_ListView")
                {
                    if (column.PropertyName == "Code")
                    {
                        column.SortIndex = 0;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Descending;
                    }
                }

                if (View.Id == "FTS11OFAM_FTS11_vw_FTS11JDT1A_ListView" || View.Id == "FTS11_vw_FTS11JDT1A_ListView")
                {
                    if (column.PropertyName == "DocDate")
                    {
                        column.SortIndex = 0;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                    if (column.PropertyName == "DocEntry")
                    {
                        column.SortIndex = 1;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                    if (column.PropertyName == "TransType")
                    {
                        column.SortIndex = 2;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                    if (column.PropertyName == "TransID")
                    {
                        column.SortIndex = 3;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                }

                if (View.Id == "FTS11OFAM_FTS11_vw_Component_ListView" || View.Id == "FTS11_vw_Component_ListView")
                {
                    if (column.PropertyName == "Order")
                    {
                        column.SortIndex = 0;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                    if (column.PropertyName == "ADocEntry")
                    {
                        column.SortIndex = 1;
                        column.SortOrder = DevExpress.Data.ColumnSortOrder.Ascending;
                    }
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
