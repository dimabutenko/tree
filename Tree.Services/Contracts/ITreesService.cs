using Tree.Services.Models;

namespace Tree.Services.Contracts;

public interface ITreesService
{
    Task<TreeNodeViewModel[]> GetTrees(CancellationToken cancellationToken = default);
    Task<TreeNodeViewModel[]> GetTreeByRoot(long rootNodeId, CancellationToken cancellationToken = default);
    Task<TreeNodeViewModel> GetTreeNode(long id, CancellationToken cancellationToken = default);
    Task<TreeNodeViewModel> CreateTreeNode(TreeNodeAddModel editModel, CancellationToken cancellationToken = default);
    Task UpdateTreeNode(long id, TreeNodeEditModel editModel, CancellationToken cancellationToken = default);
    Task DeleteTreeNode(long id, CancellationToken cancellationToken = default);
}
