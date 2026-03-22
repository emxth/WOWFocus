namespace WOWFocus.Application.Models;

public class StudentUpdateRequest
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
}