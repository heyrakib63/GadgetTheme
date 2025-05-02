using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public partial record AddSupplierProductModel : BaseNopModel
{
    #region Ctor

    public AddSupplierProductModel()
    {
        SelectedProductIds = new List<int>();
    }
    #endregion

    #region Properties

    public int ProductId { get; set; }
    public Guid PurchaseOrderNo { get; set; }

    public IList<int> SelectedProductIds { get; set; }

    #endregion
}
