using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Trees.Data.Entities;

namespace Trees.Data.Configurations;

public class TreeNodeEntityConfiguration : IEntityTypeConfiguration<TreeNodeEntity>
{
    public void Configure(EntityTypeBuilder<TreeNodeEntity> builder)
    {
        builder.ToTable("TreeNodes").HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedOnAdd();
        builder.Property(c => c.Name).HasMaxLength(maxLength: 255);

        builder.HasOne<TreeNodeEntity>()
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
