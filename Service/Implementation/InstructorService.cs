using Microsoft.EntityFrameworkCore;
using sem2week1.Database;
using sem2week1.Database.Entities;
using sem2week1.DTOS;
using sem2week1.Service.Interface;

namespace sem2week1.Service.Implementation;

public class InstructorService : IInstructorService
{
    private readonly AppDbContext _dbContext;

    public InstructorService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Instructor>> GetAllInstructorsAsync()
    {
        return await _dbContext.Instructors.ToListAsync();
    }

    public async Task<Instructor?> GetInstructorByIdAsync(int id)
    {
        return await _dbContext.Instructors.FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Instructor> AddInstructorAsync(CreateInstructorDTO dto)
    {
        var instructor = new Instructor
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            HireDate = dto.HireDate
        };

        _dbContext.Instructors.Add(instructor);
        await _dbContext.SaveChangesAsync();
        return instructor;
    }

    public async Task<Instructor?> UpdateInstructorAsync(int id, Instructor updatedInstructor)
    {
        var instructor = await _dbContext.Instructors.FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
            return null;

        instructor.FirstName = updatedInstructor.FirstName;
        instructor.LastName = updatedInstructor.LastName;
        instructor.Email = updatedInstructor.Email;
        instructor.HireDate = updatedInstructor.HireDate;

        await _dbContext.SaveChangesAsync();
        return instructor;
    }

    public async Task<bool> DeleteInstructorAsync(int id)
    {
        var instructor = await _dbContext.Instructors.FirstOrDefaultAsync(i => i.Id == id);

        if (instructor == null)
            return false;

        _dbContext.Instructors.Remove(instructor);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}
