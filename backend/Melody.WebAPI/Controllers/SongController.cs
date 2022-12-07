using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.DTO.Song;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;
using Melody.WebAPI.DTO.Genre;
using NAudio.Wave;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongRepository _songRepository;
    private readonly IGenreRepository _genreRepository;
    private readonly ITokenService _tokenService;
    private readonly UserManager<UserIdentity> _userManager;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _newSongDtoValidator;
    private readonly IValidator<UpdateSongDto> _updateSongDtoValidator;
    private const string soundExtension = ".mp3";
    private const string folderName = "Sounds";
    private const long userUploadsLimit = 1000000000;

    public SongController(ISongRepository songRepository, IGenreRepository genreRepository, IMapper mapper,
        IValidator<NewSongDto> newSongDtoValidator,
        IValidator<UpdateSongDto> updateSongDtoValidator, ITokenService tokenService,
        UserManager<UserIdentity> userManager)
    {
        _songRepository = songRepository;
        _genreRepository = genreRepository;
        _mapper = mapper;
        _newSongDtoValidator = newSongDtoValidator;
        _updateSongDtoValidator = updateSongDtoValidator;
        _tokenService = tokenService;
        _userManager = userManager;
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsUploadedByUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        return Ok(_mapper.Map<List<SongDto>>(await _songRepository.GetSongsUploadedByUserId(currentUserFromToken.UserId)));
    }

    [HttpGet("file/{id:long}")]
    public async Task<FileStreamResult> GetSong(long id)
    {
        var song = await _songRepository.GetById(id);
        if (song is null)
        {
            throw new KeyNotFoundException("Song is not found");
        }

        var fileStream = System.IO.File.OpenRead(song.Path);
        return File(
            fileStream: fileStream,
            contentType: "audio/mpeg",
            enableRangeProcessing: true
        );
    }

    [Authorize]
    [HttpGet("genres")]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
    {
        return Ok(_mapper.Map<List<GenreDto>>(await _genreRepository.GetAll()));
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<SongDto>> CreateSong([FromForm] NewSongDto newSong, IFormFile uploadedSoundFile)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);

        var userUploadsSize = await _songRepository.GetTotalBytesSumUploadsByUser(currentUserFromToken.UserId);
        if (userUploadsSize + uploadedSoundFile.Length > userUploadsLimit)
        {
            throw new Exception("You have reached your upload limit 1 Gb");
        }

        var extension = Path.GetExtension(uploadedSoundFile.FileName);
        if (extension != soundExtension)
        {
            throw new Exception("Your sound file has wrong extension");
        }

        var path = await WriteFile(uploadedSoundFile);
        var reader = new MediaFoundationReader(path);
        var duration = reader.TotalTime;

        // validate path
        var result = await _newSongDtoValidator.ValidateAsync(newSong);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var song = new Song(currentUserFromToken.UserId, newSong.Name, path, newSong.AuthorName, newSong.Year,
            newSong.GenreId,
            uploadedSoundFile.Length, DateTime.Now, duration);
        return Ok(_mapper.Map<SongDto>(await _songRepository.Create(song)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSong(UpdateSongDto song)
    {
        var result = await _updateSongDtoValidator.ValidateAsync(song);
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
        return NoContent();
    }

    private static async Task<string> WriteFile(IFormFile uploadedSoundFile)
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var pathToSave = Path.Combine(currentDirectory, folderName);
        if (!Directory.Exists(pathToSave))
        {
            Directory.CreateDirectory(pathToSave);
        }
        var guidFileName = Guid.NewGuid() + soundExtension;
        var guidSubFolders = string.Empty;
        for (var i = 0; i < 6; i += 2)
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
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await uploadedSoundFile.CopyToAsync(stream);
        return Path.Combine(folderName, guidSubFolders, guidFileName);
    }
}