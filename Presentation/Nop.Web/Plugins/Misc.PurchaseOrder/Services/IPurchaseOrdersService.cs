using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.Misc.PurchaseOrder.Domains;

namespace Nop.Plugin.Misc.PurchaseOrder.Services;

public interface IPurchaseOrdersService
{
    Task InsertPurchaseOrdersAsync(PurchaseOrders purchaseOrders);
    Task<PurchaseOrders> GetPurchaseOrdersByIdAsync(int purchaseOrderId);
    Task<IPagedList<PurchaseOrders>> SearchPurchaseOrdersAsync(int supplierId=0, DateTime? createdOnFrom = null,
    DateTime? createdOnTo = null, int pageIndex = -1, int pageSize = int.MaxValue);
    Task<IList<PurchaseOrders>> GetAllPurchaseOrdersAsync();
    Task<IList<PurchaseOrderItems>> GetSupplierProductsBySupplierIdAsync(Guid purchaseOrderNo, bool showHidden = false);
    PurchaseOrderItems FindSupplierProduct(IList<PurchaseOrderItems> source, Guid purchaseOrderNo, int productId);
    Task InsertSupplierProductAsync(PurchaseOrderItems supplierProduct);
    Task<PurchaseOrderItems> GetSupplierProductByIdAsync(int supplierProductId);
    Task UpdateSupplierProductAsync(PurchaseOrderItems supplierProduct);
    Task DeleteSupplierProductAsync(PurchaseOrderItems supplierProduct);
    Task<IPagedList<PurchaseOrderItems>> SearchPurchaseOrderItemsAsync(Guid purchaseOrderNo,int pageIndex = 0,int pageSize = int.MaxValue);
    Task<IList<PurchaseOrderItems>> GetTotalOfAllProductsPriceByPurchaseOrderNoAsync(Guid purchaseOrderNo);
    Task<Product> GetProductByIdAsync(int productId);
}