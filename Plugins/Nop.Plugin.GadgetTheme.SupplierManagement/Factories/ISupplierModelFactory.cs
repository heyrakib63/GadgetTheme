using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Factories;
public interface ISupplierModelFactory
{
    Task<SupplierList> PrepareSupplierListModelAsync();
}
