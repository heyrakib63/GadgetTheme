using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Core.Events;
using Nop.Data;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Domains;
using Nop.Plugin.Misc.PurchaseOrder.Events;
using Nop.Plugin.Misc.PurchaseOrder.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc;
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
    private readonly IPriceFormatter _priceFormatter;
    private readonly IEventPublisher _eventPublisher;

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
        IRepository<ProductPicture> productPictureRepository,
        IPriceFormatter priceFormatter,
        IEventPublisher eventPublisher
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
        _priceFormatter = priceFormatter;
        _eventPublisher = eventPublisher;
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
        var generatedGuid = Guid.NewGuid();
        var model = new CreatePurchaseOrderModel
        {
            PurchaseOrderNo = generatedGuid,
            SupplierProductSearchModel = new SupplierProductSearchModel
            {
                PurchaseOrderNo = generatedGuid
            }
        };

        // Load supplier dropdown
        var suppliers = await _supplierServices.GetAllSupplierAsync();
        model.AvailableSuppliers = suppliers
            .Select(s => new SelectListItem
            {
                Text = s.SupplierName,
                Value = s.Id.ToString()
            }).ToList();
        model.AvailableSuppliers.Insert(0, new SelectListItem
        {
            Text = "Select a Supplier",
            Value = "0"
        });
        model.OrderDate = DateTime.UtcNow;

        var supplierProducts = await _purchaseOrdersService.GetTotalOfAllProductsPriceByPurchaseOrderNoAsync(model.PurchaseOrderNo);
        model.TotalCost = supplierProducts.Sum(p => p.TotalCost);
        model.OrderTotalFormatted = await _priceFormatter.FormatPriceAsync(model.TotalCost);

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

        // Await the task to get the result before calling Sum
        var selectedProducts = await _purchaseOrdersService.GetTotalOfAllProductsPriceByPurchaseOrderNoAsync(model.PurchaseOrderNo);

        model.TotalCost = selectedProducts.Sum(sp => sp.TotalCost);
        model.OrderTotalFormatted = await _priceFormatter.FormatPriceAsync(model.TotalCost);

        // Create new PurchaseOrder
        var purchaseOrder = new PurchaseOrders
        {
            SupplierId = model.SupplierId,
            CreatedOnUtc = DateTime.UtcNow,
            TotalCost = model.TotalCost,
            PurchaseOrderNo = model.PurchaseOrderNo
        };

        // Save the Purchase Order
        await _purchaseOrdersService.InsertPurchaseOrdersAsync(purchaseOrder);
        await _eventPublisher.PublishAsync(new PurchaseOrderCreatedEvent(model.PurchaseOrderNo));

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
    public virtual async Task<IActionResult> SupplierProductAddPopup(int supplierId, Guid purchaseOrderNo)
    {
        //prepare model
        var searchmodel = new AddSupplierProductSearchModel
        {
            SupplierId = supplierId,
            PurchaseOrderNo = purchaseOrderNo,
        };
        var model = await _purchaseOrdersModelFactory.PrepareAddSupplierProductSearchModelAsync(searchmodel);

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
    public virtual async Task<IActionResult> SupplierProductAddPopup(AddSupplierProductModel model, string formId, string btnId)
    {
        if (model.PurchaseOrderNo == Guid.Empty)
            throw new Exception("PurchaseOrderNo is empty in POST!");

        if (model.SelectedProductIds == null || !model.SelectedProductIds.Any())
            throw new Exception("No products selected!");

        var selectedProducts = await _productService.GetProductsByIdsAsync(model.SelectedProductIds.ToArray());
        if (selectedProducts.Any())
        {
            var existingSupplierProducts = await _purchaseOrdersService.GetSupplierProductsBySupplierIdAsync(model.PurchaseOrderNo, showHidden: true);
            
            foreach (var product in selectedProducts)
            {
                if (_purchaseOrdersService.FindSupplierProduct(existingSupplierProducts, model.PurchaseOrderNo, product.Id) != null)
                    continue;

                await _purchaseOrdersService.InsertSupplierProductAsync(new PurchaseOrderItems
                {
                    ProductId = product.Id,
                    PurchaseOrderNo = model.PurchaseOrderNo,
                    UnitPrice = product?.Price ?? 0,
                    Quantity = 0,
                    TotalCost = 0
                });
            }
        }
        var searchModel = new AddSupplierProductSearchModel
        {
            PurchaseOrderNo = model.PurchaseOrderNo,
            SupplierId = model.SupplierId
        };
        ViewBag.RefreshPage = true;
        ViewBag.FormId = formId;
        ViewBag.BtnId = btnId;
        return View(searchModel);
    }

    [HttpPost]
    public virtual async Task<IActionResult> SupplierProductUpdate(SupplierProductModel model, Guid purchaseOrderNo)
    {
        var supplierProduct = await _purchaseOrdersService.GetSupplierProductByIdAsync(model.Id)
            ?? throw new ArgumentException("No suppliers product found with the specified id");

        supplierProduct.Quantity = model.Quantity;
        supplierProduct.TotalCost = (supplierProduct.Quantity * supplierProduct.UnitPrice);
        await _purchaseOrdersService.UpdateSupplierProductAsync(supplierProduct);

        var updatedTotalCost = await _priceFormatter.FormatPriceAsync(supplierProduct.TotalCost);
        return Json(new
        {
            success = true,
            id = model.Id,
            totalCostFormatted = updatedTotalCost
        });
    }

    [HttpPost]
    public virtual async Task<IActionResult> SupplierProductDelete(int id)
    {
        var supplierProduct = await _purchaseOrdersService.GetSupplierProductByIdAsync(id)
            ?? throw new ArgumentException("No suppliers product found with the specified id");

        var productId = supplierProduct.ProductId;

        await _purchaseOrdersService.DeleteSupplierProductAsync(supplierProduct);

        return new NullJsonResult();
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var order = await _purchaseOrdersService.GetPurchaseOrdersByIdAsync(id);
        Guid purchaseOrderNo = (Guid)(order?.PurchaseOrderNo);
        var searchModel = new PurchaseOrderItemsSearchModel
        {
            PurchaseOrderNo = purchaseOrderNo
        };
        var model = await _purchaseOrdersModelFactory.PreparePurchaseOrderItemsSearchModelAsync(searchModel);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, PurchaseOrderItemsSearchModel searchModel)
    {
        var order = await _purchaseOrdersService.GetPurchaseOrdersByIdAsync(id);
        Guid purchaseOrderNo = (Guid)(order?.PurchaseOrderNo);
        searchModel.PurchaseOrderNo = purchaseOrderNo;
        var model = await _purchaseOrdersModelFactory.PreparePurchaseOrderItemsListModelAsync(searchModel);
        return Json(model);
    }
}