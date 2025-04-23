using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Infrastructure;
internal class RouteProvider : IRouteProvider
{
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    { 
    }
    public int Priority => 0; // Default priority, can be adjusted as needed
}
