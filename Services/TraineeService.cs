using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using TraineeManagementApi.Helpers;
using Microsoft.AspNetCore.Identity;

public class TraineeService(ILogger<TraineeService> logger, AppDbContext db) : ITraineeService
{
    private readonly AppDbContext _db = db;
    private readonly ILogger<TraineeService> _logger = logger;

    // This function returns the list of all the Trainees
    public async Task<List<Trainee>> GetAllTrainees()
    {
        return await _db.Trainees.ToListAsync();
    }

    // This function fetches a Trainee based on its Id
    public async Task<Trainee?> GetTraineeById(int id)
    {
        var result = await _db.Trainees.SingleOrDefaultAsync(t => t.Id == id);
        if(result != null)
        {
            _logger.LogError("Trainee not found");
            return result;
        }

        return null;
    }

    // This funciton creates a new trainee and pushed it into the in-memory Trainee list
    public async Task<TraineeResponse> CreateTrainee(CreateTraineeRequest trainee)
    {

        Trainee newTrainee = new Trainee
        {
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        await _db.Trainees.AddAsync(newTrainee);
        await _db.SaveChangesAsync();

        TraineeResponse traineeResponse = new TraineeResponse
        {   
            Id = newTrainee.Id,
            FirstName = newTrainee.FirstName,
            LastName = newTrainee.LastName,
            Email = newTrainee.Email,
            Status = newTrainee.Status,
            TechStack = newTrainee.TechStack,
            CreatedDate = newTrainee.CreatedDate,
            UpdatedDate = newTrainee.UpdatedDate
        };
        _logger.LogInformation("New Trainee created successfully");
        return traineeResponse;
    }

    // This function fetches the trainee based on its Id and updates certain fields entered through the body
    public async Task<Trainee?> UpdateTraineeDetails(int id, UpdateTraineeRequest trainee)
    {
        var findTrainee = await _db.Trainees.SingleOrDefaultAsync(t => t.Id == id);
        if(findTrainee == null)
        {
            _logger.LogError("Trainee not found");
            return null;
        }

        if(trainee.FirstName != null)
            findTrainee.FirstName = trainee.FirstName;
        
        if(trainee.LastName != null)
            findTrainee.LastName = trainee.LastName;

        if(trainee.Email != null)
            findTrainee.Email = trainee.Email;

        if(trainee.TechStack != null)        
            findTrainee.TechStack = trainee.TechStack;

        if(trainee.Status != null)
            findTrainee.Status = trainee.Status.Value;

        findTrainee.UpdatedDate = DateTime.Now;

        await _db.SaveChangesAsync();

        _logger.LogInformation("Trainee updated successfully");

        return findTrainee;
    }

    // This function fetches by Id and deletes a trainee from the in-memory list
    public async Task<bool> DeleteTraineeDetails(int id)
    {
        var trainee = await _db.Trainees.SingleOrDefaultAsync(t => t.Id == id);
        if(trainee == null)
        {
            _logger.LogError("Trainee not found");
            return false;
        }

        _db.Trainees.Remove(trainee);
        await _db.SaveChangesAsync();

        _logger.LogInformation("Trainee deletd successfully");

        return true;
    }

    // This function searches trainees based on query parameters
    public async Task<IQueryable<Trainee>> SearchTrainees(string search)
    {
        var result = _db.Trainees.Where(
            t => 
                t.FirstName.Contains(search) ||
                t.LastName.Contains(search) ||
                t.Email.Contains(search) ||
                t.TechStack.Contains(search)
        );
        await _db.SaveChangesAsync();

        return result;
    }

    // This function returns paginated response based on search and status query parameters
    public async Task<PagedResponse<Trainee>> GetTraineeUsingPagination(PaginationParams paginationParams, string? search, Status? status)
    { 
        IQueryable<Trainee> trainees = _db.Trainees.AsQueryable();
        if(!string.IsNullOrWhiteSpace(search))
        {
            // search = search.ToLower();

            trainees = trainees.Where(
                t => 
                    t.FirstName.Contains(search) ||
                    t.LastName.Contains(search) ||
                    t.Email.Contains(search) ||
                    t.TechStack.Contains(search) 
            );
        }
            
        if(status.HasValue)
        {   
            trainees = trainees.Where(t => string.Equals(t.Status.ToString(), status.ToString()));
        }

        var totalRecords = await trainees.CountAsync();
        var items = await trainees.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                .Take(paginationParams.PageSize)
                                .ToListAsync();
        var pagedResponse = new PagedResponse<Trainee>(items, paginationParams.PageNumber, paginationParams.PageSize, totalRecords);
        return pagedResponse;

    }
}