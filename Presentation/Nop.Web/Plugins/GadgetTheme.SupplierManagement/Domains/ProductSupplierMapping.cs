// File: Domains/ProductSupplierMapping.cs
using Nop.Core;
using Nop.Core.Domain.Catalog;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Domains
{
    public class ProductSupplierMapping : BaseEntity
    {
        public int ProductId { get; set; }
        public int SupplierId { get; set; }

    }
}
