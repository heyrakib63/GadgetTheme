using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core.Domain.Vendors;
using Nop.Web.Areas.Admin.Models.Vendors;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
public record SupplierModel : BaseNopEntityModel, ILocalizedModel<SupplierLocalizedModel>
{
    public SupplierModel()
    {
        Locales = new List<SupplierLocalizedModel>();
    }
    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Address")]
    public string Address { get; set; }
    public IList<SupplierLocalizedModel> Locales { get; set; }
}

public partial record SupplierLocalizedModel : ILocalizedLocaleModel
{

    public int LanguageId { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Name")]
    public string Name { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Address")]
    public string Address { get; set; }

    [NopResourceDisplayName("Admin.Suppliers.Fields.Email")]
    public string Email { get; set; }
}
