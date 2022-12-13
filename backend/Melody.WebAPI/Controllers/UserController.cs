using System.Security.Claims;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.WebAPI.DTO.Auth.Models;
using Melody.WebAPI.DTO.User;
using Melody.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly UserManager<UserIdentity> _userManager;

    public UserController(UserManager<UserIdentity> userManager)
    {
        _userManager = userManager;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        var user = new UserIdentity
        {
            UserName = userRegister.UserName,
            Email = userRegister.Email,
            PhoneNumber = userRegister.PhoneNumber
        };

        var result = await _userManager.CreateAsync(user, userRegister.Password);

        if (!result.Succeeded) return BadRequest(result.Errors);

        await _userManager.AddToRoleAsync(user, "User");
        return StatusCode(201);
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = HttpContext.User.GetId();
        var userIdentity = await _userManager.FindByIdAsync(userId.ToString());
        var roles = await _userManager.GetRolesAsync(userIdentity);
        return Ok(new UserDto(userIdentity) { Roles = roles });
    }

    [AllowAnonymous]
    [HttpGet("check-email")]
    public async Task<ActionResult<bool>> CheckExistingEmail(string email)
    {
        return Ok(await _userManager.FindByEmailAsync(email) != null);
    }

    [AllowAnonymous]
    [HttpGet("check-username")]
    public async Task<ActionResult<bool>> CheckExistingUsername(string username)
    {
        return Ok(await _userManager.FindByNameAsync(username) != null);
    }

    [HttpGet("Admins")]
    [Authorize(Roles = "Admin")]
    public IActionResult AdminsEndpoint()
    {
        var userId = HttpContext.User.GetId();
        var roles = HttpContext.User.GetUserRoles();
        return Ok($"Hi {userId}, you are an {roles.First()}");
    }

    [HttpGet("AdminsAndUsers")]
    [Authorize(Roles = "Admin,User")]
    public IActionResult AdminsAndUsersEndpoint()
    {
        var userId = HttpContext.User.GetId();
        var roles = HttpContext.User.GetUserRoles();
        return Ok($"Hi {userId}, you are an {roles.First()}");
    }
}