using System.ComponentModel.DataAnnotations;

namespace ArzonOL.Dtos.AuthDtos;

public class SmsReciverDto
{
    [Required]
    public string? PhoneNumber {get; set;}
    [Required]
    public string? Code {get; set;}
}