using Nop.Core.Caching;
using Nop.Data;
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
    public async Task InsertOrUpdateDescriptionAsync(int productId, string description)
    {
        var entity = await _productDescriptionRepository.Table.FirstOrDefaultAsync(x => x.ProductId == productId);
        if (entity != null)
        {
            entity.Description = description;
            await _productDescriptionRepository.UpdateAsync(entity);
            await _staticCacheManager.RemoveByPrefixAsync(ModelCacheKey.ProductDescriptionByProductIdPrefix);
        }
        else
        {
            await _productDescriptionRepository.InsertAsync(new ProductDescription
            {
                ProductId = productId,
                Description = description
            });
        }
        
    }

    public async Task<string> GetExtraDescriptionByProductIdAsync(int productId)
    {
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(ModelCacheKey.ProductDescriptionByProductIdCacheKey, productId);

        var description = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            return await _productDescriptionRepository.Table
                .Where(pd => pd.ProductId == productId)
                .Select(pd => pd.Description)
                .FirstOrDefaultAsync();
        });

        return description;
    }
}