﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Common.Internal.Cache;
using Microsoft.Extensions.Caching.Memory;
using Nop.Core.Caching;
using Nop.Core.Domain.Vendors;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Services.Catalog;
using Nop.Services.Localization;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Framework.Models.Extensions;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Factories;
public class SupplierModelFactory : ISupplierModelFactory
{

    private readonly ILocalizationService _localizationService;
    private readonly ISupplierServices _supplierService;

    public SupplierModelFactory(ILocalizationService localizationService, ISupplierServices supplierService)
    {
        _localizationService = localizationService;
        _supplierService = supplierService;
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
        var count = suppliers.Count();

        var model = await new SupplierListModel().PrepareToGridAsync(searchModel, suppliers, () =>
        {
            return suppliers.SelectAwait(async supplier =>
            {
                return await PrepareSupplierModelAsync(null, supplier, true);
            });
        });

        return model;
    }







    // Returns the a single supplier model
    public async Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false)
    {
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
        }
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
