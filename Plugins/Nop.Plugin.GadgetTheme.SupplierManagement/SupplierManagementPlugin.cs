using Nop.Services.Plugins;
using Nop.Services.Localization;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.GadgetTheme.SupplierManagement;

/// <summary>
/// Supplier Management Plugin
/// </summary>
public class SupplierManagementPlugin : BasePlugin
{
    protected readonly ILocalizationService _localizationService;

    public SupplierManagementPlugin(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
    }

    //public bool HideInWidgetList => false;

    //public async Task<IList<string>> GetWidgetZonesAsync()
    //{
    //    // Return the widget zones where this plugin should be displayed
    //    return await Task.FromResult(new List<string> { "home_page_top" });
    //}

    //public Type GetWidgetViewComponent(string widgetZone)
    //{
    //    // Return the view component type for the specified widget zone
    //    return typeof(Components.SupplierManagementWidgetViewComponent);
    //}

    public override async Task InstallAsync()
    {
        // Add the "Suppliers" menu item
        //await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        //{
        //    ["Admin.Menu.Suppliers"] = "Suppliers",
        //});

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        // Remove the "Suppliers" menu item
        //await _localizationService.DeleteLocaleResourcesAsync("Plugins.GadgetTheme.SupplierManagement");
        await base.UninstallAsync();
    }
}


public class EventConsumer : IConsumer<AdminMenuCreatedEvent>
{
    private readonly IPermissionService _permissionService;

    public EventConsumer(IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }

    public Task HandleEventAsync(AdminMenuCreatedEvent eventMessage)
    {
        return HandleEventInternalAsync(eventMessage);
    }

    private async Task HandleEventInternalAsync(AdminMenuCreatedEvent eventMessage)
    {
        if (!await _permissionService.AuthorizeAsync(StandardPermission.Configuration.MANAGE_PLUGINS))
            return;

        eventMessage.RootMenuItem.InsertBefore("Local plugins",
            new AdminMenuItem
            {
                SystemName = "GadgetTheme.SupplierManagement",
                Title = "Suppliers",
                Url = eventMessage.GetMenuItemUrl("Supplier", "List"),
                IconClass = "fa fa-dot-circle",
                Visible = true,
            });
    }
}