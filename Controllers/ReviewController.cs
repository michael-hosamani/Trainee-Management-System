using Microsoft.AspNetCore.Mvc;
using TraineeManagementApi.Helpers;
using Microsoft.AspNetCore.Authorization;
using TraineeManagementApi.Models;
using TraineeManagementApi.Services;
using TraineeManagementApi.Dto;

namespace TraineeManagementApi.Controllers;

[Authorize(Roles = $"{nameof(Role.Mentor)}, {nameof(Role.Trainee)}")]
[ApiController]
[Route("api/reviews")]
public class ReviewsController: ControllerBase 
{
    private IReviewService _service;

    public ReviewsController(IReviewService service)
    {
        _service = service;
    }

    // GET /api/reviews
    /// <summary>
    /// Retrieves all the Reviews.
    /// </summary>
    /// <returns>All the Reviews.</returns>
    /// <response code="200">Returns all the Reviews.</response>
    [HttpGet]
    public async Task<ActionResult> GetAllReviews()
    {
        var reviewData = await _service.GetAllReviews();
        return Ok(reviewData);
    }

    // GET /api/reviews/{id}
    /// <summary>
    /// Retrieves a specific Review by ID.
    /// </summary>
    /// <param name="id">The ID of the Review to retrieve.</param>
    /// <returns>The requested Review.</returns>
    /// <response code="200">Returns the requested Review.</response>
    /// <response code="404">If the Review is not found.</response>
    [HttpGet("{id}")]
    public async Task<ActionResult> GetReviewById(int id)
    {
        Review? review = await _service.GetReviewById(id);
        if(review == null)
        {
            return NotFound(new { message = "Review not found" });
        }

        return Ok(review);
    }   

    // POST /api/reviews
    /// <summary>
    /// Creates a new Review.
    /// </summary>
    /// <param name="review">Review details required for creation</param>
    /// <returns>The newly created Review.</returns>
    /// <response code="200">Returns the newly created Review.</response>
    [HttpPost]
    public async Task<ActionResult> CreateReview(CreateReviewRequest review)
    {
        ReviewResponse reviewResponse = await _service.CreateReview(review);

        return Ok(reviewResponse);
    }
}