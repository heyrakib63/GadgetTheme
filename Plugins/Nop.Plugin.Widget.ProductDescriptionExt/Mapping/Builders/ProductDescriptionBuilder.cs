using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Widget.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Widget.ProductDescriptionExt.Mapping.Builders;
public class ProductDescriptionBuilder : NopEntityBuilder<ProductDescription>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(ProductDescription.Id)).AsInt32().PrimaryKey().Identity()
        .WithColumn(nameof(ProductDescription.ProductId)).AsInt32().NotNullable().ForeignKey("Product","Id")
        .WithColumn(nameof(ProductDescription.Description)).AsString(400).Nullable();
    }
}