using Nop.Core;

namespace Nop.Plugin.Misc.PurchaseOrder.Domains;
public class PurchaseOrders : BaseEntity
{
    public Guid PurchaseOrderNo { get; set; }
    public int SupplierId { get; set; }
    public DateTime CreatedOnUtc { get; set; }
    public decimal TotalCost { get; set; }
}

