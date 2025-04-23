using Nop.Data;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
namespace Nop.Plugin.GadgetTheme.SupplierManagement.Services;
public class ProductSupplierMappingService : IProductSupplierMappingService
{
    private readonly IRepository<ProductSupplierMapping> _repository;
    public ProductSupplierMappingService(IRepository<ProductSupplierMapping> repository)
    {
        _repository = repository;
    }
    public async Task InsertMappingAsync(int productId, int supplierId)
    {
        var mapping = new ProductSupplierMapping
        {
            ProductId = productId,
            SupplierId = supplierId
        };
        await _repository.InsertAsync(mapping);
    }
    public async Task<ProductSupplierMapping> GetMappingByProductIdAsync(int productId)
    {
        return await _repository.Table
            .Where(x => x.ProductId == productId)
            .FirstOrDefaultAsync();
    }
}

