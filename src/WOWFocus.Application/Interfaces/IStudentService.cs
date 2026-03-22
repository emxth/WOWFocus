using WOWFocus.Domain.Entities;
using WOWFocus.Application.Models;

namespace WOWFocus.Application.Interfaces;

public interface IStudentService
{
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student> CreateAsync(StudentCreateRequest request);
    Task<bool> UpdateAsync(StudentUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
}