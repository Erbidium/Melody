using Dapper;
using Melody.Core.Entities;
using Melody.Core.ValueObjects;
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

    public async Task<IEnumerable<Song>> GetSongs()
    {
        var query = "SELECT * FROM Songs WHERE IsDeleted = 0";
        using var connection = _context.CreateConnection();
        var songs = await connection.QueryAsync<SongRecord>(query);
        return songs.ToList().Select(record => new Song(record.Id, record.Name, record.Path, record.AuthorName, record.Year, record.GenreId));
    }

    public async Task<Song> GetSong(long id)
    {
        var query = "SELECT * FROM Songs WHERE Id = @Id AND IsDeleted = 0";

        using var connection = _context.CreateConnection();
        var record = await connection.QuerySingleOrDefaultAsync<SongRecord>(query, new { id });
        return new Song(record.Id, record.Name, record.Path, record.AuthorName, record.Year, record.GenreId);
    }

    public async Task<Song> CreateSong(SongInfo song)
    {
        var query = "INSERT INTO Songs (Name, Path, AuthorName, Year, GenreId, IsDeleted) VALUES (@Name, @Path, @AuthorName, @Year, @GenreId, 0)" +
            "SELECT CAST(SCOPE_IDENTITY() as int)";

        var parameters = new DynamicParameters();
        parameters.Add("Name", song.Name, DbType.String);
        parameters.Add("Path", song.Path, DbType.String);
        parameters.Add("AuthorName", song.AuthorName, DbType.String);
        parameters.Add("Year", song.Year, DbType.Int32);
        parameters.Add("GenreId", song.GenreId, DbType.Int64);

        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, parameters);

        return new Song(id, song.Name, song.Path, song.AuthorName, song.Year, song.GenreId);
    }
}
