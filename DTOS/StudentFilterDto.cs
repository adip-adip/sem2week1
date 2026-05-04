using System.ComponentModel.DataAnnotations;

namespace sem2week1.DTOS;

public class StudentFilterDto
{
    [Range(0, 120)]
    public int? MinAge { get; set; }
    
    [Range(0, 120)]
    public int? MaxAge { get; set; }
    
    [StringLength(200)]
    public string? Address { get; set; }
}
