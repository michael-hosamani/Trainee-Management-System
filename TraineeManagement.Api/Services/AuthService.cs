using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Dto;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Shared.Models;
using Shared.Data;
using TraineeManagement.Api.Helpers;

namespace TraineeManagement.Api.Services;

public class AuthService: IAuthService
{
    private readonly AppDbContext _db;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;
    private readonly ITokenService _TokenService;
    private readonly Hasher _hasher;

    public AuthService(AppDbContext db, IConfiguration configuration, ILogger<AuthService> logger, ITokenService TokenService, Hasher hasher)
    {   
        _db = db;
        _configuration = configuration;
        _logger = logger;
        _TokenService = TokenService;
        _hasher = hasher;
    }

    public async Task<LoginResponse?> UserLogin(LoginRequest loginRequest)
    {
        User? user = _db.Users.Where(u => u.Username == loginRequest.Username).FirstOrDefault();

        if (user == null)
        {
            _logger.LogError("User not found");
            return null;
        }
  
        var hasher = new PasswordHasher<User>();
        var isCorrectPassword = hasher.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);
        if(isCorrectPassword == PasswordVerificationResult.Failed)
        {
            return null;
        }
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),  
            new Claim(ClaimTypes.Name, user.Username), 
            new Claim(ClaimTypes.Role, user.Role.ToString())
        };

        var accessToken = _TokenService.GenerateToken(user, AuthTokenType.AccessToken);
        var refreshToken = _TokenService.GenerateToken(user, AuthTokenType.RefreshToken);

        string hashedRefreshToken = _hasher.ComputeSha256Hash(refreshToken.jwt);

        _logger.LogInformation("hashed refresh token: {hashedRefreshToken}", hashedRefreshToken);

        user.RefreshToken = hashedRefreshToken;
        user.RefreshTokenExpiry = refreshToken.expiryDate;

        await _db.SaveChangesAsync(); 

        UserWithoutPassword userWithoutPassword = new UserWithoutPassword
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate,
            RefreshToken = refreshToken.jwt,
            RefreshTokenExpiry = user.RefreshTokenExpiry
        };

        _logger.LogInformation("Logged In Successfully By user: {user} at {time}", user.Id, DateTime.UtcNow); 
        
        return new LoginResponse
        {
            User = userWithoutPassword,
            Token = accessToken.jwt,
            ExpiresIn = accessToken.expiryDate
        };
    }

    public async Task<LoginResponse?> Refresh(RefreshTokenDto refreshTokenDto)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]));

        var validationParameters = new TokenValidationParameters()
        {
            IssuerSigningKey = securityKey,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ClockSkew = TimeSpan.Zero
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var result = tokenHandler.ValidateToken(refreshTokenDto.RefreshToken, validationParameters, out SecurityToken validatedToken);
        var jwtToken = (JwtSecurityToken)validatedToken;
  
        var userId = jwtToken.Claims.First(claim => claim.Type == ClaimTypes.NameIdentifier).Value;

        User? user =  _db.Users.Where(u => u.Id == int.Parse(userId)).FirstOrDefault();
        if(user == null)
        {
            _logger.LogError("Refresh token is invalid");
            return null;
        }

        string hashedToken = _hasher.ComputeSha256Hash(refreshTokenDto.RefreshToken);
        if(hashedToken != user.RefreshToken)
        {
            _logger.LogError("Refresh token is invalid");
            return null;
        }

        if(user.RefreshTokenExpiry <= DateTime.UtcNow)
        {
            _logger.LogError("Refresh token has expired");
            return null;
        }

        var newAccessToken = _TokenService.GenerateToken(user, AuthTokenType.AccessToken);
        var newRefreshToken = _TokenService.GenerateToken(user, AuthTokenType.RefreshToken);

        user.RefreshToken = _hasher.ComputeSha256Hash(newRefreshToken.jwt);
        user.RefreshTokenExpiry = newRefreshToken.expiryDate;

        await _db.SaveChangesAsync(); 

        UserWithoutPassword userWithoutPassword = new UserWithoutPassword
        {
            Id = user.Id,
            Email = user.Email,
            Username = user.Username,
            Role = user.Role,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate,
            RefreshToken = newRefreshToken.jwt,
            RefreshTokenExpiry = user.RefreshTokenExpiry
        };
        
        _logger.LogInformation("Refresh token and access token generated");
        return new LoginResponse
        {
            Token = newAccessToken.jwt,
            ExpiresIn = newAccessToken.expiryDate,
            User = userWithoutPassword
        };
    }
}