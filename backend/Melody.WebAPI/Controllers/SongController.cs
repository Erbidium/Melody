using AutoMapper;
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

    public SongController(ISongRepository songRepository, IMapper mapper)
    {
        _songRepository = songRepository;
        _mapper = mapper;
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
        return Ok(await _songRepository.Create(_mapper.Map<Song>(song)));
    }
}