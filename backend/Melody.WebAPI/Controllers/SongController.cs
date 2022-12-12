using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Entities;
using Melody.WebAPI.DTO.Song;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Melody.Core.Interfaces;
using Melody.Core.ValueObjects;
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
    private readonly ISongService _songService;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _newSongDtoValidator;
    private readonly IValidator<UpdateSongDto> _updateSongDtoValidator;

    public SongController(ISongRepository songRepository, IGenreRepository genreRepository, IMapper mapper,
        IValidator<NewSongDto> newSongDtoValidator, IValidator<UpdateSongDto> updateSongDtoValidator,
        ITokenService tokenService, ISongService songService)
    {
        _songRepository = songRepository;
        _genreRepository = genreRepository;
        _mapper = mapper;
        _newSongDtoValidator = newSongDtoValidator;
        _updateSongDtoValidator = updateSongDtoValidator;
        _tokenService = tokenService;
        _songService = songService;
    }

    [Authorize]
    [HttpGet("favourite")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetFavouriteUserSongs()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        return Ok(_mapper.Map<List<SongDto>>(await _songRepository.GetFavouriteUserSongs(currentUserFromToken.UserId)));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsUploadedByUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        return Ok(_mapper.Map<List<SongDto>>(
            await _songRepository.GetSongsUploadedByUserId(currentUserFromToken.UserId)));
    }

    [Authorize]
    [HttpGet("favourite-and-uploaded")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetFavoriteAndUploadedSongs()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        return Ok(_mapper.Map<List<SongDto>>(
            await _songRepository.GetFavouriteAndUploadedUserSongs(currentUserFromToken.UserId)));
    }

    [HttpGet("file/{id:long}")]
    public async Task<FileStreamResult> GetSong(long id)
    {
        var song = await _songRepository.GetById(id);
        if (song is null) throw new KeyNotFoundException("Song is not found");

        var fileStream = System.IO.File.OpenRead(song.Path);
        return File(
            fileStream,
            "audio/mpeg",
            true
        );
    }

    [Authorize]
    [HttpPost("new-listening")]
    public async Task<IActionResult> SaveNewListening(NewListeningDto newListeningDto)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        await _songRepository.SaveNewSongListening(newListeningDto.SongId, currentUserFromToken.UserId);
        return Ok();
    }

    [Authorize]
    [HttpGet("genres")]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
    {
        return Ok(_mapper.Map<List<GenreDto>>(await _genreRepository.GetAll()));
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateSong([FromForm] NewSongDto newSong, IFormFile uploadedSoundFile)
    {
        var validationResult = await _newSongDtoValidator.ValidateAsync(newSong);
        if (!validationResult.IsValid)
        {
            validationResult.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var extension = Path.GetExtension(uploadedSoundFile.FileName);

        var result = await _songService.Upload(
            uploadedSoundFile.OpenReadStream(),
            new NewSongData(currentUserFromToken.UserId, newSong.Name, newSong.AuthorName, newSong.Year,
                newSong.GenreId, extension));
        return result.Match<IActionResult>(
            song => Ok(_mapper.Map<SongDto>(song)),
            exception => BadRequest(exception.Message));
    }

    [Authorize]
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

    [Authorize]
    [HttpPatch("{id:long}/like")]
    public async Task<IActionResult> UpdateSongStatus(SongStatusDto songStatusDto, long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        if (songStatusDto.IsLiked)
            await _songRepository.CreateFavouriteSong(id, currentUserFromToken.UserId);
        else
            await _songRepository.DeleteFavouriteSong(id, currentUserFromToken.UserId);
        return Ok();
    }

    [Authorize]
    [HttpDelete("favourite/{id:long}")]
    public async Task<IActionResult> DeleteFavouriteSong(long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        await _songRepository.DeleteFavouriteSong(id, currentUserFromToken.UserId);
        return Ok();
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteSong(long id)
    {
        await _songRepository.Delete(id);
        return NoContent();
    }
}