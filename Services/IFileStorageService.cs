using TraineeManagementApi.Dto;

namespace TraineeManagementApi.Services;

public interface IFileStorageService
{
    Task<string> SaveAsync(IFormFile formFile);
}