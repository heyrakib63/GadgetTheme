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
using Nop.Web.Framework.Models.DataTables;

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
        if (!ModelState.IsValid)
        {
            return View(model); // Return to the form if validation fails
        }

        // Create new PurchaseOrder
        var purchaseOrder = new PurchaseOrders
        {
            SupplierId = model.SupplierId,
            CreatedOnUtc = DateTime.UtcNow,
            TotalCost = model.TotalCost
        };

        // Save the Purchase Order
        await _purchaseOrdersService.InsertPurchaseOrdersAsync(purchaseOrder);

        // Save PurchaseOrderItems (Products)
        foreach (var product in model.SelectedProducts)
        {
            var purchaseOrderItem = new PurchaseOrderItems
            {
                PurchaseOrderId = purchaseOrder.Id,
                ProductId = product.ProductId,
                UnitPrice = product.UnitPrice,
                Quantity = product.Quantity,
                TotalCost = product.TotalCost
            };

            await _purchaseOrderItemRepository.InsertAsync(purchaseOrderItem);

            // Update inventory: Increase stock quantity
            var productEntity = await _productService.GetProductByIdAsync(product.ProductId);
            if (productEntity != null)
            {
                productEntity.StockQuantity += product.Quantity;
                await _productService.UpdateProductAsync(productEntity); // Assuming the service updates inventory
            }
        }

        // Optionally: Redirect to the list page with a success message
        TempData["SuccessMessage"] = "Purchase Order has been saved successfully!";
        return RedirectToAction("List");
    }


    // Fix for the CS1061 error in the AddProductPopup method
    public IActionResult AddProductPopup(int supplierId)
    {
        // Await the asynchronous method to get the list of products
        var productsTask = _supplierServices.GetProductsBySupplierIdAsync(supplierId);
        var products = productsTask.Result.Select(p => new PurchaseOrderPopupProductModel
        {
            ProductId = p.Id,
            ProductName = p.Name,
            UnitPrice = p.Price // Or any other field that holds the unit price
        }).ToList();

        // Create and return the view with products
        var model = new AddProductPopupModel
        {
            SupplierId = supplierId,
            Products = products
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult AddSelectedProducts(List<PurchaseOrderItemModel> products)
    {
        // Save to TempData as JSON string
        TempData["SelectedProducts"] = JsonConvert.SerializeObject(products);
        TempData.Keep("SelectedProducts");

        return Json(new { success = true });
    }


    [HttpPost]
    public IActionResult LoadSelectedProducts()
    {
        try
        {
            var productJson = TempData.Peek("SelectedProducts") as string;

            // Safety: always keep TempData alive to avoid wipe after peek
            if (!string.IsNullOrEmpty(productJson))
                TempData.Keep("SelectedProducts");

            var products = string.IsNullOrEmpty(productJson)
                ? new List<PurchaseOrderItemModel>()
                : JsonConvert.DeserializeObject<List<PurchaseOrderItemModel>>(productJson);

            return Json(new
            {
                data = products,
                recordsTotal = products.Count,
                recordsFiltered = products.Count
            });
        }
        catch (Exception ex)
        {
            // Optional: Log the error
            return Json(new
            {
                data = new List<object>(),
                recordsTotal = 0,
                recordsFiltered = 0
            });
        }
    }


    [HttpPost]
    public IActionResult DeleteProduct(int productId)
    {
        var productJson = TempData["SelectedProducts"] as string;
        if (string.IsNullOrEmpty(productJson))
            return Json(new { success = false });

        var products = JsonConvert.DeserializeObject<List<PurchaseOrderItemModel>>(productJson);

        var productToRemove = products.FirstOrDefault(p => p.ProductId == productId);
        if (productToRemove != null)
        {
            products.Remove(productToRemove);
            TempData["SelectedProducts"] = JsonConvert.SerializeObject(products);
            TempData.Keep("SelectedProducts");
        }

        return Json(new { success = true });
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