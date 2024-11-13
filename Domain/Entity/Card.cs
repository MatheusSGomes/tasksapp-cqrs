using System.ComponentModel.DataAnnotations;
using Domain.Enum;

namespace Domain.Entity;

public class Card
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    [StringLength(45, MinimumLength = 2)]
    public string? Title { get; set; }

    [StringLength(120)]
    public string? Description { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;
    public DateTime? Deadline { get; set; }
    public ListCard? List { get; set; }
    public StatusCardEnum Status { get; set; }
}
