using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sem2week1.Database;
using sem2week1.Database.Entities;
using sem2week1.DTOS;
using System.Diagnostics;

namespace sem2week1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public StudentController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("getAll")]
    public async Task<ActionResult<List<Student>>> GetAllStudents()
    {
        var students = await _dbContext.Students.ToListAsync();
        return Ok(students);
    }
    
    [HttpGet("get/{id:int}")]
    public async Task<IActionResult> GetStudentById(int id)
    {
        Student? student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (student == null)
        {
            return NotFound($"Student with id {id} not found");
        }
        return Ok(student);
    }

    [HttpPost("add")]
    public async Task<IActionResult> AddStudent(Student student)
    {
        _dbContext.Students.Add(student);
        await _dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(GetStudentById), new { id = student.Id }, student); 
    }

    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateStudent(int id, Student updateStudent)
    {
        Student? student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (student == null)
        {
            return NotFound($"Student with id {id} not found");
        }
        student.FirstName = updateStudent.FirstName;
        student.LastName = updateStudent.LastName;
        student.DateOfBirth = updateStudent.DateOfBirth;
        student.Phone = updateStudent.Phone;
        student.Email = updateStudent.Email;
        student.Address = updateStudent.Address;
        
        await _dbContext.SaveChangesAsync();
        return Ok(student);
    }

    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        Student? student = await _dbContext.Students.FirstOrDefaultAsync(x => x.Id == id);
        if (student == null)
        {
            return NotFound($"Student with id {id} not found");
        }
        _dbContext.Students.Remove(student);
        await _dbContext.SaveChangesAsync();
        return Ok("Success");
    }

    // Helper method to calculate age from DateOfBirth
    private static int CalculateAge(DateTime dateOfBirth)
    {
        var today = DateTime.Today;
        var age = today.Year - dateOfBirth.Year;
        if (dateOfBirth.Date > today.AddYears(-age)) age--;
        return age;
    }

    // Helper method to map Student entity to StudentResponseDto
    private static StudentResponseDto MapToResponseDto(Student student)
    {
        return new StudentResponseDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Age = CalculateAge(student.DateOfBirth),
            Address = student.Address,
            Email = student.Email
        };
    }

    /// <summary>
    /// Filter students using IEnumerable (loads all data into memory first)
    /// </summary>
    [HttpGet("filtered/ienumerable")]
    public async Task<IActionResult> GetFilteredStudentsIEnumerable([FromQuery] StudentFilterDto filter)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // Load ALL students into memory (IEnumerable approach)
        var allStudents = await _dbContext.Students.ToListAsync();
        
        // Apply filters in memory using LINQ to Objects
        var query = allStudents.AsEnumerable();
        
        if (filter.MinAge.HasValue)
        {
            query = query.Where(s => CalculateAge(s.DateOfBirth) >= filter.MinAge.Value);
        }
        
        if (filter.MaxAge.HasValue)
        {
            query = query.Where(s => CalculateAge(s.DateOfBirth) <= filter.MaxAge.Value);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            query = query.Where(s => s.Address != null && s.Address.Contains(filter.Address, StringComparison.OrdinalIgnoreCase));
        }
        
        var result = query.Select(MapToResponseDto).ToList();
        
        stopwatch.Stop();
        
        var response = new
        {
            Method = "IEnumerable",
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
            RecordCount = result.Count,
            Data = result
        };
        
        return Ok(response);
    }

    /// <summary>
    /// Filter students using IQueryable (filters at database level)
    /// </summary>
    [HttpGet("filtered/iqueryable")]
    public async Task<IActionResult> GetFilteredStudentsIQueryable([FromQuery] StudentFilterDto filter)
    {
        var stopwatch = Stopwatch.StartNew();
        
        // Start with IQueryable - filters will be translated to SQL
        IQueryable<Student> query = _dbContext.Students;
        
        if (filter.MinAge.HasValue)
        {
            // For IQueryable, calculate date range for age filtering using UTC
            var maxDateOfBirth = DateTime.UtcNow.AddYears(-filter.MinAge.Value).Date;
            query = query.Where(s => s.DateOfBirth <= maxDateOfBirth);
        }
        
        if (filter.MaxAge.HasValue)
        {
            var minDateOfBirth = DateTime.UtcNow.AddYears(-filter.MaxAge.Value - 1).Date;
            query = query.Where(s => s.DateOfBirth >= minDateOfBirth);
        }
        
        if (!string.IsNullOrWhiteSpace(filter.Address))
        {
            // Use ToLower for case-insensitive search (database-agnostic)
            var addressLower = filter.Address.ToLower();
            query = query.Where(s => s.Address != null && s.Address.ToLower().Contains(addressLower));
        }
        
        var result = await query.Select(s => new StudentResponseDto
        {
            Id = s.Id,
            FirstName = s.FirstName,
            LastName = s.LastName,
            Age = CalculateAge(s.DateOfBirth),
            Address = s.Address,
            Email = s.Email
        }).ToListAsync();
        
        stopwatch.Stop();
        
        var response = new
        {
            Method = "IQueryable",
            ExecutionTimeMs = stopwatch.ElapsedMilliseconds,
            RecordCount = result.Count,
            Data = result
        };
        
        return Ok(response);
    }

    /// <summary>
    /// Unified endpoint - choose implementation via 'method' query parameter
    /// </summary>
    [HttpGet("filtered")]
    public async Task<IActionResult> GetFilteredStudents([FromQuery] StudentFilterDto filter, [FromQuery] string method = "iqueryable")
    {
        if (string.Equals(method, "ienumerable", StringComparison.OrdinalIgnoreCase))
        {
            return await GetFilteredStudentsIEnumerable(filter);
        }
        else if (string.Equals(method, "iqueryable", StringComparison.OrdinalIgnoreCase))
        {
            return await GetFilteredStudentsIQueryable(filter);
        }
        
        return BadRequest("Invalid method. Use 'ienumerable' or 'iqueryable'.");
    }
}
