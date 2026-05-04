namespace sem2week1.DTOS;

public class StudentResponseDto
{
    public int Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public int Age { get; set; }
    public string Address { get; set; } = null!;
    public string Email { get; set; } = null!;
}
