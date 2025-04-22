using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Services;

public interface IProductSupplierMappingService
{
    Task InsertMappingAsync(int productId, int supplierId);
    Task<ProductSupplierMapping> GetMappingByProductIdAsync(int productId);
}