using System.ComponentModel.DataAnnotations;

// Dto for validating inputs of creation of trainee request
public class CreateTraineeRequest
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50, ErrorMessage = "First name should not exceed 50 charachters")]
    public required string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50, ErrorMessage = "Last name should not exceed 50 charachters")]
    public required string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Vadid email is required")]
    public required string Email { get; set; }

    [Required(ErrorMessage = "TechStack is required")]
    public required string TechStack { get; set; }
    
    [Required(ErrorMessage = "Status is required")]
    [EnumDataType(typeof(Status), ErrorMessage = "Status must be valid")]
    public required Status Status { get; set; }
}
