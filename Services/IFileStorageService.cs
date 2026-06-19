using TraineeManagementApi.Dto;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public interface IFileStorageService
{
    Task<string> SaveAsync(IFormFile formFile);
    DownloadFileType OpenReadAsync(SubmissionFile file);
    bool ExistsAsync(string filePath);
    bool DeleteAsync(string filePath);
}