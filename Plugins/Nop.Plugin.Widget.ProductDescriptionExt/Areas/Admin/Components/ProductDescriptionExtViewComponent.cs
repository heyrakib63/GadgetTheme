using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Widget.ProductDescriptionExt.Services;
using Nop.Web.Areas.Admin.Models.Catalog;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Components;
public class ProductDescriptionExtViewComponent : ViewComponent
{
    private readonly IProductDescriptionService _productDescriptionService;
    public ProductDescriptionExtViewComponent(
        IProductDescriptionService productDescriptionService
        )
    {
        _productDescriptionService = productDescriptionService;
    }
    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
    {
        int productId = 0;
        if (additionalData is ProductModel productModel)
        {
            productId = productModel.Id;
        }
        var extraDescription = await _productDescriptionService.GetExtraDescriptionByProductIdAsync(productId);
        var viewModel = new ProductDescriptionExtModel
        {
            ProductId = productId,
            Description = extraDescription
        };
        return View(viewModel);
    }
}
