using Microsoft.AspNetCore.Mvc;
using Tree.Services.Contracts;
using Tree.Services.Models;

namespace Trees.Controllers;

[ApiController]
[Route("[controller]")]
public class TreesNodesController : ControllerBase
{

    private readonly ITreesService _treesService;

    public TreesNodesController(ITreesService treesService)
    {
        _treesService = treesService;
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> Get(long id, CancellationToken cancellationToken = default)
    {
        return Ok(await _treesService.GetTreeNode(id, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Post(TreeNodeAddModel editModel, CancellationToken cancellationToken = default)
    {
        var result = await _treesService.CreateTreeNode(editModel, cancellationToken);
        return CreatedAtAction(nameof(Get), new { result.Id }, result);
    }

    [HttpPut("{id:long}")]
    public async Task Put(long id, [FromBody] TreeNodeEditModel model, CancellationToken cancellationToken = default)
    {
        await _treesService.UpdateTreeNode(id, model, cancellationToken);
    }

    [HttpDelete("{id:long}")]
    public async Task Delete(long id, CancellationToken cancellationToken = default)
    {
        await _treesService.DeleteTreeNode(id, cancellationToken);
    }
}
