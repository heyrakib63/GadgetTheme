using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Domains;

namespace Nop.Plugin.Misc.PurchaseOrder.Mapping.Builders;
public class PurchaseOrdersBuilder : NopEntityBuilder<PurchaseOrders>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(PurchaseOrders.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(PurchaseOrders.SupplierId)).AsInt32()
            .WithColumn(nameof(PurchaseOrders.CreatedOnUtc)).AsDateTime()
            .WithColumn(nameof(PurchaseOrders.TotalCost)).AsDecimal();
    }
}