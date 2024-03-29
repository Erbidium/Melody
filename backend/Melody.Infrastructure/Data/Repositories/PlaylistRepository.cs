﻿using System.Data;
using Ardalis.GuardClauses;
using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Core.ValueObjects;
using Melody.Infrastructure.Data.Context;
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

    public async Task<bool> AddSongs(long id, long[] songIds)
    {
        using var connection = _context.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();

        foreach (var songId in songIds)
            await connection.ExecuteAsync(SqlScriptsResource.InsertPlaylistSong,
                new { PlaylistId = id, SongId = songId }, transaction);

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

    public async Task<IReadOnlyCollection<FavouritePlaylist>> GetPlaylistsCreatedByUser(long userId)
    {
        using var connection = _context.CreateConnection();

        var playlists =
            await connection.QueryAsync<FavouritePlaylistDb, FavouriteSongFromPlaylistDb, FavouritePlaylistDb>(
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
            var songs = p.Songs.Select(s =>
                new FavouriteSong(s.Name, s.AuthorName, s.GenreId, s.UploadedAt, s.Duration, s.IsFavourite)
                {
                    Id = s.Id
                }).ToList();
            var playlist = new FavouritePlaylist(p.Name, p.AuthorId, p.IsFavourite)
            {
                Id = p.Id,
                Songs = songs
            };
            return playlist;
        }).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Playlist>> GetFavouritePlaylists(long userId)
    {
        using var connection = _context.CreateConnection();

        var playlists = await connection.QueryAsync<PlaylistDb, FavouriteSongFromPlaylistDb, PlaylistDb>(
            SqlScriptsResource.GetFavouritePlaylists,
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
            var songs = p.Songs.Select(s =>
                new FavouriteSong(s.Name, s.AuthorName, s.GenreId, s.UploadedAt, s.Duration, s.IsFavourite)
                {
                    Id = s.Id
                }).ToList();
            var playlist = new Playlist(p.Name, p.AuthorId)
            {
                Id = p.Id,
                Songs = songs
            };
            return playlist;
        }).ToList().AsReadOnly();
    }

    public async Task<FavouritePlaylist?> GetById(long id, long userId, int page = 1, int pageSize = 10)
    {
        Guard.Against.NegativeOrZero(page, nameof(page));
        Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

        using var connection = _context.CreateConnection();

        var records =
            await connection.QueryAsync<FavouritePlaylistDb, FavouriteSongFromPlaylistDb, GenreDb, FavouritePlaylistDb>(
                SqlScriptsResource.GetPlaylistById, (playlist, song, genre) =>
                {
                    if (song != null)
                    {
                        song.Genre = genre;
                        playlist.Songs.Add(song);
                    }

                    return playlist;
                }, new { id, userId, Offset = (page - 1) * pageSize, pageSize });

        var playlistWithSongs = records.GroupBy(p => p.Id).Select(g =>
        {
            var playlistWithSong = g.First();
            playlistWithSong.Songs = g.Where(p => p.Songs.Count > 0).Select(p => p.Songs.Single()).ToList();
            return playlistWithSong;
        }).FirstOrDefault();

        if (playlistWithSongs is null) return null;

        var songs = playlistWithSongs.Songs.Select(s =>
            new FavouriteSong(s.Name, s.AuthorName, s.GenreId, s.UploadedAt, s.Duration, s.IsFavourite)
            {
                Id = s.Id,
                Genre = new Genre(s.Genre.Name) { Id = s.Genre.Id }
            }).ToList();
        var playlist =
            new FavouritePlaylist(playlistWithSongs.Name, playlistWithSongs.AuthorId, playlistWithSongs.IsFavourite)
                { Id = playlistWithSongs.Id, Songs = songs };

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
            await connection.ExecuteAsync(SqlScriptsResource.InsertPlaylistSong,
                new { PlaylistId = id, SongId = songId }, transaction);

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

    public async Task CreateFavouritePlaylist(long id, long userId)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.CreateFavouritePlaylist, new { id, userId });
    }

    public async Task DeleteFavouritePlaylist(long id, long userId)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.DeleteFavouritePlaylist, new { id, userId });
    }
}