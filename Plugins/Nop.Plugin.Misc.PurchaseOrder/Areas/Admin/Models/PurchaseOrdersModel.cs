using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public record PurchaseOrdersModel : BaseNopEntityModel, ILocalizedModel<PurchaseOrdersLocalizedModel>
{
    public PurchaseOrdersModel()
    {
        Locales = new List<PurchaseOrdersLocalizedModel>();
    }
    [NopResourceDisplayName("Admin.PurchaseOrdder.Fields.SupplierId")]
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public string SupplierEmail { get; set; }

    [NopResourceDisplayName("Admin.PurchaseOrder.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.PurchaseOrder.Fields.TotalCost")]
    public decimal TotalCost { get; set; }
    public string TotalCostFormatted { get; set; }
    public IList<PurchaseOrdersLocalizedModel> Locales { get; set; }
}
//This class is used to localize the PurchaseOrdersModel
public partial record PurchaseOrdersLocalizedModel : ILocalizedLocaleModel
{
    [NopResourceDisplayName("Admin.PurchaseOrder.Fields.Language")]
    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.PurchaseOrder.Fields.SupplierId")]
    public int SupplierId { get; set; }

    [NopResourceDisplayName("Admin.PurchaseOrder.Fields.CreatedOnUtc")]
    public DateTime CreatedOnUtc { get; set; }

    [NopResourceDisplayName("Admin.PurchaseOrder.Fields.TotalCost")]
    public decimal TotalCost { get; set; }
}
