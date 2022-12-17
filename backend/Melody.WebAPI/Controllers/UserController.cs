using AutoMapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.DTO.Auth.Models;
using Melody.WebAPI.DTO.Playlist;
using Melody.WebAPI.DTO.Song;
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
    private readonly Core.Interfaces.IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IMapper _mapper;

    public UserController(UserManager<UserIdentity> userManager, Core.Interfaces.IUserRepository userRepository, IMapper mapper, IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _mapper = mapper;
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
    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserDto>> GetUserById(long id)
    {
        var userRoles = HttpContext.User.GetUserRoles();
        var userIdentity = await _userManager.FindByIdAsync(id.ToString());

        if (userIdentity is null || (!userRoles.Contains("Admin") && userIdentity.IsBanned))
        {
            return NotFound("User is not found");
        }
        var roles = await _userManager.GetRolesAsync(userIdentity);
        return Ok(new UserDto(userIdentity) { Roles = roles });
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

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserForAdminDto>>> GetUsersForAdministrator()
    {
        return Ok(_mapper.Map<List<UserForAdminDto>>(await _userRepository.GetUsersWithoutAdministratorRole()));
    }

    [HttpPatch("{id:long}/ban")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BanUser(BannedStatusDto bannnedStatusDto, long id)
    {
        if (bannnedStatusDto.IsBanned)
        {
            _refreshTokenRepository.DeleteByUserIdAsync(id);
        }
        return Ok();
    }
}