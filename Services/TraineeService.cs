using System.Data.Common;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
// using TraineeManagementApi.Data;

public class TraineeService : ITraineeService
{
    private static int Index = 0; // index to keep track of Ids of Trainees

    private readonly AppDbContext _db;

    public TraineeService(
        AppDbContext db
    )
    {
        _db = db;
    }

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
            return result;
        }
        await _db.SaveChangesAsync();

        return null;
    }

    // This funciton creates a new trainee and pushed it into the in-memory Trainee list
    public async Task<TraineeResponse> CreateTrainee(CreateTraineeRequest trainee)
    {
        int traineeId = Index;
        Index++;
        Trainee newTrainee = new Trainee
        {
            Id = Index,
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
            Id = Index,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
        };
       
        return traineeResponse;
    }

    // This function fetches the trainee based on its Id and updates certain fields entered through the body
    public async Task<Trainee?> UpdateTraineeDetails(int id, UpdateTraineeRequest trainee)
    {
        var findTrainee = await _db.Trainees.SingleOrDefaultAsync(t => t.Id == id);
        if(findTrainee == null)
        {
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

        return findTrainee;
    }

    // This function fetches by Id and deletes a trainee from the in-memory list
    public async Task<bool> DeleteTraineeDetails(int id)
    {
        var trainee = await _db.Trainees.SingleOrDefaultAsync(t => t.Id == id);
        if(trainee == null)
        {
            return false;
        }

        _db.Trainees.Remove(trainee);
        await _db.SaveChangesAsync();

        return true;
    }

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

        // var searchResult = result

        return result;
    }
}