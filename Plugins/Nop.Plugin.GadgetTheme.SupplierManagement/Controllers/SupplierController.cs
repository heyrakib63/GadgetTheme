using Microsoft.AspNetCore.Mvc;
//using GadgetTheme.SupplierManagement.Services;
using Nop.Web.Framework.Controllers;
using System.Threading.Tasks;
//using MyCompany.SupplierManagement.Services;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Web.Framework.Mvc.Filters;
using Nop.Web.Framework.Models.DataTables;

namespace GadgetTheme.SupplierManagement.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class SupplierController : BasePluginController
    {
        private readonly ISupplierServices _supplierService;

        public SupplierController(ISupplierServices supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            
            SupplierList supplierList = new SupplierList
            {
                AvailablePageSizes = "10"
            };
            return View("~/Plugins/GadgetTheme.SupplierManagement/Areas/Admin/Supplier/List.cshtml", supplierList);


            //var suppliers = await _supplierService.GetAllSupplierAsync();
            //return View("~/Plugins/GadgetTheme.SupplierManagement/Views/List.cshtml", suppliers);
        }

        [HttpPost]
        public async Task<IActionResult> GetAllSupplier()
        {
            try
            {
                return Ok(new DataTablesModel { Data = await _supplierService.GetAllSupplierAsync() });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        //public IActionResult Create() => View("~/Plugins/GadgetTheme.SupplierManagement/Views/Supplier/Create.cshtml");

        //[HttpPost]
        //public async Task<IActionResult> Create(Supplier supplier)
        //{
        //    if (!ModelState.IsValid)
        //        return View(supplier);

        //    await _supplierService.InsertAsync(supplier);
        //    return RedirectToAction("List");
        //}

        //public async Task<IActionResult> Edit(int id)
        //{
        //    var supplier = await _supplierService.GetByIdAsync(id);
        //    return View("~/Plugins/GadgetTheme.SupplierManagement/Views/Supplier/Edit.cshtml", supplier);
        //}

        //[HttpPost]
        //public async Task<IActionResult> Edit(Supplier supplier)
        //{
        //    if (!ModelState.IsValid)
        //        return View(supplier);

        //    await _supplierService.UpdateAsync(supplier);
        //    return RedirectToAction("List");
        //}

        //public async Task<IActionResult> Delete(int id)
        //{
        //    var supplier = await _supplierService.GetByIdAsync(id);
        //    if (supplier != null)
        //        await _supplierService.DeleteAsync(supplier);

        //    return RedirectToAction("List");
        //}
    }
}
