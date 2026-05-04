using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace sem2week1.Database.Entities;

[PrimaryKey((nameof(ModuleId)),nameof(InstructorId))]
public class ModuleInstructor
{
    
    [ForeignKey(nameof(Module))]
    public int ModuleId { get; set; }
    [JsonIgnore]
    public Module Module { get; set; } = null!;
    
    [ForeignKey(nameof(Instructor))]
    public int InstructorId { get; set; }
    [JsonIgnore]
    public Instructor Instructor { get; set; } = null!;
}
