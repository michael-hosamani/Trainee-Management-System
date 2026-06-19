namespace TraineeManagementApi.Dto;

public class DownloadFileType
{
    public required MemoryStream Bytes { get; set; } 
    public required string FileName { get; set; } 
    public required string ContentType { get; set; }
}