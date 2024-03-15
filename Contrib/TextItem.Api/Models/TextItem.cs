namespace RecAll.Contrib.TextItem.Api.Models;

public class TextItem
{
    public int Id { get; set; }
    public string? ItemId { get; set; }
    public string Content { get; set; }
    public string UserIdentityGuid { get; set; }
    public bool IsDeleted { get; set; }
}

