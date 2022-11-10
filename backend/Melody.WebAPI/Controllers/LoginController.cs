using Melody.Infrastructure.Auth.Models;
using Melody.Infrastructure.Data.Interfaces;
using Melody.Infrastructure.Data.Records;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Melody.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        private UserManager<UserIdentity> _userManager;
        private IRefreshTokenRepository _refreshTokenRepository;

        public LoginController(IConfiguration configuration, UserManager<UserIdentity> userManager, IRefreshTokenRepository refreshTokenRepository)
        {
            _configuration = configuration;
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await Authenticate(userLogin);

            if (user != null)
            {
                var accessToken = await GenerateAccessToken(user);
                var refreshToken = GenerateRefreshToken(user);
                await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
                return Ok(new { accessToken, refreshToken });
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> GetAccessToken(string refreshTokenString)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = GetValidationParameters();
            try
            {
                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(refreshTokenString, validationParameters, out validatedToken);
                var dbEntry = await _refreshTokenRepository.FindAsync(refreshTokenString);
                if (dbEntry != null)
                {
                    var email = principal.FindFirst(c => c.Type == ClaimTypes.Email).Value;
                    var user = await _userManager.FindByEmailAsync(email);
                    return Ok(await GenerateAccessToken(user));
                }
                return Unauthorized();
            }
            catch (Exception)
            {
                await _refreshTokenRepository.DeleteAsync(refreshTokenString);
                return Unauthorized();
            }
            
        }

        private async Task<string> GenerateAccessToken(UserIdentity user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var roles = await _userManager.GetRolesAsync(user);

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

        private string GenerateRefreshToken(UserIdentity user)
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

        private TokenValidationParameters GetValidationParameters()
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

        private async Task<UserIdentity?> Authenticate(UserLogin userLogin)
        {
            var currentUser = await _userManager.FindByEmailAsync(userLogin.Email);

            if (currentUser != null && await _userManager.CheckPasswordAsync(currentUser, userLogin.Password))
            {
                return currentUser;
            }

            return null;
        }
    }
}
