using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
public record SupplierSearchModel : BaseSearchModel
{
    public SupplierSearchModel()
    {
        // Initialize any collections or properties if needed
    }
    [NopResourceDisplayName("Plugins.GadgetTheme.SupplierManagement.Fields.Name")]
    public string Name { get; set; }
    [NopResourceDisplayName("Plugins.GadgetTheme.SupplierManagement.Fields.Email")]
    public string Email { get; set; }
}
