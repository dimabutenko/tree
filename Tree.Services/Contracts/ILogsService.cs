using Tree.Services.Models;

namespace Tree.Services.Contracts;

public interface ILogsService
{
    Task<PagedModel<LogViewModel>> GetLogs(int skip, int take, CancellationToken cancellationToken = default);
    Task<LogViewModel[]> GetLogsByTraceId(string traceId, CancellationToken cancellationToken = default);
    Task<LogViewModel> GetLogById(Guid id, CancellationToken cancellationToken = default);
    Task AddLog(LogAddModel addModel, CancellationToken cancellationToken = default);
}
