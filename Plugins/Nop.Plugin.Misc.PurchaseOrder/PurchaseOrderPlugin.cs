using Nop.Core;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;

namespace Nop.Plugin.Misc.PurchaseOrder;
public class PurchaseOrderPlugin : BasePlugin
{
    private readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;
    public PurchaseOrderPlugin(ILocalizationService localizationService, IWebHelper webHelper)
    {
        _localizationService = localizationService;
        _webHelper = webHelper;
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
            ["Admin.PurchaseOrder"] = "Purchase Order",
            ["Admin.PurchaseOrder.AddNew"] = "Add new Orders",
            ["Admin.PurchaseOrder.BackToList"] = "Back to Order List",
            ["Admin.PurchaseOrder.Fields.Name"] = "Name",
            ["Admin.PurchaseOrder.Fields.Name.Hint"] = "Enter PurchaseOrder Name.",
            ["Admin.PurchaseOrders.Fields.Name.Required"] = "Name is Required",
            ["Admin.PurchaseOrders.Fields.Email"] = "Email",
            ["Admin.PurchaseOrders.Fields.Email.Hint"] = "Enter PurchaseOrders Email.",
            ["Admin.PurchaseOrders.Fields.Email.Required"] = "Email is Required",
            ["Admin.PurchaseOrders.Fields.Address"] = "Address",
            ["Admin.PurchaseOrders.Fields.Address.Hint"] = "Enter PurchaseOrders Address.",
            ["Admin.PurchaseOrders.Fields.Address.Required"] = "Address is Required",
            ["Admin.PurchaseOrders.List.Name"] = "Name",
            ["Admin.PurchaseOrders.List.Email"] = "Email",
            ["Admin.PurchaseOrders.List.Name.Hint"] = "Search by PurchaseOrders Name",
            ["Admin.PurchaseOrders.List.Email.Hint"] = "Search by Employee Email",
            ["Admin.PurchaseOrders.Info"] = "PurchaseOrders Info",
            ["Admin.PurchaseOrders.EditPurchaseOrdersDetails"] = "Edit PurchaseOrders Details",
            ["Admin.PurchaseOrders.Updated"] = "PurchaseOrders Updated Successfully",
            ["Admin.PurchaseOrders.Deleted"] = "Deleted succusfully",
            ["Admin.PurchaseOrders.Added"] = "Created Successfully",
            ["Admin.Catalog.Products.ProductPurchaseOrders.SaveBeforeEdit"] = "You need to save the product before you can link a PurchaseOrders for this product page.",
        };
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
                SystemName = "Misc.PurchaseOrder",
                Title = "Purchase Order",
                Url = eventMessage.GetMenuItemUrl("PurchaseOrder", "List"),
                IconClass = "fa-solid fa-cart-shopping",
                Visible = true,
            }
         );
    }
}




