using System.ComponentModel.DataAnnotations;

namespace WOWFocus.Web.ViewModels;

public class RoleEditViewModel
{
    [Required]
    public string Name { get; set; } = string.Empty;

    public List<string> AvailablePermissions { get; set; } = new();
    public List<string> SelectedPermissions { get; set; } = new();
}