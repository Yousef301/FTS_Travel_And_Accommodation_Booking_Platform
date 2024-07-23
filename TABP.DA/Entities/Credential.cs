using TABP.DAL.Interfaces;

namespace TABP.DAL.Entities;

public class Credential : IAuditableEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Username { get; set; } = "";
    public string HashedPassword { get; set; } = "";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public DateTime? ModifiedAt { get; set; }
    public User User { get; set; }
}