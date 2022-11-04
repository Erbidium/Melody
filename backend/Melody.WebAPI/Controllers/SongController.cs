using Melody.Core.Entities;
using Melody.Core.ValueObjects;
using Melody.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers
{
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
        public async Task<IActionResult> GetSongs()
        {
            return Ok(await _songRepository.GetSongs());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetSong(long id)
        {
            return Ok(await _songRepository.GetSong(id));
        }

        [HttpPost]
        public async Task<IActionResult> CreateSong(SongInfo song)
        {
            var createdSong = await _songRepository.CreateSong(song);
            return Ok(createdSong);
        }
    }
}