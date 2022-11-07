using Melody.Core.Entities;
using Melody.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class SongController : ControllerBase
{
    private readonly ISongRepository _songRepository;

    public SongController(ISongRepository songRepository)
    {
        _songRepository = songRepository;
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
    public async Task<ActionResult<Song>> CreateSong(Song song)
    {
        var createdSong = await _songRepository.Create(song);
        return Ok(createdSong);
    }
}