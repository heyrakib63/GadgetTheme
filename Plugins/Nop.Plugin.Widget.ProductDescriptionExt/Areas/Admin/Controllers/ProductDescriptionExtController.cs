using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Nop.Core.Caching;
using Nop.Data;
using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Widget.ProductDescriptionExt.Domain;
using Nop.Plugin.Widget.ProductDescriptionExt.Services;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Mvc.ModelBinding;
namespace Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Controllers;
[Area("admin")]
[AuthorizeAdmin]
[AutoValidateAntiforgeryToken]
public class ProductDescriptionExtController : BasePluginController
{
    private readonly IProductDescriptionService _productDescriptionService;
    private readonly IRepository<ProductDescription> _productDescriptionRepository;

    public ProductDescriptionExtController(
        IProductDescriptionService productDescriptionService,
        IRepository<ProductDescription> productDescriptionRepository
        )
    {
        _productDescriptionService = productDescriptionService;
        _productDescriptionRepository = productDescriptionRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductDescriptionExtModel model)
    {
        //First we validate the model
        if (!ModelState.IsValid)
            return ErrorJson(ModelState.SerializeErrors());

        var productDescription = await _productDescriptionService.GetProductDescriptionByProductIdAsync(model.ProductId);

        if (productDescription == null)
        {
            productDescription = new ProductDescription
            {
                ProductId = model.ProductId,
                Description = model.Description
            };
            await _productDescriptionService.InsertDescriptionAsync(productDescription);
        }
        else
        {
            productDescription.Description = model.Description;
            await _productDescriptionService.UpdateDescriptionAsync(productDescription);
        }
        return Json(new { Result = true });
    }
}