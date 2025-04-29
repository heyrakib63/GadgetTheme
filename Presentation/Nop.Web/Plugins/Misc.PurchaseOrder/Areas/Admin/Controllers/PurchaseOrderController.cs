using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Controllers;
[AuthorizeAdmin]
[Area("admin")]
public class PurchaseOrderController : BasePluginController
{
    private readonly IPurchaseOrdersService _purchaseOrdersService;
    private readonly IPurchaseOrdersModelFactory _purchaseOrdersModelFactory;
    private readonly IWorkContext _workContext;
    private readonly ILocalizationService _localizationService;
    private readonly INotificationService _notificationService;
    private readonly ILocalizedEntityService _localizedEntityService;
    private readonly IStaticCacheManager _staticCacheManager;

    public PurchaseOrderController(
        IPurchaseOrdersService purchaseOrdersService,
        IPurchaseOrdersModelFactory purchaseOrdersModelFactory,
        IWorkContext workContext,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ILocalizedEntityService localizedEntityService,
        IStaticCacheManager staticCacheManager
        )
    {
        _purchaseOrdersService = purchaseOrdersService;
        _purchaseOrdersModelFactory = purchaseOrdersModelFactory;
        _workContext = workContext;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _localizedEntityService = localizedEntityService;
        _staticCacheManager = staticCacheManager;
    }
    // Logics for List view
    public async Task<IActionResult> List()
    {
        var searchModel = await _purchaseOrdersModelFactory.PreparePurchaseOrdersSearchModelAsync(new PurchaseOrdersSearchModel());
        return View(searchModel);
    }
    // Logics for posting list of Suppliers
    [HttpPost]
    public async Task<IActionResult> List(PurchaseOrdersSearchModel searchModel)
    {
        var model = await _purchaseOrdersModelFactory.PreparePurchaseOrdersListModelAsync(searchModel);
        return Json(model);
    }
    // To generate the create view.
    public async Task<IActionResult> Create()
    {
        var model = await _purchaseOrdersModelFactory.PreparePurchaseOrdersModelAsync(new PurchaseOrdersModel(), null);
        return View(model);
    }
    // The post method for Insert and logic where should it go after Inserting the data.
    [HttpPost]
    public async Task<IActionResult> Create(PurchaseOrdersModel model)
    {
        if (ModelState.IsValid)
        {
            var purchaseOrder = new PurchaseOrders
            {
                SupplierId = model.SupplierId,
                CreatedOnUtc = model.CreatedOnUtc,
                TotalCost = model.TotalCost,
            };

            await _purchaseOrdersService.InsertPurchaseOrdersAsync(purchaseOrder);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.PurchaseOrders.Added"));
            return RedirectToAction("List");
        }
        model = await _purchaseOrdersModelFactory.PreparePurchaseOrdersModelAsync(model, null);
        return View(model);
    }
}