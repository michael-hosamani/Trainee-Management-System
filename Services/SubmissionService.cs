using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using TraineeManagementApi.Models;
using TraineeManagementApi.Dto;
using TraineeManagementApi.Services;
using System.Security.Cryptography;

namespace TraineeManagementApi.Services;

public class SubmissionService: ISubmissionService 
{
    private readonly AppDbContext _db;
    private readonly ILogger<SubmissionService> _logger;

    private readonly IFileStorageService _fileStorageService;

    public SubmissionService(AppDbContext db, ILogger<SubmissionService> logger, IFileStorageService fileStorageService){
        _db = db;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }

    // This function returns the list of all the Submissions
    public async Task<List<Submission>> GetAllSubmissions()
    {
        return await _db.Submissions
                            .Include(s => s.Reviews)
                            .Include(s => s.SubmissionFiles)
                            .ToListAsync();
    }

    // This function fetches a Submission based on its Id
    public async Task<Submission?> GetSubmissionById(int id)
    {
        var result = await _db.Submissions
                                .Include(s => s.Reviews)
                                .Include(s => s.SubmissionFiles)
                                .SingleOrDefaultAsync(t => t.Id == id);
        if(result == null)
        {
            _logger.LogWarning("Submissoin not found with {id}", id);
            return null;
        }
        return result;
    }

    // This funciton creates a new Submission
    public async Task<SubmissionResponse> CreateSubmission(CreateSubmissionRequest submission)
    {
        TaskAssignment? findTaskAssignment = await _db.TaskAssignments.SingleOrDefaultAsync(t => t.Id == submission.TaskAssignmentId);
        if(findTaskAssignment == null)
        {
            _logger.LogWarning("Task Assignment not found with {id}", submission.TaskAssignmentId);
            throw new NotFoundException("Task Assignment not found", submission.TaskAssignmentId);
        }
        
        Submission newSubmission = new Submission
        {
            TaskAssignmentId = submission.TaskAssignmentId,
            SubmissionUrl = submission.SubmissionUrl,
            Notes = submission.Notes,
            Status = submission.Status,
            SubmissionDate = submission.SubmissionDate
        };

        await _db.Submissions.AddAsync(newSubmission);
        await _db.SaveChangesAsync();

        SubmissionResponse submissionResponse = new SubmissionResponse
        {   
            Id = newSubmission.Id,
            TaskAssignmentId = newSubmission.TaskAssignmentId,
            SubmissionUrl = newSubmission.SubmissionUrl,
            Notes = newSubmission.Notes,
            Status = newSubmission.Status,
            SubmissionDate = newSubmission.SubmissionDate
        };
        _logger.LogInformation("New Submission created successfully");
        return submissionResponse;
    }

    // This function is used to add submissionfile to the SubmissionFile model in the database and store the file using fileStorageService
    public async Task<string> UploadFile(int submissionId, CreateSubmissionFileRequest createSubmissionFileRequest)
    {   
        Submission? findSubmission = await _db.Submissions.SingleOrDefaultAsync(s => s.Id == submissionId);
        if(findSubmission == null)
        {
            _logger.LogWarning("Submissions not found with {id}", submissionId);
            throw new NotFoundException("Submissions not found", submissionId);
        }

        string generatedPath = await _fileStorageService.SaveAsync(createSubmissionFileRequest.File);

        string checksum = GetFileChecksum(generatedPath, "SHA256");

        SubmissionFile submissionFile = new SubmissionFile
        {
            OriginalFileName = createSubmissionFileRequest.File.FileName,
            GeneratedStorageName = generatedPath,
            SubmissionId = submissionId,
            UploadedByUser = createSubmissionFileRequest.UploadedByUser,
            ContentType = createSubmissionFileRequest.File.ContentType,
            Size = createSubmissionFileRequest.File.Length,
            Checksum = checksum,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };
        await _db.SubmissionFiles.AddAsync(submissionFile);
        await _db.SaveChangesAsync();
        
        return generatedPath;
    }

    /// <summary>
    /// Generates a checksum for a given file using the specified algorithm.
    /// Supported algorithms: MD5, SHA1, SHA256, SHA384, SHA512
    /// </summary>
    private static string GetFileChecksum(string filePath, string algorithmName)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
            throw new FileNotFoundException("File not found.", filePath);

        using (var algorithm = HashAlgorithm.Create(algorithmName))
        {
            if (algorithm == null)
                throw new ArgumentException("Invalid hash algorithm name.");

            using (var stream = File.OpenRead(filePath))
            {
                byte[] hashBytes = algorithm.ComputeHash(stream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }
    }
}