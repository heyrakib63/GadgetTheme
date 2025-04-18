using ExCSS;
using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.GadgetTheme.SupplierManagement.Domains;

namespace Nop.Plugin.GadgetTheme.SupplierManagement.Migrations;

[NopSchemaMigration("2025/05/11 07:35:50:1687541", "GadgetTheme.SupplierManagement sixth schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<Supplier>();
    }
}
