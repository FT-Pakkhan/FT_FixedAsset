using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using FT_FixedAsset.Module.BusinessObjects;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class CuztomizeJDT1PopUp : ViewController
    {
        private ListViewProcessCurrentObjectController processCurrentObjectController;
        public CuztomizeJDT1PopUp()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            //JDT1PopUp.Active.SetItemValue("Active", false);

            processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();

            if (processCurrentObjectController != null)
                processCurrentObjectController.CustomProcessSelectedItem += processCurrentObjectController_CustomProcessSelectedItem;

        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            processCurrentObjectController = Frame.GetController<ListViewProcessCurrentObjectController>();

            if (processCurrentObjectController != null)
                processCurrentObjectController.CustomProcessSelectedItem -= processCurrentObjectController_CustomProcessSelectedItem;

            base.OnDeactivated();
        }

        private void processCurrentObjectController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            if (View.Id == "FTS11OFAM_FTS11_vw_FTS11JDT1A_ListView")
            {
                e.Handled = true;
                FTS11_vw_FTS11JDT1A selectedObject = (FTS11_vw_FTS11JDT1A)e.InnerArgs.CurrentObject;

                switch (selectedObject.TransType)
                {
                    case "T0101":
                    case "T0102":
                    case "T0103":
                    case "T0104":
                    case "T0105":
                    case "T9901":
                    case "T9902":
                        IObjectSpace os = Application.CreateObjectSpace();
                        FTS11TJDT TJDT = os.GetObjectByKey<FTS11TJDT>(selectedObject.DocEntry);
                        DetailView dv = Application.CreateDetailView(os, TJDT);
                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.View;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                        break;
                }

                //Write Off
                if (selectedObject.TransType == "T0201")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0201 T0201 = os.GetObjectByKey<FTS11T0201>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0201);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Disposed
                if (selectedObject.TransType == "T0202")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0202 T0202 = os.GetObjectByKey<FTS11T0202>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0202);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Correction Out
                if (selectedObject.TransType == "T0203")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0203 T0203 = os.GetObjectByKey<FTS11T0203>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0203);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Transferred Out
                if (selectedObject.TransType == "T0204")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0204 T0204 = os.GetObjectByKey<FTS11T0204>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0204);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Reclassed Out
                if (selectedObject.TransType == "T0205")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0205 T0205 = os.GetObjectByKey<FTS11T0205>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0205);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Review Useful Life
                if (selectedObject.TransType == "T0301")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0301 T0301 = os.GetObjectByKey<FTS11T0301>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0301);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Revalue Cost
                if (selectedObject.TransType == "T0302")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0302 T0302 = os.GetObjectByKey<FTS11T0302>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0302);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Adjust Accumulated Depr
                if (selectedObject.TransType == "T0303")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0303 T0303 = os.GetObjectByKey<FTS11T0303>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0303);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Move
                if (selectedObject.TransType == "T0304")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0304 T0304 = os.GetObjectByKey<FTS11T0304>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0304);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }

                //Revalue Residual Value
                if (selectedObject.TransType == "T0305")
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11T0305 T0305 = os.GetObjectByKey<FTS11T0305>(selectedObject.BaseEntry);
                    DetailView dv = Application.CreateDetailView(os, T0305);
                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.View;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                }
            }
        }
    }
}
