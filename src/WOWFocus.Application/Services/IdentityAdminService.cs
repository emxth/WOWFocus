using Microsoft.AspNetCore.Identity;
using WOWFocus.Application.Interfaces;
using WOWFocus.Domain.Identity;

namespace WOWFocus.Application.Services;

public class IdentityAdminService
{
    private readonly IIdentityRepository _repo;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public IdentityAdminService(IIdentityRepository repo) => _repo = repo;

    public async Task<List<AppUser>> GetUsersAsync()
        => (await _repo.LoadAsync()).Users;

    public async Task<List<AppRole>> GetRolesAsync()
        => (await _repo.LoadAsync()).Roles;

    public async Task<AppUser?> GetUserAsync(Guid id)
        => (await _repo.LoadAsync()).Users.FirstOrDefault(u => u.Id == id);

    public async Task<AppRole?> GetRoleAsync(string name)
        => (await _repo.LoadAsync()).Roles.FirstOrDefault(r => r.Name == name);

    public async Task CreateUserAsync(string username, string password, List<string> roles)
    {
        var store = await _repo.LoadAsync();
        if (store.Users.Any(u => u.UserName == username)) return;

        var user = new AppUser
        {
            UserName = username,
            Roles = roles
        };
        user.PasswordHash = _hasher.HashPassword(user, password);

        store.Users.Add(user);
        await _repo.SaveAsync(store);
    }

    public async Task UpdateUserAsync(Guid id, string username, string? newPassword, List<string> roles)
    {
        var store = await _repo.LoadAsync();
        var user = store.Users.FirstOrDefault(u => u.Id == id);
        if (user == null) return;

        user.UserName = username;
        user.Roles = roles;

        if (!string.IsNullOrWhiteSpace(newPassword))
            user.PasswordHash = _hasher.HashPassword(user, newPassword);

        await _repo.SaveAsync(store);
    }

    public async Task DeleteUserAsync(Guid id)
    {
        var store = await _repo.LoadAsync();
        store.Users.RemoveAll(u => u.Id == id);
        await _repo.SaveAsync(store);
    }

    public async Task CreateRoleAsync(string name, List<string> permissions)
    {
        var store = await _repo.LoadAsync();
        if (store.Roles.Any(r => r.Name == name)) return;

        store.Roles.Add(new AppRole { Name = name, Permissions = permissions });
        await _repo.SaveAsync(store);
    }

    public async Task UpdateRoleAsync(string name, List<string> permissions)
    {
        var store = await _repo.LoadAsync();
        var role = store.Roles.FirstOrDefault(r => r.Name == name);
        if (role == null) return;

        role.Permissions = permissions;
        await _repo.SaveAsync(store);
    }

    public async Task DeleteRoleAsync(string name)
    {
        var store = await _repo.LoadAsync();
        store.Roles.RemoveAll(r => r.Name == name);

        // Remove role from users
        foreach (var user in store.Users)
            user.Roles.RemoveAll(r => r == name);

        await _repo.SaveAsync(store);
    }
}