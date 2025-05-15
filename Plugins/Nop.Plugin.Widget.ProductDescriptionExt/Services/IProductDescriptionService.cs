using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Services;

public interface IProductDescriptionService
{
    Task InsertDescriptionAsync(ProductDescriptionExtModel model);
    Task UpdateDescriptionAsync(ProductDescriptionExtModel model);
    Task<string> GetExtraDescriptionByProductIdAsync(int productId);
}