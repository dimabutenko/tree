using System.ComponentModel.DataAnnotations;

namespace Tree.Services.Models;

public class TreeNodeEditModel
{
    [Required] public required string Name { get; init; }
}
