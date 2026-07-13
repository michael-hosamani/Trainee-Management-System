using System.ComponentModel.DataAnnotations;

namespace Shared.Models;
 
public class LearningTask
{
    public int Id { get; set; } 

    [MaxLength(100)]
    public required string Title { get; set; }

    [MaxLength(250)]
    public required string Description { get; set; }
    
    [MaxLength(200)]
    public required string  ExpectedTechStack { get; set; }
    public required DateTime DueDate { get; set; }
    public required LearningTaskStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedDate { get; set; } 
    public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
}

public enum LearningTaskStatus{
    Draft,
    Published,
    Closed
}