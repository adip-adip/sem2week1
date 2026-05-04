using sem2week1.DTOS;
using sem2week1.DTOS.Response;
using sem2week1.Database.Entities;


namespace sem2week1.Service.Interface;

public interface IAuthService
{
    Task<(bool Success, string Message, string? UserId)> RegisterUserAsync(RegisterDto registerDto);
    
    Task<(bool Success, string Message, string? UserId)> RegisterStudentAsync(RegisterStudentDto registerDto);
    
    Task<(bool Success, string Message, string? UserId)> RegisterInstructorAsync(RegisterInstructorDto registerDto);
    
    Task<(bool Success, string Message, string? UserId)> RegisterAdminAsync(RegisterAdminDto registerDto);
    
    Task<LoginResponse> LoginAsync(LoginDto loginDto);
}
