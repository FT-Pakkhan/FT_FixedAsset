using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Xpo;
using FT_FixedAsset.Module.BusinessObjects;
using FT_FixedAsset.Module.Controllers;
using System;
using System.Collections;
using System.Data.SqlClient;
using System.Linq;

namespace FT_FixedAsset.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class WebModifyControllers : WebModificationsController
    {
        GenController genCon;
        public WebModifyControllers()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
            genCon = Frame.GetController<GenController>();
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        protected override void SaveAndClose(SimpleActionExecuteEventArgs args)
        {
            string query = "";
            SystemUsers user = (SystemUsers)SecuritySystem.CurrentUser;

            // General Settings
            if (View.ObjectTypeInfo.Type == typeof(FTS11OADM))
            {
                ObjectSpace.CommitChanges();
                genCon.RegenerateView();
            }

            //Fixed Asset Class
            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAC))
            {
                foreach (FTS11OFAC selectedObject in args.SelectedObjects)
                {
                    FTS11OFAC OFAC = (FTS11OFAC)selectedObject;

                    if (OFAC.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OFAC.Object, "A", OFAC.DocEntry);
                        genCon.RecordChangeLog(OFAC.Object, OFAC.DocEntry);
                    }
                    else
                    {
                        OFAC.U_UpdateBy = user.UserName.ToString();
                        OFAC.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OFAC.Object, "U", OFAC.DocEntry);
                        genCon.RecordChangeLog(OFAC.Object, OFAC.DocEntry);
                    }
                }
            }

            //Indicator
            if (View.ObjectTypeInfo.Type == typeof(FTS11OIND))
            {
                foreach (FTS11OIND selectedObject in args.SelectedObjects)
                {
                    FTS11OIND OIND = (FTS11OIND)selectedObject;

                    if (OIND.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OIND.Object, "A", OIND.DocEntry);
                        genCon.RecordChangeLog(OIND.Object, OIND.DocEntry);
                    }
                    else
                    {
                        OIND.U_UpdateBy = user.UserName.ToString();
                        OIND.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OIND.Object, "U", OIND.DocEntry);
                        genCon.RecordChangeLog(OIND.Object, OIND.DocEntry);
                    }
                }
            }

            //Fixed Asset Location
            if (View.ObjectTypeInfo.Type == typeof(FTS11OLOC))
            {
                foreach (FTS11OLOC selectedObject in args.SelectedObjects)
                {
                    FTS11OLOC OLOC = (FTS11OLOC)selectedObject;

                    if (OLOC.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OLOC.Object, "A", OLOC.DocEntry);
                        genCon.RecordChangeLog(OLOC.Object, OLOC.DocEntry);
                    }
                    else
                    {
                        OLOC.U_UpdateBy = user.UserName.ToString();
                        OLOC.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OLOC.Object, "U", OLOC.DocEntry);
                        genCon.RecordChangeLog(OLOC.Object, OLOC.DocEntry);
                    }
                }
            }

            //Fixed Asset Group
            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAG))
            {
                foreach (FTS11OFAG selectedObject in args.SelectedObjects)
                {
                    FTS11OFAG OFAG = (FTS11OFAG)selectedObject;

                    if (OFAG.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OFAG.Object, "A", OFAG.DocEntry);
                        genCon.RecordChangeLog(OFAG.Object, OFAG.DocEntry);
                    }
                    else
                    {
                        OFAG.U_UpdateBy = user.UserName.ToString();
                        OFAG.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OFAG.Object, "U", OFAG.DocEntry);
                        genCon.RecordChangeLog(OFAG.Object, OFAG.DocEntry);
                    }
                }
            }

            //Fixed Asset Master
            if (View.ObjectTypeInfo.Type == typeof(FTS11OFAM))
            {
                foreach (FTS11OFAM selectedObject in args.SelectedObjects)
                {
                    string docType = "";
                    FTS11OFAM OFAM = (FTS11OFAM)selectedObject;

                    if (OFAM.IsNew == true && string.IsNullOrEmpty(OFAM.U_FACode))
                    {
                        IObjectSpace os = Application.CreateObjectSpace();
                        FTS11OADM OADM = os.FindObject<FTS11OADM>(CriteriaOperator.Parse("DocEntry = '1'"));

                        if (OADM.U_FACodeBy.Code == "FAClsCod")
                            docType = OFAM.U_FAGrpKey == null ? "" : OFAM.U_FAGrpKey.U_FAClsCod;
                        if (OADM.U_FACodeBy.Code == "FAGrpCod")
                            docType = OFAM.U_FAGrpKey == null ? "" : OFAM.U_FAGrpCod;
                        if (OADM.U_FACodeBy.Code == "IndCod1")
                            docType = OFAM.U_IndKey1 == null ? "" : OFAM.U_IndCod1;
                        if (OADM.U_FACodeBy.Code == "IndCod2")
                            docType = OFAM.U_IndKey2 == null ? "" : OFAM.U_IndCod2;
                        if (OADM.U_FACodeBy.Code == "IndCod3")
                            docType = OFAM.U_IndKey3 == null ? "" : OFAM.U_IndCod3;
                        if (OADM.U_FACodeBy.Code == "IndCod4")
                            docType = OFAM.U_IndKey4 == null ? "" : OFAM.U_IndCod4;
                        if (OADM.U_FACodeBy.Code == "IndCod5")
                            docType = OFAM.U_IndKey5 == null ? "" : OFAM.U_IndCod5;
                        if (OADM.U_FACodeBy.Code == "IndCod6")
                            docType = OFAM.U_IndKey6 == null ? "" : OFAM.U_IndCod6;
                        if (OADM.U_FACodeBy.Code == "Project")
                            docType = OFAM.U_Project == null ? "" : OFAM.U_Project.PrjCode;
                        if (OADM.U_FACodeBy.Code == "ProfitCo")
                            docType = OFAM.U_ProfitCode == null ? "" : OFAM.U_ProfitCode.PrcCode;
                        if (OADM.U_FACodeBy.Code == "OcrCode2")
                            docType = OFAM.U_OcrCode2 == null ? "" : OFAM.U_OcrCode2.PrcCode;
                        if (OADM.U_FACodeBy.Code == "OcrCode3")
                            docType = OFAM.U_OcrCode3 == null ? "" : OFAM.U_OcrCode3.PrcCode;
                        if (OADM.U_FACodeBy.Code == "OcrCode4")
                            docType = OFAM.U_OcrCode4 == null ? "" : OFAM.U_OcrCode4.PrcCode;
                        if (OADM.U_FACodeBy.Code == "OcrCode5")
                            docType = OFAM.U_OcrCode5 == null ? "" : OFAM.U_OcrCode5.PrcCode;
                        if (OADM.U_FACodeBy.Code == "Location")
                            docType = OFAM.U_LocationKey == null ? "" : OFAM.U_Location;

                        FTS11NNM1 code = os.FindObject<FTS11NNM1>(CriteriaOperator.Parse("DocType=?", docType));

                        if (code != null)
                        {
                            genCon.AutoNumbering(OADM, code, OFAM, os);
                            os.CommitChanges();
                        }
                        else
                        {
                            genCon.MsgBox(InformationType.Error, "Error", "F/A Code cannot be empty, kindly set numbering for " + OADM.U_FACodeBy.Name + ".");
                            return;
                        }

                    }

                    if (OFAM.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OFAM.Object, "A", OFAM.DocEntry);
                        genCon.RecordChangeLog(OFAM.Object, OFAM.DocEntry);
                    }
                    else
                    {
                        OFAM.U_UpdateBy = user.UserName.ToString();
                        OFAM.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(OFAM.Object, "U", OFAM.DocEntry);
                        genCon.RecordChangeLog(OFAM.Object, OFAM.DocEntry);
                    }
                }
            }

            //Write Off
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0201))
            {
                foreach (FTS11T0201 selectedObject in args.SelectedObjects)
                {
                    FTS11T0201 T0201 = (FTS11T0201)selectedObject;

                    if (T0201.FTS11T0201D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0201.FTS11T0201D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0201D T0201D in T0201.FTS11T0201D)
                    {
                        if (T0201D.LineStatus.Code == "O")
                        {
                            query = "DECLARE @DocEntry int " +
                            "SELECT @DocEntry = 0 " +
                            "SELECT @DocEntry = DocEntry FROM FTS11JDT1 " +
                            "WHERE BaseType = '" + T0201.Object +"' AND BaseEntry = '" + T0201.DocEntry +"' AND BaseLineEntry = '" + T0201D.Oid + "' " +
                            "AND TransStatus <> 'C' " +
                            "SELECT T0.FAKey, T0.Location, SUM(T0.Quantity) AS [Quantity] FROM FTS11JDT1 T0 " +
                            "WHERE T0.TransStatus <> 'C' AND T0.Location = '" + T0201D.U_LocationKey.Code +"' AND T0.FAKey = '" + T0201D.U_FAKey.DocEntry + "' " +
                            "AND ISNULL(T0.DocEntry,0) <> @DocEntry " +
                            "GROUP BY T0.FAKey, T0.Location " +
                            "HAVING SUM(T0.Quantity) < '" + T0201D.U_Quantity +"' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0201D.U_FACode + " - " + T0201D.U_FAName + ": Insufficient Quantity in Location.");
                                return;
                            }

                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0201D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0201.U_DocDate.Year, T0201.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0201D.U_FACode + " - " + T0201D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0201.U_DocDate < T0201D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0201D.U_FACode + " - " + T0201D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0201.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0201.Object, "A", T0201.DocEntry);
                        genCon.RecordChangeLog(T0201.Object, T0201.DocEntry);
                    }
                    else
                    {
                        T0201.U_UpdateBy = user.UserName.ToString();
                        T0201.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0201.Object, "U", T0201.DocEntry);
                        genCon.RecordChangeLog(T0201.Object, T0201.DocEntry);
                    }
                }
            }

            //Disposed
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0202))
            {
                foreach (FTS11T0202 selectedObject in args.SelectedObjects)
                {
                    FTS11T0202 T0202 = (FTS11T0202)selectedObject;

                    if (T0202.FTS11T0202D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0202.FTS11T0202D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0202D T0202D in T0202.FTS11T0202D)
                    {
                        if (T0202D.LineStatus.Code == "O")
                        {
                            query = "DECLARE @DocEntry int " +
                            "SELECT @DocEntry = 0 " +
                            "SELECT @DocEntry = DocEntry FROM FTS11JDT1 " +
                            "WHERE BaseType = '" + T0202.Object + "' AND BaseEntry = '" + T0202.DocEntry + "' AND BaseLineEntry = '" + T0202D.Oid + "' " +
                            "AND TransStatus <> 'C' " +
                            "SELECT T0.FAKey, T0.Location, SUM(T0.Quantity) AS [Quantity] FROM FTS11JDT1 T0 " +
                            "WHERE T0.TransStatus <> 'C' AND T0.Location = '" + T0202D.U_LocationKey.Code + "' AND T0.FAKey = '" + T0202D.U_FAKey.DocEntry + "' " +
                            "AND ISNULL(T0.DocEntry,0) <> @DocEntry " +
                            "GROUP BY T0.FAKey, T0.Location " +
                            "HAVING SUM(T0.Quantity) < '" + T0202D.U_Quantity + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0202D.U_FACode + " - " + T0202D.U_FAName + ": Insufficient Quantity in Location.");
                                return;
                            }

                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0202D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0202.U_DocDate.Year, T0202.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0202D.U_FACode + " - " + T0202D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0202.U_DocDate < T0202D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0202D.U_FACode + " - " + T0202D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0202.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0202.Object, "A", T0202.DocEntry);
                        genCon.RecordChangeLog(T0202.Object, T0202.DocEntry);
                    }
                    else
                    {
                        T0202.U_UpdateBy = user.UserName.ToString();
                        T0202.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0202.Object, "U", T0202.DocEntry);
                        genCon.RecordChangeLog(T0202.Object, T0202.DocEntry);
                    }

                }
            }

            //Correction Out
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0203))
            {
                foreach (FTS11T0203 selectedObject in args.SelectedObjects)
                {
                    FTS11T0203 T0203 = (FTS11T0203)selectedObject;

                    if (T0203.FTS11T0203D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0203.FTS11T0203D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0203D T0203D in T0203.FTS11T0203D)
                    {
                        if (T0203D.LineStatus.Code == "O")
                        {
                            query = "DECLARE @DocEntry int " +
                            "SELECT @DocEntry = 0 " +
                            "SELECT @DocEntry = DocEntry FROM FTS11JDT1 " +
                            "WHERE BaseType = '" + T0203.Object + "' AND BaseEntry = '" + T0203.DocEntry + "' AND BaseLineEntry = '" + T0203D.Oid + "' " +
                            "AND TransStatus <> 'C' " +
                            "SELECT T0.FAKey, T0.Location, SUM(T0.Quantity) AS [Quantity] FROM FTS11JDT1 T0 " +
                            "WHERE T0.TransStatus <> 'C' AND T0.Location = '" + T0203D.U_LocationKey.Code + "' AND T0.FAKey = '" + T0203D.U_FAKey.DocEntry + "' " +
                            "AND ISNULL(T0.DocEntry,0) <> @DocEntry " +
                            "GROUP BY T0.FAKey, T0.Location " +
                            "HAVING SUM(T0.Quantity) < '" + T0203D.U_Quantity + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0203D.U_FACode + " - " + T0203D.U_FAName + ": Insufficient Quantity in Location.");
                                return;
                            }

                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0203D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0203.U_DocDate.Year, T0203.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0203D.U_FACode + " - " + T0203D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0203.U_DocDate < T0203D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0203D.U_FACode + " - " + T0203D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0203.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0203.Object, "A", T0203.DocEntry);
                        genCon.RecordChangeLog(T0203.Object, T0203.DocEntry);
                    }
                    else
                    {
                        T0203.U_UpdateBy = user.UserName.ToString();
                        T0203.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0203.Object, "U", T0203.DocEntry);
                        genCon.RecordChangeLog(T0203.Object, T0203.DocEntry);
                    }
                }
            }

            //Transferred Out
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0204))
            {
                foreach (FTS11T0204 selectedObject in args.SelectedObjects)
                {
                    FTS11T0204 T0204 = (FTS11T0204)selectedObject;

                    if (T0204.FTS11T0204D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0204.FTS11T0204D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0204D T0204D in T0204.FTS11T0204D)
                    {
                        if (T0204D.LineStatus.Code == "O")
                        {
                            query = "DECLARE @DocEntry int " +
                            "SELECT @DocEntry = 0 " +
                            "SELECT @DocEntry = DocEntry FROM FTS11JDT1 " +
                            "WHERE BaseType = '" + T0204.Object + "' AND BaseEntry = '" + T0204.DocEntry + "' AND BaseLineEntry = '" + T0204D.Oid + "' " +
                            "AND TransStatus <> 'C' " +
                            "SELECT T0.FAKey, T0.Location, SUM(T0.Quantity) AS [Quantity] FROM FTS11JDT1 T0 " +
                            "WHERE T0.TransStatus <> 'C' AND T0.Location = '" + T0204D.U_LocationKey.Code + "' AND T0.FAKey = '" + T0204D.U_FAKey.DocEntry + "' " +
                            "AND ISNULL(T0.DocEntry,0) <> @DocEntry " +
                            "GROUP BY T0.FAKey, T0.Location " +
                            "HAVING SUM(T0.Quantity) < '" + T0204D.U_Quantity + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0204D.U_FACode + " - " + T0204D.U_FAName + ": Insufficient Quantity in Location.");
                                return;
                            }

                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0204D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0204.U_DocDate.Year, T0204.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0204D.U_FACode + " - " + T0204D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0204.U_DocDate < T0204D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0204D.U_FACode + " - " + T0204D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0204.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0204.Object, "A", T0204.DocEntry);
                        genCon.RecordChangeLog(T0204.Object, T0204.DocEntry);
                    }
                    else
                    {
                        T0204.U_UpdateBy = user.UserName.ToString();
                        T0204.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0204.Object, "U", T0204.DocEntry);
                        genCon.RecordChangeLog(T0204.Object, T0204.DocEntry);
                    }
                }
            }

            //Reclassed Out
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0205))
            {
                foreach (FTS11T0205 selectedObject in args.SelectedObjects)
                {
                    FTS11T0205 T0205 = (FTS11T0205)selectedObject;

                    if (T0205.FTS11T0205D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0205.FTS11T0205D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0205D T0205D in T0205.FTS11T0205D)
                    {
                        if (T0205D.LineStatus.Code == "O")
                        {
                            query = "DECLARE @DocEntry int " +
                            "SELECT @DocEntry = 0 " +
                            "SELECT @DocEntry = DocEntry FROM FTS11JDT1 " +
                            "WHERE BaseType = '" + T0205.Object + "' AND BaseEntry = '" + T0205.DocEntry + "' AND BaseLineEntry = '" + T0205D.Oid + "' " +
                            "AND TransStatus <> 'C' " +
                            "SELECT T0.FAKey, T0.Location, SUM(T0.Quantity) AS [Quantity] FROM FTS11JDT1 T0 " +
                            "WHERE T0.TransStatus <> 'C' AND T0.Location = '" + T0205D.U_LocationKey.Code + "' AND T0.FAKey = '" + T0205D.U_FAKey.DocEntry + "' " +
                            "AND ISNULL(T0.DocEntry,0) <> @DocEntry " +
                            "GROUP BY T0.FAKey, T0.Location " +
                            "HAVING SUM(T0.Quantity) < '" + T0205D.U_Quantity + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0205D.U_FACode + " - " + T0205D.U_FAName + ": Insufficient Quantity in Location.");
                                return;
                            }

                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0205D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0205.U_DocDate.Year, T0205.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0205D.U_FACode + " - " + T0205D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0205.U_DocDate < T0205D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0205D.U_FACode + " - " + T0205D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0205.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0205.Object, "A", T0205.DocEntry);
                        genCon.RecordChangeLog(T0205.Object, T0205.DocEntry);
                    }
                    else
                    {
                        T0205.U_UpdateBy = user.UserName.ToString();
                        T0205.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0205.Object, "U", T0205.DocEntry);
                        genCon.RecordChangeLog(T0205.Object, T0205.DocEntry);
                    }
                }
            }

            //Review Useful Life
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0301))
            {
                foreach (FTS11T0301 selectedObject in args.SelectedObjects)
                {
                    FTS11T0301 T0301 = (FTS11T0301)selectedObject;

                    if (T0301.FTS11T0301D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0301.FTS11T0301D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0301D T0301D in T0301.FTS11T0301D)
                    {
                        if (T0301D.LineStatus.Code == "O")
                        {
                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0301D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0301.U_DocDate.Year, T0301.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0301D.U_FACode + " - " + T0301D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0301.U_DocDate < T0301D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0301D.U_FACode + " - " + T0301D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0301.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0301.Object, "A", T0301.DocEntry);
                        genCon.RecordChangeLog(T0301.Object, T0301.DocEntry);
                    }
                    else
                    {
                        T0301.U_UpdateBy = user.UserName.ToString();
                        T0301.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0301.Object, "U", T0301.DocEntry);
                        genCon.RecordChangeLog(T0301.Object, T0301.DocEntry);
                    }
                }
            }

            //Revalue Cost
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0302))
            {
                foreach (FTS11T0302 selectedObject in args.SelectedObjects)
                {
                    FTS11T0302 T0302 = (FTS11T0302)selectedObject;

                    if (T0302.FTS11T0302D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0302.FTS11T0302D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0302D T0302D in T0302.FTS11T0302D)
                    {
                        if (T0302D.LineStatus.Code == "O")
                        {
                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0302D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0302.U_DocDate.Year, T0302.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0302D.U_FACode + " - " + T0302D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0302.U_DocDate < T0302D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0302D.U_FACode + " - " + T0302D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0302.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0302.Object, "A", T0302.DocEntry);
                        genCon.RecordChangeLog(T0302.Object, T0302.DocEntry);
                    }
                    else
                    {
                        T0302.U_UpdateBy = user.UserName.ToString();
                        T0302.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0302.Object, "U", T0302.DocEntry);
                        genCon.RecordChangeLog(T0302.Object, T0302.DocEntry);
                    }
                }
            }

            //Adjust Accumulated Depr
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0303))
            {
                foreach (FTS11T0303 selectedObject in args.SelectedObjects)
                {
                    FTS11T0303 T0303 = (FTS11T0303)selectedObject;

                    if (T0303.FTS11T0303D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0303.FTS11T0303D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0303D T0303D in T0303.FTS11T0303D)
                    {
                        if (T0303D.LineStatus.Code == "O")
                        {
                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0303D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0303.U_DocDate.Year, T0303.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0303D.U_FACode + " - " + T0303D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0303.U_DocDate < T0303D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0303D.U_FACode + " - " + T0303D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0303.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0303.Object, "A", T0303.DocEntry);
                        genCon.RecordChangeLog(T0303.Object, T0303.DocEntry);
                    }
                    else
                    {
                        T0303.U_UpdateBy = user.UserName.ToString();
                        T0303.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0303.Object, "U", T0303.DocEntry);
                        genCon.RecordChangeLog(T0303.Object, T0303.DocEntry);
                    }
                }
            }

            //Move
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0304))
            {
                foreach (FTS11T0304 selectedObject in args.SelectedObjects)
                {
                    FTS11T0304 T0304 = (FTS11T0304)selectedObject;

                    if (T0304.FTS11T0304D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0304.FTS11T0304D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0304D T0304D in T0304.FTS11T0304D)
                    {
                        if (T0304D.LineStatus.Code == "O")
                        {
                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0304D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0304.U_DocDate.Year, T0304.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0304D.U_FACode + " - " + T0304D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0304.U_DocDate < T0304D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0304D.U_FACode + " - " + T0304D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0304.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0304.Object, "A", T0304.DocEntry);
                        genCon.RecordChangeLog(T0304.Object, T0304.DocEntry);
                    }
                    else
                    {
                        T0304.U_UpdateBy = user.UserName.ToString();
                        T0304.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0304.Object, "U", T0304.DocEntry);
                        genCon.RecordChangeLog(T0304.Object, T0304.DocEntry);
                    }
                }
            }

            //Revalue Residual Value
            if (View.ObjectTypeInfo.Type == typeof(FTS11T0305))
            {
                foreach (FTS11T0305 selectedObject in args.SelectedObjects)
                {
                    FTS11T0305 T0305 = (FTS11T0305)selectedObject;

                    if (T0305.FTS11T0305D.Count <= 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "No Asset selected.");
                        return;
                    }

                    var list = T0305.FTS11T0305D.GroupBy(x => x.U_FAKey)
                        .Where(g => g.Count() > 1)
                        .Select(x => x.Key).ToList();

                    if (list.Count > 0)
                    {
                        genCon.MsgBox(InformationType.Error, "Error", "Duplicate Asset found in list.");
                        return;
                    }

                    foreach (FTS11T0305D T0305D in T0305.FTS11T0305D)
                    {
                        if (T0305D.LineStatus.Code == "O")
                        {
                            query = "SELECT * FROM FTS11JDT1 T0 INNER JOIN FTS11STRN T1 ON T0.TransType = T1.Code " +
                                "WHERE T0.TransStatus = 'P' " +
                                "AND T0.AcctCat = 'ACCU' AND T1.U_TrnsGrp = 'DEP' " +
                                "AND T0.FAKey = '" + T0305D.U_FAKey.DocEntry + "' AND T0.DocDate >= '" + new DateTime(T0305.U_DocDate.Year, T0305.U_DocDate.Month, 1).ToString("yyyyMMdd") + "' ";

                            if (genCon.TransactionNotification(query) == true)
                            {
                                genCon.MsgBox(InformationType.Error, "Error", T0305D.U_FACode + " - " + T0305D.U_FAName + ": Posted Depreciation found after Document Date.");
                                return;
                            }
                        }

                        if (T0305.U_DocDate < T0305D.U_FAKey.U_AcqDate)
                        {
                            genCon.MsgBox(InformationType.Error, "Error", T0305D.U_FACode + " - " + T0305D.U_FAName + ": Document Date earlier than Asset Acquisition Date.");
                            return;
                        }
                    }

                    if (T0305.IsNew == true)
                    {
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0305.Object, "A", T0305.DocEntry);
                        genCon.RecordChangeLog(T0305.Object, T0305.DocEntry);
                    }
                    else
                    {
                        T0305.U_UpdateBy = user.UserName.ToString();
                        T0305.UpdateDate = DateTime.Now;
                        ObjectSpace.CommitChanges();
                        genCon.PostTransactionNotice(T0305.Object, "U", T0305.DocEntry);
                        genCon.RecordChangeLog(T0305.Object, T0305.DocEntry);
                    }
                }
            }
            
            base.SaveAndClose(args);
        }

    }
}
