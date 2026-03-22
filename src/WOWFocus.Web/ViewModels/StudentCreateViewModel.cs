using System.ComponentModel.DataAnnotations;

namespace WOWFocus.Web.ViewModels;

public class StudentCreateViewModel
{
    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Range(1, 3)]
    public int Gender { get; set; }
}