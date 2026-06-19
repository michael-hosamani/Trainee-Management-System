
using TraineeManagementApi.Dto;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public class FileStorageService:IFileStorageService
{
    private readonly string _uploadPath;
    private readonly long _maxFileSize = 5 * 1024 * 1024;
    private readonly string[] _allowedExtensions = { ".pdf", ".docx", ".txt" };

    public FileStorageService(IWebHostEnvironment environment)
    {
        _uploadPath = Path.Combine(environment.ContentRootPath, "Uploads");

        Directory.CreateDirectory(_uploadPath);
    }

    // this function stores the input file in the file-system
    public async Task<string> SaveAsync(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new BadRequestException("File is null or empty.");

        if (file.Length > _maxFileSize)
            throw new BadRequestException($"File exceeds maximum size of {_maxFileSize / (1024 * 1024)} MB.");


        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!_allowedExtensions.Contains(extension))
            throw new BadRequestException($"Unsupported file extension: {extension}. Allowed: {string.Join(", ", _allowedExtensions)}.");
        
        var fileName = $"{Guid.NewGuid()}{extension}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }
 
        return filePath;
    }

    public DownloadFileType OpenReadAsync(SubmissionFile file)
    {
        var net = new System.Net.WebClient();
        var data = net.DownloadData(file.GeneratedStorageName);
        var content = new System.IO.MemoryStream(data);
        var contentType = file.ContentType;
        var fileName = file.OriginalFileName;
        return new DownloadFileType{Bytes = content, FileName = fileName, ContentType = contentType};
    }

    public bool ExistsAsync(string filePath)
    {
        if (File.Exists(filePath))
        {
            return true;
        }
        return false;
    }

    public bool DeleteAsync(string filePath)
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            return true;
        }
        return false;
    }
}