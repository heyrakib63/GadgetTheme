using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public partial record SupplierProductSearchModel : BaseSearchModel
{
    #region Properties

    public Guid PurchaseOrderNo { get; set; }

    #endregion
}
