using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using FT_FixedAsset.Module.BusinessObjects;
using System;

namespace FT_FixedAsset.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class GetHeaderValueController : ViewController
    {
        private NewObjectViewController controller;

        public GetHeaderValueController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.ObjectCreated += controller_ObjectCreated;
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            if (controller != null)
            {
                controller.ObjectCreated -= controller_ObjectCreated;
            }
            base.OnDeactivated();
        }

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;

            if (nestedFrame != null)
            {
                FTS11T0201 WriteOff = e.CreatedObject as FTS11T0201;
                if (WriteOff != null)
                {
                    FTS11OFAM FAMaster = ((NestedFrame)Frame).ViewItem.CurrentObject as FTS11OFAM;
 
                    if (FAMaster.U_FACode != null)
                    {
                        if (FAMaster != null)
                        {
                            //WriteOff.FAKey = FAMaster;
                            //WriteOff.FACode = FAMaster.FACode;
                            //WriteOff.FAName = FAMaster.FAName;
                        }

                    }
                }
            }
        }
    }
}
