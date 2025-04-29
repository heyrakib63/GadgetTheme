using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Core.Caching;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Models;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Factories;
public class PurchaseOrdersModelFactory : IPurchaseOrdersModelFactory
{
    private readonly ILocalizationService _localizationService;
    private readonly IPurchaseOrdersService _purchaseOrdersService;
    private readonly ILocalizedModelFactory _localizedModelFactory;
    private readonly ISupplierServices _supplierServices;
    //private readonly IStaticCacheManager _staticCacheManager;
    public PurchaseOrdersModelFactory(
        ILocalizationService localizationService,
        IPurchaseOrdersService purchaseOrdersService,
        ILocalizedModelFactory localizedModelFactory,
        ISupplierServices supplierServices
        )
    {
        _localizationService = localizationService;
        _purchaseOrdersService = purchaseOrdersService;
        _localizedModelFactory = localizedModelFactory;
        _supplierServices = supplierServices;
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
                //fill in model values from the entity
                model = new PurchaseOrdersModel()
                {
                    Id = purchaseOrder.Id,
                    SupplierId = purchaseOrder.SupplierId,
                    CreatedOnUtc = purchaseOrder.CreatedOnUtc,
                    TotalCost = purchaseOrder.TotalCost,
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
}

