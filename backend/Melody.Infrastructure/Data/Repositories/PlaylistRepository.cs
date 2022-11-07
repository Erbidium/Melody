using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Records;
using System.Data;

namespace Melody.Infrastructure.Data.Repositories;

public class PlaylistRepository : IPlaylistRepository
{
    private readonly DapperContext _context;
    public PlaylistRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<IReadOnlyCollection<Playlist>> GetAll()
    {
        var query = "SELECT Id, Name, Link, AuthorId, IsDeleted FROM Playlists WHERE IsDeleted = 0";

        using var connection = _context.CreateConnection();

        var playlists = await connection.QueryAsync<PlaylistRecord>(query);
        return playlists.Select(record => new Playlist(record.Name, record.Link, record.AuthorId, record.Id)).ToList().AsReadOnly();
    }

    public async Task<Playlist?> GetById(long id)
    {
        var query = "SELECT Id, Name, Link, AuthorId, IsDeleted FROM Playlists WHERE Id = @Id AND IsDeleted = 0";

        using var connection = _context.CreateConnection();

        var record = await connection.QuerySingleOrDefaultAsync<PlaylistRecord>(query, new { id });
        return record == null ? null : new Playlist(record.Name, record.Link, record.AuthorId, record.Id);
    }

    public async Task<Playlist> Create(Playlist playlist)
    {
        var query = "INSERT INTO Playlists (Name, Link, AuthorId) OUTPUT Inserted.Id VALUES (@Name, @Link, @AuthorId)";

        var parameters = new DynamicParameters();
        parameters.Add("Name", playlist.Name, DbType.String);
        parameters.Add("Link", playlist.Link, DbType.String);
        parameters.Add("AuthorId", playlist.AuthorId, DbType.Int64);

        using var connection = _context.CreateConnection();

        var id = await connection.ExecuteScalarAsync<long>(query, parameters);

        return new Playlist(playlist.Name, playlist.Link, playlist.AuthorId, playlist.Id);
    }

    public async Task Update(Playlist playlist)
    {
        var query = "UPDATE Playlists SET Name = @Name, Link = @Link, AuthorId = @AuthorId WHERE Id = @Id AND IsDeleted = 0";

        var parameters = new DynamicParameters();
        parameters.Add("Name", playlist.Name, DbType.String);
        parameters.Add("Link", playlist.Link, DbType.String);
        parameters.Add("AuthorId", playlist.AuthorId, DbType.Int64);

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(query, parameters);
    }

    public async Task Delete(long id)
    {
        var query = "UPDATE Playlists SET IsDeleted = 1 WHERE Id = @Id AND IsDeleted = 0";

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(query, new { id });
    }
}
