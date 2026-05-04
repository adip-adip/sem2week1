using System.ComponentModel.DataAnnotations;

namespace sem2week1.DTOS;

public class CreateInstructorDTO
{
    [Required]
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [StringLength(100)]
    public string Email { get; set; } = null!;
    
    [Required]
    public DateTime HireDate { get; set; }
}
