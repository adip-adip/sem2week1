using System.ComponentModel.DataAnnotations;

namespace sem2week1.DTOS;

public class RegisterInstructorDto
{
    [Required]
    public string FirstName { get; set; } = null;
    
    [Required]
    public string LastName { get; set; } = null;
    
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null;
    
    [Required]
    public string Password { get; set; } = null;
    
    public string? Department { get; set; }
    
    public string? Specialization { get; set; }
}