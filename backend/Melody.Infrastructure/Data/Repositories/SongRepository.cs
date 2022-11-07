using Dapper;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Records;
using System.Data;

namespace Melody.Infrastructure.Data.Repositories;

public class SongRepository : ISongRepository
{
    private readonly DapperContext _context;
    public SongRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Song>> GetAll()
    {
        var query = "SELECT Id, Name, Path, AuthorName, Year, GenreId, IsDeleted FROM Songs WHERE IsDeleted = 0";
        using var connection = _context.CreateConnection();
        var songs = await connection.QueryAsync<SongRecord>(query);
        return songs.Select(record => new Song(record.Name, record.Path, record.AuthorName, record.Year, record.GenreId, record.Id)).ToList().AsReadOnly();
    }

    public async Task<Song> GetById(long id)
    {
        var query = "SELECT Id, Name, Path, AuthorName, Year, GenreId, IsDeleted FROM Songs WHERE Id = @Id AND IsDeleted = 0";

        using var connection = _context.CreateConnection();
        var record = await connection.QuerySingleOrDefaultAsync<SongRecord>(query, new { id });
        return new Song(record.Name, record.Path, record.AuthorName, record.Year, record.GenreId, record.Id);
    }

    public async Task<Song> Create(Song song)
    {
        var query = "INSERT INTO Songs (Name, Path, AuthorName, Year, GenreId) OUTPUT Inserted.Id VALUES (@Name, @Path, @AuthorName, @Year, @GenreId)";

        var parameters = new DynamicParameters();
        parameters.Add("Name", song.Name, DbType.String);
        parameters.Add("Path", song.Path, DbType.String);
        parameters.Add("AuthorName", song.AuthorName, DbType.String);
        parameters.Add("Year", song.Year, DbType.Int32);
        parameters.Add("GenreId", song.GenreId, DbType.Int64);

        using var connection = _context.CreateConnection();
        var id = await connection.ExecuteScalarAsync<long>(query, parameters);

        return new Song(song.Name, song.Path, song.AuthorName, song.Year, song.GenreId, id);
    }
}
