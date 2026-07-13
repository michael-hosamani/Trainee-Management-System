using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Shared.Models;

public class Review
{
    public int Id { get; set; }
    public required int SubmissionId { get; set; }
    public required int MentorId { get; set; }

    [MaxLength(300)]
    public required string Feedback { get; set; }
    public string? Score { get; set; } = "";
    public required ReviewStatus Status { get; set; }
    public required DateTime ReviewedDate { get; set; }

    [JsonIgnore]
    public Submission Submission { get; set; } = null!;
    
    [JsonIgnore]
    public Mentor Mentor { get; set; } = null!;
}

public enum ReviewStatus
{
    Accepted,
    ChangesRequired,
    Rejected
}