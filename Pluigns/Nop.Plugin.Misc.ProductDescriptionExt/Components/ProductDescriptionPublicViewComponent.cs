using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ProductDescriptionExt.Services;
using Nop.Web.Models.Catalog;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Components
{
    public class ProductDescriptionPublicViewComponent : ViewComponent
    {
        private readonly IProductDescriptionService _productDescriptionService;

        public ProductDescriptionPublicViewComponent(IProductDescriptionService productDescriptionService)
        {
            _productDescriptionService = productDescriptionService;
        }
        public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
        {
            if (additionalData is not ProductDetailsModel model || model.Id == 0)
                return Content("");

            var extraDescription = await _productDescriptionService.GetExtraDescriptionByProductIdAsync(model.Id);

            if (string.IsNullOrEmpty(extraDescription))
                return Content("");

            ViewData["ExtraDescription"] = extraDescription;

            return View("~/Plugins/Misc.ProductDescriptionExt/Views/Components/ProductDescriptionPublic.cshtml");
        }
    }
}
