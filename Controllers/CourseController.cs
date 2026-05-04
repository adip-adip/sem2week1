using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using sem2week1.Database;
using sem2week1.Database.Entities;
using sem2week1.Models;

namespace sem2week1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly AppDbContext _dbContext;
    private readonly IMemoryCache _cache;

    public CourseController(AppDbContext dbContext, IMemoryCache cache)
    {
        _dbContext = dbContext;
        _cache = cache;
    }
    
    [Authorize]
    [HttpGet]
    public IActionResult GetAllCourses()
    {
        var courses = _dbContext.Courses
            .Include(c => c.Modules)
            .ToList();
        return Ok(courses);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetCourseCount()
    {
        var count = _dbContext.Courses.Count();
        return Ok(new {CourseCount = count });
    }

    // [HttpGet("{id:int}")]
    // public IActionResult GetCourseById(int id)
    // {
    //     var course = _dbContext.Courses
    //         .Include(c => c.Modules)
    //         .FirstOrDefault(c => c.Id == id);
    //
    //     if (course == null)
    //         return NotFound($"Course with id {id} not found.");
    //
    //     return Ok(course);
    // }

    [HttpPost]
    public IActionResult AddCourse(CreateCourseDto dto)
    {
        var course = new Course
        {
            Name = dto.Name,
            DurationYear = dto.DurationYear,
            Modules = dto.Modules?.Select(m => new Module
            {
                Title = m.Title,
                Credits = m.Credits,
                CourseId = 0 // Will be set after course is added
            }).ToList()
        };

        _dbContext.Courses.Add(course);
        _dbContext.SaveChanges();

        // Update CourseId for each module after course is saved
        if (course.Modules != null && course.Modules.Any())
        {
            foreach (var module in course.Modules)
            {
                module.CourseId = course.Id;
            }
            _dbContext.SaveChanges();
        }

        return Ok(course);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetCourseById(int id)
    {
        var course = _dbContext.Courses
            .Include(c => c.Modules)
            .FirstOrDefault(c => c.Id == id);

        if (course == null)
            return NotFound($"Course with id {id} not found.");

        return Ok(course);
    }

    [HttpGet("/api/courses")]
    [Authorize]
    public IActionResult GetCourses()
    {
        // Try to get courses from cache
        if (!_cache.TryGetValue("courses", out List<Course>? courses))
        {
            // Cache miss - fetch from database
            courses = _dbContext.Courses
                .Include(c => c.Modules)
                .ToList();

            // Set cache options with 5 minute expiration
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(5));

            // Store in cache
            _cache.Set("courses", courses, cacheEntryOptions);
        }

        return Ok(courses);
    }
}
