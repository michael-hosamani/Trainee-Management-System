using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
 
public class Mentor
{
    public int Id { get; set; } 

    [MaxLength(50)]
    public required string FirstName { get; set; }

    [MaxLength(50)]
    public required string LastName { get; set; }

    [EmailAddress(), MaxLength(254)]
    public required string Email { get; set; }

    [MaxLength(200)]
    public required string  Expertise { get; set; }
    public required MentorStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedDate { get; set; } 
    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
}

public enum MentorStatus {
    Active,
    Inactive
}