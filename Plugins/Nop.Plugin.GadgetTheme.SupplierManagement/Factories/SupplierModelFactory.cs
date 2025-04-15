//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
//using Nop.Plugin.GadgetTheme.SupplierManagement.Services;

//namespace Nop.Plugin.GadgetTheme.SupplierManagement.Factories;
//public class SupplierModelFactory : ISupplierModelFactory
//{
//    private readonly ISupplierServices _supplierService;

//    public SupplierModelFactory(ISupplierServices supplierService)
//    {
//        _supplierService = supplierService;
//    }

//    public async Task<SupplierList> PrepareSupplierListModelAsync()
//    {
//        var allSuppliers = await _supplierService.GetAllSupplierAsync();

//        var model = new SupplierList();

//        // Use AddRange to populate the base List<T> inside the model
//        model.SetGridPageSize(); // Default page size = 10
//        model.AddRange(allSuppliers.Select(x => new Supplier
//        {
//            Id = x.Id,
//            SupplierId = x.SupplierId,
//            SupplierName = x.SupplierName,
//            SupplierEmail = x.SupplierEmail,
//            SupplierAddress = x.SupplierAddress
//            //SupplierImageUrl = x.ImageUrl
//        }));

//        return model;
//    }

//}
