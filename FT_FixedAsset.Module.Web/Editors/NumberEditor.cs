using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using System;
using DevExpress.ExpressApp.Model;
using System.Web.UI.WebControls;
using DevExpress.Web;
using DevExpress.ExpressApp;
using FT_FixedAsset.Module.BusinessObjects;
using DevExpress.Data.Filtering;

namespace FT_FixedAsset.Module.Web.Editors
{
    [PropertyEditor(typeof(decimal), true)]
    public partial class DecimalPropertyEditor : ASPxDecimalPropertyEditor
    {
        public DecimalPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);

            var spinEditor = control as ASPxSpinEdit;
            if (spinEditor == null) return;
            spinEditor.SpinButtons.ShowIncrementButtons = false;
            spinEditor.AllowMouseWheel = false;
            spinEditor.SelectInputTextOnClick = true;
            spinEditor.DecimalPlaces = 6;
        }
    }

    [PropertyEditor(typeof(double), true)]
    public partial class DoublePropertyEditor : ASPxDoublePropertyEditor, IComplexViewItem
    {
        private IObjectSpace os;
        public DoublePropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);

            var spinEditor = control as ASPxSpinEdit;
            if (spinEditor == null) return;
            spinEditor.SpinButtons.ShowIncrementButtons = false;
            spinEditor.AllowMouseWheel = false;
            spinEditor.SelectInputTextOnClick = true;
            spinEditor.DecimalPlaces = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec;
            spinEditor.DisplayFormatString = "{0:n" + os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
        }
        void IComplexViewItem.Setup(IObjectSpace objectSpace, XafApplication application)
        {
            os = objectSpace;
        }
    }

    [PropertyEditor(typeof(int), true)]
    public partial class IntPropertyEditor : ASPxIntPropertyEditor
    {
        public IntPropertyEditor(Type objectType, IModelMemberViewItem model) : base(objectType, model) { }

        protected override void SetupControl(WebControl control)
        {
            base.SetupControl(control);

            var spinEditor = control as ASPxSpinEdit;
            if (spinEditor == null) return;
            spinEditor.SpinButtons.ShowIncrementButtons = false;
            spinEditor.AllowMouseWheel = false;
            spinEditor.SelectInputTextOnClick = true;
            spinEditor.DecimalPlaces = 2;
            spinEditor.NumberType = SpinEditNumberType.Integer;
        }
    }
}