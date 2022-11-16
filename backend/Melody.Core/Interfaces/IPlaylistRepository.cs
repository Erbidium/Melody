using Melody.Core.Entities;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist, long>
{
}