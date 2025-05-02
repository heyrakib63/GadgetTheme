using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Domain.Catalog;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Services.Seo;
using Nop.Web.Areas.Admin.Factories;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Models.Catalog;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
public class PurchaseOrdersModelFactory : IPurchaseOrdersModelFactory
{
    private readonly ILocalizationService _localizationService;
    private readonly IPurchaseOrdersService _purchaseOrdersService;
    private readonly ILocalizedModelFactory _localizedModelFactory;
    private readonly ISupplierServices _supplierServices;
    private readonly IPriceFormatter _priceFormatter;
    private readonly IProductService _productService;
    private readonly IBaseAdminModelFactory _baseAdminModelFactory;
    private readonly IWorkContext _workContext;
    private readonly IUrlRecordService _urlRecordService;
    //private readonly IStaticCacheManager _staticCacheManager;
    public PurchaseOrdersModelFactory(
        ILocalizationService localizationService,
        IPurchaseOrdersService purchaseOrdersService,
        ILocalizedModelFactory localizedModelFactory,
        ISupplierServices supplierServices,
        IPriceFormatter priceFormatter,
        IProductService productService,
        IBaseAdminModelFactory baseAdminModelFactory,
        IWorkContext workContext,
        IUrlRecordService urlRecordService
        )
    {
        _localizationService = localizationService;
        _purchaseOrdersService = purchaseOrdersService;
        _localizedModelFactory = localizedModelFactory;
        _supplierServices = supplierServices;
        _priceFormatter = priceFormatter;
        _productService = productService;
        _baseAdminModelFactory = baseAdminModelFactory;
        _workContext = workContext;
        _urlRecordService = urlRecordService;
        //_staticCacheManager = staticCacheManager;
    }
    // Return lists of Suppliers aka grid
    public async Task<PurchaseOrdersListModel> PreparePurchaseOrdersListModelAsync(PurchaseOrdersSearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));

        var purchaseOrders = await _purchaseOrdersService.SearchPurchaseOrdersAsync(
            searchModel.SupplierId, 
            searchModel.CreatedOnFrom,
            searchModel.CreatedOnTo,
            pageIndex: searchModel.Page - 1,
            pageSize: searchModel.PageSize);
        //prepare grid model
        var count = purchaseOrders.Count;
        //var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.AdminSupplierAllModelKey, searchModel.Name, searchModel.Email, 0);
        //var output = await _staticCacheManager.GetAsync(cacheKey, async () =>
        //{
            var model = await new PurchaseOrdersListModel().PrepareToGridAsync(searchModel, purchaseOrders, () =>
            {
                return purchaseOrders.SelectAwait(async purchaseOrder =>
                {
                    return await PreparePurchaseOrdersModelAsync(null, purchaseOrder, true);
                });
            });
            return model;
        //});
        //return output;
    }
    // Returns a single supplier model
    public async Task<PurchaseOrdersModel> PreparePurchaseOrdersModelAsync(PurchaseOrdersModel model, PurchaseOrders purchaseOrder, bool excludeProperties = false)
    {
        Func<PurchaseOrdersLocalizedModel, int, Task> localizedModelConfiguration = null;
        if (purchaseOrder != null)
        {
            if (model == null)
            {
                var supplier = await _supplierServices.GetSupplierByIdAsync(purchaseOrder.SupplierId);
                //fill in model values from the entity
                model = new PurchaseOrdersModel()
                {
                    Id = purchaseOrder.Id,
                    SupplierId = purchaseOrder.SupplierId,
                    SupplierName = supplier?.SupplierName,
                    SupplierEmail = supplier?.SupplierEmail,
                    CreatedOnUtc = purchaseOrder.CreatedOnUtc,
                    TotalCost = purchaseOrder.TotalCost,
                    TotalCostFormatted = await _priceFormatter.FormatPriceAsync(purchaseOrder.TotalCost)
                };
            }
        }
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);
        // Simulate async behavior to resolve CS1998
        await Task.CompletedTask;
        return model;
    }

    // For the search model. 
    public async Task<PurchaseOrdersSearchModel> PreparePurchaseOrdersSearchModelAsync(PurchaseOrdersSearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));
        // Simulate async behavior to resolve CS1998

        // Load available suppliers
        var suppliers = await _supplierServices.GetAllSupplierAsync();
        searchModel.AvailableSuppliers = suppliers.Select(supplier => new SelectListItem
        {
            Text = supplier.SupplierName,    // display supplier name
            Value = supplier.Id.ToString()  // supplier id
        }).ToList();

        // Optionally add a "All Suppliers" choice
        searchModel.AvailableSuppliers.Insert(0, new SelectListItem
        {
            Text = "All Suppliers",
            Value = "0"
        });
        searchModel.SetGridPageSize();
        return searchModel;
    }



    // Added DataTabel Codes:







    public virtual async Task<SupplierProductListModel> PrepareSupplierProductListModelAsync(SupplierProductSearchModel searchModel, Guid purchaseOrderNo)
    {
        ArgumentNullException.ThrowIfNull(searchModel);

        //get related products
        var supplierProducts = (await _purchaseOrdersService
            .GetSupplierProductsBySupplierIdAsync(purchaseOrderNo: purchaseOrderNo, showHidden: true)).ToPagedList(searchModel);

        //prepare grid model
        var model = await new SupplierProductListModel().PrepareToGridAsync(searchModel, supplierProducts, () =>
        {
            return supplierProducts.SelectAwait(async supplierProduct =>
            {
                //fill in model values from the entity
                var supplierProductModel = new SupplierProductModel
                {
                    Id = supplierProduct.Id,
                    ProductId2 = supplierProduct.ProductId,
                    Product2Name = (await _productService.GetProductByIdAsync(supplierProduct.ProductId))?.Name,
                };

                //fill in additional values (not existing in the entity)
                supplierProductModel.Product2Name = (await _productService.GetProductByIdAsync(supplierProduct.ProductId))?.Name;

                return supplierProductModel;
            });
        });
        return model;
    }

    public virtual async Task<AddSupplierProductSearchModel> PrepareAddSupplierProductSearchModelAsync(AddSupplierProductSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);


        //prepare available categories
        await _baseAdminModelFactory.PrepareCategoriesAsync(searchModel.AvailableCategories);

        //prepare available manufacturers
        await _baseAdminModelFactory.PrepareManufacturersAsync(searchModel.AvailableManufacturers);

        //prepare available stores
        await _baseAdminModelFactory.PrepareStoresAsync(searchModel.AvailableStores);

        //prepare available vendors
        await _baseAdminModelFactory.PrepareVendorsAsync(searchModel.AvailableVendors);

        //prepare available product types
        await _baseAdminModelFactory.PrepareProductTypesAsync(searchModel.AvailableProductTypes);

        //prepare page parameters
        searchModel.SetPopupGridPageSize();

        return searchModel;
    }

    public virtual async Task<AddSupplierProductListModel> PrepareAddSupplierProductListModelAsync(AddSupplierProductSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(searchModel);


        //get products
        var products = await _productService.SearchProductsAsync(showHidden: true,
            categoryIds: new List<int> { searchModel.SearchCategoryId },
            manufacturerIds: new List<int> { searchModel.SearchManufacturerId },
            storeId: searchModel.SearchStoreId,
            vendorId: searchModel.SearchVendorId,
            productType: searchModel.SearchProductTypeId > 0 ? (ProductType?)searchModel.SearchProductTypeId : null,
            keywords: searchModel.SearchProductName,
            pageIndex: searchModel.Page - 1, pageSize: searchModel.PageSize);

        if (searchModel.SupplierId > 0)
        {
            var mappedProductIds = await _supplierServices.GetProductsBySupplierIdAsync(searchModel.SupplierId);
        }

        //prepare grid model
        var model = await new AddSupplierProductListModel().PrepareToGridAsync(searchModel, products, () =>
        {
            return products.SelectAwait(async product =>
            {
                var productModel = product.ToModel<ProductModel>();

                productModel.SeName = await _urlRecordService.GetSeNameAsync(product, 0, true, false);

                return productModel;
            });
        });

        return model;
    }
}

