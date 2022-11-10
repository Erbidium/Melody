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
        using var connection = _context.CreateConnection();

        var playlists = await connection.QueryAsync<PlaylistDb>(SqlScriptsResource.GetAllPlaylists);
        return playlists.Select(record => new Playlist(record.Name, record.Link, record.AuthorId, record.Id)).ToList().AsReadOnly();
    }

    public async Task<Playlist?> GetById(long id)
    {
        using var connection = _context.CreateConnection();

        var record = await connection.QuerySingleOrDefaultAsync<PlaylistDb>(SqlScriptsResource.GetPlaylistById, new { id });
        return record == null ? null : new Playlist(record.Name, record.Link, record.AuthorId, record.Id);
    }

    public async Task<Playlist> Create(Playlist playlist)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", playlist.Name, DbType.String);
        parameters.Add("Link", playlist.Link, DbType.String);
        parameters.Add("AuthorId", playlist.AuthorId, DbType.Int64);

        using var connection = _context.CreateConnection();

        var id = await connection.ExecuteScalarAsync<long>(SqlScriptsResource.CreatePlaylist, parameters);

        return new Playlist(playlist.Name, playlist.Link, playlist.AuthorId, playlist.Id);
    }

    public async Task Update(Playlist playlist)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", playlist.Name, DbType.String);
        parameters.Add("Link", playlist.Link, DbType.String);
        parameters.Add("AuthorId", playlist.AuthorId, DbType.Int64);

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.UpdatePlaylist, parameters);
    }   

    public async Task Delete(long id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.DeletePlaylist, new { id });
    }
}
