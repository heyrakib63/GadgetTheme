using Nop.Core;
using Nop.Core.Domain.Localization;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

public class Supplier : BaseEntity, ILocalizedEntity
{
    public string SupplierName { get; set; }
    public string SupplierEmail { get; set; }
    public string SupplierAddress { get; set; }
}
