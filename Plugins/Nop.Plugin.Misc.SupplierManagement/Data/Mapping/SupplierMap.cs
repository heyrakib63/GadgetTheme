using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nop.Data.Mapping;
using Nop.Plugin.Misc.SupplierManagement.Domain;

namespace Nop.Plugin.Misc.SupplierManagement.Data.Mapping
{
    public class SupplierMap : IEntityTypeConfiguration<Supplier> // Replace NopEntityTypeConfiguration with IEntityTypeConfiguration
    {
        public void Configure(EntityTypeBuilder<Supplier> builder) // Update method signature to match IEntityTypeConfiguration
        {
            builder.ToTable(nameof(Supplier)); // Ensure the Microsoft.EntityFrameworkCore namespace is referenced for this method
            builder.HasKey(s => s.Id);

            builder.Property(s => s.Name).HasMaxLength(400).IsRequired();
            builder
                .Property(s => s.Email)
                .HasMaxLength(400);
            builder.Property(s => s.MetaKeywords).HasMaxLength(400);
            builder.Property(s => s.MetaDescription).HasMaxLength(4000);
            builder.Property(s => s.MetaTitle).HasMaxLength(400);
            builder.Property(s => s.Phone).HasMaxLength(100);
            builder.Property(s => s.ContactName).HasMaxLength(400);
        }
    }
}