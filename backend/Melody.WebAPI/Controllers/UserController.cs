using Melody.Infrastructure.Auth.Models;
using Melody.Infrastructure.Data.Records;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Melody.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<UserIdentity> _userManager;
    private readonly ITokenService _tokenService;

    public UserController(UserManager<UserIdentity> userManager, ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        var user = new UserIdentity
        {
            UserName = userRegister.UserName,
            Email = userRegister.Email,
            PhoneNumber = userRegister.PhoneNumber,
            IsBanned = false,
            IsDeleted = false,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, userRegister.Password);

        if (!result.Succeeded)
        {
            return Ok(result.Errors);
        }

        await _userManager.AddToRoleAsync(user, "User");
        return StatusCode(201);
    }

    [HttpGet("Admins")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminsEndpoint()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUser = _tokenService.GetCurrentUser(identity);
        return Ok($"Hi {currentUser.UserId}, you are an {currentUser.Roles.FirstOrDefault()}");
    }

    [HttpGet("AdminsAndUsers")]
    [Authorize(Roles = "Admin,User")]
    public IActionResult AdminsAndUsersEndpoint()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUser = _tokenService.GetCurrentUser(identity);
        return Ok($"Hi {currentUser.UserId}, you are an {currentUser.Roles.FirstOrDefault()}");
    }

    [HttpGet("Public")]
    public IActionResult Public()
    {
        return Ok("Hi, you're on public property");
    }

    [HttpGet("CreateUser")]
    public IActionResult CreateUser()
    {
        return Ok("Hi, you're on public property");
    }
}