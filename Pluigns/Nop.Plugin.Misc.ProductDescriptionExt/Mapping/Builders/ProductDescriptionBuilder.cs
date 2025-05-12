using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Mapping.Builders;
public class ProductDescriptionBuilder : NopEntityBuilder<ProductDescription>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(ProductDescription.Id)).AsInt32().PrimaryKey().Identity()
        .WithColumn(nameof(ProductDescription.ProductId)).AsInt32()
        .WithColumn(nameof(ProductDescription.Description)).AsString();
    }
}
