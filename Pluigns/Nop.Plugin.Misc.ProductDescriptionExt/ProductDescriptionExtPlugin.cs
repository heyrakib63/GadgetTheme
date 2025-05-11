namespace Nop.Plugin.Misc.ProductDescriptionExt;

using global::Nop.Core;
using global::Nop.Services.Cms;
using global::Nop.Services.Localization;
using global::Nop.Services.Plugins;
using global::Nop.Web.Framework.Infrastructure;
using Nop.Plugin.Misc.ProductDescriptionExt.Areas.Admin.Components;
using Nop.Plugin.Misc.ProductDescriptionExt.Components;

public class SupplierManagementPlugin : BasePlugin, IWidgetPlugin
{
    private readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;
    public SupplierManagementPlugin(ILocalizationService localizationService, IWebHelper webHelper)
    {
        _localizationService = localizationService;
        _webHelper = webHelper;
    }
    // Invoking the viewcomponents.
    public bool HideInWidgetList => false;
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> {
                    AdminWidgetZones.ProductDetailsBlock,
                    "productdetails_custom_description"
                });
    }
    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone == AdminWidgetZones.ProductDetailsBlock)
        {
            return typeof(ProductDescriptionViewComponent);
        }else if(widgetZone == "productdetails_custom_description")
        {
            return typeof(ProductDescriptionPublicViewComponent);
        }
        return null;
    }
    // Install and Uninstall Logics
    public override async Task InstallAsync()
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(GetLocaleResources());
        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string oldVersion, string newVersion)
    {
        await _localizationService.AddOrUpdateLocaleResourceAsync(GetLocaleResources());

        await base.UpdateAsync(oldVersion, newVersion);
    }

    private Dictionary<string, string> GetLocaleResources()
    {
        return new Dictionary<string, string>
        {
            ["Admin.ProductDescription"] = "Product additional description",
            ["Admin.Catalog.Products.ProductDescription.SaveBeforeEdit"] = "Please save the product before adding extra description"
        };
    }
}




