using sem2week1.Database.Entities;
using sem2week1.DTOS;

namespace sem2week1.Service.Interface;

public interface IInstructorService
{
    Task<IEnumerable<Instructor>> GetAllInstructorsAsync();
    Task<Instructor?> GetInstructorByIdAsync(int id);
    Task<Instructor> AddInstructorAsync(CreateInstructorDTO dto);
    Task<Instructor?> UpdateInstructorAsync(int id, Instructor updatedInstructor);
    Task<bool> DeleteInstructorAsync(int id);
}
