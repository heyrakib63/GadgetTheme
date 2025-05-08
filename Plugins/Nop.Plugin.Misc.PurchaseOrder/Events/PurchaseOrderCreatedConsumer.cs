using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Services.Catalog;
using Nop.Services.Events;

namespace Nop.Plugin.Misc.PurchaseOrder.Events
{
    public class PurchaseOrderCreatedConsumer : IConsumer<PurchaseOrderCreatedEvent>
    {
        private readonly IProductService _productService;
        private readonly IPurchaseOrdersService _purchaseOrdersService;

        public PurchaseOrderCreatedConsumer(
            IProductService productService,
            IPurchaseOrdersService purchaseOrdersService)
        {
            _productService = productService;
            _purchaseOrdersService = purchaseOrdersService;
        }

        public async Task HandleEventAsync(PurchaseOrderCreatedEvent eventMessage)
        {
            var items = await _purchaseOrdersService.GetSupplierProductsBySupplierIdAsync(eventMessage.PurchaseOrderNo);

            foreach (var item in items)
            {
                var product = await _productService.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.StockQuantity += item.Quantity;
                    await _productService.UpdateProductAsync(product);
                }
            }
        }
    }
}
