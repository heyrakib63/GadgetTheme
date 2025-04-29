using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Model;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Areas.Admin.Factories;
public interface ISupplierModelFactory
{
    Task<SupplierListModel> PrepareSupplierListModelAsync(SupplierSearchModel searchModel);
    Task<SupplierSearchModel> PrepareSupplierSearchModelAsync(SupplierSearchModel searchModel);
    Task<SupplierModel> PrepareSupplierModelAsync(SupplierModel model, Supplier supplier, bool excludeProperties = false);
}
