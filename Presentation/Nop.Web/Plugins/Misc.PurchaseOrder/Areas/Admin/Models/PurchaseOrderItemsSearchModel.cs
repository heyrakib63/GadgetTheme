using Nop.Web.Framework.Models;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
public record PurchaseOrderItemsSearchModel: BaseSearchModel
{
    public Guid PurchaseOrderNo { get; set; }
}
