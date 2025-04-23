using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Components;
public class SupplierDropdownViewComponent : ViewComponent
{
    private readonly ISupplierServices _supplierService;
    private readonly IProductService _productService;
    public SupplierDropdownViewComponent(ISupplierServices supplierService, IProductService productService)
    {
        _supplierService = supplierService;
        _productService = productService;
    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        int productId = 0;
        if (additionalData is ProductModel productModel)
        {
            productId = productModel.Id;
        }
        var product = await _productService.GetProductByIdAsync(productId);
        var suppliers = await _supplierService.GetAllSupplierAsync();
        var viewModel = new ProductSupplierViewModel
        {
            ProductId = product?.Id ?? 0,
            Suppliers = suppliers.Select(s => new SelectListItem
            {
                Text = s.SupplierName,
                Value = s.Id.ToString()
            }).ToList()
        };
        return View("~/Plugins/GadgetTheme.SupplierManagement/Areas/Admin/Views/Components/SupplierDropdown.cshtml", viewModel);
    }
}
