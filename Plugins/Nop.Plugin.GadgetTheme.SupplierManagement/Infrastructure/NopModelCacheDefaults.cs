using Nop.Core.Caching;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;
public static partial class NopModelCacheDefaults
{
    public static CacheKey AdminSupplierAllModelKey => new("Nop.Plugin.GadgetTheme.SupplierManagement.Admin.{0}-{1}-{2}", AdminSupplierAllPrefixCacheKey);
    public static string AdminSupplierAllPrefixCacheKey => "Nop.Plugin.GadgetTheme.SupplierManagement.Admin";
    public static CacheKey PublicSupplierAllModelKey => new("Nop.Plugin.GadgetTheme.SupplierManagement.Public.{0}-{1}-{2}", PublicSupplierAllPrefixCacheKey);
    public static string PublicSupplierAllPrefixCacheKey => "Nop.Plugin.GadgetTheme.SupplierManagement.Public";
}
