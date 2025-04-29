using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public class CreatePurchaseOrderModel
{
    public int SupplierId { get; set; }
    public IList<SelectListItem> AvailableSuppliers { get; set; } = new List<SelectListItem>();

    public DateTime OrderDate { get; set; }
    public string AdminComment { get; set; }

    public IList<PurchaseOrderItemModel> Items { get; set; } = new List<PurchaseOrderItemModel>();
    public decimal TotalCost { get; set; }
}
public class PurchaseOrderItemModel
{
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalCost => UnitPrice * Quantity;
}
