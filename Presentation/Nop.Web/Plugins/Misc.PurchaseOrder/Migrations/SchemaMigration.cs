using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.PurchaseOrder.Areas.Admin.Domains;

namespace Nop.Plugin.Misc.PurchaseOrder.Migrations;
[NopSchemaMigration("2025/04/28 07:35:50:1687541", "Misc.PurchaseOrder base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<PurchaseOrders>();
        Create.TableFor<PurchaseOrderItems>();
    }
}
