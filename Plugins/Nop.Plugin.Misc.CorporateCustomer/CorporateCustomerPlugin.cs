using Nop.Core;
using Nop.Services.Events;
using Nop.Services.Localization;
using Nop.Services.Plugins;
using Nop.Services.Security;
using Nop.Web.Framework.Events;
using Nop.Web.Framework.Menu;
using Nop.Web.Framework.Infrastructure;

namespace Nop.Plugin.Misc.CorporateCustomer;

public class CorporateCustomerPlugin : BasePlugin
{
    private readonly IWebHelper _webHelper;
    protected readonly ILocalizationService _localizationService;

    public CorporateCustomerPlugin(ILocalizationService localizationService, IWebHelper webHelper)
    {
        _localizationService = localizationService;
        _webHelper = webHelper;
    }

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
            ["Admin.CorporateCustomers"] = "Corporate Customers",
            ["Admin.CorporateCustomers.AddNew"] = "Add new corporate customer",
            ["Admin.CorporateCustomers.EditDetails"] = "Edit corporate customer details",
            ["Admin.CorporateCustomers.BackToList"] = "Back to corporate customer list",
            ["Admin.CorporateCustomers.Fields.CompanyName"] = "Company Name",
            ["Admin.CorporateCustomers.Fields.CompanyName.Hint"] = "Enter company name",
            ["Admin.CorporateCustomers.Fields.CompanyName.Required"] = "Company name is required",
            ["Admin.CorporateCustomers.Fields.ContactPerson"] = "Contact Person",
            ["Admin.CorporateCustomers.Fields.ContactPerson.Hint"] = "Enter contact person name",
            ["Admin.CorporateCustomers.Fields.ContactPerson.Required"] = "Contact person is required",
            ["Admin.CorporateCustomers.Fields.Email"] = "Email",
            ["Admin.CorporateCustomers.Fields.Email.Hint"] = "Enter contact email",
            ["Admin.CorporateCustomers.Fields.Email.Required"] = "Email is required",
            ["Admin.CorporateCustomers.Fields.Phone"] = "Phone",
            ["Admin.CorporateCustomers.Fields.Phone.Hint"] = "Enter contact phone",
            ["Admin.CorporateCustomers.Fields.DiscountPercentage"] = "Discount Percentage",
            ["Admin.CorporateCustomers.Fields.DiscountPercentage.Hint"] = "Enter discount percentage for this corporate customer",
            ["Admin.CorporateCustomers.List.CompanyName"] = "Company Name",
            ["Admin.CorporateCustomers.List.ContactPerson"] = "Contact Person",
            ["Admin.CorporateCustomers.List.Email"] = "Email",
            ["Admin.CorporateCustomers.List.CompanyName.Hint"] = "Search by company name",
            ["Admin.CorporateCustomers.List.Email.Hint"] = "Search by email",
            ["Admin.CorporateCustomers.Info"] = "Corporate Customer Info",
            ["Admin.CorporateCustomers.EditDetails"] = "Edit Corporate Customer Details",
            ["Admin.CorporateCustomers.Updated"] = "Corporate customer updated successfully",
            ["Admin.CorporateCustomers.Deleted"] = "Corporate customer deleted successfully",
            ["Admin.CorporateCustomers.Added"] = "Corporate customer created successfully"
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
                SystemName = "Misc.CorporateCustomer",
                Title = "Corporate Customers",
                Url = eventMessage.GetMenuItemUrl("CorporateCustomer", "List"),
                IconClass = "fa-solid fa-building",
                Visible = true,
            }
         );
    }
} 