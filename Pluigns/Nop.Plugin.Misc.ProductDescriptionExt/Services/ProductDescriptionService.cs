using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nop.Data;
using Nop.Plugin.Misc.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Services;
public class ProductDescriptionService: IProductDescriptionService
{
    private readonly IRepository<ProductDescription> _repository;
    public ProductDescriptionService(IRepository<ProductDescription> repository)
    {
        _repository = repository;
    }
    public async Task InsertMappingAsync(int productId, string description)
    {
        var mapping = new ProductDescription
        {
            ProductId = productId,
            Description = description
        };
        await _repository.InsertAsync(mapping);
    }

    public async Task<string> GetExtraDescriptionByProductIdAsync(int productId)
    {
        if (productId <= 0)
            return string.Empty;

        var record = await _repository
            .Table
            .FirstOrDefaultAsync(x => x.ProductId == productId);

        return record?.Description ?? string.Empty;
    }
}
