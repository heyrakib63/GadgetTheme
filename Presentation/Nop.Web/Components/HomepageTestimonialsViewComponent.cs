using Microsoft.AspNetCore.Mvc;
using Nop.Services.Catalog;
using Nop.Services.Security;
using Nop.Services.Stores;
using Nop.Web.Factories;
using Nop.Web.Framework.Components;

namespace Nop.Web.Components;

public partial class HomepageTestimonialsViewComponent : NopViewComponent
{
    //protected readonly IAclService _aclService;

    //public HomepageTestimonialsViewComponent(IAclService aclService)
    //{
    //    _aclService = aclService;
       
    //}

    public async Task<IViewComponentResult> InvokeAsync()
    { 
        return View();
    }
}