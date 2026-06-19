
using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Dto;
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
        DownloadFileType file = await _service.DownloadFile(id);

        return File(file.Bytes, file.ContentType, file.FileName);
    }

    // GET /api/submission-files/{id}/download route
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id){
        bool isDeleted = await _service.DeleteFile(id);
        if(isDeleted == false)
        {
            return NotFound("Invalid submission file Id");
        }

        return NoContent();
    }
}