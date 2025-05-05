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
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalCost { get; set; }
    public string PictureUrl { get; set; }


    #endregion
}
