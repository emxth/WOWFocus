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

    public Task<IReadOnlyList<Student>> GetAllAsync() => _repo.GetAllAsync();

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
            Sequence = nextSequence
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


    // Helper method to build StudentId based on the specified format
    private static string BuildStudentId(int registeredYear, int birthYear, int gender, int sequence)
    {
        return $"{registeredYear}{birthYear}{gender}{sequence:D4}";
    }
}