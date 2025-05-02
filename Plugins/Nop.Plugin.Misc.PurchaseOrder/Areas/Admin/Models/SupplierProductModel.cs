using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public partial record SupplierProductModel : BaseNopEntityModel
{
    #region Properties

    public int ProductId2 { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.RelatedProducts.Fields.Product")]
    public string Product2Name { get; set; }

    [NopResourceDisplayName("Admin.Catalog.Products.RelatedProducts.Fields.DisplayOrder")]
    public int DisplayOrder { get; set; }

    #endregion
}
