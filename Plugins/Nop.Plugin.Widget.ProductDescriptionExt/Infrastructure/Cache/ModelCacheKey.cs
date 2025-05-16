using Nop.Core.Caching;

public static class ModelCacheKey
{
    public static CacheKey ProductDescriptionByProductIdCacheKey => 
        new("Nop.productextradescription.byproductid.{0}", ProductDescriptionByProductIdPrefix);
    public static string ProductDescriptionByProductIdPrefix => "Nop.productextradescription.byproductid.";
}
