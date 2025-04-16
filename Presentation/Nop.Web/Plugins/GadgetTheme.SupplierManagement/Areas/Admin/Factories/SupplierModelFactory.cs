using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Nop.Core.Caching;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Services.Localization;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Factories;
public class SupplierModelFactory : ISupplierModelFactory
{

    private readonly ILocalizationService _localizationService;
    private readonly ISupplierServices _supplierService;
    private readonly IStaticCacheManager _staticCacheManager;

    public SupplierModelFactory(ILocalizationService localizationService, ISupplierServices supplierService)
    {
        _localizationService = localizationService;
        _supplierService = supplierService;
    }

    public async Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchModel));

        var suppliers = await _supplierService.SearchSupplierAsync(searchModel.Name,

                       pageIndex: searchModel.Page - 1,
                       pageSize: searchModel.PageSize);

        //prepare grid model

        var count = suppliers.Count();




        //var cacheKey = _staticCacheManager.PrepareKeyForDefaultCache(NopModelCacheDefaults.AdminEmployeeAllModelKey, searchModel.IsMVP, searchModel.IsCertified, 0);

        //var output = await _staticCacheManager.GetAsync(cacheKey, async () =>
        //{
            var model = await new SupplierListModel().PrepareToGridAsync(searchModel, suppliers, () =>
            {
                return suppliers.SelectAwait(async supplier =>
                {
                    return await PrepareSupplierModelAsync(null, supplier, true);
                });
            });

            //await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.PublicEmployeeAllPrefixCacheKey);

        return model;
    //});

    //    return output;
    }





    public async Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false)
    {
        if (supplier != null)
        {
            if (model == null)
                //fill in model values from the entity
                model = new SupplierModel()
                {
                    Id = supplier.Id,
                    Name = supplier.SupplierName,
                    Email = supplier.SupplierEmail,
                    Address = supplier.SupplierAddress,
                };
        }

        //if (!excludeProperties)
        //    model.AvailableEmployeeStatusOptions = (await EmployeeStatus.Active.ToSelectListAsync()).ToList();

        return model;
    }



    public async Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchModel));



        //searchModel.AvailableEmployeeStatusOptions = (await EmployeeStatus.Active.ToSelectListAsync()).ToList();
        //searchModel.AvailableEmployeeStatusOptions.Insert(0,
        //     new Microsoft.AspNetCore.Mvc.Rendering.SelectListItem
        //     {
        //         Text = "All",
        //         Value = "0"

        //     });

        //prepare page parameters
        searchModel.SetGridPageSize();

        return searchModel;
    }

}
