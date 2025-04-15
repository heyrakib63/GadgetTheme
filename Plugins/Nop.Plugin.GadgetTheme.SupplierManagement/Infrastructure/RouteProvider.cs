using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;
internal class RouteProvider : IRouteProvider
{
    // Implementing the RegisterRoutes method from IRouteProvider
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        endpointRouteBuilder.MapControllerRoute("Plugin.GadgetTheme.SupplierManagement", "Plu/Areas/Admin/Supplier/List",
            new { controller = "Supplier", action = "GetAllSupplier" });
    }

    // Implementing the Priority property from IRouteProvider
    public int Priority => 0; // Default priority, can be adjusted as needed
}
