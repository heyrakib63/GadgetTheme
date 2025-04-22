using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Web.Framework;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Controllers;
[Area("admin")]
[AuthorizeAdmin]
[Route("Admin/ProductSupplier")]
public class ProductSupplierController : BasePluginController
{
    private readonly IProductSupplierMappingService _mappingService;

    public ProductSupplierController(IProductSupplierMappingService mappingService)
    {
        _mappingService = mappingService;
    }

    [HttpPost]
    public async Task<IActionResult> MapSupplier(int productId, int supplierId)
    {
        await _mappingService.InsertMappingAsync(productId, supplierId);
        return Json(new { success = true });
    }
}
