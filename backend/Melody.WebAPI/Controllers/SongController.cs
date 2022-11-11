using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Records;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.DTO.Song;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongRepository _songRepository;
    private readonly TokenService _tokenService;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _newSongDtoValidator;
    private readonly IValidator<UpdateSongDto> _updateSongDtoValidator;
    private const string soundExtension = ".mp3";
    private const string folderName = "Sounds";
    private const long userUploadsLimit = 1000000000;

    public SongController(ISongRepository songRepository, IMapper mapper, IValidator<NewSongDto> newSongDtoValidator, IValidator<UpdateSongDto> updateSongDtoValidator, TokenService tokenService, UserManager<UserIdentity> userManager)
    {
        _songRepository = songRepository;
        _mapper = mapper;
        _newSongDtoValidator = newSongDtoValidator;
        _updateSongDtoValidator = updateSongDtoValidator;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
    {
        return Ok(await _songRepository.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Song>> GetSong(long id)
    {
        var song = await _songRepository.GetById(id);
        if (song is null)
        {
            throw new KeyNotFoundException("Song is not found");
        }
        return Ok(song);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Song>> CreateSong(NewSongDto newSong)
    {
        // validate path
        ValidationResult result = await _newSongDtoValidator.ValidateAsync(newSong);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var user = await _userManager.FindByEmailAsync(currentUserFromToken.Email);
        var song = new Song(user.Id, newSong.Name, newSong.Path, newSong.AuthorName, newSong.Year, newSong.GenreId, newSong.SizeInBytes, DateTime.Now);
        return Ok(await _songRepository.Create(song));
    }

    [Authorize]
    [HttpPost("Upload")]
    public async Task<IActionResult> UploadFile(IFormFile uploadedSoundFile)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var user = await _userManager.FindByEmailAsync(currentUserFromToken.Email);

        var userUploadsSize = await _songRepository.GetTotalBytesSumUploadsByUser(user.Id);
        if (userUploadsSize + uploadedSoundFile.Length > userUploadsLimit)
        {
            return BadRequest("You have reached your upload limit 1 Gb");
        }

        var extension = Path.GetExtension(uploadedSoundFile.FileName);
        if (extension != soundExtension)
        {
            return BadRequest();
        }
        
        return Ok(new { Path = await WriteFile(uploadedSoundFile), Size = uploadedSoundFile.Length });

    }

    [HttpPut]
    public async Task<IActionResult> UpdateSong(UpdateSongDto song)
    {
        ValidationResult result = await _updateSongDtoValidator.ValidateAsync(song);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        await _songRepository.Update(_mapper.Map<Song>(song));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSong(long id)
    {
        await _songRepository.Delete(id);
        // TODO: delete files
        return NoContent();
    }

    private async Task<string> WriteFile(IFormFile uploadedSoundFile)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var pathToSave = Path.Combine(currentDirectory, folderName);
        if (!Directory.Exists(pathToSave))
        {
            Directory.CreateDirectory(pathToSave);
        }

        //string fileName = Path.GetFileName(uploadedSoundFile.FileName);
        var guidFileName = Guid.NewGuid().ToString() + soundExtension;
        var guidSubFolders = string.Empty;
        for (int i = 0; i < 6; i += 2)
        {
            var guidSubstr = guidFileName.Substring(i, 2);
            guidSubFolders = Path.Combine(guidSubFolders, guidSubstr);
            var currentPath = Path.Combine(pathToSave, guidSubFolders);
            if (!Directory.Exists(currentPath))
            {
                Directory.CreateDirectory(currentPath);
            }
        }
        var fullPath = Path.Combine(pathToSave, guidSubFolders, guidFileName);
        using var stream = new FileStream(fullPath, FileMode.Create);
        await uploadedSoundFile.CopyToAsync(stream);
        return Path.Combine(folderName, guidSubFolders, guidFileName);
    }
}