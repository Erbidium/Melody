using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Interfaces;
using Melody.Core.ValueObjects;
using Melody.WebAPI.DTO.Playlist;
using Melody.WebAPI.DTO.Song;
using Melody.WebAPI.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IValidator<NewPlaylistDto> _newPlaylistDtoValidator;
    private readonly IPlaylistRepository _playlistRepository;
    private readonly ISongRepository _songRepository;
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
    public async Task<ActionResult<IEnumerable<PlaylistDto>>> GetPlaylists()
    {
        return Ok(_mapper.Map<PlaylistDto>(await _playlistRepository.GetAll()));
    }

    [HttpGet("{id:long}/new-songs-to-add")]
    public async Task<ActionResult<IEnumerable<SongDto>>> GetSongsToAddToPlaylist(long id)
    {
        var userId = HttpContext.User.GetId();
        var songs = await _songRepository.GetSongsForPlaylistToAdd(id, userId);
        return Ok(_mapper.Map<List<SongDto>>(songs));
    }

    [HttpGet("created")]
    public async Task<ActionResult<IEnumerable<FavouritePlaylistWithPerformersDto>>> GetPlaylistsCreatedByUser()
    {
        var userId = HttpContext.User.GetId();
        var playlists = await _playlistRepository.GetPlaylistsCreatedByUser(userId);
        return Ok(_mapper.Map<List<FavouritePlaylistWithPerformersDto>>(playlists));
    }

    [HttpGet("favourite")]
    public async Task<ActionResult<IEnumerable<FavouritePlaylistWithPerformersDto>>> GetFavouritePlaylists()
    {
        var userId = HttpContext.User.GetId();
        var playlists = await _playlistRepository.GetFavouritePlaylists(userId);
        return Ok(_mapper.Map<List<FavouritePlaylistWithPerformersDto>>(playlists));
    }

    [HttpGet("{id:long}")]
    public async Task<ActionResult<FavouritePlaylistDto>> GetPlaylist(long id)
    {
        var userId = HttpContext.User.GetId();
        var playlist = await _playlistRepository.GetById(id, userId);
        if (playlist is null) throw new KeyNotFoundException("Playlist is not found");

        return Ok(_mapper.Map<FavouritePlaylistDto>(playlist));
    }

    [HttpPost]
    public async Task<IActionResult> CreatePlaylist(NewPlaylistDto playlist)
    {
        var userId = HttpContext.User.GetId();
        var result = await _newPlaylistDtoValidator.ValidateAsync(playlist);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        await _playlistRepository.Create(new CreatePlaylist(playlist.Name, userId,
            playlist.SongIds));
        return Ok();
    }

    [Authorize]
    [HttpPatch("{id:long}/like")]
    public async Task<IActionResult> UpdatePlaylistStatus(PlaylistStatusDto playlistStatusDto, long id)
    {
        var userId = HttpContext.User.GetId();
        if (playlistStatusDto.IsLiked)
            await _playlistRepository.CreateFavouritePlaylist(id, userId);
        else
            await _playlistRepository.DeleteFavouritePlaylist(id, userId);
        return Ok();
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> UpdatePlaylist([FromBody] UpdatePlaylistDto updatePlaylistDto, long id)
    {
        var userId = HttpContext.User.GetId();
        var playlist = await _playlistRepository.GetById(id, userId);
        if (playlist == null || playlist.AuthorId != userId) return BadRequest();
        await _playlistRepository.AddSongs(id, updatePlaylistDto.NewSongIds);
        return Ok();
    }

    [HttpDelete("{id:long}/song/{songId:long}")]
    public async Task<IActionResult> DeleteSongFromPlaylist(long id, long songId)
    {
        await _playlistRepository.DeleteSong(id, songId);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("favourite/{id:long}")]
    public async Task<IActionResult> DeleteFavouritePlaylist(long id)
    {
        var userId = HttpContext.User.GetId();
        await _playlistRepository.DeleteFavouritePlaylist(id, userId);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeletePlaylist(long id)
    {
        var userId = HttpContext.User.GetId();
        var playlist = await _playlistRepository.GetById(id, userId);
        if (playlist == null || playlist.AuthorId != userId) return BadRequest();
        await _playlistRepository.Delete(id);
        return NoContent();
    }
}