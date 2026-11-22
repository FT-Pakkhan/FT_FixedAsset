using DevExpress.ExpressApp.Data;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using System.ComponentModel;

namespace FT_FixedAsset.Module.BusinessObjects
{
    /// <summary>
    /// Centred Yes / No confirmation popup (not stored): Cancel Assets / Cancel Trans. (ButtonController)
    /// and every other button with a confirmation message (ConfirmationPopupController - Run Depreciation, Post, Delete...).
    /// The question is shown by the static text item "MessageText" of CancelConfirmation_DetailView
    /// (plain read-only text - no editor).
    /// </summary>
    [DomainComponent]
    [XafDisplayName("Confirmation")]
    [ImageName("Question_16x16")]
    public class CancelConfirmation
    {
        [Browsable(false), Key]
        public int Id { get; set; }

        [Browsable(false)]
        public string Message { get; set; }
    }
}
