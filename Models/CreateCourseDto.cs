using System.ComponentModel.DataAnnotations;

namespace sem2week1.Models;

public class CreateModuleDto
{
    [Required]
    [StringLength(100)]
    public string Title { get; set; } = null!;
    
    [Required]
    public int Credits { get; set; }
}

public class CreateCourseDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = null!;
    
    [Required]
    public int DurationYear { get; set; }
    
    public List<CreateModuleDto>? Modules { get; set; }
}
