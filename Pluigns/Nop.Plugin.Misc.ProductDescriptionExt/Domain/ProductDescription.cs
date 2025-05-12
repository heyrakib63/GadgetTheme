using Nop.Core;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Domain;
public class ProductDescription: BaseEntity
{
    public int ProductId { get; set; }
    public string Description { get; set; }
}
