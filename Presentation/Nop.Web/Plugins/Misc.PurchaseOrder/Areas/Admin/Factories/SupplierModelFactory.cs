using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Nop.Core.Caching;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Factories;
using Nop.Web.Framework.Models.Extensions;
using Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Factories;
public class SupplierModelFactory : ISupplierModelFactory
{
    private readonly ILocalizationService _localizationService;
    private readonly ISupplierServices _supplierService;
    private readonly ILocalizedModelFactory _localizedModelFactory;
    private readonly IStaticCacheManager _staticCacheManager;
    public SupplierModelFactory(
        ILocalizationService localizationService,
        ISupplierServices supplierService,
        ILocalizedModelFactory localizedModelFactory,
        IStaticCacheManager staticCacheManager
        )
    {
        _localizationService = localizationService;
        _supplierService = supplierService;
        _localizedModelFactory = localizedModelFactory;
        _staticCacheManager = staticCacheManager;
    }
    // Return lists of Suppliers aka grid
    public async Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));

        var suppliers = await _supplierService.SearchSupplierAsync(searchModel.Name, searchModel.Email,
                       pageIndex: searchModel.Page - 1,
                       pageSize: searchModel.PageSize);
        //prepare grid model
        var count = suppliers.Count;
        var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.AdminSupplierAllModelKey,searchModel.Name, searchModel.Email,0);
        var output = await _staticCacheManager.GetAsync(cacheKey, async () =>
        {
            var model = await new SupplierListModel().PrepareToGridAsync(searchModel, suppliers, () =>
            {
                return suppliers.SelectAwait(async supplier =>
                {
                    return await PrepareSupplierModelAsync(null, supplier, true);
                });
            });
            return model;
        });
        return output;
    }
    // Returns the a single supplier model
    public async Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false)
    {
        Func<SupplierLocalizedModel, int, Task> localizedModelConfiguration = null;
        if (supplier != null)
        {
            if (model == null)
            {
                //fill in model values from the entity
                model = new SupplierModel()
                {
                    Id = supplier.Id,
                    Name = supplier.SupplierName,
                    Email = supplier.SupplierEmail,
                    Address = supplier.SupplierAddress,
                };
            }
            localizedModelConfiguration = async (locale, languageId) =>
            {
                locale.Name = await _localizationService.GetLocalizedAsync(supplier, entity => entity.SupplierName, languageId, false, false);
                locale.Address = await _localizationService.GetLocalizedAsync(supplier, entity => entity.SupplierAddress, languageId, false, false);

            };
        }
        if (!excludeProperties)
            model.Locales = await _localizedModelFactory.PrepareLocalizedModelsAsync(localizedModelConfiguration);
        // Simulate async behavior to resolve CS1998
        await Task.CompletedTask;
        return model;
    }

    // For the search model. 
    public async Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel)
    {
        if (searchModel == null)
            throw new ArgumentNullException(nameof(searchModel));
        // Simulate async behavior to resolve CS1998
        await Task.CompletedTask;
        //prepare page parameters
        searchModel.SetGridPageSize();
        return searchModel;
    }
}
