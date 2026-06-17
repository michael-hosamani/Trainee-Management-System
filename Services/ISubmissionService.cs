using TraineeManagementApi.Dto;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public interface ISubmissionService
{
    Task<List<Submission>> GetAllSubmissions();
    Task<Submission?> GetSubmissionById(int id);
    Task<SubmissionResponse> CreateSubmission(CreateSubmissionRequest submission);
}