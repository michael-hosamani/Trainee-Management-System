using TraineeManagementApi.Models;

namespace TraineeManagementApi.Dto;

public class CreateMentorRequest
{
    public required String FirstName { get; set; }
    public required String LastName { get; set; }
    public required String Email { get; set; }
    public required String  Expertise { get; set; }
    public required MentorStatus Status { get; set; }
}