using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models;

public class TaskAssignment
{
    public int Id { get; set; }
    public required int TraineeId { get; set; }
    public required int MentorId { get; set; }
    public required int LearningTaskId { get; set; }
    public required DateTime AssignedDate { get; set; }
    public required DateTime DueDate { get; set; }
    public required TaskAssignmentStatus Status { get; set; }

    [MaxLength(300)]
    public string? Remarks { get; set; } = "";
    
    [JsonIgnore]
    public Trainee Trainee { get; set; } = null!;
    
    [JsonIgnore]
    public Mentor Mentor { get; set; } = null!;
    
    [JsonIgnore]
    public LearningTask LearningTask { get; set; } = null!;
    public ICollection<Submission> Submissions { get; set; } = new List<Submission>();
}

public enum TaskAssignmentStatus
{
    Assigned,
    InProgress,
    Submitted,
    Reviewed,
    Completed
}