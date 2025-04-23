namespace z_workshop_server.DAL.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Role { get; set; } = null!;

    public string? Avatar { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual Customer? Customer { get; set; }

    public virtual Employee? Employee { get; set; }
}
