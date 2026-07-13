using System.ComponentModel.DataAnnotations;

namespace Shared.Models;

public class ProcessingJob
{
    public int Id { get; set; }
    public ProcessingJobStatus Status { get; set; }
    public int Attempts { get; set; }

    [MaxLength(300)]
    public string? ErrorSummary { get; set; } = "";
    public Guid CorrelationId { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}

public enum ProcessingJobStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}