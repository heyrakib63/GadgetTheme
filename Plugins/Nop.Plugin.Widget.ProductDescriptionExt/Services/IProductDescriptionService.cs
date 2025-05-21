using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Widget.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Services;

public interface IProductDescriptionService
{
    Task InsertDescriptionAsync(ProductDescription productDescription);
    Task UpdateDescriptionAsync(ProductDescription productDescription);
    Task<ProductDescription> GetProductDescriptionByProductIdAsync(int productId);
}