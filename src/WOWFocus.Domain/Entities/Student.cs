namespace WOWFocus.Domain.Entities;

public class Student
{
    public Guid Id { get; set; } = Guid.NewGuid();

    // Auto-generated numeric string: RegisteredYear + BirthYear + Gender + Sequence
    public string StudentId { get; set; } = string.Empty;

    public string FullName { get; set; } = string.Empty;

    public DateTime DateOfBirth { get; set; }

    // Derived from DateOfBirth
    public int BirthYear { get; set; }

    // Auto at registration time
    public int RegisteredYear { get; set; }

    // Numeric gender code (e.g., 1=Male, 2=Female, 3=Other)
    public int Gender { get; set; }

    // Auto sequence within registered year
    public int Sequence { get; set; }

    public bool IsArchived { get; set; }

    public DateTime? ArchivedAt { get; set; }
}