namespace TABP.DAL.Entities;

public class Image
{
    public Guid Id { get; set; }
    public string ImagePath { get; set; } = "";
    public string? Description { get; set; }
}