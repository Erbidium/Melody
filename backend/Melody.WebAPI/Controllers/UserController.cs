using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Interfaces;
using Melody.WebAPI.DTO.Auth.Models;
using Melody.WebAPI.DTO.RecommendationsPreferences;
using Melody.WebAPI.DTO.User;
using Melody.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly Core.Interfaces.IUserRepository _userRepository;
    private readonly IRefreshTokenRepository _refreshTokenRepository;
    private readonly IMapper _mapper;
    private readonly ITokenService _tokenService;
    private readonly IValidator<CreateRecommendationsPreferencesDto> _createRecommendationsPreferencesDtoValidator;
    private readonly IValidator<UserRegister> _userRegisterValidator;

    public UserController(
        IUserService userService,
        Core.Interfaces.IUserRepository userRepository,
        IMapper mapper,
        IRefreshTokenRepository refreshTokenRepository,
        IValidator<CreateRecommendationsPreferencesDto> createRecommendationsPreferencesDtoValidator,
        IValidator<UserRegister> userRegisterValidator,
        ITokenService tokenService)
    {
        _userService = userService;
        _userRepository = userRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _mapper = mapper;
        _tokenService = tokenService;
        _createRecommendationsPreferencesDtoValidator = createRecommendationsPreferencesDtoValidator;
        _userRegisterValidator = userRegisterValidator;
    }

    [AllowAnonymous]
    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] UserRegister userRegister)
    {
        var validationResult = await _userRegisterValidator.ValidateAsync(userRegister);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var user = new User(
            userRegister.UserName,
            userRegister.Email,
            userRegister.PhoneNumber
            );
        
        return (await _userService.Register(user, userRegister.Password)).ToStatusCode(201);
    }

    [Authorize]
    [HttpGet("{id:long}")]
    public async Task<ActionResult<UserDto>> GetUserById(long id)
    {
        var userRoles = HttpContext.User.GetUserRoles();
        var user = await _userService.GetUserById(id);

        if (user is null || (!userRoles.Contains("Admin") && user.IsBanned))
            return NotFound("User is not found");

        return Ok(_mapper.Map<UserDto>(user));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<UserDto>> GetCurrentUser()
    {
        var userId = HttpContext.User.GetId();
        var user = await _userService.GetUserById(userId);
        return Ok(_mapper.Map<UserDto>(user));
    }

    [AllowAnonymous]
    [HttpGet("check-email")]
    public async Task<ActionResult<bool>> CheckExistingEmail(string email)
    {
        return Ok(await _userService.EmailIsUsed(email));
    }

    [AllowAnonymous]
    [HttpGet("check-username")]
    public async Task<ActionResult<bool>> CheckExistingUsername(string username)
    {
        return Ok(await _userService.UsernameIsUsed(username));
    }

    [Authorize]
    [HttpGet("check-preferences")]
    public async Task<ActionResult<bool>> CheckUserRecommendationsPreferences()
    {
        var userId = HttpContext.User.GetId();
        return Ok(await _userRepository.GetUserRecommendationsPreferences(userId) != null);
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UserForAdminDto>>> GetUsersForAdministrator(string? searchText,
        int page = 1, int pageSize = 10)
    {
        return Ok(_mapper.Map<List<UserForAdminDto>>(
            await _userRepository.GetUsersWithoutAdministratorRole(searchText ?? "", page, pageSize)));
    }

    [Authorize]
    [HttpGet("recommendations-preferences")]
    public async Task<ActionResult<RecommendationsPreferences>> GetUserRecommendationsPreferences()
    {
        var userId = HttpContext.User.GetId();
        var preferences = await _userRepository.GetUserRecommendationsPreferences(userId);

        return preferences is null
            ? NotFound()
            : Ok(_mapper.Map<RecommendationsPreferencesDto>(preferences));
    }

    [Authorize]
    [HttpPut("recommendations-preferences")]
    public async Task<IActionResult> SaveUserRecommendationsPreferences(
        [FromBody]CreateRecommendationsPreferencesDto createRecommendationsPreferences)
    {
        var validationResult =
            await _createRecommendationsPreferencesDtoValidator.ValidateAsync(createRecommendationsPreferences);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var userId = HttpContext.User.GetId();
        var preferences = new RecommendationsPreferences(
            userId,
            createRecommendationsPreferences.GenreId,
            createRecommendationsPreferences.AuthorName,
            createRecommendationsPreferences.StartYear,
            createRecommendationsPreferences.EndYear,
            createRecommendationsPreferences.AverageDurationInMinutes);
        return await _userRepository.CreateOrUpdateUserRecommendationsPreferences(preferences)
            ? Ok()
            : BadRequest();
    }

    [HttpPatch("{id:long}/ban")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> BanUser(BannedStatusDto bannedStatusDto, long id)
    {
        if (bannedStatusDto.IsBanned) await _refreshTokenRepository.DeleteByUserIdAsync(id);
        await _userRepository.SetUserBannedStatus(bannedStatusDto.IsBanned, id);
        return Ok();
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser()
    {
        var userId = HttpContext.User.GetId();
        var email = await _userService.GetUserEmail(userId);
        var refreshToken = await _tokenService.GenerateRefreshToken(email, true);
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