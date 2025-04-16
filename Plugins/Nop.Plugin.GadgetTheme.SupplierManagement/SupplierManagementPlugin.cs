using Nop.Services.Plugins;
using Nop.Services.Localization;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using Nop.Core;

namespace Nop.Plugin.GadgetTheme.SupplierManagement;

/// <summary>
/// Supplier Management Plugin
/// </summary>
public class SupplierManagementPlugin : BasePlugin
{
    private readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;

    public SupplierManagementPlugin(ILocalizationService localizationService, IWebHelper webHelper)
    {
        _localizationService = localizationService;
        _webHelper = webHelper;
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

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {

            ["Admin.GadgetTheme.Suppliers"] = "Suppliers",
            ["Admin.GadgetTheme.Suppliers.AddNew"] = "Add new Suppliers",
            ["Admin.GadgetTheme.Suppliers.EditDetails"] = "Edit Supplier Details",
            ["Admin.GadgetTheme.Suppliers.BackToList"] = "Back to Supplier List",


            ["Admin.GadgetTheme.Suppliers.Fields.Name"] = "Name",

            ["Admin.GadgetTheme.Suppliers.Fields.Name.Hint"] = "Enter Supplier Name.",


            ["Admin.GadgetTheme.Suppliers.List.Name"] = "Name",
            ["Admin.GadgetTheme.Suppliers.List.Email"] = "Email",
            ["Admin.GadgetTheme.Suppliers.List.SupplierName.Hint"] = "Search by Supplier Name",
            ["Admin.GadgetTheme.Suppliers.List.SupplierEmail.Hint"] = "Search by Employee Email",
        });
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