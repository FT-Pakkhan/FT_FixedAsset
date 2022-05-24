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
    public partial class NavNonPersistentController : ViewController
    {
        public NavNonPersistentController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
            ShowNavigationItemController showNavigationItemController = Frame.GetController<ShowNavigationItemController>();
            showNavigationItemController.CustomShowNavigationItem += showNavigationItemController_CustomShowNavigationItem;
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

        void showNavigationItemController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            if (e.ActionArguments.SelectedChoiceActionItem.Id == "DeprnWidget_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(DeprnWidget));
                DeprnWidget newDeprnWidget = objectSpace.CreateObject<DeprnWidget>();
                DetailView detailView = Application.CreateDetailView(objectSpace, newDeprnWidget);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                View.BreakLinksToControls();
                View.CreateControls();
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;
                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "PostWidget_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(PostWidget));
                PostWidget newPostWidget = objectSpace.CreateObject<PostWidget>();
                DetailView detailView = Application.CreateDetailView(objectSpace, newPostWidget);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                View.BreakLinksToControls();
                View.CreateControls();
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;
                e.Handled = true;
            }

            if (e.ActionArguments.SelectedChoiceActionItem.Id == "ReportViewer_ListView")
            {
                IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ReportViewer));
                ReportViewer newReportViewer = objectSpace.CreateObject<ReportViewer>();
                DetailView detailView = Application.CreateDetailView(objectSpace, newReportViewer);
                detailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.Edit;
                View.BreakLinksToControls();
                View.CreateControls();
                e.ActionArguments.ShowViewParameters.CreatedView = detailView;
                e.Handled = true;
            }
        }
    }
}
