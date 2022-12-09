﻿using Melody.Infrastructure.Auth.Models;
using Melody.Infrastructure.Data.Interfaces;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Melody.Infrastructure.Data.DbEntites;

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
            var userIsAuthenticated = await Authenticate(userLogin);

            if (userIsAuthenticated)
            {
                var tokens = await _tokenService.CreateAccessTokenAndRefreshToken(userLogin.Email);
                Response.Cookies.Append("X-Refresh-Token", tokens.refreshToken,
                    new CookieOptions
                    {
                        HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                        Expires = DateTimeOffset.Now.AddDays(60)
                    });
                return Ok(new { tokens.accessToken });
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> GetAccessToken()
        {
            if (!Request.Cookies.TryGetValue("X-Refresh-Token", out var refreshTokenString))
                return BadRequest();

            try
            {
                var dbEntry = await _refreshTokenRepository.FindAsync(refreshTokenString);
                if (dbEntry != null)
                {
                    var tokens = await _tokenService.GetAccessTokenAndUpdatedRefreshToken(refreshTokenString);
                    Response.Cookies.Append("X-Refresh-Token", tokens.refreshToken,
                        new CookieOptions
                        {
                            HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                            Expires = DateTimeOffset.Now.AddDays(60)
                        });
                    return Ok(new { AccessToken = tokens.accessToken });
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
            var refreshToken = await _tokenService.GenerateRefreshToken(user.Email, true);
            Response.Cookies.Append("X-Refresh-Token", refreshToken,
                new CookieOptions
                {
                    HttpOnly = true, SameSite = SameSiteMode.None, Secure = true,
                    Expires = DateTimeOffset.Now.AddDays(-1)
                });
            return Ok();
        }


        private async Task<bool> Authenticate(UserLogin userLogin)
        {
            var currentUser = await _userManager.FindByEmailAsync(userLogin.Email);

            return currentUser != null &&
                   await _userManager.CheckPasswordAsync(currentUser, userLogin.Password);
        }
    }
}