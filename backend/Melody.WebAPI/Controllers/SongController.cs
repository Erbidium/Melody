using AutoMapper;
using FluentValidation;
using FluentValidation.AspNetCore;
using FluentValidation.Results;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.DTO.Song;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongRepository _songRepository;
    private readonly IMapper _mapper;
    private readonly IValidator<NewSongDto> _validator;

    public SongController(ISongRepository songRepository, IMapper mapper, IValidator<NewSongDto> validator)
    {
        _songRepository = songRepository;
        _mapper = mapper;
        _validator = validator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Song>>> GetSongs()
    {
        return Ok(await _songRepository.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Song>> GetSong(long id)
    {
        return Ok(await _songRepository.GetById(id));
    }

    [HttpPost]
    public async Task<ActionResult<Song>> CreateSong(NewSongDto song)
    {
        ValidationResult result = await _validator.ValidateAsync(song);
        if (!result.IsValid)
        {
            result.AddToModelState(ModelState);
            return BadRequest(ModelState);
        }
        return Ok(await _songRepository.Create(_mapper.Map<Song>(song)));
    }
}