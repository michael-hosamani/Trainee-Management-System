using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

// Class for Trainee
public class Trainee
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    [EmailAddress(), MaxLength(254)]
    public required string Email { get; set; }

    [MaxLength(150)]
    public required string TechStack { get; set; }
    public required Status Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}

public enum Status
{
    Active,
    Inactive,
    Complete
}
