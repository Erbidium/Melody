using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.WebAPI.DTO.Playlist;
using Melody.WebAPI.DTO.Song;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ISongRepository _songRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<NewPlaylistDto> _newPlaylistDtoValidator;
    private readonly ITokenService _tokenService;

    public PlaylistController(IPlaylistRepository playlistRepository, ISongRepository songRepository, IMapper mapper,
        IValidator<NewPlaylistDto> newPlaylistDtoValidator,
        ITokenService tokenService)
    {
        _playlistRepository = playlistRepository;
        _songRepository = songRepository;
        _mapper = mapper;
        _newPlaylistDtoValidator = newPlaylistDtoValidator;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
    {
        return Ok(_mapper.Map<PlaylistDto>(await _playlistRepository.GetAll()));
    }

    [HttpGet("{id:long}/new-songs-to-add")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsToAddToPlaylist(long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var songs = await _songRepository.GetSongsForPlaylistToAdd(id, currentUserFromToken.UserId);
        return Ok(_mapper.Map<List<SongDto>>(songs));
    }

    [HttpGet("created")]
    public async Task<ActionResult<IEnumerable<PlaylistWithPerformersDto>>> GetPlaylistsCreatedByUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var playlists = await _playlistRepository.GetPlaylistsCreatedByUser(currentUserFromToken.UserId);
        return Ok(playlists.Select(p => new PlaylistWithPerformersDto
        {
            Id = p.Id, Name = p.Name, AuthorId = p.AuthorId, PerformersNames = p.Songs.Select(s => s.AuthorName).ToList()
        }));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<PlaylistDto>> GetPlaylist(long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var playlist = await _playlistRepository.GetById(id, currentUserFromToken.UserId);
        if (playlist is null)
        {
            throw new KeyNotFoundException("Playlist is not found");
        }

        return Ok(_mapper.Map<PlaylistDto>(playlist));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlaylist(NewPlaylistDto playlist)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var result = await _newPlaylistDtoValidator.ValidateAsync(playlist);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        await _playlistRepository.Create(new CreatePlaylist
            { Name = playlist.Name, SongIds = playlist.SongIds, AuthorId = currentUserFromToken.UserId });
        return Ok();
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdatePlaylist([FromBody] UpdatePlaylistDto updatePlaylistDto, long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var playlist = await _playlistRepository.GetById(id, currentUserFromToken.UserId);
        if (playlist == null || playlist.AuthorId != currentUserFromToken.UserId)
        {
            return BadRequest();
        }
        await _playlistRepository.AddSongs(id, updatePlaylistDto.NewSongIds);
        return Ok();
    }

    [HttpDelete("{id:long}/song/{songId:long}")]
    public async Task<IActionResult> DeleteSongFromPlaylist(long id, long songId)
    {
        await _playlistRepository.DeleteSong(id, songId);
        return NoContent();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePlaylist(long id)
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var playlist = await _playlistRepository.GetById(id, currentUserFromToken.UserId);
        if (playlist == null || playlist.AuthorId != currentUserFromToken.UserId)
        {
            return BadRequest();
        }
        await _playlistRepository.Delete(id);
        return NoContent();
    }
}