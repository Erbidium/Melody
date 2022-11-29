using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.WebAPI.DTO.Playlist;
using Melody.WebAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController : ControllerBase
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<NewPlaylistDto> _newPlaylistDtoValidator;
    private readonly IValidator<UpdatePlaylistDto> _updatePlaylistDtoValidator;
    private readonly ITokenService _tokenService;

    public PlaylistController(IPlaylistRepository playlistRepository, IMapper mapper,
        IValidator<NewPlaylistDto> newPlaylistDtoValidator, IValidator<UpdatePlaylistDto> updatePlaylistDtoValidator,
        ITokenService tokenService)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
        _newPlaylistDtoValidator = newPlaylistDtoValidator;
        _updatePlaylistDtoValidator = updatePlaylistDtoValidator;
        _tokenService = tokenService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylists()
    {
        return Ok(await _playlistRepository.GetAll());
    }

    [Authorize]
    [HttpGet("created")]
    public async Task<ActionResult<IEnumerable<Playlist>>> GetPlaylistsCreatedByUser()
    {
        var identity = HttpContext.User.Identity as ClaimsIdentity;
        var currentUserFromToken = _tokenService.GetCurrentUser(identity);
        var playlists = await _playlistRepository.GetPlaylistsCreatedByUser(currentUserFromToken.UserId);
        return Ok(playlists.Select(p => new PlaylistWithPerformersDto
        {
            Id = p.Id, Name = p.Name, AuthorId = p.AuthorId, PerformersNames = p.Songs.Select(s => s.Name).ToList()
        }));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Playlist>> GetPlaylist(long id)
    {
        var playlist = await _playlistRepository.GetById(id);
        if (playlist is null)
        {
            throw new KeyNotFoundException("Playlist is not found");
        }

        return Ok(playlist);
    }

    [Authorize]
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePlaylist([FromBody] UpdatePlaylistDto playlist, long id)
    {
        var result = await _updatePlaylistDtoValidator.ValidateAsync(playlist);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        await _playlistRepository.Update(new UpdatePlaylist
            { Id = id, Name = playlist.Name, SongIds = playlist.SongIds });
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(long id)
    {
        await _playlistRepository.Delete(id);
        return NoContent();
    }
}