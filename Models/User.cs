using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public enum Role
{
    Admin, 
    Mentor, 
    Trainee
}