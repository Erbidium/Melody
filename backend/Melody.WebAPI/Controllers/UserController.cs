using AutoMapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Melody.WebAPI.DTO.Auth.Models;
using Melody.WebAPI.DTO.RecommendationsPreferences;
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
    private readonly ITokenService _tokenService;

    public UserController(
        UserManager<UserIdentity> userManager,
        Core.Interfaces.IUserRepository userRepository,
        IMapper mapper,
        IRefreshTokenRepository refreshTokenRepository,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _mapper = mapper;
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

    [HttpGet("check-preferences")]
    public async Task<ActionResult<bool>> CheckUserRecommendationsPreferences()
    {
        var userId = HttpContext.User.GetId();
        return Ok(await _userRepository.GetUserRecommendationsPreferences(userId) != null);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserForAdminDto>>> GetUsersForAdministrator(string? searchText, int page = 1, int pageSize = 10)
    {
        return Ok(_mapper.Map<List<UserForAdminDto>>(await _userRepository.GetUsersWithoutAdministratorRole(searchText ?? "", page, pageSize)));
    }

    [Authorize]
    [HttpGet("recommendations-preferences")]
    public async Task<ActionResult<RecommendationsPreferences>> GetUserRecommendationsPreferences()
    {
        var userId = HttpContext.User.GetId();
        return Ok(_mapper.Map<RecommendationsPreferencesDto>(await _userRepository.GetUserRecommendationsPreferences(userId)));
    }

    [Authorize]
    [HttpPut("recommendations-preferences")]
    public async Task<IActionResult> SaveUserRecommendationsPreferences(CreateRecommendationsPreferencesDto createRecommendationsPreferencesDto)
    {
        await _userRepository.CreateOrUpdateUserRecommendationsPreferences(_mapper.Map<RecommendationsPreferences>(createRecommendationsPreferencesDto));
        return Ok();
    }

    [HttpPatch("{id:long}/ban")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BanUser(BannedStatusDto bannnedStatusDto, long id)
    {
        if (bannnedStatusDto.IsBanned)
        {
            await _refreshTokenRepository.DeleteByUserIdAsync(id);
        }
        await _userRepository.SetUserBannedStatus(bannnedStatusDto.IsBanned, id);
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
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
        await _refreshTokenRepository.DeleteByUserIdAsync(userId);
        return await _userRepository.DeleteAsync(userId)
            ? NoContent()
            : NotFound();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteUser(long id)
    {
        await _refreshTokenRepository.DeleteByUserIdAsync(id);
        return await _userRepository.DeleteAsync(id)
            ? NoContent()
            : NotFound();
    }
}