using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using sem2week1.DTOS;
using sem2week1.Service.Interface;
using sem2week1.Database.Entities;

namespace sem2week1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly UserManager<Users> _userManager;

    public AuthController(IAuthService authService, UserManager<Users> userManager)
    {
        _authService = authService;
        _userManager = userManager;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterUserAsync(registerDto);

        if (result.Success)
        {
            return Ok(new { message = result.Message, userId = result.UserId });
        }

        return BadRequest(new { message = result.Message });
    }
    
    [HttpPost("register-student")]
    public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterStudentAsync(registerDto);

        if (result.Success)
        {
            return Ok(new { message = result.Message, userId = result.UserId });
        }

        return BadRequest(new { message = result.Message });
    }
    
    [HttpPost("register-instructor")]
    public async Task<IActionResult> RegisterInstructor([FromBody] RegisterInstructorDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterInstructorAsync(registerDto);

        if (result.Success)
        {
            return Ok(new { message = result.Message, userId = result.UserId });
        }

        return BadRequest(new { message = result.Message });
    }
    
    [HttpPost("register-admin")]
    public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.RegisterAdminAsync(registerDto);

        if (result.Success)
        {
            return Ok(new { message = result.Message, userId = result.UserId });
        }

        return BadRequest(new { message = result.Message });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _authService.LoginAsync(loginDto);

        if (result.Success)
        {
            return Ok(result);
        }

        return Unauthorized(new { message = result.Message });
    }

    /// <summary>
    /// Uploads a profile picture for the currently authenticated user.
    /// Only image files are allowed.
    /// </summary>
    /// <param name="file">The image file to upload</param>
    /// <returns>Success message if upload successful</returns>
    [HttpPost("profile/upload")]
    [Authorize]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadProfilePicture(IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file uploaded.");
        }

        // Validate file type (only images)
        if (!file.ContentType.StartsWith("image/"))
        {
            return BadRequest("Only image files are allowed.");
        }

        // Get current user
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound("User not found.");
        }

        // Read file into byte array
        using var memoryStream = new MemoryStream();
        await file.CopyToAsync(memoryStream);
        user.ProfilePicture = memoryStream.ToArray();
        user.ProfilePictureContentType = file.ContentType;

        var result = await _userManager.UpdateAsync(user);
        if (result.Succeeded)
        {
            return Ok(new { message = "Profile picture uploaded successfully." });
        }

        var errors = string.Join(", ", result.Errors.Select(e => e.Description));
        return BadRequest(errors);
    }

    /// <summary>
    /// Downloads the profile picture for the currently authenticated user.
    /// </summary>
    /// <returns>The profile picture file if found</returns>
    [HttpGet("profile/download")]
    [Authorize]
    public async Task<IActionResult> DownloadProfilePicture()
    {
        // Get current user
        var userId = _userManager.GetUserId(User);
        if (userId == null)
        {
            return Unauthorized();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null || user.ProfilePicture == null || user.ProfilePicture.Length == 0)
        {
            return NotFound("No profile picture found.");
        }

        // Return the image file
        return File(user.ProfilePicture, user.ProfilePictureContentType ?? "image/jpeg");
    }
}
