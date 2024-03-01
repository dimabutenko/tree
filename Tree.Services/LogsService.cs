using Microsoft.EntityFrameworkCore;
using Tree.Services.Contracts;
using Tree.Services.Exceptions;
using Tree.Services.Models;
using Trees.Data;
using Trees.Data.Entities;

namespace Tree.Services;

public class LogsService : ILogsService
{
    private readonly IDbContextFactory<TreesContext> _contextFactory;

    public LogsService(IDbContextFactory<TreesContext> contextFactory)
    {
        _contextFactory = contextFactory;
    }

    public async Task<PagedModel<LogViewModel>> GetLogs(int skip, int take, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var result = await context.Logs
            .AsNoTracking()
            .Skip(skip)
            .Take(take)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new LogViewModel { Id = x.Id, TraceId = x.TraceId, Text = x.Text, CreatedAt  = x.CreatedAt })
            .ToArrayAsync(cancellationToken);

        return new PagedModel<LogViewModel> { Skip = skip, Take = take, Items = result };
    }

    public async Task<LogViewModel[]> GetLogsByTraceId(string traceId, CancellationToken cancellationToken = default)
    {
        await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Logs
            .AsNoTracking()
            .Where(x => x.TraceId == traceId)
            .Select(x => new LogViewModel { Id = x.Id, TraceId = x.TraceId, Text = x.Text, CreatedAt  = x.CreatedAt })
            .ToArrayAsync(cancellationToken);
    }

    public async Task<LogViewModel> GetLogById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            return await context.Logs
                .AsNoTracking()
                .Select(x => new LogViewModel { Id = x.Id, TraceId = x.TraceId, Text = x.Text, CreatedAt  = x.CreatedAt })
                .SingleAsync(x => x.Id == id, cancellationToken);
        }
        catch (InvalidOperationException exception)
        {
            throw new SecureException($"Log with Id = {id} was not found", exception);
        }
    }

    public async Task AddLog(LogAddModel addModel, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(addModel);

        try
        {
            await using var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

            var entity = new LogEntity { TraceId = addModel.TraceId, Text = addModel.Text };
            context.Logs.Add(entity);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException exception)
        {
            throw new SecureException("An error is encountered while inserting new node to the database", exception);
        }
    }
}
