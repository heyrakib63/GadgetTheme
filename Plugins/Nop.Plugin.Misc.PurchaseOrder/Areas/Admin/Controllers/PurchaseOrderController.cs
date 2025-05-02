using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var model = new CreatePurchaseOrderModel
        {
            PurchaseOrderNo = Guid.NewGuid().ToString()
        };

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
    //The post method for Insert and logic where should it go after Inserting the data.
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

    // For DataTable
    [HttpPost]
    public virtual async Task<IActionResult> SupplierProductList(SupplierProductSearchModel searchModel)
    {
        var model = await _purchaseOrdersModelFactory.PrepareSupplierProductListModelAsync(searchModel, searchModel.PurchaseOrderNo);

        return Json(model);
    }

    // For Popup
    public virtual async Task<IActionResult> SupplierProductAddPopup(int supplierId)
    {
        //prepare model
        var model = await _purchaseOrdersModelFactory.PrepareAddSupplierProductSearchModelAsync(new AddSupplierProductSearchModel());

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SupplierProductAddPopupList(AddSupplierProductSearchModel searchModel, int supplierId)
    {
        searchModel.SupplierId = supplierId;
        //prepare model
        var model = await _purchaseOrdersModelFactory.PrepareAddSupplierProductListModelAsync(searchModel);

        return Json(model);
    }

    [HttpPost]
    [FormValueRequired("save")]
    public virtual async Task<IActionResult> SupplierProductAddPopup(AddSupplierProductModel model)
    {
        var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
        if (selectedProducts.Any())
        {
            var existingSupplierProducts = await _purchaseOrdersService.GetSupplierProductsBySupplierIdAsync(model.PurchaseOrderNo.ToString(), showHidden: true);
            
            foreach (var product in selectedProducts)
            {
                if (_purchaseOrdersService.FindSupplierProduct(existingSupplierProducts, model.PurchaseOrderNo.ToString(), product.Id) != null)
                    continue;

                await _purchaseOrdersService.InsertSupplierProductAsync(new PurchaseOrderItems
                {
                    ProductId = product.Id,
                });
            }
        }
        ViewBag.RefreshPage = true;
        return View(new AddSupplierProductSearchModel());
    }

    [HttpPost]
    public async Task<IActionResult> LoadSupplierProductsPartial(int supplierId)
    {
        var model = new AddSupplierProductSearchModel
        {
            SupplierId = supplierId
        };

        var searchModel = await _purchaseOrdersModelFactory.PrepareAddSupplierProductSearchModelAsync(model);
        return PartialView("SupplierProductAddPopup", searchModel);
    }


}