using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domains;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;

public interface IPurchaseOrdersModelFactory
{
    Task<PurchaseOrdersModel> PreparePurchaseOrdersModelAsync(PurchaseOrdersModel model, PurchaseOrders purchaseOrders, bool excludeProperties = false);
    Task<PurchaseOrdersSearchModel> PreparePurchaseOrdersSearchModelAsync(PurchaseOrdersSearchModel searchModel);
    Task<PurchaseOrdersListModel> PreparePurchaseOrdersListModelAsync(PurchaseOrdersSearchModel searchModel);
    Task<SupplierProductListModel> PrepareSupplierProductListModelAsync(SupplierProductSearchModel searchModel, Guid purchaseOrderNo);
    Task<AddSupplierProductSearchModel> PrepareAddSupplierProductSearchModelAsync(AddSupplierProductSearchModel searchModel);
    Task<AddSupplierProductListModel> PrepareAddSupplierProductListModelAsync(AddSupplierProductSearchModel searchModel);
    Task<PurchaseOrderItemsListModel> PreparePurchaseOrderItemsListModelAsync(PurchaseOrderItemsSearchModel searchModel);
    Task<PurchaseOrderItemsModel> PreparePurchaseOrderItemsModelAsync(PurchaseOrderItemsModel model, PurchaseOrderItems purchaseOrderItem, bool excludeProperties = false);
    Task<PurchaseOrderItemsSearchModel> PreparePurchaseOrderItemsSearchModelAsync(PurchaseOrderItemsSearchModel searchModel);
}