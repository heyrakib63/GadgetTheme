using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public record PurchaseOrdersSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.PurchaseOrders.List.Supplier")]
    public int SupplierId { get; set; }
    public IList<SelectListItem> AvailableSuppliers { get; set; } = new List<SelectListItem>();
    public DateTime? CreatedOnFrom { get; set; }
    public DateTime? CreatedOnTo { get; set; }

}
