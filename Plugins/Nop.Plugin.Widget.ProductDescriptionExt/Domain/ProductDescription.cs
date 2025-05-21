using Nop.Core;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Domain;
public class ProductDescription : BaseEntity
{
    public int ProductId { get; set; }
    public string Description { get; set; }
}