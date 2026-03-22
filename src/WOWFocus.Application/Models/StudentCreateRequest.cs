namespace WOWFocus.Application.Models;

public class StudentCreateRequest
{
    public string FullName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public int Gender { get; set; }
}