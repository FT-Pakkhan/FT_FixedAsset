using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.ExpressApp.Win.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using FT_FixedAsset.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FT_FixedAsset.Module.Web.Controllers
{
 
    public partial class UpdateXafDisplayNameController : ViewController
    {
        public UpdateXafDisplayNameController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
            {
                FTS11OADM OADM = ObjectSpace.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = 1"));

                if (View is ListView listView && listView.Editor != null)
                {
                    IModelColumn U_IndKey1 = listView.Model.Columns["U_IndKey1"];
                    IModelColumn U_IndKey2 = listView.Model.Columns["U_IndKey2"];
                    IModelColumn U_IndKey3 = listView.Model.Columns["U_IndKey3"];
                    IModelColumn U_IndKey4 = listView.Model.Columns["U_IndKey4"];
                    IModelColumn U_IndKey5 = listView.Model.Columns["U_IndKey5"];
                    IModelColumn U_IndKey6 = listView.Model.Columns["U_IndKey6"];

                    if (U_IndKey1 != null)
                        U_IndKey1.Caption = OADM.U_Ind1Capt;

                    if (U_IndKey2 != null)
                        U_IndKey2.Caption = OADM.U_Ind2Capt;

                    if (U_IndKey3 != null)
                        U_IndKey3.Caption = OADM.U_Ind3Capt;

                    if (U_IndKey4 != null)
                        U_IndKey4.Caption = OADM.U_Ind4Capt;

                    if (U_IndKey5 != null)
                        U_IndKey5.Caption = OADM.U_Ind5Capt;

                    if (U_IndKey6 != null)
                        U_IndKey6.Caption = OADM.U_Ind6Capt;
                }

                if (View is DetailView)
                {
                    DetailView detailView = (DetailView)View;

                    PropertyEditor U_IndKey1 = detailView.FindItem("U_IndKey1") as PropertyEditor;
                    PropertyEditor U_IndKey2 = detailView.FindItem("U_IndKey2") as PropertyEditor;
                    PropertyEditor U_IndKey3 = detailView.FindItem("U_IndKey3") as PropertyEditor;
                    PropertyEditor U_IndKey4 = detailView.FindItem("U_IndKey4") as PropertyEditor;
                    PropertyEditor U_IndKey5 = detailView.FindItem("U_IndKey5") as PropertyEditor;
                    PropertyEditor U_IndKey6 = detailView.FindItem("U_IndKey6") as PropertyEditor;

                    if (U_IndKey1 != null)
                        U_IndKey1.Caption = OADM.U_Ind1Capt;

                    if (U_IndKey2 != null)
                        U_IndKey2.Caption = OADM.U_Ind2Capt;

                    if (U_IndKey3 != null)
                        U_IndKey3.Caption = OADM.U_Ind3Capt;

                    if (U_IndKey4 != null)
                        U_IndKey4.Caption = OADM.U_Ind4Capt;

                    if (U_IndKey5 != null)
                        U_IndKey5.Caption = OADM.U_Ind5Capt;

                    if (U_IndKey6 != null)
                        U_IndKey6.Caption = OADM.U_Ind6Capt;
                }
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
