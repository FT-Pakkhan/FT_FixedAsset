namespace FT_FixedAsset.Module.Web.Controllers
{
    partial class ReportController
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
            this.Preview = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.PostTrans = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // Preview
            // 
            this.Preview.Caption = "Preview";
            this.Preview.ConfirmationMessage = null;
            this.Preview.Id = "Preview";
            this.Preview.ImageName = "ModelEditor_Actions";
            this.Preview.ToolTip = null;
            this.Preview.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.Preview_Execute);
            // 
            // PostTrans
            // 
            this.PostTrans.Caption = "Post";
            this.PostTrans.ConfirmationMessage = "Are you sure to post transaction?";
            this.PostTrans.Id = "PostTrans";
            this.PostTrans.ImageName = "ModelEditor_Actions";
            this.PostTrans.ToolTip = null;
            this.PostTrans.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.PostTrans_Execute);
            // 
            // ReportController
            // 
            this.Actions.Add(this.Preview);
            this.Actions.Add(this.PostTrans);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Preview;
        private DevExpress.ExpressApp.Actions.SimpleAction PostTrans;
    }
}
