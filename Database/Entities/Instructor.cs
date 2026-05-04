using System.ComponentModel.DataAnnotations;

namespace sem2week1.Database.Entities;

public class Instructor
{
    [Key]
    public int Id { get; set; }
    [StringLength(50)]
    public string FirstName { get; set; } = null!;
    [StringLength(50)]
    public string LastName { get; set; } = null!;
    [StringLength(100)]
    public string Email { get; set; } = null!;
    public DateTime HireDate { get; set; }
    
    // Navigation property for many-to-many relationship with Module through join table
    public ICollection<ModuleInstructor> ModuleInstructors { get; set; } = new List<ModuleInstructor>();
}
