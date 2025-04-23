using Microsoft.AspNetCore.Mvc.Rendering;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
public class ProductSupplierViewModel
{
    public int ProductId { get; set; }
    public List<SelectListItem> Suppliers { get; set; } = new();
}