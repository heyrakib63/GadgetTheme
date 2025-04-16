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

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Controllers
{
    [AuthorizeAdmin]
    [Area("admin")]
    public class SupplierController : BasePluginController
    {
        private readonly ISupplierServices _supplierService;
        //private readonly IEmployeeModelFactory _employeeModelFactory;
        private readonly ISupplierModelFactory _supplierModelFactory;

        public SupplierController(ISupplierServices supplierService, ISupplierModelFactory supplierModelFactory)
        {
            _supplierService = supplierService;
            _supplierModelFactory = supplierModelFactory;
        }

        public async Task<IActionResult> List()
        {

            var searchModel = await _supplierModelFactory.PrepareSupplierSearchModelAsync(new SupplierSearchModel());


            return View("List", searchModel);
        }



        [HttpPost]
        public async Task<IActionResult> List(SupplierSearchModel searchModel)
        {
            var model = await _supplierModelFactory.PrepareSupplierListModelAsync(searchModel);


            return Json(model);
        }
    }
}
