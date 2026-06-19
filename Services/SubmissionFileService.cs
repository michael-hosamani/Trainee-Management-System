using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public class SubmissionFileService: ISubmissionFileService
{
    private readonly AppDbContext _db;
    private readonly ILogger<SubmissionFileService> _logger;

    public SubmissionFileService(AppDbContext db, ILogger<SubmissionFileService> logger)
    {
        _db = db;
        _logger = logger;
    }
    public async Task<SubmissionFile> DownloadFile(int id)
    {
        SubmissionFile? submissionFile = await _db.SubmissionFiles.FindAsync(id);
        if (submissionFile == null)
        {
            _logger.LogWarning("Submission File not found with {id}", id);
            throw new NotFoundException("Submission File not found", id);
        }
        return submissionFile;
    }
}