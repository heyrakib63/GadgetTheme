using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Validators;
public partial class SupplierValidator: BaseNopValidator<SupplierModel>
{
    public SupplierValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Suppliers.Fields.Name.Required"));
        RuleFor(x => x.Address).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Suppliers.Fields.Address.Required"));
        RuleFor(x => x.Email).NotEmpty().WithMessageAwait(localizationService.GetResourceAsync("Admin.Suppliers.Fields.Email.Required"));
        RuleFor(x => x.Email)
            .IsEmailAddress()
            .WithMessageAwait(localizationService.GetResourceAsync("Admin.Common.WrongEmail"));

        SetDatabaseValidationRules<Supplier>();
    }
}
