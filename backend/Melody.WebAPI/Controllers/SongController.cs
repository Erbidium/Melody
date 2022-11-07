using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.DTO.Song;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongRepository _songRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _newSongDtoValidator;
    private readonly IValidator<UpdateSongDto> _updateSongDtoValidator;

    public SongController(ISongRepository songRepository, IMapper mapper, IValidator<NewSongDto> newSongDtoValidator, IValidator<UpdateSongDto> updateSongDtoValidator)
    {
        _songRepository = songRepository;
        _mapper = mapper;
        _newSongDtoValidator = newSongDtoValidator;
        _updateSongDtoValidator = updateSongDtoValidator;
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

    [HttpPost]
    public async Task<ActionResult<Song>> CreateSong(NewSongDto song)
    {
        ValidationResult result = await _newSongDtoValidator.ValidateAsync(song);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        return Ok(await _songRepository.Create(_mapper.Map<Song>(song)));
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
        return NoContent();
    }
}