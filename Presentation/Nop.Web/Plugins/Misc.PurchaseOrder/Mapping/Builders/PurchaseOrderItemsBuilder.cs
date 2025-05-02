using FluentMigrator.Builders.Create.Table;
using Nop.Data.Mapping.Builders;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;

namespace Nop.Plugin.Misc.PurchaseOrder.Mapping.Builders;
public class PurchaseOrderItemsBuilder : NopEntityBuilder<PurchaseOrderItems>
{
    public override void MapEntity(CreateTableExpressionBuilder table)
    {
        table.WithColumn(nameof(PurchaseOrderItems.Id)).AsInt32().PrimaryKey().Identity()
            .WithColumn(nameof(PurchaseOrders.PurchaseOrderNo)).AsGuid().NotNullable()
            .WithColumn(nameof(PurchaseOrderItems.ProductId)).AsInt32()
            .WithColumn(nameof(PurchaseOrderItems.UnitPrice)).AsDecimal()
            .WithColumn(nameof(PurchaseOrderItems.Quantity)).AsInt32()
            .WithColumn(nameof(PurchaseOrderItems.TotalCost)).AsDecimal();
    }
}
