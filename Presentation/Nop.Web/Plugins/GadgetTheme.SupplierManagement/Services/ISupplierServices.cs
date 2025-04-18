using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Core;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Services;
public partial interface ISupplierServices
{

    Task InsertSupplierAsync(Supplier supplier);

    Task UpdateSupplierAsync(Supplier supplier);

    Task DeleteSupplierAsync(Supplier supplier);

    Task<Supplier> GetSupplierByIdAsync(int supplierId);
    Task<IPagedList<Supplier>> SearchSupplierAsync(string name = "", string email="",int pageIndex = -1, int pageSize = int.MaxValue);
}
