namespace TABP.DAL.Interfaces;

public interface IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}