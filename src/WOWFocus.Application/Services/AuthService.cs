using Microsoft.AspNetCore.Identity;
using WOWFocus.Application.Interfaces;
using WOWFocus.Domain.Identity;

namespace WOWFocus.Application.Services;

public class AuthService
{
    private readonly IIdentityRepository _repo;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public AuthService(IIdentityRepository repo) => _repo = repo;

    public async Task<AppUser?> ValidateAsync(string username, string password)
    {
        var store = await _repo.LoadAsync();
        var user = store.Users.FirstOrDefault(u => u.UserName == username);
        if (user == null) return null;

        var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, password);
        return result == PasswordVerificationResult.Success ? user : null;
    }

    public async Task<List<string>> GetPermissionsAsync(AppUser user)
    {
        var store = await _repo.LoadAsync();
        var rolePerms = store.Roles
            .Where(r => user.Roles.Contains(r.Name))
            .SelectMany(r => r.Permissions)
            .Distinct()
            .ToList();

        return rolePerms;
    }
}