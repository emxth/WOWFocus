using WOWFocus.Application.Interfaces;
using WOWFocus.Application.Models;
using WOWFocus.Domain.Entities;

namespace WOWFocus.Application.Services;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repo;

    public StudentService(IStudentRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<Student>> GetAllAsync()
        => (await _repo.GetAllAsync()).Where(s => !s.IsArchived).ToList();

    public async Task<IReadOnlyList<Student>> GetArchivedAsync()
        => (await _repo.GetAllAsync()).Where(s => s.IsArchived).ToList();

    public Task<Student?> GetByIdAsync(Guid id) => _repo.GetByIdAsync(id);


    // Create a new student with auto-generated StudentId based on the specified format
    public async Task<Student> CreateAsync(StudentCreateRequest request)
    {
        var registeredYear = DateTime.UtcNow.Year;
        var birthYear = request.DateOfBirth.Year;

        var all = await _repo.GetAllAsync();
        var maxSequence = all
            .Where(s => s.RegisteredYear == registeredYear)
            .Select(s => s.Sequence)
            .DefaultIfEmpty(0)
            .Max();

        var nextSequence = maxSequence + 1;

        var student = new Student
        {
            FullName = request.FullName,
            DateOfBirth = request.DateOfBirth,
            Gender = request.Gender,
            BirthYear = birthYear,
            RegisteredYear = registeredYear,
            Sequence = nextSequence,
            IsArchived = false,
            ArchivedAt = null
        };

        student.StudentId = BuildStudentId(student.RegisteredYear, student.BirthYear, student.Gender, student.Sequence);

        await _repo.AddAsync(student);
        return student;
    }


    // Update student details, but keep RegisteredYear and Sequence unchanged to preserve StudentId format
    public async Task<bool> UpdateAsync(StudentUpdateRequest request)
    {
        var existing = await _repo.GetByIdAsync(request.Id);
        if (existing == null) return false;

        existing.FullName = request.FullName;
        existing.DateOfBirth = request.DateOfBirth;
        existing.Gender = request.Gender;
        existing.BirthYear = request.DateOfBirth.Year;

        // Keep RegisteredYear + Sequence unchanged, regenerate ID
        existing.StudentId = BuildStudentId(existing.RegisteredYear, existing.BirthYear, existing.Gender, existing.Sequence);

        await _repo.UpdateAsync(existing);
        return true;
    }


    // Delete a student by ID
    public async Task<bool> DeleteAsync(Guid id)
    {
        var existing = await _repo.GetByIdAsync(id);
        if (existing == null) return false;

        await _repo.DeleteAsync(id);
        return true;
    }


    // Archive a student by ID (soft delete)
    public async Task<bool> ArchiveAsync(Guid id)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student == null) return false;

        student.IsArchived = true;
        student.ArchivedAt = DateTime.UtcNow;

        await _repo.UpdateAsync(student);
        return true;
    }

    // Restore an archived student by ID
    public async Task<bool> RestoreAsync(Guid id)
    {
        var student = await _repo.GetByIdAsync(id);
        if (student == null) return false;

        student.IsArchived = false;
        student.ArchivedAt = null;

        await _repo.UpdateAsync(student);
        return true;
    }


    // Permanently delete archived students that have been archived for longer than the specified retention period
    public async Task<int> PurgeExpiredArchivedAsync(int retentionDays)
    {
        var all = await _repo.GetAllAsync();
        var cutoff = DateTime.UtcNow.AddDays(-retentionDays);

        var toDelete = all
            .Where(s => s.IsArchived && s.ArchivedAt.HasValue && s.ArchivedAt.Value <= cutoff)
            .Select(s => s.Id)
            .ToList();

        foreach (var id in toDelete)
            await _repo.DeleteAsync(id);

        return toDelete.Count;
    }


    // Helper method to build StudentId based on the specified format
    private static string BuildStudentId(int registeredYear, int birthYear, int gender, int sequence)
    {
        return $"{registeredYear}{birthYear}{gender}{sequence:D4}";
    }
}