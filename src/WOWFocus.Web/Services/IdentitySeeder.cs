using WOWFocus.Application.Interfaces;
using WOWFocus.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace WOWFocus.Web.Services;

public class IdentitySeeder
{
    private readonly IIdentityRepository _repo;
    private readonly IConfiguration _config;
    private readonly PasswordHasher<AppUser> _hasher = new();

    public IdentitySeeder(IIdentityRepository repo, IConfiguration config)
    {
        _repo = repo;
        _config = config;
    }

    public async Task SeedAsync()
    {
        var store = await _repo.LoadAsync();
        if (store.Users.Any()) return;

        var adminUser = _config["Auth:InitialAdmin:UserName"] ?? "admin";
        var adminPass = _config["Auth:InitialAdmin:Password"] ?? "Admin@password$123";

        if (!store.Roles.Any(r => r.Name == "Admin"))
        {
            store.Roles.Add(new AppRole
            {
                Name = "Admin",
                Permissions = _config.GetSection("Permissions").Get<List<string>>() ?? new()
            });
        }

        var user = new AppUser { UserName = adminUser, Roles = new List<string> { "Admin" } };
        user.PasswordHash = _hasher.HashPassword(user, adminPass);

        store.Users.Add(user);
        await _repo.SaveAsync(store);
    }
}