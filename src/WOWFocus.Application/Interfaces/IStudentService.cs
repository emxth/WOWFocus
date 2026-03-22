using WOWFocus.Domain.Entities;
using WOWFocus.Application.Models;

namespace WOWFocus.Application.Interfaces;

public interface IStudentService
{
    Task<IReadOnlyList<Student>> GetAllAsync();
    Task<IReadOnlyList<Student>> GetArchivedAsync();
    Task<Student?> GetByIdAsync(Guid id);
    Task<Student> CreateAsync(StudentCreateRequest request);
    Task<bool> UpdateAsync(StudentUpdateRequest request);
    Task<bool> DeleteAsync(Guid id);
    Task<bool> ArchiveAsync(Guid id);
    Task<bool> RestoreAsync(Guid id);
    Task<int> PurgeExpiredArchivedAsync(int retentionDays);
}