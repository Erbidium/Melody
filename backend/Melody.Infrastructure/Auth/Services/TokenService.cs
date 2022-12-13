using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Melody.Infrastructure.Auth.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly UserManager<UserIdentity> _userManager;

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

    public async Task<(string? accessToken, string? refreshToken)> CreateAccessTokenAndRefreshToken(string email,
        string password)
    {
        var currentUser = await _userManager.FindByEmailAsync(email);

        if (currentUser == null ||
            !await _userManager.CheckPasswordAsync(currentUser, password))
            return (null, null);
        ;
        var refreshToken = await GenerateRefreshToken(email);
        var user = await _userManager.FindByEmailAsync(email);
        var accessToken = await GenerateAccessToken(user);
        await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
        return (accessToken, refreshToken);
    }

    public async Task<(string? accessToken, string? refreshToken)> GetAccessTokenAndUpdatedRefreshToken(
        string refreshTokenString)
    {
        var dbEntry = await _refreshTokenRepository.FindAsync(refreshTokenString);
        if (dbEntry == null) return (null, null);
        var email = GetEmailFromRefreshToken(refreshTokenString);
        var user = await _userManager.FindByEmailAsync(email);
        var refreshToken = await GenerateRefreshToken(email);
        await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
        var accessToken = await GenerateAccessToken(user);
        return (accessToken, refreshToken);
    }

    private async Task<string> GenerateAccessToken(UserIdentity user)
    {
        var roles = await _userManager.GetRolesAsync(user);
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new("UserId", user.Id.ToString())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)).ToArray());

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
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