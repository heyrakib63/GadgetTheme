using FluentMigrator.Builders.Create.Table;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Data.Mapping.Builders;
//using Nop.Plugin.Other.ProductViewTracker.Domains;
using Nop.Data.Extensions;
using System.Data;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Mapping.Builders;

public class SupplierBuilder : NopEntityBuilder<Supplier>
{
    /// <summary>
    /// Apply entity configuration
    /// </summary>
    /// <param name="table">Create table expression builder</param>
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        //map the primary key (not necessary if it is Id field)
        table.WithColumn(nameof(Supplier.Id)).AsInt32().PrimaryKey()

        //map the additional properties as foreign keys
        //.WithColumn(nameof(Supplier.SupplierId)).AsInt32().ForeignKey<Supplier>(onDelete: Rule.Cascade)

        //avoiding truncation/failure
        //so we set the same max length used in the product name
        .WithColumn(nameof(Supplier.SupplierName)).AsString(400)
        //not necessary if we don't specify any rules
        .WithColumn(nameof(Supplier.SupplierEmail)).AsString()
        .WithColumn(nameof(Supplier.SupplierAddress)).AsString();
    }
}