using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Core.Caching;
using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
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
    public async Task<IActionResult> Create([FromBody] ProductDescriptionExtModel model)
    {
        if (!ModelState.IsValid)
        {
            var firstError = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .FirstOrDefault();
            return Json(new { success = false, message = firstError });
        }
        var description = await _productDescriptionService.GetExtraDescriptionByProductIdAsync(model.ProductId);

        if (String.IsNullOrEmpty(description))
        {
            await _productDescriptionService.InsertDescriptionAsync(model);
        }
        else
        {
            await _productDescriptionService.UpdateDescriptionAsync(model);
        } 
        return Json(new { success = true });
    }
}