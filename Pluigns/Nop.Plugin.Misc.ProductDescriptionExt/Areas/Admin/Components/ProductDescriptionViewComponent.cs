using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Plugin.Misc.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Misc.ProductDescriptionExt.Services;
using Nop.Services.Catalog;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Areas.Admin.Components;
public class ProductDescriptionViewComponent : ViewComponent
{
    private readonly IProductDescriptionService _productDescriptionService;
    private readonly IProductService _productService;
    public ProductDescriptionViewComponent(IProductDescriptionService productDescriptionService, IProductService productService)
    {
        _productDescriptionService = productDescriptionService;
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

        var viewModel = new ProductDescriptionViewModel
        {
            ProductId = product?.Id ?? 0
        };
        return View("~/Plugins/Misc.ProductDescriptionExt/Areas/Admin/Views/Components/ProductDescription.cshtml", viewModel);
    }
}

