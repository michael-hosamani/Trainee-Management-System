using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Shared.Models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    [EmailAddress()]
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required Role Role { get; set; }
    public required string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiry { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime UpdatedDate { get; set; }
}

public enum Role
{
    Admin, 
    Mentor, 
    Trainee
}