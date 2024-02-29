using Microsoft.EntityFrameworkCore;
using Tree.Services.Contracts;
using Tree.Services.Exceptions;
using Tree.Services.Models;
using Trees.Data;
using Trees.Data.Entities;

namespace Tree.Services;

public class TreesService : ITreesService
{
    private readonly IDbContextFactory<TreesContext> _contextFactory;

    public TreesService(IDbContextFactory<TreesContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<TreeNodeViewModel[]> GetTrees(CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var nodes = await context.TreeNodes
            .AsNoTracking()
            .ToArrayAsync(cancellationToken);

        return ConvertToHierarchy(nodes, x => x.ParentId == null);
    }

    public async Task<TreeNodeViewModel[]> GetTreeByRoot(long rootNodeId, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var nodes = await context.TreeNodes.FromSqlRaw("""
                                     WITH RECURSIVE nodes AS
                                     (
                                         SELECT "Id", "Name", "ParentId"
                                         FROM "TreeNodes"
                                         WHERE "Id" = {0}
                                         UNION
                                         SELECT tn."Id", tn."Name", tn."ParentId"
                                         FROM "TreeNodes" AS tn
                                         INNER JOIN nodes AS n ON n."Id" = tn."ParentId"
                                     )
                                     SELECT * FROM nodes;
                                     """, rootNodeId).ToArrayAsync(cancellationToken);

        return ConvertToHierarchy(nodes, x => x.Id == rootNodeId);
    }

    public async Task<TreeNodeViewModel> GetTreeNode(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var result = await context.TreeNodes
                .AsNoTracking()
                .Where(x => x.Id == id)
                .Include(x => x.Children)
                .SingleAsync(cancellationToken);

            return ConvertToModel(result);
        }
        catch (InvalidOperationException exception)
        {
            throw new TreeNodeException($"Node with Id = {id} was not found", exception);
        }
    }

    public async Task<TreeNodeViewModel> CreateTreeNode(TreeNodeAddModel editModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(editModel);

        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var entity = new TreeNodeEntity { Name = editModel.Name, ParentId = editModel.ParentId };
            context.TreeNodes.Add(entity);
            await context.SaveChangesAsync(cancellationToken);

            return new TreeNodeViewModel { Id = entity.Id, ParentId = entity.ParentId, Name = entity.Name };
        }
        catch (DbUpdateException exception)
        {
            throw new DatabaseUpdateException("An error is encountered while inserting new node to the database", exception);
        }
    }

    public async Task UpdateTreeNode(long id, TreeNodeEditModel editModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(editModel);

        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);
            var entity = await context.TreeNodes.SingleAsync(x => x.Id == id, cancellationToken);
            entity.Name = editModel.Name;
            context.TreeNodes.Update(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            throw new TreeNodeException($"Node with Id = {id} was not found", exception);
        }
        catch (DbUpdateException exception)
        {
            throw new DatabaseUpdateException($"An error is encountered while updating node with Id = {id} in the database", exception);
        }
    }

    public async Task DeleteTreeNode(long id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            if (await context.TreeNodes.AnyAsync(x => x.ParentId == id, cancellationToken: cancellationToken))
            {
                throw new TreeNodeException($"You have to delete all children nodes first for the parent node with Id = {id}");
            }

            var entity = await context.TreeNodes.SingleAsync(x => x.Id == id, cancellationToken);
            context.TreeNodes.Remove(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            throw new TreeNodeException($"Node with Id = {id} was not found", exception);
        }
        catch (DbUpdateException exception)
        {
            throw new DatabaseUpdateException($"An error is encountered while deleting the node with Id = {id} from the database", exception);
        }
    }

    private static TreeNodeViewModel[] ConvertToHierarchy(TreeNodeEntity[] items, Func<TreeNodeEntity, bool> condition)
    {
        return items
            .Where(condition.Invoke)
            .Select(x => ToTreeViewModel(x, items))
            .ToArray();

        TreeNodeViewModel ToTreeViewModel(TreeNodeEntity item, TreeNodeEntity[] list)
        {
            return new TreeNodeViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Children = list
                    .Where(x => x.ParentId == item.Id)
                    .Select(x => ToTreeViewModel(x, list))
                    .ToArray()
            };
        }
    }

    private static TreeNodeViewModel ConvertToModel(TreeNodeEntity item)
    {
        return new TreeNodeViewModel
        {
            Id = item.Id,
            ParentId = item.ParentId,
            Name = item.Name,
            Children = item.Children?.Select(ConvertToModel).ToArray()
        };
    }
}
