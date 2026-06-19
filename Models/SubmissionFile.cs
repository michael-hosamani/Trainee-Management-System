using System.Text.Json.Serialization;

namespace TraineeManagementApi.Models;

public class SubmissionFile
{
    public int Id { get; set; }
    public required string OriginalFileName { get; set; }
    public required string GeneratedStorageName { get; set; } 
    public required string ContentType { get; set; }
    public long Size { get; set; }
    public required string Checksum { get; set; }
    public required int UploadedByUser { get; set; }
    public required int SubmissionId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }

    [JsonIgnore]
    public Submission Submission { get; set; } = null!;
}