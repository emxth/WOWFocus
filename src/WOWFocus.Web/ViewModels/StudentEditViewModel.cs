using System.ComponentModel.DataAnnotations;

namespace WOWFocus.Web.ViewModels;

public class StudentEditViewModel
{
    public Guid Id { get; set; }

    public string StudentId { get; set; } = string.Empty;

    [Required]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [Required]
    [Range(1, 3)]
    public int Gender { get; set; }
}