using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.Web;
using System;
using System.Web.UI.WebControls;

namespace FT_FixedAsset.Module.Web.Editors
{
    [PropertyEditor(typeof(object), false)]
    public partial class DropDownPropertyEditor : ASPxLookupPropertyEditor
    {
        public DropDownPropertyEditor(Type objectType, IModelMemberViewItem info) : base(objectType, info) { }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);
            if (DropDownEdit != null)
            {
                DropDownEdit.PreRender += new EventHandler(DropDownEdit_PreRender);
            } 
        }

    void DropDownEdit_PreRender(object sender, EventArgs e)
        {
            DropDownEdit.ClearButton.Visible = false;
            DropDownEdit.NewButton.Enabled = false;
        }
    }
}