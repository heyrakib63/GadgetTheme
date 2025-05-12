namespace Nop.Plugin.Misc.ProductDescriptionExt.Services;
public interface IProductDescriptionService
{
    Task InsertMappingAsync(int productId, string description);
    Task<string> GetExtraDescriptionByProductIdAsync(int productId);
}
