using Melody.Infrastructure.Auth.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Melody.WebAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    public TokenService(IConfiguration configuration, UserManager<UserIdentity> userManager,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _configuration = configuration;
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
    }

    public async Task<string> GenerateRefreshToken(string email, bool isExpired = false)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.UserName),
            new(ClaimTypes.Email, user.Email)
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: isExpired ? DateTime.UtcNow.AddDays(-1) : DateTime.Now.AddDays(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<(string accessToken, string refreshToken)> CreateAccessTokenAndRefreshToken(string email)
    {
        var accessToken = await GenerateAccessToken(email);
        var refreshToken = await GenerateRefreshToken(email);
        var user = await _userManager.FindByEmailAsync(email);
        await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
        return (accessToken, refreshToken);
    }

    public async Task<string> GenerateAccessToken(string email)
    {
        var user = await _userManager.FindByEmailAsync(email);
        var roles = await _userManager.GetRolesAsync(user);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new ("UserId", user.Id.ToString()),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)).ToArray());

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public UserToken GetCurrentUser(ClaimsIdentity identity)
    {
        var userClaims = identity.Claims;
        return new UserToken
        {
            UserId = long.Parse(userClaims.FirstOrDefault(o => o.Type == "UserId")?.Value),
            Roles = userClaims.Where(o => o.Type == ClaimTypes.Role).Select(r => r.Value),
        };
    }

    public async Task<(string accessToken, string refreshToken)> GetAccessTokenAndUpdatedRefreshToken(string refreshTokenString)
    {
        var email = GetEmailFromRefreshToken(refreshTokenString);
        var user = await _userManager.FindByEmailAsync(email);
        var refreshToken = await GenerateRefreshToken(email);
        await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
        var accessToken = await GenerateAccessToken(email);
        return (accessToken, refreshToken);
    }

    private string? GetEmailFromRefreshToken(string refreshTokenString)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = GetValidationParameters();
        var principal = tokenHandler.ValidateToken(refreshTokenString, validationParameters, out _);
        return principal.FindFirst(c => c.Type == ClaimTypes.Email)?.Value;

    }

    private TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters
        {
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
        };
    }
}