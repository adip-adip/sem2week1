using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sem2week1.Database;
using sem2week1.Database.Entities;

namespace sem2week1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ModuleController : ControllerBase
{
    private readonly AppDbContext _dbContext;

    public ModuleController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    public IActionResult GetAllModules()
    {
        var modules = _dbContext.Modules
            .Include(m => m.Course)
            .ToList();
        return Ok(modules);
    }

    [HttpGet("{id:int}")]
    public IActionResult GetModuleById(int id)
    {
        var module = _dbContext.Modules
            .Include(m => m.Course)
            .Include(m => m.ModuleInstructors)
            .ThenInclude(mi => mi.Instructor)
            .FirstOrDefault(m => m.Id == id);

        if (module == null)
            return NotFound($"Module with id {id} not found.");

        return Ok(module);
    }

    [HttpGet("course/{courseId:int}")]
    public IActionResult GetModulesByCourse(int courseId)
    {
        var modules = _dbContext.Modules
            .Where(m => m.CourseId == courseId)
            .ToList();

        return Ok(modules);
    }

    [HttpPost]
    public IActionResult AddModule(Module module)
    {
        _dbContext.Modules.Add(module);
        _dbContext.SaveChanges();
        return Ok(module);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateModule(int id, Module updatedModule)
    {
        var module = _dbContext.Modules.Find(id);

        if (module == null)
            return NotFound($"Module with id {id} not found.");

        module.Title = updatedModule.Title;
        module.Credits = updatedModule.Credits;
        module.CourseId = updatedModule.CourseId;

        _dbContext.SaveChanges();
        return Ok(module);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteModule(int id)
    {
        var module = _dbContext.Modules.Find(id);

        if (module == null)
            return NotFound($"Module with id {id} not found.");

        _dbContext.Modules.Remove(module);
        _dbContext.SaveChanges();
        return Ok("Module deleted successfully");
    }
}