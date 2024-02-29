using Microsoft.EntityFrameworkCore;
using Trees.Data.Configurations;
using Trees.Data.Entities;

namespace Trees.Data;

public class TreesContext : DbContext
{
    public TreesContext(DbContextOptions options) : base(options) { }

    public DbSet<TreeNodeEntity> TreeNodes { get; init; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new TreeNodeEntityConfiguration());
    }
}
