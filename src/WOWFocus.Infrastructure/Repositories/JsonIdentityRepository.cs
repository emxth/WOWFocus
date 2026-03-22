using System.Text.Json;
using WOWFocus.Application.Interfaces;
using WOWFocus.Domain.Identity;

namespace WOWFocus.Infrastructure.Repositories;

public class JsonIdentityRepository : IIdentityRepository
{
    private readonly string _filePath;
    private static readonly JsonSerializerOptions _options = new() { WriteIndented = true };

    public JsonIdentityRepository(string filePath)
    {
        _filePath = filePath;
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
    }

    public async Task<IdentityStore> LoadAsync()
    {
        if (!File.Exists(_filePath)) return new IdentityStore();
        var json = await File.ReadAllTextAsync(_filePath);
        return JsonSerializer.Deserialize<IdentityStore>(json) ?? new IdentityStore();
    }

    public async Task SaveAsync(IdentityStore store)
    {
        var json = JsonSerializer.Serialize(store, _options);
        await File.WriteAllTextAsync(_filePath, json);
    }
}