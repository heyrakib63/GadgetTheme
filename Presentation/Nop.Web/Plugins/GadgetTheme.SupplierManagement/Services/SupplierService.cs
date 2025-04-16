using Nop.Core.Domain.Vendors;
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

    public virtual async Task<IPagedList<Supplier>> SearchSupplierAsync(string name = "", string email = "", int pageIndex = 0, int pageSize = int.MaxValue)
    {


        var query = from e in _supplierRepository.Table
                    select e;

        if (!string.IsNullOrEmpty(name))
            query = query.Where(e => e.SupplierName.Contains(name));
        if (!string.IsNullOrEmpty(email))
            query = query.Where(e => e.SupplierEmail == email);
        query = query.OrderBy(e => e.SupplierName);

        return await query.ToPagedListAsync(pageIndex, pageSize);



        //var suppliers = await _supplierRepository.GetAllPagedAsync(query =>
        //{
        //    query = query.OrderBy(v => v.SupplierName);
        //    return query;
        //}, pageIndex, pageSize);

        //return suppliers;
    }
}
//{
//    private readonly IAddressService _addressService;
//    private readonly ICountryService _countryService;
//    private readonly ICustomerService _customerService;

//    public CustomersByCountry(IAddressService addressService,
//        ICountryService countryService,
//        ICustomerService customerService)
//    {
//        _addressService = addressService;
//        _countryService = countryService;
//        _customerService = customerService;
//    }

//    public async Task<List<CustomersDistribution>> GetCustomersDistributionByCountryAsync()
//    {
//        return await _customerService.GetAllCustomersAsync()
//            .Where(c => c.ShippingAddressId != null)
//            .Select(c => new
//            {
//                await (_countryService.GetCountryByAddressAsync(_addressService.GetAddressById(c.ShippingAddressId ?? 0))).Name,
//                c.Username
//            })
//            .GroupBy(c => c.Name)
//            .Select(cbc => new CustomersDistribution { Country = cbc.Key, NoOfCustomers = cbc.Count() }).ToList();
//    }
//}