namespace sem2week1.DTOS;

public class LoginResponseDto
{
    public string Token { get; set; } = null;
    public string UserId { get; set; } = null;
    public string Email { get; set; } = null;
    public string FirstName { get; set; } = null;
    public string LastName { get; set; } = null;
    public string Role { get; set; } = null;
}