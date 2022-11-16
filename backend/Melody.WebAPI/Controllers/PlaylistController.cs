using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.WebAPI.DTO.Playlist;
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

    public PlaylistController(IPlaylistRepository playlistRepository, IMapper mapper,
        IValidator<NewPlaylistDto> newPlaylistDtoValidator, IValidator<UpdatePlaylistDto> updatePlaylistDtoValidator)
    {
        _playlistRepository = playlistRepository;
        _mapper = mapper;
        _newPlaylistDtoValidator = newPlaylistDtoValidator;
        _updatePlaylistDtoValidator = updatePlaylistDtoValidator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Song>>> GetPlaylist()
    {
        return Ok(await _playlistRepository.GetAll());
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

    [HttpPost]
    public async Task<ActionResult<Song>> CreatePlaylist(NewPlaylistDto playlist)
    {
        ValidationResult result = await _newPlaylistDtoValidator.ValidateAsync(playlist);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        return Ok(await _playlistRepository.Create(_mapper.Map<Playlist>(playlist)));
    }

    [HttpPut]
    public async Task<IActionResult> UpdatePlaylist(UpdatePlaylistDto playlist)
    {
        ValidationResult result = await _updatePlaylistDtoValidator.ValidateAsync(playlist);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }

        await _playlistRepository.Update(_mapper.Map<Playlist>(playlist));
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePlaylist(long id)
    {
        await _playlistRepository.Delete(id);
        return NoContent();
    }
}