using System.Security.Claims;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Melody.WebAPI.DTO.Auth.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserIdentity> _userManager;

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
        var tokens = await _tokenService.CreateAccessTokenAndRefreshToken(userLogin.Email, userLogin.Password);

        if (tokens.accessToken is null || tokens.refreshToken is null) return NotFound("User not found");

        Response.Cookies.Append("X-Refresh-Token", tokens.refreshToken,
            new CookieOptions
            {
                HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                Expires = DateTimeOffset.Now.AddDays(60)
            });
        return Ok(new { tokens.accessToken });
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> GetAccessToken()
    {
        if (!Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshTokenString))
            return BadRequest();

        try
        {
            var tokens = await _tokenService.GetAccessTokenAndUpdatedRefreshToken(refreshTokenString);
            if (tokens.accessToken is null || tokens.refreshToken is null) return Unauthorized();

            Response.Cookies.Append("X-Refresh-Token", tokens.refreshToken,
                new CookieOptions
                {
                    HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                    Expires = DateTimeOffset.Now.AddDays(60)
                });
            return Ok(new { AccessToken = tokens.accessToken });
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
        var refreshToken = await _tokenService.GenerateRefreshToken(user.Email, true);
        Response.Cookies.Append("X-Refresh-Token", refreshToken,
            new CookieOptions
            {
                HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                Expires = DateTimeOffset.Now.AddDays(-1)
            });
        return Ok();
    }
}