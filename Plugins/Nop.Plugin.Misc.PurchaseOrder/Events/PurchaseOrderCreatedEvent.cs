namespace Nop.Plugin.Misc.PurchaseOrder.Events;
public class PurchaseOrderCreatedEvent
{
    public Guid PurchaseOrderNo { get; }

    public PurchaseOrderCreatedEvent(Guid purchaseOrderNo)
    {
        PurchaseOrderNo = purchaseOrderNo;
    }
}
