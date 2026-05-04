using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace sem2week1.Database.Entities;

[Table("Users")]
public class Users : IdentityUser
{
    [Required] public string FirstName { get; set; } = null;
    
    [Required]
    public string LastName { get; set; } =  null;
    
    // Profile picture stored as byte array (optional)
    public byte[]? ProfilePicture { get; set; }
    
    // Content type for the profile picture (e.g., "image/jpeg", "image/png")
    [StringLength(100)]
    public string? ProfilePictureContentType { get; set; }
}