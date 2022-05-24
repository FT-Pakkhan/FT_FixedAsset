using DevExpress.Data;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace FT_FixedAsset.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AdditionFieldController : ViewController<ListView>
    {
        public AdditionFieldController()
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
            ASPxGridListEditor gridListEditor = View.Editor as ASPxGridListEditor;
            if (gridListEditor != null)
            {   
                if (View.Id == "FTS11OFAM_FTS11_vw_FTS11JDT1A_ListView")
                {
                    GridViewDataTextColumn AddNBVColumn = new GridViewDataTextColumn();
                    AddNBVColumn.Caption = "NBV (LC)";
                    AddNBVColumn.FieldName = "NbvLC";
                    AddNBVColumn.ReadOnly = true;
                    AddNBVColumn.UnboundType = UnboundColumnType.Decimal;
                    AddNBVColumn.Width = 20;
                    AddNBVColumn.PropertiesTextEdit.DisplayFormatString = "n2";
                    gridListEditor.Grid.Columns.Add(AddNBVColumn);
                    AddNBVColumn.VisibleIndex = 10;
                    gridListEditor.Grid.CustomUnboundColumnData += GridView_CustomColumnNBV;
                }
            }
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void GridView_CustomColumnNBV(object sender, ASPxGridViewColumnDataEventArgs e)
        {
            decimal NBV = 0;
            if (e.Column.FieldName == "NbvLC")
            {

                for (int i = -1; i <= e.ListSourceRowIndex - 1; i++)
                {
                    NBV += Convert.ToDecimal(e.GetListSourceFieldValue(i + 1, "CostLC")) - Convert.ToDecimal(e.GetListSourceFieldValue(i + 1, "AccuLC"));
                }
                e.Value = NBV;
            }
        }
    }
}