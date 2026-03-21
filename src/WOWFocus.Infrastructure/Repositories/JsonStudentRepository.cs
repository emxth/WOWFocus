using System.Text.Json;
using WOWFocus.Application.Interfaces;
using WOWFocus.Domain.Entities;

namespace WOWFocus.Infrastructure.Repositories;

public class JsonStudentRepository : IStudentRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonStudentRepository(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
    }

    public async Task<IReadOnlyList<Student>> GetAllAsync()
    {
        if (!File.Exists(_filePath)) return new List<Student>();
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<List<Student>>(json) ?? new List<Student>();
    }

    public async Task<Student?> GetByIdAsync(Guid id)
    {
        var all = await GetAllAsync();
        return all.FirstOrDefault(x => x.Id == id);
    }

    public async Task AddAsync(Student student)
    {
        var all = (await GetAllAsync()).ToList();
        all.Add(student);
        await SaveAsync(all);
    }

    public async Task UpdateAsync(Student student)
    {
        var all = (await GetAllAsync()).ToList();
        var index = all.FindIndex(x => x.Id == student.Id);
        if (index >= 0)
        {
            all[index] = student;
            await SaveAsync(all);
        }
    }

    public async Task DeleteAsync(Guid id)
    {
        var all = (await GetAllAsync()).ToList();
        all.RemoveAll(x => x.Id == id);
        await SaveAsync(all);
    }

    private async Task SaveAsync(List<Student> students)
    {
        var json = JsonSerializer.Serialize(students, _options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}