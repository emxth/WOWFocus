namespace WOWFocus.Domain.Identity;

public class AppUser
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string UserName { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;

    public List<string> Roles { get; set; } = new();
}