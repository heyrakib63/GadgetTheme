using Nop.Core;
using Nop.Data;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;
using Nop.Services.Html;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;

public class PurchaseOrdersService : IPurchaseOrdersService
{
    private readonly IRepository<PurchaseOrders> _purchaseOrdersRepository;
    protected readonly IHtmlFormatter _htmlFormatter;

    public PurchaseOrdersService(IRepository<PurchaseOrders> purchaseOrdersRepository, IHtmlFormatter htmlFormatter)
    {
        _purchaseOrdersRepository = purchaseOrdersRepository;
        _htmlFormatter = htmlFormatter;
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
    public virtual async Task<PurchaseOrders> GetPurchaseOrdersByIdAsync(int purchaseOrderId)
    {
        return await _purchaseOrdersRepository.GetByIdAsync(purchaseOrderId);
    }
    public virtual async Task InsertPurchaseOrdersAsync(PurchaseOrders purchaseOrder)
    {
        await _purchaseOrdersRepository.InsertAsync(purchaseOrder);
    }
}

