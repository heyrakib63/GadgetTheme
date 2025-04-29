using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;
public class PurchaseOrderItems : BaseEntity
{
    public int PurchaseOrderId { get; set; }
    public int ProductId { get; set; }
    public decimal UnitPrice { get; set; }
    public int Quantity { get; set; }
    public decimal TotalCost { get; set; }
}

