using Melody.Infrastructure.Data.Entities;
using Melody.Infrastructure.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Melody.WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SongController : ControllerBase
    {
        private readonly SongRepository _songRepository;

        public SongController(SongRepository songRepository)
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
        public async Task<IActionResult> CreateSong(Song song)
        {
            var createdSong = await _songRepository.CreateSong(song);
            return Ok(createdSong);
        }
    }
}