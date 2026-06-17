using TraineeManagementApi.Dto;
using TraineeManagementApi.Models;

namespace TraineeManagementApi.Services;

public interface IReviewService
{
    Task<List<Review>> GetAllReviews();
    Task<Review?> GetReviewById(int id);
    Task<ReviewResponse> CreateReview(CreateReviewRequest review);
}