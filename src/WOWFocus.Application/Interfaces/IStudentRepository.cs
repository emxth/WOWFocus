using WOWFocus.Domain.Entities;

namespace WOWFocus.Application.Interfaces;

public interface IStudentRepository
{
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task AddAsync(Student student);
    Task UpdateAsync(Student student);
    Task DeleteAsync(Guid id);
}