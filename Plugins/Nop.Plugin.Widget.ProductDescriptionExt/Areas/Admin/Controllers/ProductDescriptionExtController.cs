using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Widget.ProductDescriptionExt.Services;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
namespace Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Controllers;
[Area("admin")]
[AuthorizeAdmin]
public class ProductDescriptionExtController : BasePluginController
{
    private readonly IProductDescriptionService _productDescriptionService;

    public ProductDescriptionExtController(IProductDescriptionService productDescriptionService)
    {
        _productDescriptionService = productDescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrUpdateDescription(int productId, string description)
    {
        await _productDescriptionService.InsertOrUpdateDescriptionAsync(productId, description);
        return Json(new { success = true });
    }
}