using System.Runtime.InteropServices;
using Microsoft.Identity.Client;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Widget.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Services;
public class ProductDescriptionService : IProductDescriptionService
{
    private readonly IRepository<ProductDescription> _productDescriptionRepository;
    private readonly IStaticCacheManager _staticCacheManager;
    public ProductDescriptionService(
        IRepository<ProductDescription> productDescriptionRepository,
        IStaticCacheManager staticCacheManager
        )
    {
        _productDescriptionRepository = productDescriptionRepository;
        _staticCacheManager = staticCacheManager;
    }
    public async Task InsertDescriptionAsync(ProductDescription productDescription)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheKey.ProductDescriptionByProductIdCacheKey, productDescription.ProductId);
        await _productDescriptionRepository.InsertAsync(productDescription);
        await _staticCacheManager.SetAsync(cacheKey, productDescription);
    }
    
    public async Task UpdateDescriptionAsync(ProductDescription productDescription)
    {
        await _productDescriptionRepository.UpdateAsync(productDescription);
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheKey.ProductDescriptionByProductIdCacheKey, productDescription.ProductId);
        await _staticCacheManager.RemoveByPrefixAsync(ModelCacheKey.ProductDescriptionByProductIdPrefix);
        await _staticCacheManager.SetAsync(cacheKey, productDescription);
    }

    public async Task<ProductDescription> GetProductDescriptionByProductIdAsync(int productId)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheKey.ProductDescriptionByProductIdCacheKey, productId);

        var productDescription = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _productDescriptionRepository.Table
                .Where(pd => pd.ProductId == productId)
                .FirstOrDefaultAsync();
        });
        return productDescription;
    }
}