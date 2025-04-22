using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping.Builders;
//using Nop.Plugin.Other.ProductViewTracker.Domains;
using Nop.Data.Extensions;
using System.Data;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Mapping.Builders;

public class ProductSupplierMappingBuilder : NopEntityBuilder<ProductSupplierMapping>
{

    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        //map the primary key (not necessary if it is Id field)
        table.WithColumn(nameof(ProductSupplierMapping.Id)).AsInt32().PrimaryKey().Identity()

        .WithColumn(nameof(ProductSupplierMapping.SupplierId)).AsInt32()
        .WithColumn(nameof(ProductSupplierMapping.ProductId)).AsInt32();
    }
}
