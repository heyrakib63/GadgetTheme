using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public record AddProductPopupModel : BaseNopModel
{
    public int SupplierId { get; set; }
    public Guid PurchaseOrderNo { get; set; }
    public IList<PurchaseOrderPopupProductModel> Products { get; set; } = new List<PurchaseOrderPopupProductModel>();
}
public record PurchaseOrderPopupProductModel : BaseNopModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public bool Selected { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalCost => UnitPrice * Quantity;
}
