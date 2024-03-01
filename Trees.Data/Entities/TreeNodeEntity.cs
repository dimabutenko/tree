namespace Trees.Data.Entities;

public class TreeNodeEntity
{
    public long Id { get; }
    public long? ParentId { get; set; }
    public string Name { get; set; }
    public virtual ICollection<TreeNodeEntity>? Children { get; } = new List<TreeNodeEntity>();
}
