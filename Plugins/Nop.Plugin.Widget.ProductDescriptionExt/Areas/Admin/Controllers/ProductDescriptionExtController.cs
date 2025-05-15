using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Core.Caching;
using Nop.Plugin.Widget.ProductDescriptionExt.Services;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
namespace Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Controllers;
[Area("admin")]
[AuthorizeAdmin]
public class ProductDescriptionExtController : BasePluginController
{
    private readonly IProductDescriptionService _productDescriptionService;

    public ProductDescriptionExtController(
        IProductDescriptionService productDescriptionService
        )
    {
        _productDescriptionService = productDescriptionService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(int productId, string description)
    {
        var hasDescription = await _productDescriptionService.GetExtraDescriptionByProductIdAsync(productId);

        if (String.IsNullOrEmpty(hasDescription))
        {
            await _productDescriptionService.InsertDescriptionAsync(productId, description);
        }
        else
        {
            await _productDescriptionService.UpdateDescriptionAsync(productId, description);
        } 
        return Json(new { success = true });
    }
}