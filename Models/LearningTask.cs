namespace TraineeManagementApi.Models;
 
public class LearningTask
{
    public int Id { get; set; } 
    public required String Title { get; set; }
    public required String Description { get; set; }
    public required String  ExpectedTechStack { get; set; }
    public required DateTime DueDate { get; set; }
    public required LearningTaskStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } 
    public DateTime UpdatedDate { get; set; } 
}

public enum LearningTaskStatus{
    Draft,
    Published,
    Closed
}