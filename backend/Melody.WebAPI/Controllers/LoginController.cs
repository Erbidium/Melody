using Melody.Infrastructure.Auth.Models;
using Melody.Infrastructure.Data.Interfaces;
using Melody.Infrastructure.Data.Records;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Melody.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IRefreshTokenRepository _refreshTokenRepository;
        private readonly ITokenService _tokenService;

        public LoginController(UserManager<UserIdentity> userManager,
            IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService)
        {
            _userManager = userManager;
            _refreshTokenRepository = refreshTokenRepository;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
        {
            var user = await Authenticate(userLogin);

            if (user != null)
            {
                var roles = await _userManager.GetRolesAsync(user);
                var accessToken = _tokenService.GenerateAccessToken(user, roles);
                var refreshToken = _tokenService.GenerateRefreshToken(user);
                await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
                Response.Cookies.Append("X-Refresh-Token", refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                        Expires = DateTimeOffset.Now.AddDays(60)
                    });
                return Ok(new { accessToken });
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> GetAccessToken()
        {
            if (!Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshTokenString))
                return BadRequest();

            var tokenHandler = new JwtSecurityTokenHandler();
            var validationParameters = _tokenService.GetValidationParameters();
            try
            {
                SecurityToken validatedToken;
                var principal =
                    tokenHandler.ValidateToken(refreshTokenString, validationParameters, out validatedToken);
                var dbEntry = await _refreshTokenRepository.FindAsync(refreshTokenString);
                if (dbEntry != null)
                {
                    var email = principal.FindFirst(c => c.Type == ClaimTypes.Email).Value;
                    var user = await _userManager.FindByEmailAsync(email);
                    var roles = await _userManager.GetRolesAsync(user);
                    var refreshToken = _tokenService.GenerateRefreshToken(user);
                    await _refreshTokenRepository.CreateOrUpdateAsync(refreshToken, user.Id);
                    Response.Cookies.Append("X-Refresh-Token", refreshToken,
                        new CookieOptions
                        {
                            HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                            Expires = DateTimeOffset.Now.AddDays(60)
                        });
                    return Ok(_tokenService.GenerateAccessToken(user, roles));
                }

                return Unauthorized();
            }
            catch (Exception)
            {
                await _refreshTokenRepository.DeleteAsync(refreshTokenString);
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpPost("/api/logout")]
        public async Task<IActionResult> Logout()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            var currentUser = _tokenService.GetCurrentUser(identity);
            var user = await _userManager.FindByIdAsync(currentUser.UserId.ToString());
            var refreshToken = _tokenService.GenerateRefreshToken(user, true);
            Response.Cookies.Append("X-Refresh-Token", refreshToken,
                new CookieOptions
                {
                    HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                    Expires = DateTimeOffset.Now.AddDays(-1)
                });
            return Ok();
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