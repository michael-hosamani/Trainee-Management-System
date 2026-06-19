
using System.Text.Json.Serialization;

namespace TraineeManagementApi.Models;

public class Submission
{
    public int Id { get; set; }
    public required int TaskAssignmentId { get; set; }
    public required string SubmissionUrl { get; set; }
    public required string Notes { get; set; }
    public DateTime SubmissionDate { get; set; }
    public required SubmissionStatus Status { get; set; }

    [JsonIgnore]
    public TaskAssignment TaskAssignment { get; set; } = null!;
    public ICollection<Review> Reviews { get; set; } = new List<Review>();
    public ICollection<SubmissionFile> SubmissionFiles { get; set; } = new List<SubmissionFile>();
}

public enum SubmissionStatus
{
    Submitted,
    Resubmitted
}