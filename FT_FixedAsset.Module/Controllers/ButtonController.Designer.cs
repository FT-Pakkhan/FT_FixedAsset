namespace FT_FixedAsset.Module.Web.Controllers
{
    partial class ButtonController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RunDeprn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TestConn = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Action = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.CancelTrans = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Duplicate = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.Advanced = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // RunDeprn
            // 
            this.RunDeprn.Caption = "Run Depreciation";
            this.RunDeprn.ConfirmationMessage = "Are you sure to run the depreciation?";
            this.RunDeprn.Id = "RunDeprn";
            this.RunDeprn.ImageName = "ModelEditor_Actions";
            this.RunDeprn.ToolTip = null;
            this.RunDeprn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RunDeprn_Execute);
            // 
            // TestConn
            // 
            this.TestConn.Caption = "Test Connection";
            this.TestConn.ConfirmationMessage = null;
            this.TestConn.Id = "TestConn";
            this.TestConn.ImageName = "ActionGroup_EasyTestRecorder";
            this.TestConn.ToolTip = null;
            this.TestConn.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TestConn_Execute);
            // 
            // Action
            // 
            this.Action.Caption = "Action";
            this.Action.ConfirmationMessage = null;
            this.Action.Id = "Action";
            this.Action.ImageName = "Action_SingleChoiceAction";
            this.Action.ItemType = DevExpress.ExpressApp.Actions.SingleChoiceActionItemType.ItemIsOperation;
            this.Action.ToolTip = null;
            this.Action.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.Action_Execute);
            // 
            // CancelTrans
            // 
            this.CancelTrans.Caption = "Cancel";
            this.CancelTrans.ConfirmationMessage = "Are you sure to cancel this transaction?";
            this.CancelTrans.Id = "CancelTrans";
            this.CancelTrans.ImageName = "Cancel_16x16";
            this.CancelTrans.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.CancelTrans.ToolTip = null;
            this.CancelTrans.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CancelTrans_Execute);
            // 
            // Duplicate
            // 
            this.Duplicate.Caption = "Duplicate";
            this.Duplicate.ConfirmationMessage = null;
            this.Duplicate.Id = "Duplicate";
            this.Duplicate.ImageName = "Group2_16x16";
            this.Duplicate.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.CaptionAndImage;
            this.Duplicate.ToolTip = null;
            this.Duplicate.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Duplicate_Execute);
            // 
            // Advanced
            // 
            this.Advanced.Caption = "Advanced";
            this.Advanced.Category = "Advanced";
            this.Advanced.ConfirmationMessage = null;
            this.Advanced.Id = "Advanced";
            this.Advanced.ToolTip = null;
            this.Advanced.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Advanced_Execute);
            // 
            // ButtonController
            // 
            this.Actions.Add(this.RunDeprn);
            this.Actions.Add(this.TestConn);
            this.Actions.Add(this.Action);
            this.Actions.Add(this.CancelTrans);
            this.Actions.Add(this.Duplicate);
            this.Actions.Add(this.Advanced);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction RunDeprn;
        private DevExpress.ExpressApp.Actions.SimpleAction TestConn;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction Action;
        private DevExpress.ExpressApp.Actions.SimpleAction CancelTrans;
        private DevExpress.ExpressApp.Actions.SimpleAction Duplicate;
        private DevExpress.ExpressApp.Actions.SimpleAction Advanced;
    }
}
