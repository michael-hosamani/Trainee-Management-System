using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public interface ISubmissionFileService
{
    Task<SubmissionFile> DownloadFile(int Id);
}