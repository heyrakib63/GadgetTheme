using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Nop.Web.Framework.Mvc.Routing;
using Nop.Web.Infrastructure;

namespace NopStation.Plugin.Widget.ProductDescriptionExt.Infrastructure;
public class RouteProvider : BaseRouteProvider, IRouteProvider
{
    #region Methods

    /// <summary>
    /// Register routes
    /// </summary>
    /// <param name="endpointRouteBuilder">Route builder</param>
    public void RegisterRoutes(IEndpointRouteBuilder endpointRouteBuilder)
    {
        //get language pattern
        //it's not needed to use language pattern in AJAX requests and for actions returning the result directly (e.g. file to download),
        //use it only for URLs of pages that the user can go to
        var lang = GetLanguageRoutePattern();


        endpointRouteBuilder.MapControllerRoute(name: "CustomerInfo",
            pattern: $"{lang}/customer/info",
            defaults: new { controller = "CustomerExtended", action = "Info" });

        endpointRouteBuilder.MapControllerRoute(name: "CustomerAddressExtendedEdit",
            pattern: $"{lang}/customer/addressedit/{{addressId:min(0)}}",
            defaults: new { controller = "CustomerExtended", action = "AddressEdit" });

    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets a priority of route provider
    /// </summary>
    public int Priority => 1;

    #endregion
}