using Nop.Core;
using Nop.Web.Framework.Models;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

public class Supplier : BaseEntity
{
    public int SupplierId { get; set; }
    public string SupplierName { get; set; }
    public string SupplierEmail { get; set; }
    public string SupplierAddress { get; set; }
    //public string ImageUrl { get; set; }
}