using Microsoft.AspNetCore.Http.HttpResults;

public class TraineeService : ITraineeService
{
    private static int Index = 0; // index to keep track of Ids of Trainees
    // In-memory List to store Trainee Data
    private static List<Trainee> trainees = new List<Trainee>{
        // Initialising with a dummy user
        new Trainee{ Id = Index++, FirstName = "Michael", LastName = "Hosamani", Email = "michael.hosamani@zeuslearning.com", Status = "Active", TechStack = "Typescript", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
    };

    // This function returns the list of all the Trainees
    public List<Trainee> GetAllTrainees()
    {
        return (trainees);
    }

    // This function fetches a Trainee based on its Id
    public Trainee? GetTraineeById(int id)
    {
        Trainee? trainee = trainees.FirstOrDefault(t => t.Id == id);

        return trainee;
    }

    // This funciton creates a new trainee and pushed it into the in-memory Trainee list
    public TraineeResponse CreateTrainee(CreateTraineeRequest trainee)
    {
        var traineeId = Index++;
        Trainee newTrainee = new Trainee
        {
            Id = traineeId,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        trainees.Add(newTrainee);

        TraineeResponse traineeResponse = new TraineeResponse
        {
            Id = traineeId,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            Status = trainee.Status,
            TechStack = trainee.TechStack,
        };

        return traineeResponse;
    }

    // This function fetches the trainee based on its Id and updates certain fields entered through the body
    public Trainee? UpdateTraineeDetails(int id, UpdateTraineeRequest trainee)
    {
        var findTrainee = trainees.FirstOrDefault(t => t.Id == id);
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
            findTrainee.Status = trainee.Status;


        return findTrainee;
    }

    // This function fetches by Id and deletes a trainee from the in-memory list
    public bool DeleteTraineeDetails(int id)
    {
        int traineeIndex = trainees.FindIndex(t => t.Id == id);
        if(traineeIndex == -1)
        {
            return false;
        }

        trainees.RemoveAll(t => t.Id == id);

        return true;
    }
}