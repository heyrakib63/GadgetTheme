using FluentMigrator;
using FluentMigrator.Infrastructure;
using Nop.Data.Extensions;
using Nop.Data.Migrations;
using Nop.Plugin.Widget.ProductDescriptionExt.Domain;

namespace Nop.Plugin.Misc.ProductDescriptionExt.Migrations;
[NopSchemaMigration("2025/05/25 07:35:50:1687541", "Widget.ProductDescriptionExt base schema", MigrationProcessType.Installation)]
public class SchemaMigration : AutoReversingMigration
{
    public override void Up()
    {  
        var productDescriptionTable = nameof(ProductDescription);
        if (!Schema.Table(productDescriptionTable).Exists())
            Create.TableFor<ProductDescription>();
    }
}