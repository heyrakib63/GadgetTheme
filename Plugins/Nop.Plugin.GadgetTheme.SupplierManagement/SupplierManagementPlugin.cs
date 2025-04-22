using Nop.Services.Plugins;
using Nop.Services.Localization;
using Nop.Services.Cms;
using Nop.Services.Events;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using Nop.Core;
using DocumentFormat.OpenXml.Spreadsheet;
using Nop.Plugin.GadgetTheme.SupplierManagement.Components;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.GadgetTheme.SupplierManagement;

/// <summary>
/// Supplier Management Plugin
/// </summary>
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
        return Task.FromResult<IList<string>>(new List<string> { AdminWidgetZones.ProductDetailsBlock});
    }

    /// <summary>
    /// Gets a type of a view component for displaying widget
    /// </summary>
    /// <param name="widgetZone">Name of the widget zone</param>
    /// <returns>View component type</returns>
    public Type GetWidgetViewComponent(string widgetZone)
    {
        return typeof(SupplierDropdownViewComponent);
    }





    // Install and Uninstall Logics

    public override async Task InstallAsync()
    {

        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {

            ["Admin.Suppliers"] = "Suppliers",
            ["Admin.Suppliers.AddNew"] = "Add new Suppliers",
            ["Admin.Suppliers.EditDetails"] = "Edit Supplier Details",
            ["Admin.Suppliers.BackToList"] = "Back to Supplier List",


            ["Admin.Suppliers.Fields.Name"] = "Name",
            ["Admin.Suppliers.Fields.Name.Hint"] = "Enter Supplier Name.",
            ["Admin.Suppliers.Fields.Name.Required"] = "Name is Required",
            ["Admin.Suppliers.Fields.Email"] = "Email",
            ["Admin.Suppliers.Fields.Email.Hint"] = "Enter Supplier Email.",
            ["Admin.Suppliers.Fields.Email.Required"] = "Email is Required",
            ["Admin.Suppliers.Fields.Address"] = "Address",
            ["Admin.Suppliers.Fields.Address.Hint"] = "Enter Supplier Address.",
            ["Admin.Suppliers.Fields.Address.Required"] = "Address is Required",


            ["Admin.Suppliers.List.Name"] = "Name",
            ["Admin.Suppliers.List.Email"] = "Email",
            ["Admin.Suppliers.List.Name.Hint"] = "Search by Supplier Name",
            ["Admin.Suppliers.List.Email.Hint"] = "Search by Employee Email",


            ["Admin.Suppliers.Info"] = "Suppliers Info",

            ["Admin.Suppliers.EditSupplierDetails"] = "Edit Supplier Details",
            ["Admin.Suppliers.Updated"] = "Supplier Updated Successfully",
            ["Admin.Suppliers.Deleted"] = "Deleted succusfully",
            ["Admin.Suppliers.Added"] = "Created Successfully",
            ["Admin.Catalog.Products.ProductSupplier.SaveBeforeEdit"] = "You need to save the product before you can link a supplier for this product page.",
        });

        await base.InstallAsync();
    }

    public override async Task UninstallAsync()
    {
        // Remove the "Suppliers" menu item
        
        await base.UninstallAsync();
    }

    public override async Task UpdateAsync(string oldVersion, string newVersion)
    {
        // You can log version transition if needed
        // _logger.Information($"Updating from {oldVersion} to {newVersion}");

        // Re-add or update resources
        await _localizationService.AddOrUpdateLocaleResourceAsync(new Dictionary<string, string>
        {

            ["Admin.Suppliers"] = "Suppliers",
            ["Admin.Suppliers.AddNew"] = "Add new Suppliers",
            ["Admin.Suppliers.EditDetails"] = "Edit Supplier Details",
            ["Admin.Suppliers.BackToList"] = "Back to Supplier List",


            ["Admin.Suppliers.Fields.Name"] = "Name",
            ["Admin.Suppliers.Fields.Name.Hint"] = "Enter Supplier Name.",
            ["Admin.Suppliers.Fields.Name.Required"] = "Name is Required",
            ["Admin.Suppliers.Fields.Email"] = "Email",
            ["Admin.Suppliers.Fields.Email.Hint"] = "Enter Supplier Email.",
            ["Admin.Suppliers.Fields.Email.Required"] = "Email is Required",
            ["Admin.Suppliers.Fields.Address"] = "Address",
            ["Admin.Suppliers.Fields.Address.Hint"] = "Enter Supplier Address.",
            ["Admin.Suppliers.Fields.Address.Required"] = "Address is Required",


            ["Admin.Suppliers.List.Name"] = "Name",
            ["Admin.Suppliers.List.Email"] = "Email",
            ["Admin.Suppliers.List.Name.Hint"] = "Search by Supplier Name",
            ["Admin.Suppliers.List.Email.Hint"] = "Search by Employee Email",


            ["Admin.Suppliers.Info"] = "Suppliers Info",

            ["Admin.Suppliers.EditSupplierDetails"] = "Edit Supplier Details",
            ["Admin.Suppliers.Updated"] = "Supplier Updated Successfully",
            ["Admin.Suppliers.Deleted"] = "Deleted succusfully",
            ["Admin.Suppliers.Added"] = "Created Successfully",
            ["Admin.Catalog.Products.ProductSupplier.SaveBeforeEdit"] = "You need to save the product before you can link a supplier for this product page.",

        });

        await base.UpdateAsync(oldVersion, newVersion);
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

        eventMessage.RootMenuItem.InsertAfter("Configuration",
            new AdminMenuItem
            {
                SystemName = "GadgetTheme.SupplierManagement",
                Title = "Suppliers",
                Url = eventMessage.GetMenuItemUrl("Supplier", "List"),
                IconClass = "fa-solid fa-truck-field",
                Visible = true,
            });
    }
}



