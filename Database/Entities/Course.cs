using System.ComponentModel.DataAnnotations;

namespace sem2week1.Database.Entities;

public class Course
{
    public int Id  { get; set; }
    [StringLength(50)]
    public string Name { get; set; } = null!;
    public int DurationYear { get; set; }
    
    public List<Module>? Modules { get; set; } 
}
