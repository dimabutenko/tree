namespace Tree.Services.Models;

public class PagedModel<TModel>
{
    public int Skip { get; set; }
    public int Take { get; set; }
    public TModel[] Items { get; set; }
}
