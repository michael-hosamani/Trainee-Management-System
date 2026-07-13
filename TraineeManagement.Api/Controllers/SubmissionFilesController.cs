
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Dto;
using Shared.Models;
using TraineeManagement.Api.Services;
using Microsoft.AspNetCore.Authorization;

namespace TraineeManagement.Api.Controllers;

[Authorize(Roles = $"{nameof(Role.Mentor)}, {nameof(Role.Trainee)}")]
[ApiController]
[Route("api/submission-files")]
public class SubmissionFilesController: ControllerBase
{
    private readonly ISubmissionFileService _service;

    public SubmissionFilesController(ISubmissionFileService service){
        _service = service;
    }

    // GET /api/submission-files/{id}/download route
    /// <summary>
    /// Retrieves a download link for a specific Submission-file by ID.
    /// </summary>
    /// <param name="id">The ID of the download link for a specific Submission-file to retrieve.</param>
    /// <returns>The requested download link for a specific Submission-file.</returns>
    /// <response code="200">Returns the requested download link for a specific Submission-file.</response>
    /// <response code="404">If the Submission-file is not found.</response>
    [HttpGet("{id}/download")]
    public async Task<ActionResult> Get(int id, CancellationToken cancellationToken){
        DownloadFileType file = await _service.DownloadFile(id, cancellationToken);

        return File(file.Bytes, file.ContentType, file.FileName);
    }

    // DELETE /api/submission-files/{id} route
    /// <summary>
    /// Delete a specific Submission-file by ID.
    /// </summary>
    /// <param name="id">The ID of the Submission-file to retrieve.</param>
    /// <returns>The deleted Submission-file.</returns>
    /// <response code="200">Returns the deleted Submission-file.</response>
    /// <response code="404">If the Submission-file is not found.</response>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id, CancellationToken cancellationToken){
        bool isDeleted = await _service.DeleteFile(id, cancellationToken);
        if(isDeleted == false)
        {
            return NotFound("Invalid submission file Id");
        }

        return NoContent();
    }
}