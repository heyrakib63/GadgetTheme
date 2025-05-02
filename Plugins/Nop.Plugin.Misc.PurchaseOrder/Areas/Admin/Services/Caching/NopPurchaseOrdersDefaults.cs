using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Caching;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services.Caching;
public static partial class NopPurchaseOrdersDefaults
{
    public static CacheKey SupplierProductsCacheKey => new("Nop.supplierproduct.byproduct.{0}-{1}", SupplierProductsPrefix);
    public static string SupplierProductsPrefix => "Nop.supplierproduct.byproduct.{0}";
}
