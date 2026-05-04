using System.ComponentModel.DataAnnotations;

namespace sem2week1.DTOS;

public class LoginDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null;
    
    [Required]
    public string Password { get; set; } = null;
}