using System.ComponentModel.DataAnnotations;

namespace WOWFocus.Web.ViewModels;

public class UserEditViewModel
{
    public Guid Id { get; set; }

    [Required]
    public string UserName { get; set; } = string.Empty;

    // Only required for Create; optional for Edit
    public string? Password { get; set; }

    public List<string> AvailableRoles { get; set; } = new();
    public List<string> SelectedRoles { get; set; } = new();
}