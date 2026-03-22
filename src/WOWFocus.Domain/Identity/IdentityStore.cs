namespace WOWFocus.Domain.Identity;

public class IdentityStore
{
    public List<AppUser> Users { get; set; } = new();
    public List<AppRole> Roles { get; set; } = new();
}