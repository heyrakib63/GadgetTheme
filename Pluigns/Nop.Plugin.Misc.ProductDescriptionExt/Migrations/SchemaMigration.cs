using FluentMigrator;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Misc.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Migrations;
[NopSchemaMigration("2025/05/22 07:35:50:1687541", "Misc.ProductDescriptionExt second schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {
        Create.TableFor<ProductDescription>();
    }
}
