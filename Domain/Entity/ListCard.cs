using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Entity;

public class ListCard
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(45, MinimumLength = 2)]
    public string? Title { get; set; }

    public StatusItemEnum Status { get; set; } = StatusItemEnum.Active;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public Workspace? Workspace { get; set; }
    public ICollection<ListCard>? Cards { get; set; }
}
