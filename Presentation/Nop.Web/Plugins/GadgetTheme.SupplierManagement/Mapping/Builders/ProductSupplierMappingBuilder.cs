using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Mapping.Builders;

public class ProductSupplierMappingBuilder : NopEntityBuilder<ProductSupplierMapping>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(ProductSupplierMapping.Id)).AsInt32().PrimaryKey().Identity()
        .WithColumn(nameof(ProductSupplierMapping.SupplierId)).AsInt32()
        .WithColumn(nameof(ProductSupplierMapping.ProductId)).AsInt32();
    }
}
