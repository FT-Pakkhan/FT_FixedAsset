using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Editors;
using FT_FixedAsset.Module.BusinessObjects;
using System.Linq;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class DynamicDecimalController : ViewController
    {
        public DynamicDecimalController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.ObjectTypeInfo.Type == typeof(FTS11OADM))
                return;

            // 1. Get all properties that are 'double' or 'decimal'
            var numericMembers = View.ObjectTypeInfo.Members.Where(m => m.MemberType == typeof(double) || m.MemberType == typeof(decimal));

            foreach (var memberInfo in numericMembers)
            {
                string propertyName = memberInfo.Name;

                // 2. Update the Model Metadata dynamically
                if (View is ListView listView)
                {
                    var column = listView.Model.Columns[propertyName];
                    if (column != null)
                    {
                        if (propertyName == "U_Quantity" || propertyName == "SQuantity" || propertyName == "QtyFrom" || propertyName == "QtyTo")
                            column.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_QtyDec.ToString() + "}";

                        else if (propertyName == "U_UseLife" || propertyName == "U_BFDepr")
                            column.DisplayFormat = "{0:n2}";

                        else
                            column.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                    }
                }
                else if (View is DetailView detailView)
                {
                    var modelMember = detailView.Model.ModelClass.FindMember(propertyName);
                    if (modelMember != null)
                    {
                        if (propertyName == "U_Quantity" || propertyName == "SQuantity" || propertyName == "QtyFrom" || propertyName == "QtyTo")
                        {
                            modelMember.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_QtyDec.ToString() + "}";
                            modelMember.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_QtyDec.ToString() + "}";
                        }
                        else if (propertyName == "U_UseLife" || propertyName == "U_BFDepr")
                        {
                            modelMember.DisplayFormat = "{0:n2}";
                            modelMember.EditMask = "{0:n2}";
                        }
                        else
                        {
                            modelMember.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                            modelMember.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                        }
                    }
                }

                // 3. Update the UI Property Editors (DetailView & Editable ListView)
                if (View is CompositeView compositeView)
                {
                    var editor = compositeView.FindItem(propertyName) as PropertyEditor;
                    if (editor != null)
                    {
                        if (propertyName == "U_Quantity" || propertyName == "SQuantity" || propertyName == "QtyFrom" || propertyName == "QtyTo")
                        {
                            editor.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_QtyDec.ToString() + "}";
                            editor.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_QtyDec.ToString() + "}";
                        }
                        else if (propertyName == "U_UseLife" || propertyName == "U_BFDepr")
                        {
                            editor.DisplayFormat = "{0:n2}";
                            editor.EditMask = "{0:n2}";
                        }
                        else
                        {
                            editor.DisplayFormat = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}"; 
                            editor.EditMask = "{0:n" + ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")).U_SumDec.ToString() + "}";
                        }

                        // Refresh if the control is already rendered
                        if (editor.Control != null)
                        {
                            editor.Refresh();
                        }
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
