using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using FT_FixedAsset.Module.BusinessObjects;
using System;
using System.Web.UI.WebControls;

namespace FT_FixedAsset.Module.Web.Editors
{
    public partial class NumberDetailViewEditor : ViewController<DetailView>
    {
        public NumberDetailViewEditor()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            foreach (ASPxDoublePropertyEditor DoublePropertyEditor in View.GetItems<ASPxDoublePropertyEditor>())
            {
                DoublePropertyEditor.ControlCreated += new EventHandler<EventArgs>(DoublePropertyEditor_ControlCreated);
            }

            foreach (ASPxDecimalPropertyEditor DecimalPropertyEditor in View.GetItems<ASPxDecimalPropertyEditor>())
            {
                DecimalPropertyEditor.ControlCreated += new EventHandler<EventArgs>(DecimalPropertyEditor_ControlCreated);
            }

            foreach (ASPxIntPropertyEditor DecimalPropertyEditor in View.GetItems<ASPxIntPropertyEditor>())
            {
                DecimalPropertyEditor.ControlCreated += new EventHandler<EventArgs>(IntPropertyEditor_ControlCreated);
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();

        }
        protected override void OnDeactivated()
        {
            foreach (ASPxDoublePropertyEditor DoublePropertyEditor in View.GetItems<ASPxDoublePropertyEditor>())
            {
                DoublePropertyEditor.ControlCreated -= new EventHandler<EventArgs>(DoublePropertyEditor_ControlCreated);
            }

            foreach (ASPxDecimalPropertyEditor DecimalPropertyEditor in View.GetItems<ASPxDecimalPropertyEditor>())
            {
                DecimalPropertyEditor.ControlCreated -= new EventHandler<EventArgs>(DecimalPropertyEditor_ControlCreated);
            }

            foreach (ASPxIntPropertyEditor DecimalPropertyEditor in View.GetItems<ASPxIntPropertyEditor>())
            {
                DecimalPropertyEditor.ControlCreated -= new EventHandler<EventArgs>(IntPropertyEditor_ControlCreated);
            }

            base.OnDeactivated();
        }
        void DoublePropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            ASPxDoublePropertyEditor propertyEditor = (ASPxDoublePropertyEditor)sender;
            if (propertyEditor.InplaceViewModeEditor is Label)
            {
                propertyEditor.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                propertyEditor.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
            }
        }
        void DecimalPropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            ASPxDecimalPropertyEditor propertyEditor = (ASPxDecimalPropertyEditor)sender;
            if (propertyEditor.InplaceViewModeEditor is Label)
            {
                propertyEditor.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                propertyEditor.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
            }
        }
        void IntPropertyEditor_ControlCreated(object sender, EventArgs e)
        {
            ASPxIntPropertyEditor propertyEditor = (ASPxIntPropertyEditor)sender;
            if (propertyEditor.InplaceViewModeEditor is Label)
            {
                propertyEditor.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                propertyEditor.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
            }
        }
    }
}
