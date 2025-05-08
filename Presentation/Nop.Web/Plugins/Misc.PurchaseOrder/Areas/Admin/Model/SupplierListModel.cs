using Nop.Web.Framework.Models;
namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
public record SupplierListModel : BasePagedListModel<SupplierModel>
{
    public string Name { get; set; }
}
