//using Microsoft.AspNetCore.Mvc;
//using Nop.Core;
//using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
//using Nop.Services.Catalog;
//using Nop.Services.Customers;
//using Nop.Services.Orders;
//using Nop.Web.Framework.Components;

//namespace Nop.Plugin.GadgetTheme.SupplierManagement.Components;

//public class SupplierManagementWidgetViewComponent : NopViewComponent
//{


//    private readonly ISupplierServices _supplierService;
//    private readonly IWorkContext _workContext;
//    public SupplierManagementWidgetViewComponent(ISupplierServices supplierServices, IWorkContext workContext)
//    {
//        _supplierService = supplierServices;
//        _workContext = workContext;
//    }

//    //public IViewComponentResult Invoke(int productId)
//    //{
//    //    throw new NotImplementedException();
//    //}
//    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
//    {
        

//        var model = _supplierService.GetAllSupplierAsync().Result;
//        if (model == null)
//        {
//            return Content("");
//        }
//        return View(model);

//        //return View("~/Plugins/GadgetTheme.SupplierManagement/Views/ManufacturersSlider.cshtml");
//    }

//}




////using Microsoft.AspNetCore.Mvc;
////using Nop.Core;
////using Nop.Plugin.Other.ProductViewTracker.Domains;
////using Nop.Plugin.Other.ProductViewTracker.Services;
////using Nop.Services.Catalog;
////using Nop.Services.Customers;
////using Nop.Web.Framework.Components;
////using Nop.Web.Models.Catalog;

////namespace Nop.Plugin.Other.ProductViewTracker.Components

////public class ProductViewTrackerViewComponent : NopViewComponent
////{
////    private readonly ICustomerService _customerService;
////    private readonly IProductService _productService;
////    private readonly IProductViewTrackerService _productViewTrackerService;
////    private readonly IWorkContext _workContext;
////    public ProductViewTrackerViewComponent(ICustomerService customerService,
////        IProductService productService,
////        IProductViewTrackerService productViewTrackerService,
////        IWorkContext workContext)
////    {
////        _customerService = customerService;
////        _productService = productService;
////        _productViewTrackerService = productViewTrackerService;
////        _workContext = workContext;
////    }
////    public async Task<IViewComponentResult> InvokeAsync(string widgetZone, object additionalData)
////    {
////        if (!(additionalData is ProductDetailsModel model))
////            return Content("");
////        //Read from the product service
////        var productById = await _productService.GetProductByIdAsync(model.Id);
////        //If the product exists we will log it
////        if (productById != null)
////        {
////            var currentCustomer = await _workContext.CurrentCustomerAsync();
////            //Setup the product to save
////            var record = new ProductViewTrackerRecord
////            {
////                ProductId = model.Id,
////                ProductName = productById.Name,
////                CustomerId = currentCustomer.Id,
////                IpAddress = currentCustomer.LastIpAddress,
////                IsRegistered = await _customerService.Async(currentCustomer)
////            };
////            //Map the values we're interested in to our new entity
////            _productViewTrackerService.Log(record);
////        }
////        return Content("");
////    }
////}
