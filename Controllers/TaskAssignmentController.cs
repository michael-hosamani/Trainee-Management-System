using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services;
using TraineeManagementApi.Dto;

namespace TraineeManagementApi.Controllers;

[Authorize(Roles = $"{nameof(Role.Admin)}, {nameof(Role.Mentor)}, {nameof(Role.Trainee)}")]
[ApiController]
[Route("api/task-assignments")]
public class TaskAssignmentController: ControllerBase 
{
    private ITaskAssignmentService _service;

    public TaskAssignmentController(ITaskAssignmentService service)
    {
        _service = service;
    }

    // GET /api/task-assignments
    /// <summary>
    /// Retrieves all the TaskAssignments.
    /// </summary>
    /// <returns>All the TaskAssignments.</returns>
    /// <response code="200">Returns all the TaskAssignments.</response>
    [HttpGet]
    public async Task<ActionResult> GetAllTaskAssignments()
    {
        var taskAssignmentData = await _service.GetAllTaskAssignments();
        return Ok(taskAssignmentData);
    }

    // GET /api/task-assignments/{id}
    /// <summary>
    /// Retrieves a specific TaskAssignment by ID.
    /// </summary>
    /// <param name="id">The ID of the TaskAssignment to retrieve.</param>
    /// <returns>The requested TaskAssignment.</returns>
    /// <response code="200">Returns the requested TaskAssignment.</response>
    /// <response code="404">If the TaskAssignment is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetTaskAssignmentById(int id)
    {
        TaskAssignment? taskAssignment = await _service.GetTaskAssignmentById(id);
        if(taskAssignment == null)
        {
            return NotFound(new { message = "TaskAssignment not found" });
        }

        return Ok(taskAssignment);
    }   

    // POST /api/task-assignments
    /// <summary>
    /// Creates a new TaskAssignments.
    /// </summary>
    /// <param name="taskAssignment">TaskAssignments details required for creation</param>
    /// <returns>The newly created TaskAssignments.</returns>
    /// <response code="200">Returns the newly created TaskAssignments.</response>
    [HttpPost]
    public async Task<ActionResult> CreateTaskAssignment(CreateTaskAssignmentRequest taskAssignment)
    {
        TaskAssignmentResponse taskAssignmentResponse = await _service.CreateTaskAssignment(taskAssignment);

        return Ok(taskAssignmentResponse);
    }

    // PUT /api/task-assignments/{id}
    /// <summary>
    /// Update a specific TaskAssignment by ID.
    /// </summary>
    /// <param name="id">The ID of the TaskAssignment to retrieve.</param>
    /// <returns>The updated TaskAssignment.</returns>
    /// <response code="200">Returns the updated TaskAssignment.</response>
    /// <response code="404">If the TaskAssignment is not found.</response>
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateTaskAssignmentDetails(int id, UpdateTaskAssignmentRequest updateTaskAssignmentRequest)
    {
        TaskAssignment? updatedTaskAssignmentDetails = await _service.UpdateTaskAssignmentDetails(id, updateTaskAssignmentRequest);

        if(updatedTaskAssignmentDetails == null)
        {
            return NotFound(new { message = "Invalid TaskAssignment Id" });
        }

        return Ok(updatedTaskAssignmentDetails);
    }
}