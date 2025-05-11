using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.Misc.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Misc.ProductDescriptionExt.Services;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Areas.Admin.Controllers;
[Area("admin")]
[AuthorizeAdmin]
public class ProductDescriptionController : BasePluginController
{
    private readonly IProductDescriptionService _mappingService;
    private readonly INotificationService _notificationService;

    public ProductDescriptionController(IProductDescriptionService mappingService, INotificationService notificationService)
    {
        _mappingService = mappingService;
        _notificationService = notificationService;
    }

    [HttpPost]
    public async Task<IActionResult> SaveDescription(int productId, string description)
    {
        //if (string.IsNullOrWhiteSpace(model.Description))
        //{
        //    ModelState.AddModelError(nameof(model.Description), "Description cannot be empty");
        //    return View(model); // or Redirect back
        //}
        await _mappingService.InsertMappingAsync(productId, description);
        return Json(new { success = true });
    }
}
