using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Core.ValueObjects;
using Melody.WebAPI.DTO.Genre;
using Melody.WebAPI.DTO.Song;
using Melody.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SongController : ControllerBase
{
    private readonly IGenreRepository _genreRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _newSongDtoValidator;
    private readonly ISongRepository _songRepository;
    private readonly IUserRepository _userRepository;
    private readonly ISongService _songService;
    private readonly IElasticClient _elasticClient;

    public SongController(ISongRepository songRepository, IGenreRepository genreRepository, IUserRepository userRepository, IMapper mapper,
        IValidator<NewSongDto> newSongDtoValidator, ISongService songService, IElasticClient elasticClient)
    {
        _songRepository = songRepository;
        _genreRepository = genreRepository;
        _userRepository = userRepository;
        _mapper = mapper;
        _newSongDtoValidator = newSongDtoValidator;
        _songService = songService;
        _elasticClient = elasticClient;
    }

    [Authorize]
    [HttpGet("favourite")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetFavouriteUserSongs()
    {
        var userId = HttpContext.User.GetId();
        return Ok(_mapper.Map<List<SongDto>>(await _songRepository.GetFavouriteUserSongs(userId)));
    }

    [Authorize]
    [HttpGet("recommendations")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetRecommendedSongs(int page = 1, int pageSize = 10)
    {
        var userId = HttpContext.User.GetId();
        var recommendationsPreferences = await _userRepository.GetUserRecommendationsPreferences(userId);
        if (recommendationsPreferences is null)
        {
            return NotFound();
        }

        var queryContainers = new List<QueryContainer>();
        var descriptor = new QueryContainerDescriptor<Song>();

        if (recommendationsPreferences.StartYear.HasValue)
        {
            queryContainers.Add(descriptor.Range(selector => selector
                .Field(song => song.Year)
                .GreaterThanOrEquals(recommendationsPreferences.StartYear)));
        }
        if (recommendationsPreferences.EndYear.HasValue)
        {
            queryContainers.Add(descriptor.Range(selector => selector
                .Field(song => song.Year)
                .LessThanOrEquals(recommendationsPreferences.EndYear)));
        }
        
        const int deltaSeconds = 30;
        if (recommendationsPreferences.AverageDurationInMinutes.HasValue)
        {
            var startDuration = recommendationsPreferences.AverageDurationInMinutes.Value - deltaSeconds > 0
                ? recommendationsPreferences.AverageDurationInMinutes.Value - deltaSeconds
                : 1;
            
            queryContainers.Add(descriptor.Range(selector => selector
                .Field(song => song.Duration)
                .GreaterThanOrEquals(startDuration)
                .LessThanOrEquals(recommendationsPreferences.AverageDurationInMinutes + deltaSeconds)));
        }

        if (!string.IsNullOrWhiteSpace(recommendationsPreferences.AuthorName))
        {
            var author = recommendationsPreferences.AuthorName;
            queryContainers.Add(descriptor.Match(selector => selector
                .Field(song => song.AuthorName)
                .Analyzer(author)
                .Fuzziness(Fuzziness.Auto)));
        }
        
        queryContainers.Add(descriptor.Term(selector => selector
            .Field(song => song.GenreId)
            .Value(recommendationsPreferences.GenreId)));

        var searchRequest = new SearchDescriptor<SongDto>();
        searchRequest
            .Index("songs")
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.Bool(b => b.Must(queryContainers.ToArray())));
        
        var songs = await _elasticClient.SearchAsync<SongDto>(searchRequest);
        if (!songs.IsValid)
        {
            return BadRequest();
        }
        return Ok(songs.Documents);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetAllSongs(string? searchText, int page = 1, int pageSize = 10)
    {
        return Ok(_mapper.Map<List<SongDto>>(await _songRepository.GetAll(searchText ?? "", page, pageSize)));
    }

    [Authorize]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsUploadedByUser(int page = 1, int pageSize = 10)
    {
        var userId = HttpContext.User.GetId();
        return Ok(_mapper.Map<List<SongDto>>(
            await _songRepository.GetSongsUploadedByUserId(userId, page, pageSize)));
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
            : NotFound();
        
    }

    [Authorize]
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteSong(long id)
    {
        var userId = HttpContext.User.GetId();
        return await _songRepository.DeleteUploadedSong(id, userId)
            ? NoContent()
            : NotFound();
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