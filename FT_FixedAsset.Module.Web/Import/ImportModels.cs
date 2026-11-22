using System.ComponentModel;
using System.IO;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;

namespace FT_FixedAsset.Module.Web.Import
{
    /// <summary>In-memory uploaded file (nothing is stored in the database).</summary>
    [DomainComponent]
    [XafDisplayName("Import File")]
    public class ImportFileData : IFileData, INotifyPropertyChanged
    {
        private string fileName = "";
        public byte[] Content { get; private set; }

        public string FileName
        {
            get { return fileName; }
            set { fileName = value; OnPropertyChanged("FileName"); }
        }

        public int Size { get { return Content == null ? 0 : Content.Length; } }

        public void LoadFromStream(string fileName, Stream stream)
        {
            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                Content = ms.ToArray();
            }
            FileName = fileName;
            OnPropertyChanged("Size");
        }

        public void SaveToStream(Stream stream)
        {
            if (Content != null) stream.Write(Content, 0, Content.Length);
        }

        public void Clear()
        {
            Content = null;
            FileName = "";
            OnPropertyChanged("Size");
        }

        public override string ToString() { return FileName; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

    /// <summary>What can be imported (order = suggested import order).</summary>
    public enum ImportObject
    {
        [XafDisplayName("Fixed Asset Class")] FixedAssetClass,
        [XafDisplayName("Fixed Asset Group")] FixedAssetGroup,
        [XafDisplayName("Indicator")] Indicator,
        [XafDisplayName("Location")] Location,
        [XafDisplayName("Fixed Asset Master")] FixedAssetMaster,
        [XafDisplayName("Write Off")] WriteOff,
        [XafDisplayName("Disposal")] Disposal,
        [XafDisplayName("Correction Out")] CorrectionOut,
        [XafDisplayName("Transferred Out")] TransferredOut,
        [XafDisplayName("Reclassed Out")] ReclassedOut,
        [XafDisplayName("Review Useful Life")] ReviewUsefulLife,
        [XafDisplayName("Revalue Cost")] RevalueCost,
        [XafDisplayName("Adjust Acc. Depr")] AdjustAccDepr,
        [XafDisplayName("Move")] Move,
        [XafDisplayName("Revalue Residual Value")] RevalueResidualValue
    }

    /// <summary>The "Data Import" screen (navigation group after Widgets).</summary>
    [DomainComponent]
    [XafDisplayName("Import Data")]
    [ImageName("Action_Export_ToExcel")]
    [NavigationItem("Data Import")]
    [Appearance("DataImport_HideLinesFile", AppearanceItemType = "ViewItem", TargetItems = "LinesFile", Criteria = "IsTransaction = False", Visibility = ViewItemVisibility.Hide, Context = "DetailView")]
    public class DataImport : INotifyPropertyChanged
    {
        public DataImport()
        {
            File = new ImportFileData();
            LinesFile = new ImportFileData();
            importInto = ImportObject.FixedAssetMaster;
        }

        [Browsable(false), Key]
        public int Id { get; set; }

        private ImportObject importInto;
        [Index(10), XafDisplayName("Import Into")]
        [ImmediatePostData]
        public ImportObject ImportInto
        {
            get { return importInto; }
            set
            {
                if (importInto == value) return;
                importInto = value;
                OnPropertyChanged("ImportInto");
                OnPropertyChanged("IsTransaction");
            }
        }

        /// <summary>Transactions can be imported as one file or as a header file + a lines file.</summary>
        [Browsable(false)]
        public bool IsTransaction
        {
            get
            {
                ImportDefinition d = ImportDefinitions.Find(ImportInto);
                return d != null && d.HasLines;   // transactions, or assets with Referenced Documents (Kossan)
            }
        }

        [Index(20), XafDisplayName("Header File")]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [FileTypeFilter("ImportFiles", 1, "*.xlsx", "*.xls", "*.txt", "*.csv")]
        public ImportFileData File { get; set; }

        [Index(25), XafDisplayName("Lines File")]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [FileTypeFilter("ImportFiles", 1, "*.xlsx", "*.xls", "*.txt", "*.csv")]
        public ImportFileData LinesFile { get; set; }

        [Index(27), XafDisplayName("Note"), ModelDefault("AllowEdit", "False")]
        public string FileNote
        {
            get { return "Supported file extensions: .xlsx, .xls, .txt, .csv"; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>Checks the whole file exactly like a real import, then rolls everything back - nothing is saved.</summary>
        [Index(30), XafDisplayName("Run Simulation")]
        [ToolTip("Check the file exactly like a real import, without saving anything")]
        public bool RunSimulation { get; set; }

        [Index(40), XafDisplayName("How it works"), ModelDefault("AllowEdit", "False"), ModelDefault("RowCount", "6")]
        [FieldSize(FieldSizeAttribute.Unlimited)]
        public string Help
        {
            get
            {
                return "1. Choose 'Import Into', click 'Download Template' and fill Sheet1 from row 3 (row 1 = captions, row 2 = field codes). " +
                       "2. Choose the file and click 'Import' (tick 'Run Simulation' to check the file first without saving). " +
                       "Suggested order: Class, Group, Indicator, Location, Fixed Asset Master, then transactions. " +
                       ".txt/.csv files use the same layout (tab, comma or semicolon separated). " +
                       "Lookups (F/A Group, Location, F/A Code, G/L accounts...) can be given by code. " +
                       "Transactions: the template is a .zip with a HEADER file (one row per document) and a LINES file (one row per asset), " +
                       "linked by 'Import Doc No.' - choose the header file as 'Header File' and the lines file as 'Lines File'. " +
                       "(A single file with header + line columns on each row also works - leave 'Lines File' empty.) " +
                       "Fixed Asset Master: Referenced Documents can be imported with the assets - give the asset an 'Import Doc No.' " +
                       "and choose the Referenced Documents file as 'Lines File' (optional). " +
                       "All rows are validated first - if any row has an error, nothing is saved.";
            }
        }
    }

    [DomainComponent]
    [XafDisplayName("Import Result")]
    [DefaultListViewOptions(false, NewItemRowPosition.None)]
    public class ImportResultLine
    {
        [Browsable(false), Key]
        public int Id { get; set; }

        [XafDisplayName("Row"), Index(0), ModelDefault("AllowEdit", "False")]
        public int Row { get; set; }

        [XafDisplayName("Import Doc No."), Index(1), ModelDefault("AllowEdit", "False")]
        public string DocNo { get; set; }

        [XafDisplayName("Code / Doc No."), Index(2), ModelDefault("AllowEdit", "False")]
        public string Code { get; set; }

        [XafDisplayName("Status"), Index(3), ModelDefault("AllowEdit", "False")]
        public string Status { get; set; }

        // long text is hidden in lists by default - the reason must be visible without opening the row
        [XafDisplayName("Message"), FieldSize(FieldSizeAttribute.Unlimited), Index(4), VisibleInListView(true), ModelDefault("AllowEdit", "False")]
        public string Message { get; set; }
    }
}
