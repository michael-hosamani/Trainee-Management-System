
namespace TraineeManagementApi.Services;

public class FileStorageService:IFileStorageService
{
    private readonly string _uploadPath;
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

}