namespace Nop.Plugin.Widget.ProductDescriptionExt.Services;

public interface IProductDescriptionService
{
    Task InsertOrUpdateDescriptionAsync(int productId, string description);
    Task<string> GetExtraDescriptionByProductIdAsync(int productId);
}