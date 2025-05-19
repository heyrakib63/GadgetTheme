using System.Text.Encodings.Web;
using Nop.Core;
using Nop.Core.Domain;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Forums;
using Nop.Core.Domain.Gdpr;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Media;
using Nop.Core.Domain.Security;
using Nop.Core.Domain.Tax;
using Nop.Core.Events;
using Nop.Services.Attributes;
using Nop.Services.Authentication.External;
using Nop.Services.Authentication.MultiFactor;
using Nop.Services.Catalog;
using Nop.Services.Common;
using Nop.Services.Customers;
using Nop.Services.Directory;
using Nop.Services.ExportImport;
using Nop.Services.Gdpr;
using Nop.Services.Helpers;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Media;
using Nop.Services.Messages;
using Nop.Services.Orders;
using Nop.Services.Security;
using Nop.Services.Tax;
using Nop.Web.Controllers;
using Nop.Web.Factories;
using Nop.Services.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Nop.Core.Domain.Messages;
using Nop.Web.Models.Customer;
using Nop.Web.Framework.Controllers;
using Nop.Services.Events;
using Microsoft.Extensions.Primitives;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Controllers;

public partial class CustomerExtendedController : BasePluginController
{
    protected readonly AddressSettings _addressSettings;
    protected readonly CaptchaSettings _captchaSettings;
    protected readonly CustomerSettings _customerSettings;
    protected readonly DateTimeSettings _dateTimeSettings;
    protected readonly ForumSettings _forumSettings;
    protected readonly GdprSettings _gdprSettings;
    protected readonly HtmlEncoder _htmlEncoder;
    protected readonly IAddressModelFactory _addressModelFactory;
    protected readonly IAddressService _addressService;
    protected readonly IAttributeParser<AddressAttribute, AddressAttributeValue> _addressAttributeParser;
    protected readonly IAttributeParser<CustomerAttribute, CustomerAttributeValue> _customerAttributeParser;
    protected readonly IAttributeService<CustomerAttribute, CustomerAttributeValue> _customerAttributeService;
    protected readonly IAuthenticationService _authenticationService;
    protected readonly ICountryService _countryService;
    protected readonly ICurrencyService _currencyService;
    protected readonly ICustomerActivityService _customerActivityService;
    protected readonly ICustomerModelFactory _customerModelFactory;
    protected readonly ICustomerRegistrationService _customerRegistrationService;
    protected readonly ICustomerService _customerService;
    protected readonly IDownloadService _downloadService;
    protected readonly IEventPublisher _eventPublisher;
    protected readonly IExportManager _exportManager;
    protected readonly IExternalAuthenticationService _externalAuthenticationService;
    protected readonly IGdprService _gdprService;
    protected readonly IGenericAttributeService _genericAttributeService;
    protected readonly IGiftCardService _giftCardService;
    protected readonly ILocalizationService _localizationService;
    protected readonly ILogger _logger;
    protected readonly IMultiFactorAuthenticationPluginManager _multiFactorAuthenticationPluginManager;
    protected readonly INewsLetterSubscriptionService _newsLetterSubscriptionService;
    protected readonly INotificationService _notificationService;
    protected readonly IOrderService _orderService;
    protected readonly IPermissionService _permissionService;
    protected readonly IPictureService _pictureService;
    protected readonly IPriceFormatter _priceFormatter;
    protected readonly IProductService _productService;
    protected readonly IStateProvinceService _stateProvinceService;
    protected readonly IStoreContext _storeContext;
    protected readonly ITaxService _taxService;
    protected readonly IWorkContext _workContext;
    protected readonly IWorkflowMessageService _workflowMessageService;
    protected readonly LocalizationSettings _localizationSettings;
    protected readonly MediaSettings _mediaSettings;
    protected readonly MultiFactorAuthenticationSettings _multiFactorAuthenticationSettings;
    protected readonly StoreInformationSettings _storeInformationSettings;
    protected readonly TaxSettings _taxSettings;
    private static readonly char[] _separator = [','];
    public CustomerExtendedController(
        AddressSettings addressSettings,
        CaptchaSettings captchaSettings,
        CustomerSettings customerSettings,
        DateTimeSettings dateTimeSettings,
        ForumSettings forumSettings,
        GdprSettings gdprSettings,
        HtmlEncoder htmlEncoder,
        IAddressModelFactory addressModelFactory,
        IAddressService addressService,
        IAttributeParser<AddressAttribute, AddressAttributeValue> addressAttributeParser,
        IAttributeParser<CustomerAttribute, CustomerAttributeValue> customerAttributeParser,
        IAttributeService<CustomerAttribute, CustomerAttributeValue> customerAttributeService,
        IAuthenticationService authenticationService,
        ICountryService countryService,
        ICurrencyService currencyService,
        ICustomerActivityService customerActivityService,
        ICustomerModelFactory customerModelFactory,
        ICustomerRegistrationService customerRegistrationService,
        ICustomerService customerService,
        IDownloadService downloadService,
        IEventPublisher eventPublisher,
        IExportManager exportManager,
        IExternalAuthenticationService externalAuthenticationService,
        IGdprService gdprService,
        IGenericAttributeService genericAttributeService,
        IGiftCardService giftCardService,
        ILocalizationService localizationService,
        ILogger logger,
        IMultiFactorAuthenticationPluginManager multiFactorAuthenticationPluginManager,
        INewsLetterSubscriptionService newsLetterSubscriptionService,
        INotificationService notificationService,
        IOrderService orderService,
        IPermissionService permissionService,
        IPictureService pictureService,
        IPriceFormatter priceFormatter,
        IProductService productService,
        IStateProvinceService stateProvinceService,
        IStoreContext storeContext,
        ITaxService taxService,
        IWorkContext workContext,
        IWorkflowMessageService workflowMessageService,
        LocalizationSettings localizationSettings,
        MediaSettings mediaSettings,
        MultiFactorAuthenticationSettings multiFactorAuthenticationSettings,
        StoreInformationSettings storeInformationSettings,
        TaxSettings taxSettings
        )
    {
        _addressSettings = addressSettings;
        _captchaSettings = captchaSettings;
        _customerSettings = customerSettings;
        _dateTimeSettings = dateTimeSettings;
        _forumSettings = forumSettings;
        _gdprSettings = gdprSettings;
        _htmlEncoder = htmlEncoder;
        _addressModelFactory = addressModelFactory;
        _addressService = addressService;
        _addressAttributeParser = addressAttributeParser;
        _customerAttributeParser = customerAttributeParser;
        _customerAttributeService = customerAttributeService;
        _authenticationService = authenticationService;
        _countryService = countryService;
        _currencyService = currencyService;
        _customerActivityService = customerActivityService;
        _customerModelFactory = customerModelFactory;
        _customerRegistrationService = customerRegistrationService;
        _customerService = customerService;
        _downloadService = downloadService;
        _eventPublisher = eventPublisher;
        _exportManager = exportManager;
        _externalAuthenticationService = externalAuthenticationService;
        _gdprService = gdprService;
        _genericAttributeService = genericAttributeService;
        _giftCardService = giftCardService;
        _localizationService = localizationService;
        _logger = logger;
        _multiFactorAuthenticationPluginManager = multiFactorAuthenticationPluginManager;
        _newsLetterSubscriptionService = newsLetterSubscriptionService;
        _notificationService = notificationService;
        _orderService = orderService;
        _permissionService = permissionService;
        _pictureService = pictureService;
        _priceFormatter = priceFormatter;
        _productService = productService;
        _stateProvinceService = stateProvinceService;
        _storeContext = storeContext;
        _taxService = taxService;
        _workContext = workContext;
        _workflowMessageService = workflowMessageService;
        _localizationSettings = localizationSettings;
        _mediaSettings = mediaSettings;
        _multiFactorAuthenticationSettings = multiFactorAuthenticationSettings;
        _storeInformationSettings = storeInformationSettings;
        _taxSettings = taxSettings;
    }

    #region Utilites
    protected virtual async Task<string> ParseCustomCustomerAttributesAsync(IFormCollection form)
    {
        ArgumentNullException.ThrowIfNull(form);

        var attributesXml = "";
        var attributes = await _customerAttributeService.GetAllAttributesAsync();
        foreach (var attribute in attributes)
        {
            var controlId = $"{NopCustomerServicesDefaults.CustomerAttributePrefix}{attribute.Id}";
            switch (attribute.AttributeControlType)
            {
                case AttributeControlType.DropdownList:
                case AttributeControlType.RadioList:
                    {
                        var ctrlAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                        {
                            var selectedAttributeId = int.Parse(ctrlAttributes);
                            if (selectedAttributeId > 0)
                                attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                    attribute, selectedAttributeId.ToString());
                        }
                    }
                    break;
                case AttributeControlType.Checkboxes:
                    {
                        var cblAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(cblAttributes))
                        {
                            foreach (var item in cblAttributes.ToString().Split(_separator, StringSplitOptions.RemoveEmptyEntries))
                            {
                                var selectedAttributeId = int.Parse(item);
                                if (selectedAttributeId > 0)
                                    attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                        attribute, selectedAttributeId.ToString());
                            }
                        }
                    }
                    break;
                case AttributeControlType.ReadonlyCheckboxes:
                    {
                        //load read-only (already server-side selected) values
                        var attributeValues = await _customerAttributeService.GetAttributeValuesAsync(attribute.Id);
                        foreach (var selectedAttributeId in attributeValues
                                     .Where(v => v.IsPreSelected)
                                     .Select(v => v.Id)
                                     .ToList())
                        {
                            attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                attribute, selectedAttributeId.ToString());
                        }
                    }
                    break;
                case AttributeControlType.TextBox:
                case AttributeControlType.MultilineTextbox:
                    {
                        var ctrlAttributes = form[controlId];
                        if (!StringValues.IsNullOrEmpty(ctrlAttributes))
                        {
                            var enteredText = ctrlAttributes.ToString().Trim();
                            attributesXml = _customerAttributeParser.AddAttribute(attributesXml,
                                attribute, enteredText);
                        }
                    }
                    break;
                case AttributeControlType.Datepicker:
                case AttributeControlType.ColorSquares:
                case AttributeControlType.ImageSquares:
                case AttributeControlType.FileUpload:
                //not supported customer attributes
                default:
                    break;
            }
        }

        return attributesXml;
    }

    protected virtual void ValidateRequiredConsents(List<GdprConsent> consents, IFormCollection form)
    {
        foreach (var consent in consents)
        {
            var controlId = $"consent{consent.Id}";
            var cbConsent = form[controlId];
            if (StringValues.IsNullOrEmpty(cbConsent) || !cbConsent.ToString().Equals("on"))
            {
                ModelState.AddModelError("", consent.RequiredMessage);
            }
        }
    }

    protected virtual async Task LogGdprAsync(Customer customer, CustomerInfoModel oldCustomerInfoModel,
        CustomerInfoModel newCustomerInfoModel, IFormCollection form)
    {
        try
        {
            //consents
            var consents = (await _gdprService.GetAllConsentsAsync()).Where(consent => consent.DisplayOnCustomerInfoPage).ToList();
            foreach (var consent in consents)
            {
                var previousConsentValue = await _gdprService.IsConsentAcceptedAsync(consent.Id, customer.Id);
                var controlId = $"consent{consent.Id}";
                var cbConsent = form[controlId];
                if (!StringValues.IsNullOrEmpty(cbConsent) && cbConsent.ToString().Equals("on"))
                {
                    //agree
                    if (!previousConsentValue.HasValue || !previousConsentValue.Value)
                    {
                        await _gdprService.InsertLogAsync(customer, consent.Id, GdprRequestType.ConsentAgree, consent.Message);
                    }
                }
                else
                {
                    //disagree
                    if (!previousConsentValue.HasValue || previousConsentValue.Value)
                    {
                        await _gdprService.InsertLogAsync(customer, consent.Id, GdprRequestType.ConsentDisagree, consent.Message);
                    }
                }
            }

            //newsletter subscriptions
            if (_gdprSettings.LogNewsletterConsent)
            {
                if (oldCustomerInfoModel.Newsletter && !newCustomerInfoModel.Newsletter)
                    await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentDisagree, await _localizationService.GetResourceAsync("Gdpr.Consent.Newsletter"));
                if (!oldCustomerInfoModel.Newsletter && newCustomerInfoModel.Newsletter)
                    await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ConsentAgree, await _localizationService.GetResourceAsync("Gdpr.Consent.Newsletter"));
            }

            //user profile changes
            if (!_gdprSettings.LogUserProfileChanges)
                return;

            if (oldCustomerInfoModel.Gender != newCustomerInfoModel.Gender)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Gender")} = {newCustomerInfoModel.Gender}");

            if (oldCustomerInfoModel.FirstName != newCustomerInfoModel.FirstName)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.FirstName")} = {newCustomerInfoModel.FirstName}");

            if (oldCustomerInfoModel.LastName != newCustomerInfoModel.LastName)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.LastName")} = {newCustomerInfoModel.LastName}");

            if (oldCustomerInfoModel.ParseDateOfBirth() != newCustomerInfoModel.ParseDateOfBirth())
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.DateOfBirth")} = {newCustomerInfoModel.ParseDateOfBirth()}");

            if (oldCustomerInfoModel.Email != newCustomerInfoModel.Email)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Email")} = {newCustomerInfoModel.Email}");

            if (oldCustomerInfoModel.Company != newCustomerInfoModel.Company)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Company")} = {newCustomerInfoModel.Company}");

            if (oldCustomerInfoModel.StreetAddress != newCustomerInfoModel.StreetAddress)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.StreetAddress")} = {newCustomerInfoModel.StreetAddress}");

            if (oldCustomerInfoModel.StreetAddress2 != newCustomerInfoModel.StreetAddress2)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.StreetAddress2")} = {newCustomerInfoModel.StreetAddress2}");

            if (oldCustomerInfoModel.ZipPostalCode != newCustomerInfoModel.ZipPostalCode)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.ZipPostalCode")} = {newCustomerInfoModel.ZipPostalCode}");

            if (oldCustomerInfoModel.City != newCustomerInfoModel.City)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.City")} = {newCustomerInfoModel.City}");

            if (oldCustomerInfoModel.County != newCustomerInfoModel.County)
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.County")} = {newCustomerInfoModel.County}");

            if (oldCustomerInfoModel.CountryId != newCustomerInfoModel.CountryId)
            {
                var countryName = (await _countryService.GetCountryByIdAsync(newCustomerInfoModel.CountryId))?.Name;
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.Country")} = {countryName}");
            }

            if (oldCustomerInfoModel.StateProvinceId != newCustomerInfoModel.StateProvinceId)
            {
                var stateProvinceName = (await _stateProvinceService.GetStateProvinceByIdAsync(newCustomerInfoModel.StateProvinceId))?.Name;
                await _gdprService.InsertLogAsync(customer, 0, GdprRequestType.ProfileChanged, $"{await _localizationService.GetResourceAsync("Account.Fields.StateProvince")} = {stateProvinceName}");
            }
        }
        catch (Exception exception)
        {
            await _logger.ErrorAsync(exception.Message, exception, customer);
        }
    }

    #endregion

    #region Methods
    [HttpPost]
    public virtual async Task<IActionResult> Info(CustomerInfoModel model, IFormCollection form)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        var oldCustomerModel = new CustomerInfoModel();

        //get customer info model before changes for gdpr log
        if (_gdprSettings.GdprEnabled & _gdprSettings.LogUserProfileChanges)
            oldCustomerModel = await _customerModelFactory.PrepareCustomerInfoModelAsync(oldCustomerModel, customer, false);

        //custom customer attributes
        var customerAttributesXml = await ParseCustomCustomerAttributesAsync(form);
        var customerAttributeWarnings = await _customerAttributeParser.GetAttributeWarningsAsync(customerAttributesXml);
        foreach (var error in customerAttributeWarnings)
        {
            ModelState.AddModelError("", error);
        }

        //GDPR
        if (_gdprSettings.GdprEnabled)
        {
            var consents = (await _gdprService
                .GetAllConsentsAsync()).Where(consent => consent.DisplayOnCustomerInfoPage && consent.IsRequired).ToList();

            ValidateRequiredConsents(consents, form);
        }

        try
        {
            if (ModelState.IsValid)
            {
                //username 
                if (_customerSettings.UsernamesEnabled && _customerSettings.AllowUsersToChangeUsernames)
                {
                    var userName = model.Username;
                    if (!customer.Username.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        //change username
                        await _customerRegistrationService.SetUsernameAsync(customer, userName);

                        //re-authenticate
                        //do not authenticate users in impersonation mode
                        if (_workContext.OriginalCustomerIfImpersonated == null)
                            await _authenticationService.SignInAsync(customer, true);
                    }
                }
                //email
                var email = model.Email;
                if (!customer.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase))
                {
                    //change email
                    var requireValidation = _customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation;
                    await _customerRegistrationService.SetEmailAsync(customer, email, requireValidation);

                    //do not authenticate users in impersonation mode
                    if (_workContext.OriginalCustomerIfImpersonated == null)
                    {
                        //re-authenticate (if usernames are disabled)
                        if (!_customerSettings.UsernamesEnabled && !requireValidation)
                            await _authenticationService.SignInAsync(customer, true);
                    }
                }

                //properties
                if (_dateTimeSettings.AllowCustomersToSetTimeZone)
                    customer.TimeZoneId = model.TimeZoneId;
                //VAT number
                if (_taxSettings.EuVatEnabled)
                {
                    var prevVatNumber = customer.VatNumber;
                    customer.VatNumber = model.VatNumber;

                    if (prevVatNumber != model.VatNumber)
                    {
                        var (vatNumberStatus, _, vatAddress) = await _taxService.GetVatNumberStatusAsync(model.VatNumber);
                        customer.VatNumberStatusId = (int)vatNumberStatus;

                        //send VAT number admin notification
                        if (!string.IsNullOrEmpty(model.VatNumber) && _taxSettings.EuVatEmailAdminWhenNewVatSubmitted)
                            await _workflowMessageService.SendNewVatSubmittedStoreOwnerNotificationAsync(customer,
                                model.VatNumber, vatAddress, _localizationSettings.DefaultAdminLanguageId);
                    }
                }

                //form fields
                if (_customerSettings.GenderEnabled)
                    customer.Gender = model.Gender;
                if (_customerSettings.FirstNameEnabled)
                    customer.FirstName = model.FirstName;
                if (_customerSettings.LastNameEnabled)
                    customer.LastName = model.LastName;
                if (_customerSettings.DateOfBirthEnabled)
                    customer.DateOfBirth = model.ParseDateOfBirth();
                if (_customerSettings.CompanyEnabled)
                    customer.Company = model.Company;
                if (_customerSettings.StreetAddressEnabled)
                    customer.StreetAddress = model.StreetAddress;
                if (_customerSettings.StreetAddress2Enabled)
                    customer.StreetAddress2 = model.StreetAddress2;
                if (_customerSettings.ZipPostalCodeEnabled)
                    customer.ZipPostalCode = model.ZipPostalCode;
                if (_customerSettings.CityEnabled)
                    customer.City = model.City;
                if (_customerSettings.CountyEnabled)
                    customer.County = model.County;
                if (_customerSettings.CountryEnabled)
                    customer.CountryId = model.CountryId;
                if (_customerSettings.CountryEnabled && _customerSettings.StateProvinceEnabled)
                    customer.StateProvinceId = model.StateProvinceId;
                if (_customerSettings.PhoneEnabled)
                    customer.Phone = model.Phone;
                if (_customerSettings.FaxEnabled)
                    customer.Fax = model.Fax;

                customer.CustomCustomerAttributesXML = customerAttributesXml;
                await _customerService.UpdateCustomerAsync(customer);

                //After updating the customer table, we also update the address table

                //find address (ensure that it belongs to the current customer)
                var addresses = await _customerService.GetAddressesByCustomerIdAsync(customer.Id);
                if (addresses == null)
                    //address is not found
                    return RedirectToRoute("CustomerInfo");

                var address = addresses.First();
                address.FirstName = model.FirstName;
                address.LastName = model.LastName;

                if (ModelState.IsValid)
                {
                    await _addressService.UpdateAddressAsync(address);
                }

                //newsletter
                if (_customerSettings.NewsletterEnabled)
                {
                    //save newsletter value
                    var store = await _storeContext.GetCurrentStoreAsync();
                    var newsletter = await _newsLetterSubscriptionService.GetNewsLetterSubscriptionByEmailAndStoreIdAsync(customer.Email, store.Id);
                    if (newsletter != null)
                    {
                        if (model.Newsletter)
                        {
                            newsletter.Active = true;
                            await _newsLetterSubscriptionService.UpdateNewsLetterSubscriptionAsync(newsletter);
                        }
                        else
                        {
                            await _newsLetterSubscriptionService.DeleteNewsLetterSubscriptionAsync(newsletter);
                        }
                    }
                    else
                    {
                        if (model.Newsletter)
                        {
                            await _newsLetterSubscriptionService.InsertNewsLetterSubscriptionAsync(new NewsLetterSubscription
                            {
                                NewsLetterSubscriptionGuid = Guid.NewGuid(),
                                Email = customer.Email,
                                Active = true,
                                StoreId = store.Id,
                                LanguageId = customer.LanguageId ?? store.DefaultLanguageId,
                                CreatedOnUtc = DateTime.UtcNow
                            });
                        }
                    }
                }

                if (_forumSettings.ForumsEnabled && _forumSettings.SignaturesEnabled)
                    await _genericAttributeService.SaveAttributeAsync(customer, NopCustomerDefaults.SignatureAttribute, model.Signature);

                //GDPR
                if (_gdprSettings.GdprEnabled)
                    await LogGdprAsync(customer, oldCustomerModel, model, form);

                _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.CustomerInfo.Updated"));

                return RedirectToRoute("CustomerInfo");
            }
        }
        catch (Exception exc)
        {
            ModelState.AddModelError("", exc.Message);
        }

        //If we got this far, something failed, redisplay form
        model = await _customerModelFactory.PrepareCustomerInfoModelAsync(model, customer, true, customerAttributesXml);

        return View(model);
    }

    [HttpPost]
    public virtual async Task<IActionResult> AddressEdit(CustomerAddressEditModel model, IFormCollection form)
    {
        var customer = await _workContext.GetCurrentCustomerAsync();
        if (!await _customerService.IsRegisteredAsync(customer))
            return Challenge();

        //find address (ensure that it belongs to the current customer)
        var address = await _customerService.GetCustomerAddressAsync(customer.Id, model.Address.Id);
        if (address == null)
            //address is not found
            return RedirectToRoute("CustomerAddresses");

        //custom address attributes
        var customAttributes = await _addressAttributeParser.ParseCustomAttributesAsync(form, NopCommonDefaults.AddressAttributeControlName);
        var customAttributeWarnings = await _addressAttributeParser.GetAttributeWarningsAsync(customAttributes);
        foreach (var error in customAttributeWarnings)
        {
            ModelState.AddModelError("", error);
        }

        if (ModelState.IsValid)
        {
            //First update the address table
            address = model.Address.ToEntity(address);
            address.CustomAttributes = customAttributes;
            await _addressService.UpdateAddressAsync(address);

            //Then update the customer table
            customer.FirstName = model.Address.FirstName;
            customer.LastName = model.Address.LastName;
            await _customerService.UpdateCustomerAsync(customer);

            _notificationService.SuccessNotification(await _localizationService.GetResourceAsync("Account.CustomerAddresses.Updated"));

            return RedirectToRoute("CustomerAddresses");
        }

        //If we got this far, something failed, redisplay form
        await _addressModelFactory.PrepareAddressModelAsync(model.Address,
            address: address,
            excludeProperties: true,
            addressSettings: _addressSettings,
            loadCountries: async () => await _countryService.GetAllCountriesAsync((await _workContext.GetWorkingLanguageAsync()).Id),
            overrideAttributesXml: customAttributes);

        return View(model);
    }
    #endregion
}
