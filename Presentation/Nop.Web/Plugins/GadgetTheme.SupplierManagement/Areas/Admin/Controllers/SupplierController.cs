using Microsoft.AspNetCore.Mvc;
//using GadgetTheme.SupplierManagement.Services;
using Nop.Web.Framework.Controllers;
using System.Threading.Tasks;
//using MyCompany.SupplierManagement.Services;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Models.DataTables;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Core;
using Nop.Core.Caching;
using Microsoft.AspNetCore.Http;
using Nop.Core.Domain.Forums;
using Nop.Services.Common;
using Nop.Services.Vendors;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Areas.Admin.Infrastructure.Mapper.Extensions;
using Nop.Web.Areas.Admin.Controllers;
using Nop.Core.Domain.Vendors;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class SupplierController : BasePluginController
    {
        private readonly ISupplierServices _supplierService;
        //private readonly IEmployeeModelFactory _employeeModelFactory;
        private readonly ISupplierModelFactory _supplierModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizedEntityService _localizedEntityService;

        public SupplierController(
            ISupplierServices supplierService, 
            ISupplierModelFactory supplierModelFactory, 
            IWorkContext workContext,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ILocalizedEntityService localizedEntityService
            )
        {
            _supplierService = supplierService;
            _supplierModelFactory = supplierModelFactory;
            _workContext = workContext;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _localizedEntityService = localizedEntityService;
        }
        protected virtual async Task UpdateLocalesAsync(Supplier supplier, SupplierModel model)
        {
            foreach (var localized in model.Locales)
            {
                await _localizedEntityService.SaveLocalizedValueAsync(supplier,
                    x => x.SupplierName,
                    localized.Name,
                    localized.LanguageId);

                await _localizedEntityService.SaveLocalizedValueAsync(supplier,
                    x => x.SupplierAddress,
                    localized.Address,
                    localized.LanguageId);


                //search engine name
                //var seName = await _urlRecordService.ValidateSeNameAsync(vendor, localized.SeName, localized.Name, false);
                //await _urlRecordService.SaveSlugAsync(vendor, seName, localized.LanguageId);
            }
        }


        // Logics for List view
        public async Task<IActionResult> List()
        {

            var searchModel = await _supplierModelFactory.PrepareSupplierSearchModelAsync(new SupplierSearchModel());


            return View("List", searchModel);
        }


        // Logics for posting list of Suppliers
        [HttpPost]
        public async Task<IActionResult> List(SupplierSearchModel searchModel)
        {
            var model = await _supplierModelFactory.PrepareSupplierListModelAsync(searchModel);

            return Json(model);
        }




        // To generate the create view.
        public async Task<IActionResult> Create()
        {
            var model = await _supplierModelFactory.PrepareSupplierModelAsync(new SupplierModel(), null);

            return View("Create", model);
        }


        // The post method for Insert and logic where should it go after Inserting the data.
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public async Task<IActionResult> Create(SupplierModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                var supplier = new Supplier
                {
                    SupplierName = model.Name,
                    SupplierEmail = model.Email,
                    SupplierAddress = model.Address,

                };

                await _supplierService.InsertSupplierAsync(supplier);
                await UpdateLocalesAsync(supplier, model);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Suppliers.Added"));
                //update picture seo file name
                //await UpdatePictureSeoNamesAsync(employee);
                //await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.AdminEmployeeAllPrefixCacheKey);
                //await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.PublicEmployeeAllPrefixCacheKey);

                return continueEditing ? RedirectToAction("Edit", new { id = supplier.Id }) : RedirectToAction("List");
            }

            model = await _supplierModelFactory.PrepareSupplierModelAsync(model, null);

            return View("Create", model);
        }



        // The edit view.
        public virtual async Task<IActionResult> Edit(int id)
        {
            //try to get a vendor with the specified id
            var supplier = await _supplierService.GetSupplierByIdAsync(id);
            if(supplier == null)
            {
                return RedirectToAction("List");
            }

            //prepare model
            var model = await _supplierModelFactory.PrepareSupplierModelAsync(null, supplier);

            return View("Edit", model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public virtual async Task<IActionResult> Edit(SupplierModel model, bool continueEditing)
        {
            //try to get a vendor with the specified id
            var supplier = await _supplierService.GetSupplierByIdAsync(model.Id);
            if (supplier == null)
            {
                return RedirectToAction("List");
            }

            if (ModelState.IsValid)
            {

                //supplier = model.ToEntity(supplier);
                supplier.SupplierName = model.Name;
                supplier.SupplierEmail = model.Email;
                supplier.SupplierAddress = model.Address;


                await _supplierService.UpdateSupplierAsync(supplier);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Suppliers.Updated"));

                return continueEditing ? RedirectToAction("Edit", new { id = supplier.Id }) : RedirectToAction("List");
            }
            await UpdateLocalesAsync(supplier, model);
            //prepare model
            model = await _supplierModelFactory.PrepareSupplierModelAsync(model, supplier, true);

            return View("Edit", model);
        }



        // Delete a single supplier.
        [HttpPost]
        public async Task<IActionResult> Delete(SupplierModel model)
        {
            var supplier = await _supplierService.GetSupplierByIdAsync(model.Id);
            if (supplier == null)
                return RedirectToAction("List");
            
            await _supplierService.DeleteSupplierAsync(supplier);
            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Suppliers.Deleted"));
            return RedirectToAction("List");
        }


        //[HttpPost]

        //public async Task<IActionResult> DeleteSelected(ICollection<int> selectedId)
        //{


        //    if (selectedId == null || !selectedId.Any())
        //        return NoContent();

        //    //var currentVendor = await _workContext.GetCurrentVendorAsync();
        //    //await _employeeService.DeleteEmployeeAsync((await _employeeService.GetEmployeeByIdAsync(selectedId.ToArray<>))
        //    //    .Where(p => currentVendor == null || p.VendorId == currentVendor.Id).ToList());
        //    try
        //    {

        //        foreach (var id in selectedId)
        //        {
        //            var supplier = await _supplierService.GetSupplierByIdAsync(id);
        //            if (supplier != null)
        //            {
        //                await _supplierService.DeleteSupplierAsync(supplier);

        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //    //await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.AdminEmployeeAllPrefixCacheKey);
        //    return Json(new { Result = true });
        //}








    }
}
