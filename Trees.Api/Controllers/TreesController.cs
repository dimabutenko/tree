using Microsoft.AspNetCore.Mvc;
using Tree.Services.Contracts;

namespace Trees.Controllers;

[ApiController]
[Route("[controller]")]
public class TreesController : ControllerBase
{

    private readonly ITreesService _treesService;

    public TreesController(ITreesService treesService)
    {
        _treesService = treesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken = default)
    {
        return Ok(await _treesService.GetTrees(cancellationToken));
    }

    [HttpGet("{rootNodeId:long}")]
    public async Task<IActionResult> Get(long rootNodeId, CancellationToken cancellationToken = default)
    {
        return Ok(await _treesService.GetTreeByRoot(rootNodeId, cancellationToken));
    }
}
