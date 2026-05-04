using Microsoft.AspNetCore.Identity;
using sem2week1.Database;
using sem2week1.DTOS;
using sem2week1.DTOS.Response;
using sem2week1.Database.Entities;
using sem2week1.Service;
using sem2week1.Service.Interface;

namespace sem2week1.Service.Implementation;

public class AuthService : IAuthService
{
    private readonly UserManager<Users> _userManager;
    private readonly SignInManager<Users> _signInManager;
    private readonly IJWTService _jwtService;
    private readonly AppDbContext _dbContext;

    public AuthService(UserManager<Users> userManager, SignInManager<Users> signInManager, IJWTService jwtService, AppDbContext dbContext)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtService = jwtService;
        _dbContext = dbContext;
    }

    public async Task<(bool Success, string Message, string? UserId)> RegisterUserAsync(RegisterDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return (false, "User with this email already exists", null);
        }

        var user = new Users
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var result = await _userManager.CreateAsync(user, registerDto.Password);

        if (result.Succeeded)
        {
            return (true, "User registered successfully", user.Id);
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return (false, errors, null);
    }
    
    public async Task<(bool Success, string Message, string? UserId)> RegisterStudentAsync(RegisterStudentDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return (false, "User with this email already exists", null);
        }

        // Start transaction to ensure atomic user creation + student profile + role assignment
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var user = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            // Assign Student role
            var roleResult = await _userManager.AddToRoleAsync(user, "Student");
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return (false, roleErrors, null);
            }

            // Create Student profile with additional fields
            var student = new sem2week1.Database.Entities.Student
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                Address = registerDto.Address,
                DateOfBirth = registerDto.DateOfBirth,
                Phone = registerDto.Phone,
                UserId = user.Id
            };

            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();

            // Commit transaction only if all operations succeed
            await transaction.CommitAsync();
            return (true, "Student registered successfully", user.Id);
        }
        catch (Exception)
        {
            // Rollback on any exception
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<(bool Success, string Message, string? UserId)> RegisterInstructorAsync(RegisterInstructorDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return (false, "User with this email already exists", null);
        }

        // Start transaction to ensure atomic user creation + role assignment
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var user = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            // Assign Instructor role
            var roleResult = await _userManager.AddToRoleAsync(user, "Instructor");
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return (false, roleErrors, null);
            }

            // Commit transaction only if both operations succeed
            await transaction.CommitAsync();
            return (true, "Instructor registered successfully", user.Id);
        }
        catch (Exception)
        {
            // Rollback on any exception
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<(bool Success, string Message, string? UserId)> RegisterAdminAsync(RegisterAdminDto registerDto)
    {
        var existingUser = await _userManager.FindByEmailAsync(registerDto.Email);
        if (existingUser != null)
        {
            return (false, "User with this email already exists", null);
        }

        // Start transaction to ensure atomic user creation + role assignment
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();
        
        try
        {
            var user = new Users
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.Password);

            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return (false, errors, null);
            }

            // Assign Admin role
            var roleResult = await _userManager.AddToRoleAsync(user, "Admin");
            if (!roleResult.Succeeded)
            {
                var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                return (false, roleErrors, null);
            }

            // Commit transaction only if both operations succeed
            await transaction.CommitAsync();
            return (true, "Admin registered successfully", user.Id);
        }
        catch (Exception)
        {
            // Rollback on any exception
            await transaction.RollbackAsync();
            throw;
        }
    }
    
    public async Task<LoginResponse> LoginAsync(LoginDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
        {
            return new LoginResponse { Success = false, Message = "User not found" };
        }

        var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);
        
        if (!result.Succeeded)
        {
            return new LoginResponse { Success = false, Message = "Invalid password" };
        }

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "";

        var token = _jwtService.GenerateToken(user, role);

        return new LoginResponse { Success = true, Message = "Login successful", Token = token };
    }
}
