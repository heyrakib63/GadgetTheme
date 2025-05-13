
using Nop.Core;
using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Components;
using Nop.Services.Cms;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Widgets.ProductDescriptionExt;
public class ProductDescriptionExtPlugin : BasePlugin, IWidgetPlugin
{
    private readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;
    public ProductDescriptionExtPlugin(ILocalizationService localizationService, IWebHelper webHelper)
    {
        _localizationService = localizationService;
        _webHelper = webHelper;
    }
    // Invoking the viewcomponents.
    public bool HideInWidgetList => false;
    public Task<IList<string>> GetWidgetZonesAsync()
    {
        return Task.FromResult<IList<string>>(new List<string> {
                    AdminWidgetZones.ProductDetailsBlock
                });
    }
    public Type GetWidgetViewComponent(string widgetZone)
    {
        if (widgetZone == AdminWidgetZones.ProductDetailsBlock)
        {
            return typeof(ProductDescriptionExtViewComponent);
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
            ["Admin.Catalog.Products.ProductDescription.SaveBeforeEdit"] = "Please save the product before adding extra description",
            ["Admin.ProductDescriptionExt.Fields.Description"] = "Description",
            ["Admin.ProductDescriptionExt.Fields.Description.Hint"] = "Enter your extra description here"
        };
    }
}