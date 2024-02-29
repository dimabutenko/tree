namespace Tree.Services.Models;

public class TreeNodeAddModel : TreeNodeEditModel
{
    public long? ParentId { get; init; }
}
