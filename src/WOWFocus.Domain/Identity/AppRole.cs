namespace WOWFocus.Domain.Identity;

public class AppRole
{
    public string Name { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
}