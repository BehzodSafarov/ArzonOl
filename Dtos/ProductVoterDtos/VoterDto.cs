using System.ComponentModel.DataAnnotations;

namespace ArzonOL.Dtos.ProductVoterDtos;

public class VoterDto
{
    public int? Vote { get; set; }
    public string? Comment { get; set; }
    [Required]
    public Guid ProductId { get; set; }
    [Required]
    public Guid UserId { get; set; }
}