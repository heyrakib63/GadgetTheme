using Microsoft.AspNetCore.Mvc;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Services.Localization;
using Nop.Services.Messages;
using Nop.Web.Framework.Controllers;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;


namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class SupplierController : BasePluginController
    {
        private readonly ISupplierServices _supplierService;
        private readonly ISupplierModelFactory _supplierModelFactory;
        private readonly IWorkContext _workContext;
        private readonly ILocalizationService _localizationService;
        private readonly INotificationService _notificationService;
        private readonly ILocalizedEntityService _localizedEntityService;
        private readonly IStaticCacheManager _staticCacheManager;

        public SupplierController(
            ISupplierServices supplierService,
            ISupplierModelFactory supplierModelFactory,
            IWorkContext workContext,
            ILocalizationService localizationService,
            INotificationService notificationService,
            ILocalizedEntityService localizedEntityService,
            IStaticCacheManager staticCacheManager
            )
        {
            _supplierService = supplierService;
            _supplierModelFactory = supplierModelFactory;
            _workContext = workContext;
            _localizationService = localizationService;
            _notificationService = notificationService;
            _localizedEntityService = localizedEntityService;
            _staticCacheManager = staticCacheManager;
        }
        //For supporting malti language.
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
                await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.AdminSupplierAllPrefixCacheKey);
                await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.PublicSupplierAllPrefixCacheKey);
                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Admin.Suppliers.Added"));
                return continueEditing ? RedirectToAction("Edit", new { id = supplier.Id }) : RedirectToAction("List");
            }
            model = await _supplierModelFactory.PrepareSupplierModelAsync(model, null);
            return View("Create", model);
        }
        // The edit view.
        public virtual async Task<IActionResult> Edit(int id)
        {
            //try to get a supplier with the specified id
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

                await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.AdminSupplierAllPrefixCacheKey);
                await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.PublicSupplierAllPrefixCacheKey);
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
            await _staticCacheManager.RemoveByPrefixAsync(NopModelCacheDefaults.AdminSupplierAllPrefixCacheKey);
            return RedirectToAction("List");
        }
    }
}
