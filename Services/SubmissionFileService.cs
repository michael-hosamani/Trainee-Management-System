using TraineeManagementApi.Dto;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public class SubmissionFileService: ISubmissionFileService
{
    private readonly AppDbContext _db;
    private readonly ILogger<SubmissionFileService> _logger;
    private readonly IFileStorageService _fileStorageService;

    public SubmissionFileService(AppDbContext db, ILogger<SubmissionFileService> logger, IFileStorageService fileStorageService)
    {
        _db = db;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }
    public async Task<DownloadFileType> DownloadFile(int id)
    {
        SubmissionFile? submissionFile = await _db.SubmissionFiles.FindAsync(id);
        if (submissionFile == null)
        {
            _logger.LogWarning("Submission File not found with {id}", id);
            throw new NotFoundException("Submission File not found", id);
        }

        _logger.LogInformation("Submission File created successfully with {id}", id);
        return _fileStorageService.OpenReadAsync(submissionFile);
    }

    public async Task<bool> DeleteFile(int id)
    {
        SubmissionFile? submissionFile = await _db.SubmissionFiles.FindAsync(id);
        if (submissionFile == null)
        {
            _logger.LogWarning("Submission File not found with {id}", id);
            return false;
        }

        bool isDeleted = _fileStorageService.DeleteAsync(submissionFile.GeneratedStorageName);

        if (isDeleted)
        {
            _db.SubmissionFiles.Remove(submissionFile);
            await _db.SaveChangesAsync();
            _logger.LogInformation("Submission File deleted successfully with {id}", id);
            return true;
        }
        else
        {
            _logger.LogWarning("File {filename} does notexist", submissionFile.OriginalFileName);
            throw new BadRequestException($"File '{submissionFile.OriginalFileName}' does not exist.");
        }
    }
}