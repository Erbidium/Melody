using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;
using System.Data;
using Melody.Infrastructure.Data.DbEntites;

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
        return playlists.Select(record => new Playlist(record.Name, record.AuthorId) { Id = record.Id }).ToList()
            .AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Playlist>> GetPlaylistsCreatedByUser(long userId)
    {
        using var connection = _context.CreateConnection();

        var playlists = await connection.QueryAsync<PlaylistDb, SongDb, PlaylistDb>(
            SqlScriptsResource.GetPlaylistsCreatedByUserId,
            (playlist, song) =>
            {
                if (song != null)
                    playlist.Songs.Add(song);
                return playlist;
            },
            new { UserId = userId });
        var playlistsGrouped = playlists.GroupBy(p => p.Id).Select(g =>
        {
            var groupedPlaylistDb = g.First();
            groupedPlaylistDb.Songs = g.Where(p => p.Songs.Count > 0).Select(p => p.Songs.Single()).ToList();
            return groupedPlaylistDb;
        });
        return playlistsGrouped.Select(p =>
        {
            var songs = p.Songs.Select(s => new Song(s.UserId, s.Name, s.Path, s.AuthorName, s.Year,
                s.GenreId, s.SizeBytes, s.UploadedAt, s.Duration)
            {
                Id = s.Id,
            }).ToList();
            var playlist = new Playlist(p.Name, p.AuthorId) { Id = p.Id };
            playlist.Songs = songs;
            return playlist;
        }).ToList().AsReadOnly();
    }

    public async Task<Playlist?> GetById(long id)
    {
        using var connection = _context.CreateConnection();

        var records =
            await connection.QueryAsync<PlaylistDb, SongDb, GenreDb, PlaylistDb>(SqlScriptsResource.GetPlaylistById, (playlist, song, genre) =>
            {
                if (song != null)
                {
                    song.Genre = genre;
                    playlist.Songs.Add(song);
                }
                return playlist;
            }, new { id });
       
        var playlistWithSongs = records.GroupBy(p => p.Id).Select(g =>
        {
            var playlistWithSong = g.First();
            playlistWithSong.Songs = g.Where(p => p.Songs.Count > 0).Select(p => p.Songs.Single()).ToList();
            return playlistWithSong;
        }).FirstOrDefault();

        if(playlistWithSongs == null)
        {
            return null;
        }

        var songs = playlistWithSongs.Songs.Select(s => new Song(s.UserId, s.Name, s.Path, s.AuthorName, s.Year,
                s.GenreId, s.SizeBytes, s.UploadedAt, s.Duration)
            {
                Id = s.Id,
                Genre = new Genre(s.Genre.Name) {Id = s.Genre.Id }
            }).ToList();
        var playlist = new Playlist(playlistWithSongs.Name, playlistWithSongs.AuthorId) { Id = playlistWithSongs.Id, Songs = songs };

       return playlist;
    }

    public async Task<bool> Create(CreatePlaylist playlist)
    {
        using var connection = _context.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        var parameters = new DynamicParameters();
        parameters.Add("Name", playlist.Name, DbType.String);
        parameters.Add("AuthorId", playlist.AuthorId, DbType.Int64);

        var id = await connection.ExecuteScalarAsync<long>(SqlScriptsResource.CreatePlaylist, parameters, transaction);

        foreach (var songId in playlist.SongIds)
        {
            await connection.ExecuteAsync(SqlScriptsResource.InsertPlaylistSong,
                new { PlaylistId = id, SongId = songId }, transaction);
        }

        try
        {
            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            return false;
        }

        return true;
    }

    public async Task Update(UpdatePlaylist playlist)
    {
        var parameters = new DynamicParameters();
        parameters.Add("Name", playlist.Name, DbType.String);

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.UpdatePlaylist, parameters);
    }

    public async Task Delete(long id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.DeletePlaylist, new { id });
    }

    public async Task DeleteSong(long id, long songId)
    {
        using var connection = _context.CreateConnection();
        
        await connection.ExecuteAsync(SqlScriptsResource.DeletePlaylistSong, new { id, songId });

    }
}