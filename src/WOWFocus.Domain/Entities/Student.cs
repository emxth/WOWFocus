namespace WOWFocus.Domain.Entities;

public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string FullName { get; set; } = string.Empty;
    public string GradeLevel { get; set; } = string.Empty; // O/L or A/L
    public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
}