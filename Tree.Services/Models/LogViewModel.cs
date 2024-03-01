namespace Tree.Services.Models;

public class LogViewModel : LogAddModel
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
}
