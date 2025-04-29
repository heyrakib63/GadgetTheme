using Nop.Core;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;

public interface IPurchaseOrdersService
{
    Task InsertPurchaseOrdersAsync(PurchaseOrders purchaseOrders);
    Task<PurchaseOrders> GetPurchaseOrdersByIdAsync(int purchaseOrderId);
    Task<IPagedList<PurchaseOrders>> SearchPurchaseOrdersAsync(int supplierId=0, DateTime? createdOnFrom = null,
    DateTime? createdOnTo = null, int pageIndex = -1, int pageSize = int.MaxValue);
    Task<IList<PurchaseOrders>> GetAllPurchaseOrdersAsync();
}