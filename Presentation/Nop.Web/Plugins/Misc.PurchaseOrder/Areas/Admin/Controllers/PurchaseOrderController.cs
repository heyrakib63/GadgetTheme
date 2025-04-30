using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Data;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
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
    private readonly ISupplierServices _supplierServices;
    private readonly IRepository<PurchaseOrders> _purchaseOrderRepository;
    private readonly IRepository<PurchaseOrderItems> _purchaseOrderItemRepository;
    private readonly IPictureService _pictureService;
    private readonly IProductService _productService;
    private readonly IRepository<ProductPicture> _productPictureRepository;

    public PurchaseOrderController(
        IPurchaseOrdersService purchaseOrdersService,
        IPurchaseOrdersModelFactory purchaseOrdersModelFactory,
        IWorkContext workContext,
        ILocalizationService localizationService,
        INotificationService notificationService,
        ILocalizedEntityService localizedEntityService,
        IStaticCacheManager staticCacheManager,
        ISupplierServices supplierServices,
        IRepository<PurchaseOrders> purchaseOrderRepository,
        IRepository<PurchaseOrderItems> purchaseOrderItemRepository,
        IPictureService pictureService,
        IProductService productService,
        IRepository<ProductPicture> productPictureRepository
        )
    {
        _purchaseOrdersService = purchaseOrdersService;
        _purchaseOrdersModelFactory = purchaseOrdersModelFactory;
        _workContext = workContext;
        _localizationService = localizationService;
        _notificationService = notificationService;
        _localizedEntityService = localizedEntityService;
        _staticCacheManager = staticCacheManager;
        _supplierServices = supplierServices;
        _purchaseOrderRepository = purchaseOrderRepository;
        _purchaseOrderItemRepository = purchaseOrderItemRepository;
        _pictureService = pictureService;
        _productService = productService;
        _productPictureRepository = productPictureRepository;
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
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new CreatePurchaseOrderModel();

        // Load supplier dropdown
        var suppliers = await _supplierServices.GetAllSupplierAsync();
        model.AvailableSuppliers = suppliers
            .Select(s => new SelectListItem
            {
                Text = s.SupplierName,
                Value = s.Id.ToString()
            }).ToList();

        model.OrderDate = DateTime.UtcNow;

        return View(model);
    }

    // The post method for Insert and logic where should it go after Inserting the data.
    [HttpPost]
    public async Task<IActionResult> Create(CreatePurchaseOrderModel model)
    {
        // Step 1: Validate form
        if (!ModelState.IsValid)
            return View(model);

        // Step 2: Deserialize product items
        if (string.IsNullOrEmpty(Request.Form["SerializedItems"]))
        {
            ModelState.AddModelError(string.Empty, "Please add at least one product.");
            return View(model);
        }

        var serializedItems = Request.Form["SerializedItems"];
        var items = JsonConvert.DeserializeObject<List<PurchaseOrderItemModel>>(serializedItems);

        if (items == null || !items.Any())
        {
            ModelState.AddModelError(string.Empty, "Product list is empty.");
            return View(model);
        }

        // Step 3: Calculate total cost
        var totalCost = items.Sum(i => i.UnitPrice * i.Quantity);

        // Step 4: Create and save PurchaseOrder entity
        var purchaseOrder = new PurchaseOrders
        {
            SupplierId = model.SupplierId,
            TotalCost = totalCost,
            CreatedOnUtc = DateTime.UtcNow
        };
        await _purchaseOrderRepository.InsertAsync(purchaseOrder);

        // Step 5: Insert PurchaseOrderItems
        foreach (var item in items)
        {
            var orderItem = new PurchaseOrderItems
            {
                PurchaseOrderId = purchaseOrder.Id,
                ProductId = item.ProductId,
                UnitPrice = item.UnitPrice,
                Quantity = item.Quantity,
                TotalCost = item.UnitPrice * item.Quantity
            };
            await _purchaseOrderItemRepository.InsertAsync(orderItem);
        }

        // Step 6: Redirect back to list
        return RedirectToAction("List");
    }

    [HttpGet]
    public async Task<IActionResult> GetProductsBySupplier(int supplierId)
    {
        var products = await _supplierServices.GetProductsBySupplierIdAsync(supplierId);

        var result = new List<object>();

        foreach (var product in products)
        {
            // Get first picture for this product
            var pictureId = (await _productPictureRepository.Table
                .Where(pp => pp.ProductId == product.Id)
                .OrderBy(pp => pp.DisplayOrder)
                .Select(pp => pp.PictureId)
                .FirstOrDefaultAsync());

            var pictureUrl = await _pictureService.GetPictureUrlAsync(pictureId, 75, true);

            result.Add(new
            {
                id = product.Id,
                name = product.Name,
                sku = product.Sku,
                pictureUrl = pictureUrl
            });
        }

        return Json(result);
    }


}