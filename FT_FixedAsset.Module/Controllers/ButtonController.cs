using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using DevExpress.Xpo.DB.Helpers;
using FT_FixedAsset.Module.BusinessObjects;
using FT_FixedAsset.Module.Controllers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FT_FixedAsset.Module.Web.Controllers
{
    public partial class ButtonController : ViewController
    {
        GenController genCon;

        public ButtonController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.

            ChoiceActionItem WriteOff = new ChoiceActionItem("WriteOff", "Write Off", null);
            ChoiceActionItem Disposal = new ChoiceActionItem("Disposal", "Disposal", null);
            ChoiceActionItem CorrectionOut = new ChoiceActionItem("CorrectionOut", "Correction Out", null);
            ChoiceActionItem TransferredOut = new ChoiceActionItem("TransferredOut", "Transferred Out", null);
            ChoiceActionItem ReclassedOut = new ChoiceActionItem("ReclassedOut", "Reclassed Out", null);
            ChoiceActionItem ReUseLife = new ChoiceActionItem("ReUseLife", "Review Useful Life", null);
            ChoiceActionItem ReCost = new ChoiceActionItem("ReCost", "Revalue Cost", null);
            ChoiceActionItem AdjDepr = new ChoiceActionItem("AdjDepr", "Adjust Accu. Depr", null);
            ChoiceActionItem Move = new ChoiceActionItem("Move", "Move/Transfer Location", null);
            ChoiceActionItem ReReValue = new ChoiceActionItem("ReReValue", "Revalue Residual Value", null);

            Action.Items.Add(WriteOff);
            Action.Items.Add(Disposal);
            Action.Items.Add(CorrectionOut);
            Action.Items.Add(TransferredOut);
            Action.Items.Add(ReclassedOut);
            Action.Items.Add(ReUseLife);
            Action.Items.Add(ReCost);
            Action.Items.Add(AdjDepr);
            Action.Items.Add(Move);
            Action.Items.Add(ReReValue);

            Action.Execute += Action_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            #region ButtonActive
            RunDeprn.Active.SetItemValue("Active", false);
            TestConn.Active.SetItemValue("Active", false);
            Action.Active.SetItemValue("Active", false);
            CancelTrans.Active.SetItemValue("Active", false);
            Duplicate.Active.SetItemValue("Active", false);
            Advanced.Active.SetItemValue("Active", false);

            if (View.ObjectTypeInfo.Type == typeof(DeprnWidget))
            {
                RunDeprn.Active.SetItemValue("Active", true);
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11OADM))
            {
                if (View.Id == "FTS11OADM_DetailView")
                {
                    TestConn.Active.SetItemValue("Active", true);
                }
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
            {
                Action.Active.SetItemValue("Active", true);
                CancelTrans.Active.SetItemValue("Active", true);
                Duplicate.Active.SetItemValue("Active", true);
                Advanced.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11TJDT))
            {
                CancelTrans.Active.SetItemValue("Active", true);
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0201))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
            {
                Action.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0202))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0203))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0204))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0205))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0301))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0302))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0303))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0304))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            if (View.ObjectTypeInfo.Type == typeof(FTS11T0305))
            {
                CancelTrans.Active.SetItemValue("Active", true);

                if (View is DetailView)
                {
                    DetailView dv = (DetailView)View;
                    dv.ViewEditModeChanged += new EventHandler<EventArgs>(ViewController1_ViewEditModeChanged);
                }
                UpdateActionState();
            }

            #endregion
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GenController>();
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void UpdateActionState()
        {
            Action.Active["Action"] = View is ListView || (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View);
            CancelTrans.Active["CancelTrans"] = View is ListView || (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View);
            Duplicate.Active["Duplicate"] = View is ListView || (View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.View);
            Advanced.Enabled["Advanced"] = View is DetailView && ((DetailView)View).ViewEditMode == ViewEditMode.Edit;
        }

        void ViewController1_ViewEditModeChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void RunDeprn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.ObjectTypeInfo.Type == typeof(DeprnWidget))
            {
                DeprnWidget RunDeprn = (DeprnWidget)View.CurrentObject;
                SystemUsers user = (SystemUsers)SecuritySystem.CurrentUser;

                //Block if Date from more earlier than Date to
                if (RunDeprn.DateFrm > RunDeprn.DateTo)
                {
                    //throw new InvalidOperationException("Invalid Date From Date To.");
                    genCon.MsgBox(InformationType.Error, "Error", "Invalid Date From Date To.");
                    return;
                }

                //Connect to Xaf Database
                string connectionString = genCon.getConnectionString();

                SqlConnection conn = new SqlConnection(connectionString);
                SqlCommand getDeprFAList = new SqlCommand("", conn);

                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }

                //Get Depreciation Filter Fixed Asset List
                try
                {
                    conn.Open();

                    //Have to assign to variable else will got object reference null error
                    string dateto = RunDeprn.DateTo.ToString("yyyy-MM-dd") == "0001-01-01" ? "1900-01-01" : RunDeprn.DateTo.ToString("yyyy-MM-dd");
                    string facodefrm = RunDeprn.FACodeFrm == null ? "" : RunDeprn.FACodeFrm.U_FACode;
                    string facodeto = RunDeprn.FACodeTo == null ? "" : RunDeprn.FACodeTo.U_FACode;
                    string fagrpfrm = RunDeprn.FAGrpFrm == null ? "" : RunDeprn.FAGrpFrm.U_FAGrpCod;
                    string fagrpto = RunDeprn.FAGrpTo == null ? "" : RunDeprn.FAGrpTo.U_FAGrpCod;
                    string faclsfrm = RunDeprn.FAClsFrm == null ? "" : RunDeprn.FAClsFrm.Code;
                    string faclsto = RunDeprn.FAClsTo == null ? "" : RunDeprn.FAClsTo.Code;

                    getDeprFAList.CommandText = "SELECT FAKey, U_FACode FROM FTS_fn_FADeprList ('" +
                        dateto + "', '" +
                        facodefrm + "', '" +
                        facodeto + "', '" +
                        fagrpfrm + "', '" +
                        fagrpto + "', '" +
                        faclsfrm + "', '" +
                        faclsto + "')";

                    SqlDataAdapter da = new SqlDataAdapter(getDeprFAList);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    da.Dispose();

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow row in dt.Rows)
                        {
                            genCon.GenerateDeprn(RunDeprn.DateFrm, RunDeprn.DateTo, int.Parse(row["FAKey"].ToString()), user.UserName.ToString());
                        }
                        genCon.MsgBox(InformationType.Info, "Information", "Depreciation has been generated.");
                    }
                    else
                    {
                        genCon.MsgBox(InformationType.Info, "Information", "Data unavailable. No record filtered.");
                    }

                    conn.Close();
                }
                 catch (Exception ex)
                {
                    genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    conn.Close();
                }
            }
        }

        private void TestConn_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SAPbobsCOM.Company oCom = new SAPbobsCOM.Company();
            #region Connect to SAP

            FTS11OADM OADMSettings = View.CurrentObject as FTS11OADM;

            string SvrType = OADMSettings.U_SvrTyp.Code;

            switch (SvrType)
            {
                case "MSSQL2005":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005;
                    break;
                case "MSSQL2008":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008;
                    break;
                case "MSSQL2012":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2012;
                    break;
                case "MSSQL2014":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2014;
                    break;
                case "MSSQL2016":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2016;
                    break;
                case "MSSQL2017":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2017;
                    break;
                case "MSSQL2019":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2019;
                    break;
                case "HANADB":
                    oCom.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_HANADB;
                    break;
            }

            oCom.Server = OADMSettings.U_SqlSvr;
            oCom.DbUserName = OADMSettings.U_SqlUID;
            oCom.DbPassword = OADMSettings.U_SqlPWD;
            oCom.language = SAPbobsCOM.BoSuppLangs.ln_English;
            //oCom.LicenseServer = ConfigurationManager.AppSettings.Get("LicenseServer").ToString();
            oCom.CompanyDB = OADMSettings.U_SboDB;
            oCom.UserName = OADMSettings.U_SboUID;
            oCom.Password = OADMSettings.U_SboPWD;

            if (oCom.Connect() != 0)
            {
                string message = oCom.GetLastErrorDescription().ToString().Replace("'", "");
                genCon.MsgBox(InformationType.Error, "Error", message);
                return;
            }
            else
            {
                genCon.MsgBox(InformationType.Success, "Success", "Diapi Successfully connected !");
                oCom.Disconnect();
                return;
            }
            #endregion
        }

        private void Action_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            if (View == null)
                return;

            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
            {
                if (e.SelectedObjects.Count <= 0)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select an asset to proceed.");
                    return;
                }

                IObjectSpace os = Application.CreateObjectSpace();

                if (e.SelectedChoiceActionItem.Id == "WriteOff")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0201 T0201 = os.CreateObject<FTS11T0201>();
                        DetailView dv = Application.CreateDetailView(os, T0201);

                        if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
                        {
                            foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.SQuantity <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0201D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                            {
                                FTS11T0201D T0201D = os.CreateObject<FTS11T0201D>();

                                T0201D.Oid = row;
                                T0201D.DocEntry = T0201;
                                T0201D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);

                                row++;
                            }
                        }

                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "Disposal")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0202 T0202 = os.CreateObject<FTS11T0202>();
                        DetailView dv = Application.CreateDetailView(os, T0202);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0202D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            FTS11T0202D T0202D = os.CreateObject<FTS11T0202D>();

                            T0202D.Oid = row;
                            T0202D.DocEntry = T0202;
                            T0202D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }

                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "CorrectionOut")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0203 T0203 = os.CreateObject<FTS11T0203>();
                        DetailView dv = Application.CreateDetailView(os, T0203);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0203D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        { 
                            FTS11T0203D T0203D = os.CreateObject<FTS11T0203D>();

                            T0203D.Oid = row;
                            T0203D.DocEntry = T0203;
                            T0203D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }

                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "TransferredOut")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0204 T0204 = os.CreateObject<FTS11T0204>();
                        DetailView dv = Application.CreateDetailView(os, T0204);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0204D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        { 
                            FTS11T0204D T0204D = os.CreateObject<FTS11T0204D>();

                            T0204D.Oid = row;
                            T0204D.DocEntry = T0204;
                            T0204D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }

                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReclassedOut")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0205 T0205 = os.CreateObject<FTS11T0205>();
                        DetailView dv = Application.CreateDetailView(os, T0205);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0205D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            FTS11T0205D T0205D = os.CreateObject<FTS11T0205D>();

                            T0205D.Oid = row;
                            T0205D.DocEntry = T0205;
                            T0205D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }

                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReUseLife")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0301 T0301 = os.CreateObject<FTS11T0301>();
                        DetailView dv = Application.CreateDetailView(os, T0301);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0301D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            FTS11T0301D T0301D = os.CreateObject<FTS11T0301D>();

                            T0301D.Oid = row;
                            T0301D.DocEntry = T0301;
                            T0301D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }
                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReCost")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0302 T0302 = os.CreateObject<FTS11T0302>();
                        DetailView dv = Application.CreateDetailView(os, T0302);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0302D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        { 
                            FTS11T0302D T0302D = os.CreateObject<FTS11T0302D>();

                            T0302D.Oid = row;
                            T0302D.DocEntry = T0302;
                            T0302D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }
                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "AdjDepr")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0303 T0303 = os.CreateObject<FTS11T0303>();
                        DetailView dv = Application.CreateDetailView(os, T0303);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0303D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            FTS11T0303D T0303D = os.CreateObject<FTS11T0303D>();

                            T0303D.Oid = row;
                            T0303D.DocEntry = T0303;
                            T0303D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }
                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "Move")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0304 T0304 = os.CreateObject<FTS11T0304>();
                        DetailView dv = Application.CreateDetailView(os, T0304);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0304D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        { 
                            FTS11T0304D T0304D = os.CreateObject<FTS11T0304D>();

                            T0304D.Oid = row;
                            T0304D.DocEntry = T0304;
                            T0304D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }
                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReReValue")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0305 T0305 = os.CreateObject<FTS11T0305>();
                        DetailView dv = Application.CreateDetailView(os, T0305);

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        {
                            if (selectedObject.SQuantity <= 0)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                return;
                            }
                        }

                        using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                        {
                            using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                            {
                                da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0305D T0 ";
                                DataTable dt = new DataTable();
                                da.Fill(dt);

                                if (dt.Rows.Count > 0)
                                {
                                    foreach (DataRow dtrow in dt.Rows)
                                    {
                                        row = int.Parse(dtrow["MaxOid"].ToString());
                                    }
                                }
                            }
                        }

                        foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                        { 
                            FTS11T0305D T0305D = os.CreateObject<FTS11T0305D>();

                            T0305D.Oid = row;
                            T0305D.DocEntry = T0305;
                            T0305D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.DocEntry);
                            row++;
                        }
                        ShowViewParameters svp = new ShowViewParameters();
                        dv.ViewEditMode = ViewEditMode.Edit;
                        dv.IsRoot = true;
                        svp.CreatedView = dv;
                        Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }
            }


            if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
            {
                if (e.SelectedObjects.Count <= 0)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select an asset to proceed.");
                    return;
                }

                IObjectSpace os = Application.CreateObjectSpace(typeof(FTS11_vw_Component));

                if (e.SelectedChoiceActionItem.Id == "WriteOff")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0201 T0201 = os.CreateObject<FTS11T0201>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0201D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0201D T0201D = os.CreateObject<FTS11T0201D>();

                                T0201D.Oid = row;
                                T0201D.DocEntry = T0201;
                                T0201D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0201, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "Disposal")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0202 T0202 = os.CreateObject<FTS11T0202>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0202D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0202D T0202D = os.CreateObject<FTS11T0202D>();

                                T0202D.Oid = row;
                                T0202D.DocEntry = T0202;
                                T0202D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0202, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "CorrectionOut")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0203 T0203 = os.CreateObject<FTS11T0203>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                                using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                                {
                                    using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                    {
                                        da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0203D T0 ";
                                        DataTable dt = new DataTable();
                                        da.Fill(dt);

                                        if (dt.Rows.Count > 0)
                                        {
                                            foreach (DataRow dtrow in dt.Rows)
                                            {
                                                row = int.Parse(dtrow["MaxOid"].ToString());
                                            }
                                        }
                                    }
                                }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0203D T0203D = os.CreateObject<FTS11T0203D>();

                                T0203D.Oid = row;
                                T0203D.DocEntry = T0203;
                                T0203D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0203, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "TransferredOut")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0204 T0204 = os.CreateObject<FTS11T0204>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0204D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 

                                FTS11T0204D T0204D = os.CreateObject<FTS11T0204D>();

                                T0204D.Oid = row;
                                T0204D.DocEntry = T0204;
                                T0204D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0204, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReclassedOut")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0205 T0205 = os.CreateObject<FTS11T0205>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0205D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0205D T0205D = os.CreateObject<FTS11T0205D>();

                                T0205D.Oid = row;
                                T0205D.DocEntry = T0205;
                                T0205D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0205, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReUseLife")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0301 T0301 = os.CreateObject<FTS11T0301>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0301D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0301D T0301D = os.CreateObject<FTS11T0301D>();

                                T0301D.Oid = row;
                                T0301D.DocEntry = T0301;
                                T0301D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0301, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReCost")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0302 T0302 = os.CreateObject<FTS11T0302>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0302D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0302D T0302D = os.CreateObject<FTS11T0302D>();

                                T0302D.Oid = row;
                                T0302D.DocEntry = T0302;
                                T0302D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0302, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "AdjDepr")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0303 T0303 = os.CreateObject<FTS11T0303>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0303D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0303D T0303D = os.CreateObject<FTS11T0303D>();

                                T0303D.Oid = row;
                                T0303D.DocEntry = T0303;
                                T0303D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0303, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "Move")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0304 T0304 = os.CreateObject<FTS11T0304>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0304D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0304D T0304D = os.CreateObject<FTS11T0304D>();

                                T0304D.Oid = row;
                                T0304D.DocEntry = T0304;
                                T0304D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0304, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

                if (e.SelectedChoiceActionItem.Id == "ReReValue")
                {
                    try
                    {
                        int row = 0;
                        FTS11T0305 T0305 = os.CreateObject<FTS11T0305>();

                        if (View.ObjectTypeInfo.Type == typeof(FTS11_vw_Component))
                        {
                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            {
                                if (selectedObject.CFQty <= 0)
                                {
                                    genCon.MsgBox(InformationType.Error, "Error", "Kindly select only asset which yet disposed.");
                                    return;
                                }
                            }

                            using (SqlConnection conn = new SqlConnection(genCon.getConnectionString()))
                            {
                                using (SqlDataAdapter da = new SqlDataAdapter("", conn))
                                {
                                    da.SelectCommand.CommandText = "SELECT ISNULL(MAX(T0.Oid),0) + 1 AS [MaxOid] FROM FTS11T0305D T0 ";
                                    DataTable dt = new DataTable();
                                    da.Fill(dt);

                                    if (dt.Rows.Count > 0)
                                    {
                                        foreach (DataRow dtrow in dt.Rows)
                                        {
                                            row = int.Parse(dtrow["MaxOid"].ToString());
                                        }
                                    }
                                }
                            }

                            foreach (FTS11_vw_Component selectedObject in e.SelectedObjects)
                            { 
                                FTS11T0305D T0305D = os.CreateObject<FTS11T0305D>();

                                T0305D.Oid = row;
                                T0305D.DocEntry = T0305;
                                T0305D.U_FAKey = os.GetObjectByKey<FTS11OFAM>(selectedObject.ADocEntry);
                                row++;
                            }
                        }

                        DetailView dv = Application.CreateDetailView(os, T0305, true);
                        dv.ViewEditMode = ViewEditMode.Edit;
                        Application.MainWindow.SetView(dv);

                    }
                    catch (Exception ex)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", ex.Message);
                    }
                }

            }
        }

        private void CancelTrans_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
            {
                if (e.SelectedObjects.Count <= 0)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                    return;
                }

                foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                {
                    if (selectedObject.U_Status.Code != "O")
                    {
                        genCon.MsgBox(InformationType.Error, "Error", selectedObject.U_FACode.ToString() + " cannot be canceled.");
                        return;
                    }

                    FTS11OFAM TOFAM = (FTS11OFAM)selectedObject;
                    SystemUsers user = (SystemUsers)SecuritySystem.CurrentUser;

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    TOFAM.U_Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(TOFAM.Object, "C", TOFAM.DocEntry);
                    genCon.RecordChangeLog(TOFAM.Object, TOFAM.DocEntry);
                }
            }

            //Trans Group IN ('IN','DEP')
            if (View.ObjectTypeInfo.Type == typeof(FTS11TJDT))
            {
                foreach (FTS11TJDT selectedObject in e.SelectedObjects)
                {
                    FTS11TJDT TJDT = (FTS11TJDT)selectedObject;
                    SystemUsers user = (SystemUsers)SecuritySystem.CurrentUser;

                    // Depreciation
                    if (TJDT.TransType == "T9901" || TJDT.TransType == "T9902")
                    {
                        if (TJDT.Status.Code == "O")
                        {
                            genCon.executeNonQuery("UPDATE T0 SET T0.TransStatus = 'C' FROM FTS11JDT1 T0 WHERE T0.DocEntry = '" + TJDT.DocEntry + "' AND " +
                                "T0.DocEntry NOT IN (SELECT ReconKey FROM FTS11JDT1 WHERE ReconKey <> 0 AND FAKey = '" + TJDT.FAKey.DocEntry + "')");

                        }
                        else if (TJDT.Status.Code == "P")
                        {
                            genCon.executeNonQuery("EXEC sp_FTS11_ReverseJDT1 '" + TJDT.DocEntry + "','" + user.UserName.ToString() + "','" + TJDT.FAKey.DocEntry + "'");
                        }

                        genCon.executeNonQuery("EXEC sp_FTS11_SetIsFieldEditable '" + TJDT.FAKey.DocEntry + "'");
                    }
                    else
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "This transaction cannot be canceled.");
                        return;
                    }
                }
            }

            //Write Off
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0201))
            {
                foreach (FTS11T0201 selectedObject in e.SelectedObjects)
                {
                    FTS11T0201 T0201 = (FTS11T0201)selectedObject;

                    if (T0201.FTS11T0201D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0201.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0201.Object, "C", T0201.DocEntry);
                    genCon.RecordChangeLog(T0201.Object, T0201.DocEntry);
                }
            }

            //Disposed
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0202))
            {
                foreach (FTS11T0202 selectedObject in e.SelectedObjects)
                {
                    FTS11T0202 T0202 = (FTS11T0202)selectedObject;

                    if (T0202.FTS11T0202D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0202.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0202.Object, "C", T0202.DocEntry);
                    genCon.RecordChangeLog(T0202.Object, T0202.DocEntry);
                }
            }

            //Correction Out
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0203))
            {
                foreach (FTS11T0203 selectedObject in e.SelectedObjects)
                {
                    FTS11T0203 T0203 = (FTS11T0203)selectedObject;

                    if (T0203.FTS11T0203D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0203.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0203.Object, "C", T0203.DocEntry);
                    genCon.RecordChangeLog(T0203.Object, T0203.DocEntry);
                }
            }

            //Transferred Out
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0204))
            {
                foreach (FTS11T0204 selectedObject in e.SelectedObjects)
                {
                    FTS11T0204 T0204 = (FTS11T0204)selectedObject;

                    if (T0204.FTS11T0204D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0204.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0204.Object, "C", T0204.DocEntry);
                    genCon.RecordChangeLog(T0204.Object, T0204.DocEntry);
                }
            }

            //Reclassed Out
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0205))
            {
                foreach (FTS11T0205 selectedObject in e.SelectedObjects)
                {
                    FTS11T0205 T0205 = (FTS11T0205)selectedObject;

                    if (T0205.FTS11T0205D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0205.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0205.Object, "C", T0205.DocEntry);
                    genCon.RecordChangeLog(T0205.Object, T0205.DocEntry);
                }
            }

            //Review Useful Life
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0301))
            {
                foreach (FTS11T0301 selectedObject in e.SelectedObjects)
                {
                    FTS11T0301 T0301 = (FTS11T0301)selectedObject;

                    if (T0301.FTS11T0301D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0301.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0301.Object, "C", T0301.DocEntry);
                    genCon.RecordChangeLog(T0301.Object, T0301.DocEntry);
                }
            }

            //Revalue Cost
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0302))
            {
                foreach (FTS11T0302 selectedObject in e.SelectedObjects)
                {
                    FTS11T0302 T0302 = (FTS11T0302)selectedObject;

                    if (T0302.FTS11T0302D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0302.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0302.Object, "C", T0302.DocEntry);
                    genCon.RecordChangeLog(T0302.Object, T0302.DocEntry);
                }
            }

            //Adjust Accumulated Depr
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0303))
            {
                foreach (FTS11T0303 selectedObject in e.SelectedObjects)
                {
                    FTS11T0303 T0303 = (FTS11T0303)selectedObject;

                    if (T0303.FTS11T0303D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0303.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0303.Object, "C", T0303.DocEntry);
                    genCon.RecordChangeLog(T0303.Object, T0303.DocEntry);
                }
            }

            //Move
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0304))
            {
                foreach (FTS11T0304 selectedObject in e.SelectedObjects)
                {
                    FTS11T0304 T0304 = (FTS11T0304)selectedObject;

                    if (T0304.FTS11T0304D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0304.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0304.Object, "C", T0304.DocEntry);
                    genCon.RecordChangeLog(T0304.Object, T0304.DocEntry);
                }
            }

            //Revalue Residual Value
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0305))
            {
                foreach (FTS11T0305 selectedObject in e.SelectedObjects)
                {
                    FTS11T0305 T0305 = (FTS11T0305)selectedObject;

                    if (T0305.FTS11T0305D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No transaction selected.");
                        return;
                    }

                    SE_FAStatus FAStatus = ObjectSpace.FindObject<SE_FAStatus>(CriteriaOperator.Parse("Code = 'C'"));

                    T0305.Status = FAStatus;
                    ObjectSpace.CommitChanges();
                    genCon.PostTransactionNotice(T0305.Object, "C", T0305.DocEntry);
                    genCon.RecordChangeLog(T0305.Object, T0305.DocEntry);
                }
            }
        }

        private void Duplicate_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
            {
                if (e.SelectedObjects.Count <= 0)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                    return;
                }

                if (e.SelectedObjects.Count > 1)
                {
                    genCon.MsgBox(InformationType.Error, "Error", "Please select only 1 Asset to duplicate.");
                    return;
                }

                foreach (FTS11OFAM selectedObject in e.SelectedObjects)
                {
                    IObjectSpace os = Application.CreateObjectSpace();
                    FTS11OFAM OFAM = os.CreateObject<FTS11OFAM>();
                    DetailView dv = Application.CreateDetailView(os, OFAM);

                    OFAM.U_FAName = selectedObject.U_FAName;
                    OFAM.U_FAGrpKey = selectedObject.U_FAGrpKey == null ? null : os.GetObjectByKey<FTS11OFAG>(selectedObject.U_FAGrpKey.DocEntry);
                    OFAM.U_FAGrpSyn = selectedObject.U_FAGrpSyn;
                    OFAM.U_LocationKey = selectedObject.U_LocationKey == null ? null : os.GetObjectByKey<FTS11OLOC>(selectedObject.U_LocationKey.DocEntry);
                    OFAM.U_Quantity = selectedObject.U_Quantity;
                    OFAM.U_UoM = selectedObject.U_UoM;
                    OFAM.U_ExSC = selectedObject.U_ExSC;
                    OFAM.U_ExFC = selectedObject.U_ExFC;
                    OFAM.U_OriCost = selectedObject.U_OriCost;
                    OFAM.U_OriCostSC = selectedObject.U_OriCostSC;
                    OFAM.U_CostLC = selectedObject.U_CostLC;
                    OFAM.U_CostSC = selectedObject.U_CostSC;
                    OFAM.U_CostFC = selectedObject.U_CostFC;
                    OFAM.U_TCostLC = selectedObject.U_TCostLC;
                    OFAM.U_TCostSC = selectedObject.U_TCostSC;
                    OFAM.U_TCostFC = selectedObject.U_TCostFC;
                    OFAM.U_BaseLC = selectedObject.U_BaseLC;
                    OFAM.U_BaseSC = selectedObject.U_BaseSC;
                    OFAM.U_TBaseLC = selectedObject.U_TBaseLC;
                    OFAM.U_TBaseSC = selectedObject.U_TBaseSC;
                    OFAM.U_Revalue = selectedObject.U_Revalue;
                    OFAM.U_RevalueSC = selectedObject.U_RevalueSC;
                    OFAM.U_AccFrmA = selectedObject.U_AccFrmA == null ? null : os.GetObjectByKey<FTS11_vw_OACT>(selectedObject.U_AccFrmA.FormatCode);
                    OFAM.U_AccFrmC = selectedObject.U_AccFrmC == null ? null : os.GetObjectByKey<FTS11_vw_OACT>(selectedObject.U_AccFrmC.FormatCode);
                    OFAM.U_AccFrmD = selectedObject.U_AccFrmD == null ? null : os.GetObjectByKey<FTS11_vw_OACT>(selectedObject.U_AccFrmD.FormatCode);
                    OFAM.U_AccFrmQ = selectedObject.U_AccFrmQ == null ? null : os.GetObjectByKey<FTS11_vw_OACT>(selectedObject.U_AccFrmQ.FormatCode);
                    OFAM.U_AccFrmV = selectedObject.U_AccFrmV == null ? null : os.GetObjectByKey<FTS11_vw_OACT_OCRD>(selectedObject.U_AccFrmV.FormatCode);
                    OFAM.U_AccFrmW = selectedObject.U_AccFrmW == null ? null : os.GetObjectByKey<FTS11_vw_OACT>(selectedObject.U_AccFrmW.FormatCode);
                    OFAM.U_DeprnYN = selectedObject.U_DeprnYN;
                    OFAM.U_DeprnTyp = selectedObject.U_DeprnTyp == null ? null : os.GetObjectByKey<SE_FADeprnTyp>(selectedObject.U_DeprnTyp.Code);
                    OFAM.U_DeprnBy = selectedObject.U_DeprnBy == null ? null : os.GetObjectByKey<SE_FADeprnBy>(selectedObject.U_DeprnBy.Code);
                    OFAM.U_Prorate = selectedObject.U_Prorate == null ? null : os.GetObjectByKey<SE_FAProrate>(selectedObject.U_Prorate.Code);
                    OFAM.U_UseLife = selectedObject.U_UseLife;
                    OFAM.U_DeprPrcnt = selectedObject.U_DeprPrcnt;
                    OFAM.U_MBaseLC = selectedObject.U_MBaseLC;
                    OFAM.U_MBaseSC = selectedObject.U_MBaseSC;
                    OFAM.U_Project = selectedObject.U_Project == null ? null : os.GetObjectByKey<FTS11_vw_OPRJ>(selectedObject.U_Project.PrjCode);
                    OFAM.U_ProfitCode = selectedObject.U_ProfitCode == null ? null : os.GetObjectByKey<FTS11_vw_OPRC>(selectedObject.U_ProfitCode.PrcCode);
                    OFAM.U_OcrCode2 = selectedObject.U_OcrCode2 == null ? null : os.GetObjectByKey<FTS11_vw_OPRC>(selectedObject.U_OcrCode2.PrcCode);
                    OFAM.U_OcrCode3 = selectedObject.U_OcrCode3 == null ? null : os.GetObjectByKey<FTS11_vw_OPRC>(selectedObject.U_OcrCode3.PrcCode);
                    OFAM.U_OcrCode4 = selectedObject.U_OcrCode4 == null ? null : os.GetObjectByKey<FTS11_vw_OPRC>(selectedObject.U_OcrCode4.PrcCode);
                    OFAM.U_OcrCode5 = selectedObject.U_OcrCode5 == null ? null : os.GetObjectByKey<FTS11_vw_OPRC>(selectedObject.U_OcrCode5.PrcCode);
                    OFAM.U_IndKey1 = selectedObject.U_IndKey1 == null ? null : os.GetObjectByKey<FTS11OIND>(selectedObject.U_IndKey1.DocEntry);
                    OFAM.U_IndKey2 = selectedObject.U_IndKey2 == null ? null : os.GetObjectByKey<FTS11OIND>(selectedObject.U_IndKey2.DocEntry);
                    OFAM.U_IndKey3 = selectedObject.U_IndKey3 == null ? null : os.GetObjectByKey<FTS11OIND>(selectedObject.U_IndKey3.DocEntry);
                    OFAM.U_IndKey4 = selectedObject.U_IndKey4 == null ? null : os.GetObjectByKey<FTS11OIND>(selectedObject.U_IndKey4.DocEntry);
                    OFAM.U_IndKey5 = selectedObject.U_IndKey5 == null ? null : os.GetObjectByKey<FTS11OIND>(selectedObject.U_IndKey5.DocEntry);
                    OFAM.U_IndKey6 = selectedObject.U_IndKey6 == null ? null : os.GetObjectByKey<FTS11OIND>(selectedObject.U_IndKey6.DocEntry);
                    OFAM.U_BarCode = selectedObject.U_BarCode;
                    OFAM.U_BarCodYN = selectedObject.U_BarCodYN;
                    OFAM.U_TagNum = selectedObject.U_TagNum;
                    OFAM.U_WarExpr = selectedObject.U_WarExpr;
                    OFAM.U_Legacy = selectedObject.U_Legacy;
                    OFAM.U_Serial = selectedObject.U_Serial;
                    OFAM.U_Model = selectedObject.U_Model;
                    OFAM.U_InsNum = selectedObject.U_InsNum;
                    OFAM.U_InsDate = selectedObject.U_InsDate;
                    OFAM.U_InsType = selectedObject.U_InsType;
                    OFAM.U_InsCov = selectedObject.U_InsCov;
                    OFAM.U_InsComp = selectedObject.U_InsComp;
                    OFAM.U_LeasRef = selectedObject.U_LeasRef;
                    OFAM.U_LeasDate = selectedObject.U_LeasDate;
                    OFAM.U_RefDoc1 = selectedObject.U_RefDoc1;
                    OFAM.U_RefDoc2 = selectedObject.U_RefDoc2;
                    OFAM.U_RefDoc3 = selectedObject.U_RefDoc3;
                    OFAM.U_RefDate1 = selectedObject.U_RefDate1;
                    OFAM.U_RefDate2 = selectedObject.U_RefDate2;
                    OFAM.U_RefDate3 = selectedObject.U_RefDate3;
                    OFAM.U_Remark1 = selectedObject.U_Remark1;
                    OFAM.U_Remark2 = selectedObject.U_Remark2;
                    OFAM.U_Remark3 = selectedObject.U_Remark3;
                    OFAM.U_Remark4 = selectedObject.U_Remark4;
                    OFAM.U_Text = selectedObject.U_Text;

                    ShowViewParameters svp = new ShowViewParameters();
                    dv.ViewEditMode = ViewEditMode.Edit;
                    dv.IsRoot = true;
                    svp.CreatedView = dv;
                    Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
                }
            }
        }

        private void Advanced_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            FTS11OFAM OFAM = (FTS11OFAM)View.CurrentObject;
            IObjectSpace os = Application.CreateObjectSpace(typeof(FTS11FAM2));

            PropertyCollectionSource propertyCollectionSource = Application.CreatePropertyCollectionSource(os, typeof(FTS11OFAM), os.GetObject(OFAM), View.ObjectTypeInfo.FindMember(nameof(OFAM.FTS11FAM2)), Application.FindListViewId(typeof(FTS11FAM2)));

            ListView listView = Application.CreateListView(Application.FindListViewId(typeof(FTS11FAM2)), propertyCollectionSource, true);

            Application.ShowViewStrategy.ShowViewInPopupWindow(listView);
        }
    }
}