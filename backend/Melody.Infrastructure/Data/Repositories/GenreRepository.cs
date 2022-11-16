using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Records;

namespace Melody.Infrastructure.Data.Repositories;

public class GenreRepository : IGenreRepository
{
    private readonly DapperContext _context;

    public GenreRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Genre>> GetAll()
    {
        using var connection = _context.CreateConnection();

        var genres = await connection.QueryAsync<GenreDb>(SqlScriptsResource.GetAllGenres);
        return genres.Select(record => new Genre(record.Id, record.Name)).ToList().AsReadOnly();
    }

    public async Task<Genre?> GetById(long id)
    {
        using var connection = _context.CreateConnection();

        var record = await connection.QuerySingleOrDefaultAsync<GenreDb>(SqlScriptsResource.GetGenreById, new { id });
        return record == null ? null : new Genre(record.Id, record.Name);
    }
}