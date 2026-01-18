using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using FT_FixedAsset.Module.BusinessObjects;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace FT_FixedAsset.Module.Web.Editors
{
    [PropertyEditor(typeof(String), "CustomLogin", false)]
    public class CustomStringEditor : ASPxPropertyEditor
    {
        ASPxComboBox dropDownControl = null;
        public CustomStringEditor(
        Type objectType, IModelMemberViewItem info) : base(objectType, info) { }
        protected override void SetupControl(WebControl control)
        {
            if (ViewEditMode == ViewEditMode.Edit)
            {
                DataTable dt = null;
                string databases = "";
                string sqlQuery = "SELECT * FROM ODBC ";
                
                dt = SQLHandler.GetDataTable(sqlQuery);
                if (dt.Rows.Count > 0)
                {
                    ((ASPxComboBox)control).Items.Add("", "");
                    foreach (DataRow row in dt.Rows)
                    {
                        ((ASPxComboBox)control).Items.Add(row["Description"].ToString(), row["DBName"].ToString());

                        if (databases == "")
                            databases = row["Description"].ToString();
                        else
                            databases += ";" + row["Description"].ToString();
                    }
                    ((ASPxComboBox)control).SelectedIndex = 0;
                }

                dt.Dispose();
            }
        }
        protected override WebControl CreateEditModeControlCore()
        {
            dropDownControl = RenderHelper.CreateASPxComboBox();
            dropDownControl.ValueChanged += EditValueChangedHandler;
            return dropDownControl;
        }
        public override void BreakLinksToControl(bool unwireEventsOnly)
        {
            if (dropDownControl != null)
            {
                dropDownControl.ValueChanged -= new EventHandler(EditValueChangedHandler);
            }
            base.BreakLinksToControl(unwireEventsOnly);
        }
    }
}
