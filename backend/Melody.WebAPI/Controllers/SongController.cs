using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Interfaces;
using Melody.Core.ValueObjects;
using Melody.WebAPI.DTO.Genre;
using Melody.WebAPI.DTO.Song;
using Melody.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _newSongDtoValidator;
    private readonly ISongRepository _songRepository;
    private readonly ISongService _songService;

    public SongController(ISongRepository songRepository, IGenreRepository genreRepository, IMapper mapper,
        IValidator<NewSongDto> newSongDtoValidator, ISongService songService)
    {
        _songRepository = songRepository;
        _genreRepository = genreRepository;
        _mapper = mapper;
        _newSongDtoValidator = newSongDtoValidator;
        _songService = songService;
    }

    [Authorize]
    [HttpGet("favourite")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetFavouriteUserSongs()
    {
        var userId = HttpContext.User.GetId();
        return Ok(_mapper.Map<List<SongDto>>(await _songRepository.GetFavouriteUserSongs(userId)));
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetAllSongs()
    {
        return Ok(_mapper.Map<List<SongDto>>(await _songRepository.GetAll()));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsUploadedByUser()
    {
        var userId = HttpContext.User.GetId();
        return Ok(_mapper.Map<List<SongDto>>(
            await _songRepository.GetSongsUploadedByUserId(userId)));
    }

    [Authorize]
    [HttpGet("favourite-and-uploaded")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetFavoriteAndUploadedSongs()
    {
        var userId = HttpContext.User.GetId();
        return Ok(_mapper.Map<List<SongDto>>(
            await _songRepository.GetFavouriteAndUploadedUserSongs(userId)));
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
        var userId = HttpContext.User.GetId();
        await _songRepository.SaveNewSongListening(newListeningDto.SongId, userId);
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

        var userId = HttpContext.User.GetId();
        var extension = Path.GetExtension(uploadedSoundFile.FileName);

        var result = await _songService.Upload(
            uploadedSoundFile.OpenReadStream(),
            new NewSongData(userId, newSong.Name, newSong.AuthorName, newSong.Year,
                newSong.GenreId, extension));
        return result.ToOk(song => _mapper.Map<SongDto>(song));
    }

    [Authorize]
    [HttpPatch("{id:long}/like")]
    public async Task<IActionResult> UpdateSongStatus(SongStatusDto songStatusDto, long id)
    {
        var userId = HttpContext.User.GetId();
        if (songStatusDto.IsLiked)
            await _songRepository.CreateFavouriteSong(id, userId);
        else
            await _songRepository.DeleteFavouriteSong(id, userId);
        return Ok();
    }

    [Authorize]
    [HttpDelete("favourite/{id:long}")]
    public async Task<IActionResult> DeleteFavouriteSong(long id)
    {
        var userId = HttpContext.User.GetId();
        return await _songRepository.DeleteFavouriteSong(id, userId)
            ? Ok()
            : BadRequest();
        
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteSong(long id)
    {
        var userId = HttpContext.User.GetId();
        return await _songRepository.DeleteUploadedSong(id, userId)
            ? NoContent()
            : BadRequest();
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("admin/{id:long}")]
    public async Task<IActionResult> DeleteSongByAdministrator(long id)
    {
        return await _songRepository.Delete(id)
            ? NoContent()
            : BadRequest();
    }
}