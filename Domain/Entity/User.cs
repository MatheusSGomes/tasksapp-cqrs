using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entity;

[Table("Users")]
public class User
{
    [Key]
    [Required]
    public Guid Id { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    // [Column("nome", TypeName = "nvarchar(50)")]
    public string? Name { get; set; }

    [Required]
    [StringLength(50, MinimumLength = 3)]
    // [Column("sobrenome", TypeName = "nvarchar(50)")]
    public string? Surname { get; set; }

    [Required]
    [EmailAddress]
    public string? Email { get; set; }

    [Required]
    public string? PasswordHash { get; set; }

    [Required]
    // [Column("usuario")]
    public string? Username { get; set; }

    public ICollection<Workspace>? Workspaces { get; set; }
    public string? RefreshToken { get; set; }
    public string? RefreshTokenExpirationTime { get; set; }
}
