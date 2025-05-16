using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
public record ProductDescriptionExtModel : BaseNopModel
{
    public int ProductId { get; set; }
    [NopResourceDisplayName("Admin.ProductDescriptionExt.Fields.Description")]
    public string Description { get; set; }
}
