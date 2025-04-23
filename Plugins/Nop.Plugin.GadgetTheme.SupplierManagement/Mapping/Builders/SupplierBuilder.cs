using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Mapping.Builders;

public class SupplierBuilder : NopEntityBuilder<Supplier>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(Supplier.Id)).AsInt32().PrimaryKey().Identity()
        .WithColumn(nameof(Supplier.SupplierName)).AsString(400)
        .WithColumn(nameof(Supplier.SupplierEmail)).AsString()
        .WithColumn(nameof(Supplier.SupplierAddress)).AsString();
    }
}