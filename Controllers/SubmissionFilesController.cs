
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services;

namespace TraineeManagementApi.Controllers;

[ApiController]
[Route("api/submission-files")]
public class SubmissionFilesController: ControllerBase
{
    private readonly ISubmissionFileService _service;

    public SubmissionFilesController(ISubmissionFileService service){
        _service = service;
    }

    // GET /api/submission-files/{id}/download route
    [HttpGet("{id}/download")]
    public async Task<ActionResult> Get(int id){
        SubmissionFile file = await _service.DownloadFile(id);
        var net = new System.Net.WebClient();
        var data = net.DownloadData(file.GeneratedStorageName);
        var content = new System.IO.MemoryStream(data);
        var contentType = file.ContentType;
        var fileName = file.OriginalFileName;
        return File(content, contentType, fileName);
    }

}