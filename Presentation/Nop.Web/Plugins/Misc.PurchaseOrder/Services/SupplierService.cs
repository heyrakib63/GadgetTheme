using Nop.Core;
using Nop.Data;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;
using Nop.Plugin.GadgetTheme.SupplierManagement.Services;
using Nop.Services.Html;
public class SupplierService : ISupplierServices
{
    private readonly IRepository<Supplier> _supplierRepository;
    protected readonly IHtmlFormatter _htmlFormatter;
    public SupplierService(IRepository<Supplier> supplierRepository, IHtmlFormatter htmlFormatter)
    {
        _supplierRepository = supplierRepository;
        _htmlFormatter = htmlFormatter;
    }
    public virtual async Task<IList<Supplier>> GetAllSupplierAsync()
    {
        var suppliers = await _supplierRepository.GetAllAsync(query =>
            query.OrderBy(s => s.SupplierName)); // Optional: sort by name
        return suppliers;
    }
    public virtual async Task<IPagedList<Supplier>> SearchSupplierAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue)
    {
        var query = from e in _supplierRepository.Table
                    select e;
        if (!string.IsNullOrEmpty(name))
            query = query.Where(e => e.SupplierName.Contains(name));
        if (!string.IsNullOrEmpty(email))
            query = query.Where(e => e.SupplierEmail.Contains(email));
        query = query.OrderBy(e => e.SupplierName);
        return await query.ToPagedListAsync(pageIndex, pageSize);
    }
    public virtual async Task<Supplier> GetSupplierByIdAsync(int supplierId)
    {
        return await _supplierRepository.GetByIdAsync(supplierId);
    }
    public virtual async Task InsertSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.InsertAsync(supplier);
    }
    public virtual async Task UpdateSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.UpdateAsync(supplier);
    }
    public virtual async Task DeleteSupplierAsync(Supplier supplier)
    {
        await _supplierRepository.DeleteAsync(supplier);
    }
}


