﻿using Melody.Infrastructure.Auth.Models;
using Melody.Infrastructure.Data.Records;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Melody.WebAPI.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;

    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateRefreshToken(UserIdentity user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddDays(60),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string GenerateAccessToken(UserIdentity user, IList<string> roles)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName),
            new Claim(ClaimTypes.Email, user.Email),
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)).ToArray());

        var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
            _configuration["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public TokenValidationParameters GetValidationParameters()
    {
        return new TokenValidationParameters()
        {
            ValidateLifetime = true,
            ValidateAudience = true,
            ValidateIssuer = true,
            ValidIssuer = _configuration["Jwt:Issuer"],
            ValidAudience = _configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]))
        };
    }

    public UserToken? GetCurrentUser(ClaimsIdentity identity)
    {
        var userClaims = identity.Claims;
        return new UserToken
        {
            UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value,
            Email = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.Email)?.Value,
            Roles = userClaims.Where(o => o.Type == ClaimTypes.Role).Select(r => r.Value),
        };
    }
}