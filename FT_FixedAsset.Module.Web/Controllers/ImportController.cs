using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.Persistent.Base;
using DevExpress.Web;
using FT_FixedAsset.Module.Controllers;
using FT_FixedAsset.Module.Web.Import;

namespace FT_FixedAsset.Module.Web.Controllers
{
    /// <summary>Opens the "Data Import > Import Data" navigation item as an editable screen (same pattern as the Widgets).</summary>
    public class DataImportNavigationController : WindowController
    {
        public const string NavigationItemId = "DataImport_ListView";
        private ShowNavigationItemController navController;

        public DataImportNavigationController()
        {
            TargetWindowType = WindowType.Main;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            navController = Frame.GetController<ShowNavigationItemController>();
            if (navController != null)
                navController.CustomShowNavigationItem += NavController_CustomShowNavigationItem;
        }

        protected override void OnDeactivated()
        {
            if (navController != null)
                navController.CustomShowNavigationItem -= NavController_CustomShowNavigationItem;
            base.OnDeactivated();
        }

        private void NavController_CustomShowNavigationItem(object sender, CustomShowNavigationItemEventArgs e)
        {
            if (e.ActionArguments.SelectedChoiceActionItem == null || e.ActionArguments.SelectedChoiceActionItem.Id != NavigationItemId) return;

            IObjectSpace os = Application.CreateObjectSpace(typeof(DataImport));
            DataImport screen = os.CreateObject<DataImport>();
            DetailView dv = Application.CreateDetailView(os, screen, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.ActionArguments.ShowViewParameters.CreatedView = dv;
            e.Handled = true;
        }
    }

    /// <summary>"Import" and "Download Template" buttons on the Data Import screen.</summary>
    public class DataImportController : ObjectViewController<DetailView, DataImport>
    {
        private readonly SimpleAction importAction;
        private readonly SimpleAction templateAction;
        private readonly List<Controller> hiddenControllers = new List<Controller>();

        public DataImportController()
        {
            importAction = new SimpleAction(this, "DataImport_Import", PredefinedCategory.Edit)
            {
                Caption = "Import",
                ToolTip = "Validate the file and import it (nothing is saved if any row has an error)",
                ImageName = "Action_Save"
            };
            importAction.Execute += ImportAction_Execute;

            templateAction = new SimpleAction(this, "DataImport_Template", PredefinedCategory.Edit)
            {
                Caption = "Download Template",
                ToolTip = "Download the Excel layout for the selected 'Import Into'",
                ImageName = "Action_Export_ToExcel"
            };
            templateAction.Execute += TemplateAction_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // uploading the file needs a full postback (the template download goes through ImportDownload.ashx instead)
            var webModel = importAction.Model as IModelActionWeb;
            if (webModel != null) webModel.IsPostBackRequired = true;

            // this screen is not a record: no Save / New / Delete
            Hide(Frame.GetController<ModificationsController>());
            Hide(Frame.GetController<NewObjectViewController>());
            Hide(Frame.GetController<DeleteObjectsViewController>());

            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
        }

        #region file picker: only .xlsx / .xls / .txt / .csv
        public static readonly string[] AllowedExtensions = { ".xlsx", ".xls", ".txt", ".csv" };

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            foreach (FileDataPropertyEditor editor in View.GetItems<FileDataPropertyEditor>())
            {
                if (editor.Editor != null) RestrictFileTypes(editor.Editor);
                else editor.ControlCreated += FileEditor_ControlCreated;
            }
        }

        private void FileEditor_ControlCreated(object sender, EventArgs e)
        {
            var editor = (FileDataPropertyEditor)sender;
            editor.ControlCreated -= FileEditor_ControlCreated;
            if (editor.Editor != null) RestrictFileTypes(editor.Editor);
        }

        private void RestrictFileTypes(FileDataEdit edit)
        {
            if (edit.UploadControl != null) RestrictFileTypes(edit.UploadControl);
            edit.UploadControlCreated -= FileDataEdit_UploadControlCreated;
            edit.UploadControlCreated += FileDataEdit_UploadControlCreated;
        }

        private void FileDataEdit_UploadControlCreated(object sender, EventArgs e)
        {
            var edit = sender as FileDataEdit;
            if (edit != null && edit.UploadControl != null) RestrictFileTypes(edit.UploadControl);
        }

        private static void RestrictFileTypes(ASPxUploadControl upload)
        {
            // rejects any other file as soon as it is chosen (also when the dialog is switched to "All files")
            upload.ValidationSettings.AllowedFileExtensions = AllowedExtensions;
            upload.ValidationSettings.ShowErrors = true;
            upload.ValidationSettings.NotAllowedFileExtensionErrorText =
                "Only Excel (.xlsx, .xls) or text (.txt, .csv) files can be imported.";
            // the Browse dialog lists only these file types
            upload.ClientSideEvents.Init =
                "function(s, e) { var el = s.GetMainElement(); if (!el) return; " +
                "var inputs = el.querySelectorAll('input[type=file]'); " +
                "for (var i = 0; i < inputs.length; i++) inputs[i].setAttribute('accept', '" + string.Join(",", AllowedExtensions) + "'); }";
        }
        #endregion

        protected override void OnDeactivated()
        {
            ObjectSpace.ModifiedChanged -= ObjectSpace_ModifiedChanged;
            foreach (Controller c in hiddenControllers) c.Active.RemoveItem("DataImport");
            hiddenControllers.Clear();
            base.OnDeactivated();
        }

        private void Hide(Controller c)
        {
            if (c == null) return;
            c.Active.SetItemValue("DataImport", false);
            hiddenControllers.Add(c);
        }

        // nothing on this screen is stored: never ask "save changes?" when leaving it
        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {
            if (ObjectSpace.IsModified) ObjectSpace.CommitChanges();
        }

        private ImportDefinition SelectedDefinition()
        {
            ImportDefinition def = ImportDefinitions.Find(ViewCurrentObject.ImportInto);
            if (def == null) throw new UserFriendlyException("Please choose what to import ('Import Into').");
            return def;
        }

        private void TemplateAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            ImportDefinition def = SelectedDefinition();
            byte[] bytes = ImportTemplateBuilder.Download(Application, def);   // transactions: .zip with header + lines files
            string name = ImportTemplateBuilder.FileName(def);
            ImportDownloadHandler.Send(name,
                name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) ? "application/zip" : "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                bytes);
        }

        private void ImportAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            DataImport p = ViewCurrentObject;
            ImportDefinition def = SelectedDefinition();
            if (p.File == null || p.File.Size == 0)
                throw new UserFriendlyException("Please choose a file to import.");

            bool canCreate = SecuritySystem.IsGranted(ObjectSpace, def.HeaderType, SecurityOperations.Create, null, null)
                && (def.LineType == null || SecuritySystem.IsGranted(ObjectSpace, def.LineType, SecurityOperations.Create, null, null));
            if (!canCreate)
                throw new UserFriendlyException("You do not have permission to create " + def.Caption + " records.");

            GenController genCon = Frame.GetController<GenController>();
            var engine = new ImportEngine(Application, genCon, def);

            DateTime started = DateTime.Now;
            var watch = System.Diagnostics.Stopwatch.StartNew();

            bool twoFiles = def.IsTransaction && p.LinesFile != null && p.LinesFile.Size > 0;
            ImportTable table = ReadFile(p.File, engine, twoFiles ? "Header file" : "File");

            if (twoFiles)
                engine.Run(table, ReadFile(p.LinesFile, engine, "Lines file"), !p.RunSimulation);
            else
                engine.Run(table, !p.RunSimulation);
            watch.Stop();

            // summary window, same style as the Run Depreciation summary (ImportResult.aspx)
            ImportSummary summary = ImportSummary.From(engine, def, p.RunSimulation, p.File.FileName,
                twoFiles ? p.LinesFile.FileName : null, SecuritySystem.CurrentUserName, started, watch.Elapsed.TotalSeconds);
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Session != null
                && DevExpress.ExpressApp.Web.WebWindow.CurrentRequestWindow != null)
            {
                System.Web.HttpContext.Current.Session[ImportSummary.SessionKey] = summary;
                DevExpress.ExpressApp.Web.WebWindow.CurrentRequestWindow.RegisterStartupScript("FTImportResult",
                    OverlayScript(System.Web.VirtualPathUtility.ToAbsolute("~/ImportResult.aspx")), true);
            }
            else
            {
                ShowResults(e.ShowViewParameters, engine, def);   // fallback: plain result list
            }
        }

        private static string OverlayScript(string url)
        {
            return @"(function(w){
  var d = w.document, old = d.getElementById('ftImportOverlay'); if (old) old.parentNode.removeChild(old);
  w.FT_CloseImportResult = function(){ var o = d.getElementById('ftImportOverlay'); if (o) o.parentNode.removeChild(o); };
  var o = d.createElement('div'); o.id = 'ftImportOverlay';
  o.style.cssText = 'position:fixed;left:0;top:0;right:0;bottom:0;background:rgba(0,0,0,.35);z-index:100000;';
  var f = d.createElement('iframe'); f.title = 'Import Result';
  f.src = '" + System.Web.HttpUtility.JavaScriptStringEncode(url) + @"?t=' + new Date().getTime();
  f.style.cssText = 'position:absolute;left:50%;top:50%;width:760px;height:560px;max-width:calc(100% - 32px);max-height:calc(100% - 32px);' +
                    'transform:translate(-50%,-50%);border:1px solid #8ba0bc;background:#fff;box-shadow:0 4px 18px rgba(0,0,0,.3);';
  o.appendChild(f); d.body.appendChild(o);
})(window.top);";
        }

        private static ImportTable ReadFile(ImportFileData file, ImportEngine engine, string what)
        {
            try
            {
                return ImportFileReader.Read(file.FileName, file.Content, engine.IsKnownCode);
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(what + " (" + file.FileName + "): " + ex.Message);
            }
        }

        private void ShowResults(ShowViewParameters svp, ImportEngine engine, ImportDefinition def)
        {
            var os = (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(ImportResultLine));
            var lines = new BindingList<ImportResultLine>(engine.Results.ToList());
            os.ObjectsGetting += (s, a) => { if (a.ObjectType == typeof(ImportResultLine)) a.Objects = lines; };

            ListView lv = Application.CreateListView(os, typeof(ImportResultLine), true);
            lv.Caption = def.Caption + " - Import Result";
            svp.CreatedView = lv;
            svp.TargetWindow = TargetWindow.NewModalWindow;
            svp.Context = TemplateContext.PopupWindow;

            var dc = Application.CreateController<DialogController>();
            dc.AcceptAction.Caption = "Close";
            dc.CancelAction.Active.SetItemValue("NotNeeded", false);
            dc.SaveOnAccept = false;
            svp.Controllers.Add(dc);
        }
    }
}
