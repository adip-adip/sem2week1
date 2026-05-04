using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace sem2week1.Database.Entities;

public class Module
{
    [Key]
    public int Id { get; set; }
    [StringLength(100)]
    public string Title { get; set; } = null!;
    public int Credits { get; set; }
    
    [ForeignKey(nameof(Course))]
    public int CourseId { get; set; }
    [JsonIgnore]
    public Course? Course { get; set; }
    
    // Use explicit join table for many-to-many relationship with Instructor
    public ICollection<ModuleInstructor> ModuleInstructors { get; set; } = new List<ModuleInstructor>();
}
