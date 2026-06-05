// Interface for TraineeService
public interface ITraineeService
{
    List<Trainee> GetAllTrainees();
    Trainee? GetTraineeById(int id);
    TraineeResponse CreateTrainee(CreateTraineeRequest trainee);
    Trainee? UpdateTraineeDetails(int id, UpdateTraineeRequest trainee);
    bool DeleteTraineeDetails(int id);
}