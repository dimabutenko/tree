namespace Tree.Services.Models;

public class TreeNodeViewModel : TreeNodeAddModel
{
    public long Id { get; init; }
    public TreeNodeViewModel[]? Children { get; set; }
}
