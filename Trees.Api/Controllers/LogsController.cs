using Microsoft.AspNetCore.Mvc;
using Tree.Services.Contracts;

namespace Trees.Controllers;

[ApiController]
[Route("[controller]")]
public class LogsController : ControllerBase
{

    private readonly ILogsService _treesService;

    public LogsController(ILogsService treesService)
    {
        _treesService = treesService;
    }

    [HttpGet("skip={skip:int}&take={take:int}")]
    public async Task<IActionResult> GetAll(int skip, int take, CancellationToken cancellationToken = default)
    {
        return Ok(await _treesService.GetLogs(skip, take, cancellationToken));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        return Ok(await _treesService.GetLogById(id, cancellationToken));
    }

    [HttpGet("{traceId}")]
    public async Task<IActionResult> Get(string traceId, CancellationToken cancellationToken = default)
    {
        return Ok(await _treesService.GetLogsByTraceId(traceId, cancellationToken));
    }
}
