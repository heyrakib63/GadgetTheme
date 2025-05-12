using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.Misc.PurchaseOrder.Services.Caching;
using Nop.Plugin.Misc.PurchaseOrder.Domains;
using Nop.Services.Catalog;
using Nop.Services.Html;

namespace Nop.Plugin.Misc.PurchaseOrder.Services;

public class PurchaseOrdersService : IPurchaseOrdersService
{
    private readonly IRepository<PurchaseOrders> _purchaseOrdersRepository;
    protected readonly IHtmlFormatter _htmlFormatter;
    private readonly IRepository<PurchaseOrderItems> _productSupplierRepository;
    private readonly IRepository<Product> _productRepository;
    private readonly IStaticCacheManager _staticCacheManager;

    public PurchaseOrdersService(
        IRepository<PurchaseOrders> purchaseOrdersRepository,
        IHtmlFormatter htmlFormatter,
        IRepository<PurchaseOrderItems> productSupplierRepository,
        IRepository<Product> productRepository,
        IStaticCacheManager staticCacheManager
        )
    {
        _purchaseOrdersRepository = purchaseOrdersRepository;
        _htmlFormatter = htmlFormatter;
        _productSupplierRepository = productSupplierRepository;
        _productRepository = productRepository;
        _staticCacheManager = staticCacheManager;
    }
    public virtual async Task<IList<PurchaseOrders>> GetAllPurchaseOrdersAsync()
    {
        var purchaseOrders = await _purchaseOrdersRepository.GetAllAsync(query =>
            query.OrderBy(s => s.SupplierId)); // Optional: sort SupplierId
        return purchaseOrders;
    }
    public virtual async Task<IPagedList<PurchaseOrders>> SearchPurchaseOrdersAsync(
        int supplierId = 0, 
        DateTime? createdOnFrom = null, 
        DateTime? createdOnTo = null, 
        int pageIndex = 0, 
        int pageSize = int.MaxValue
        )
    {
        var query = from e in _purchaseOrdersRepository.Table
                    select e;
        if (supplierId!=0)
            query = query.Where(e => e.SupplierId == supplierId);
        if (createdOnFrom.HasValue)
            query = query.Where(e => e.CreatedOnUtc >= createdOnFrom.Value);
        if (createdOnTo.HasValue)
            query = query.Where(e => e.CreatedOnUtc <= createdOnTo.Value.AddDays(1)); // Include full day
        query = query.OrderBy(e => e.SupplierId);
        return await query.ToPagedListAsync(pageIndex, pageSize);
    }
    public virtual async Task<IPagedList<PurchaseOrderItems>> SearchPurchaseOrderItemsAsync(
        Guid purchaseOrderNo,
        int pageIndex = 0,
        int pageSize = int.MaxValue
        )
    {
        var query = from e in _productSupplierRepository.Table
                    select e;
        if (purchaseOrderNo!=Guid.Empty)
            query = query.Where(e => e.PurchaseOrderNo == purchaseOrderNo);
        query = query.OrderBy(e => e.PurchaseOrderNo);
        return await query.ToPagedListAsync(pageIndex, pageSize);
    }

    public virtual async Task<PurchaseOrders> GetPurchaseOrdersByIdAsync(int purchaseOrderId)
    {
        return await _purchaseOrdersRepository.GetByIdAsync(purchaseOrderId);
    }
    public virtual async Task InsertPurchaseOrdersAsync(PurchaseOrders purchaseOrder)
    {
        await _purchaseOrdersRepository.InsertAsync(purchaseOrder);
    }

    public virtual async Task<IList<PurchaseOrderItems>> GetSupplierProductsBySupplierIdAsync(Guid purchaseOrderNo, bool showHidden = false)
    {
        var query = from rp in _productSupplierRepository.Table
                    join p in _productRepository.Table on rp.ProductId equals p.Id
                    where rp.PurchaseOrderNo == purchaseOrderNo &&
                          !p.Deleted &&
                          (showHidden || p.Published)
        orderby rp.ProductId, rp.Id
        select rp;
        var supplierProducts = await query.ToListAsync();
        return supplierProducts;
    }
    public virtual PurchaseOrderItems FindSupplierProduct(IList<PurchaseOrderItems> source, Guid purchaseOrderNo, int productId)
    {
        return source.FirstOrDefault(rp => rp.PurchaseOrderNo == purchaseOrderNo && rp.ProductId == productId);
    }

    public virtual async Task InsertSupplierProductAsync(PurchaseOrderItems supplierProduct)
    {
        await _productSupplierRepository.InsertAsync(supplierProduct);
    }

    public virtual async Task<PurchaseOrderItems> GetSupplierProductByIdAsync(int supplierProductId)
    {
        return await _productSupplierRepository.GetByIdAsync(supplierProductId);
    }

    public virtual async Task DeleteSupplierProductAsync(PurchaseOrderItems supplierProduct)
    {
        await _productSupplierRepository.DeleteAsync(supplierProduct);
    }

    public virtual async Task UpdateSupplierProductAsync(PurchaseOrderItems supplierProduct)
    {
        await _productSupplierRepository.UpdateAsync(supplierProduct);
    }
    public async Task<IList<PurchaseOrderItems>> GetTotalOfAllProductsPriceByPurchaseOrderNoAsync(Guid purchaseOrderNo)
    {
        return await _productSupplierRepository.Table
            .Where(p => p.PurchaseOrderNo == purchaseOrderNo)
            .ToListAsync();
    }
    public async Task<Product> GetProductByIdAsync(int productId)
    {
        return await _productRepository.GetByIdAsync(productId);
    }

}

