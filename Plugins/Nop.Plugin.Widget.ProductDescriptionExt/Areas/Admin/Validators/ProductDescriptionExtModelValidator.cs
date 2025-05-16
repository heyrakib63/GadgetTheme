using FluentValidation;
using Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Models;
using Nop.Plugin.Widget.ProductDescriptionExt.Domain;
using Nop.Services.Localization;
using Nop.Web.Framework.Validators;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Areas.Admin.Validators;
public partial class ProductDescriptionExtModelValidator : BaseNopValidator<ProductDescriptionExtModel>
{
    public ProductDescriptionExtModelValidator(ILocalizationService localizationService)
    {
        RuleFor(x => x.Description)
            .MinimumLength(15).WithMessageAwait(localizationService.GetResourceAsync("Admin.ProductDescriptionExt.Fields.Description.MinLengthValidation"))
            .MaximumLength(400).WithMessageAwait(localizationService.GetResourceAsync("Admin.ProductDescriptionExt.Fields.Description.MaxLengthValidation"));
        SetDatabaseValidationRules<ProductDescription>();
    }
}
