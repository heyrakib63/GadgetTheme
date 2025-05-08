using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;
namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
public record SupplierSearchModel : BaseSearchModel
{
    [NopResourceDisplayName("Admin.Suppliers.List.Name")]
    public string Name { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.List.Email")]
    public string Email { get; set; }
}
