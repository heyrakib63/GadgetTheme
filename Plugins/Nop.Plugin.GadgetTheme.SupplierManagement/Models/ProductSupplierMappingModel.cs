using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Nop.Web.Framework.Models;
using Nop.Web.Framework.Mvc.ModelBinding;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Models;
public record ProductSupplierMappingModel : BaseNopEntityModel
{

    public ProductSupplierMappingModel()
    {
        AvailableSuppliers = new List<SelectListItem>();
    }
    [NopResourceDisplayName("Admin.Suppliers.Fields.ProductId")]
    public int ProductId { get; set; }
    [NopResourceDisplayName("Admin.Suppliers.Fields.SupplierId")]
    public int SelectedSupplierId { get; set; }
    public IList<SelectListItem> AvailableSuppliers { get; set; }
}
