using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using FT_FixedAsset.Module.BusinessObjects;
using System;

namespace FT_FixedAsset.Module.Controllers
{
    /// <summary>
    /// Quantity fields follow General Settings "Quantity Decimals", amount fields "Amount Decimals",
    /// in detail views and list views (display format + edit mask).
    /// Which field is which: DecimalPlaces (shared with Data Import).
    /// </summary>
    public partial class DynamicDecimalController : ViewController
    {
        public DynamicDecimalController()
        {
            InitializeComponent();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if (View == null || View.ObjectTypeInfo == null) return;

            Type objectType = View.ObjectTypeInfo.Type;
            int[] settings = null;   // { qtyDec, sumDec }, read once per view

            foreach (IMemberInfo memberInfo in View.ObjectTypeInfo.Members)
            {
                NumberKind kind = DecimalPlaces.KindOf(objectType, memberInfo.Name, memberInfo.MemberType);
                if (kind == NumberKind.Own)   // settings screens, exchange rates, percentages, counts keep their own format
                {
                    ClearSavedFormat(memberInfo);
                    continue;
                }

                if (settings == null) settings = LoadSettings();
                string format = DecimalPlaces.Format(DecimalPlaces.Places(kind, settings[0], settings[1]));
                string propertyName = memberInfo.Name;

                // model - used when the view's controls are created
                if (View is ListView listView)
                {
                    IModelColumn column = listView.Model.Columns[propertyName];
                    if (column != null)
                    {
                        column.DisplayFormat = format;
                        column.EditMask = format;   // also limits editing inside the list (transaction lines)
                    }
                }
                else if (View is DetailView detailView)
                {
                    IModelMember modelMember = detailView.Model.ModelClass.FindMember(propertyName);
                    if (modelMember != null)
                    {
                        modelMember.DisplayFormat = format;
                        modelMember.EditMask = format;
                    }
                    if (detailView.Model.Items[propertyName] is IModelPropertyEditor item)
                    {
                        item.DisplayFormat = format;
                        item.EditMask = format;
                    }
                }

                // editors that already exist
                if (View is CompositeView compositeView && compositeView.FindItem(propertyName) is PropertyEditor editor)
                {
                    editor.DisplayFormat = format;
                    editor.EditMask = format;
                    if (editor.Control != null) editor.Refresh();
                }
            }
        }

        /// <summary>
        /// An older build also applied the amount format (0.00) to these fields, and XAF saved it in the
        /// user's stored model (ModelDifferenceAspect). Remove that saved value so the field's own
        /// format (ModelDefault attribute) is used again.
        /// </summary>
        private void ClearSavedFormat(IMemberInfo memberInfo)
        {
            Type t = Nullable.GetUnderlyingType(memberInfo.MemberType) ?? memberInfo.MemberType;
            if (t != typeof(double) && t != typeof(decimal) && t != typeof(float)) return;
            try
            {
                var nodes = new System.Collections.Generic.List<IModelNode>();
                if (View is DetailView detailView)
                {
                    nodes.Add(detailView.Model.ModelClass.FindMember(memberInfo.Name));
                    nodes.Add(detailView.Model.Items[memberInfo.Name]);
                }
                else if (View is ListView listView)
                {
                    nodes.Add(listView.Model.ModelClass.FindMember(memberInfo.Name));
                    nodes.Add(listView.Model.Columns[memberInfo.Name]);
                }
                foreach (var node in nodes)
                {
                    var modelNode = node as DevExpress.ExpressApp.Model.Core.ModelNode;
                    if (modelNode == null) continue;
                    modelNode.ClearValue("DisplayFormat");
                    modelNode.ClearValue("EditMask");
                }
                if (View is DetailView dv && dv.Model.Items[memberInfo.Name] is IModelPropertyEditor item
                    && dv.FindItem(memberInfo.Name) is PropertyEditor editor)
                {
                    editor.DisplayFormat = item.DisplayFormat;
                    editor.EditMask = item.EditMask;
                    if (editor.Control != null) editor.Refresh();
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(ex);
            }
        }

        /// <summary>{ Quantity Decimals, Amount Decimals } from General Settings.</summary>
        private int[] LoadSettings()
        {
            try
            {
                FTS11OADM oadm = ObjectSpace is XPObjectSpace ? ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1")) : null;
                if (oadm != null) return new[] { oadm.U_QtyDec, oadm.U_SumDec };

                // non-persistent screens (widgets, report viewer, data import)
                using (IObjectSpace os = Application.CreateObjectSpace(typeof(FTS11OADM)))
                {
                    oadm = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry=1"));
                    if (oadm != null) return new[] { oadm.U_QtyDec, oadm.U_SumDec };
                }
            }
            catch (Exception ex)
            {
                Tracing.Tracer.LogError(ex);
            }
            return new[] { DecimalPlaces.DefaultQtyDec, DecimalPlaces.DefaultSumDec };
        }
    }
}
