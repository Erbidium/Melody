using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Melody.WebAPI.DTO.Auth.Models;
using Melody.WebAPI.Extensions;
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
    private readonly IValidator<UserLogin> _userLoginValidator;

    public LoginController(UserManager<UserIdentity> userManager,
        IRefreshTokenRepository refreshTokenRepository, ITokenService tokenService, IValidator<UserLogin> userLoginValidator)
    {
        _userManager = userManager;
        _refreshTokenRepository = refreshTokenRepository;
        _tokenService = tokenService;
        _userLoginValidator = userLoginValidator;
    }

    [AllowAnonymous]
    [HttpPost]
    public async Task<IActionResult> Login([FromBody] UserLogin userLogin)
    {
        var validationResult = await _userLoginValidator.ValidateAsync(userLogin);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var result = await _tokenService.CreateAccessTokenAndRefreshToken(userLogin.Email, userLogin.Password);
        return result.Match(tokens =>
            {
                Response.Cookies.Append("X-Refresh-Token", tokens.refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                        Expires = DateTimeOffset.Now.AddDays(60)
                    });
                return Ok(new { tokens.accessToken });
            },
            exception => exception.ToActionResult());
    }

    [AllowAnonymous]
    [HttpPost("refresh")]
    public async Task<IActionResult> GetAccessToken()
    {
        if (!Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshTokenString))
            return BadRequest();
        
        var result = await _tokenService.GetAccessTokenAndUpdatedRefreshToken(refreshTokenString);
        if (result.IsFaulted)
        {
            await _refreshTokenRepository.DeleteByValueAsync(refreshTokenString);
        }
        return result.Match<IActionResult>(tokens =>
            {
                Response.Cookies.Append("X-Refresh-Token", tokens.refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                        Expires = DateTimeOffset.Now.AddDays(60)
                    });
                return Ok(new { AccessToken = tokens.accessToken });
            },
            _ => Unauthorized());
    }

    [Authorize]
    [HttpPost("/api/logout")]
    public async Task<IActionResult> Logout()
    {
        var userId = HttpContext.User.GetId();
        var user = await _userManager.FindByIdAsync(userId.ToString());
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