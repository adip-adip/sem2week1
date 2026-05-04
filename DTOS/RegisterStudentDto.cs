using System;
using System.ComponentModel.DataAnnotations;

namespace sem2week1.DTOS;

public class RegisterStudentDto
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
    
    [Required]
    public string Address { get; set; } = null;
    
    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }
    
    [Required]
    [StringLength(20)]
    public string Phone { get; set; } = null;
}