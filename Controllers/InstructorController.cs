using Microsoft.AspNetCore.Mvc;
using sem2week1.Database.Entities;
using sem2week1.DTOS;
using sem2week1.Service.Interface;

namespace sem2week1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructorController : ControllerBase
{
    private readonly IInstructorService _instructorService;

    public InstructorController(IInstructorService instructorService)
    {
        _instructorService = instructorService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllInstructors()
    {
        var instructors = await _instructorService.GetAllInstructorsAsync();
        return Ok(instructors);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetInstructorById(int id)
    {
        var instructor = await _instructorService.GetInstructorByIdAsync(id);

        if (instructor == null)
            return NotFound($"Instructor with id {id} not found.");

        return Ok(instructor);
    }

    [HttpPost]
    public async Task<IActionResult> AddInstructor(CreateInstructorDTO dto)
    {
        var instructor = await _instructorService.AddInstructorAsync(dto);
        return Ok(instructor);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateInstructor(int id, Instructor updatedInstructor)
    {
        var instructor = await _instructorService.UpdateInstructorAsync(id, updatedInstructor);

        if (instructor == null)
            return NotFound($"Instructor with id {id} not found.");

        return Ok(instructor);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteInstructor(int id)
    {
        var result = await _instructorService.DeleteInstructorAsync(id);

        if (!result)
            return NotFound($"Instructor with id {id} not found.");

        return Ok("Success");
    }
}
