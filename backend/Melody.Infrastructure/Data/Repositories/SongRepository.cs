using Dapper;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Entities;
using System.Data;

namespace Melody.Infrastructure.Data.Repositories;

public class SongRepository
{
    private readonly DapperContext _context;
    public SongRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Song>> GetSongs()
    {
        var query = "SELECT * FROM Songs";
        using var connection = _context.CreateConnection();
        var companies = await connection.QueryAsync<Song>(query);
        return companies.ToList();
    }

    public async Task<Song> GetSong(long id)
    {
        var query = "SELECT * FROM Songs WHERE Id = @Id";

        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<Song>(query, new { id });
    }

    public async Task<Song> CreateSong(Song song)
    {
        var query = "INSERT INTO Songs (Name, Path, AuthorName, Year, GenreId, IsDeleted) VALUES (@Name, @Path, @AuthorName, @Year, @GenreId, @IsDeleted)" +
            "SELECT CAST(SCOPE_IDENTITY() as int)";

        var parameters = new DynamicParameters();
        parameters.Add("Name", song.Name, DbType.String);
        parameters.Add("Path", song.Path, DbType.String);
        parameters.Add("AuthorName", song.AuthorName, DbType.String);
        parameters.Add("Year", song.Year, DbType.Int32);
        parameters.Add("GenreId", song.GenreId, DbType.Int64);
        parameters.Add("IsDeleted", false, DbType.Boolean);

        using var connection = _context.CreateConnection();
        var id = await connection.QuerySingleAsync<int>(query, parameters);

        var createdSong = new Song
        {
            Id = id,
            Name = song.Name,
            Path = song.Path,
            AuthorName = song.AuthorName,
            Year = song.Year,
            GenreId = song.GenreId,
            IsDeleted = song.IsDeleted,
        };
        return createdSong;
    }
}
