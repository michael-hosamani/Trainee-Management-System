using System.ComponentModel.DataAnnotations;

// Dto for validating inputs of update trainee request
public class UpdateTraineeRequest
{
    [StringLength(50, ErrorMessage = "First name should not exceed 50 charachters")]
    public string? FirstName { get; set; }

    [StringLength(50, ErrorMessage = "Last name should not exceed 50 charachters")]
    public string? LastName { get; set; }

    [EmailAddress(ErrorMessage = "Vadid email is required")]
    public string? Email { get; set; }

    public string? TechStack { get; set; }
    
    [EnumDataType(typeof(Status), ErrorMessage = "Status must be valid")]
    public string? Status { get; set; }
}