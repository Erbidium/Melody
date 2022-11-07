using Melody.Core.Entities;
using Melody.SharedKernel.Interfaces;

namespace Melody.Core.Interfaces;

public interface IGenreRepository : IReadRepository<Genre, long>
{
}
