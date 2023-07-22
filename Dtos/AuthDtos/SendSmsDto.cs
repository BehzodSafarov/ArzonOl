using System.ComponentModel.DataAnnotations;

namespace ArzonOL.Dtos.AuthDtos
{
    public class SendSmsDto
    {
        [Required]
        public string? PhoneNumber { get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public string? Password { get; set; }
    }
}