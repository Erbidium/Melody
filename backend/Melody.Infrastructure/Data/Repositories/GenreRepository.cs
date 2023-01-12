using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.DbEntites;

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
        return genres.Select(record => new Genre(record.Name) { Id = record.Id }).ToList().AsReadOnly();
    }
}