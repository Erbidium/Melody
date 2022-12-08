using Dapper;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Context;
using System.Data;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.DbEntites;

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
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb>(SqlScriptsResource.GetAllSongs);
        return songs.Select(record => new Song(record.UserId, record.Name, record.Path, record.AuthorName, record.Year,
                record.GenreId, record.SizeBytes, record.UploadedAt, record.Duration) { Id = record.Id }).ToList()
            .AsReadOnly();
    }

    public async Task<Song?> GetById(long id)
    {
        using var connection = _context.CreateConnection();

        var record = await connection.QuerySingleOrDefaultAsync<SongDb>(SqlScriptsResource.GetSongById, new { id });
        return record == null
            ? null
            : new Song(record.UserId, record.Name, record.Path, record.AuthorName, record.Year, record.GenreId,
                record.SizeBytes, record.UploadedAt, record.Duration) { Id = record.Id };
    }

    public async Task<Song> Create(Song song)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", song.UserId, DbType.Int64);
        parameters.Add("Name", song.Name, DbType.String);
        parameters.Add("Path", song.Path, DbType.String);
        parameters.Add("AuthorName", song.AuthorName, DbType.String);
        parameters.Add("Year", song.Year, DbType.Int32);
        parameters.Add("SizeBytes", song.SizeBytes, DbType.Int32);
        parameters.Add("UploadedAt", song.UploadedAt, DbType.DateTime);
        parameters.Add("GenreId", song.GenreId, DbType.Int64);
        parameters.Add("Duration", song.Duration, DbType.Time);

        using var connection = _context.CreateConnection();

        var id = await connection.ExecuteScalarAsync<long>(SqlScriptsResource.CreateSong, parameters);

        return new Song(song.UserId, song.Name, song.Path, song.AuthorName, song.Year, song.GenreId, song.SizeBytes,
            song.UploadedAt, song.Duration) { Id = song.Id };
    }

    public async Task Update(Song song)
    {
        var parameters = new DynamicParameters();
        parameters.Add("UserId", song.UserId, DbType.Int64);
        parameters.Add("Name", song.Name, DbType.String);
        parameters.Add("Path", song.Path, DbType.String);
        parameters.Add("AuthorName", song.AuthorName, DbType.String);
        parameters.Add("Year", song.Year, DbType.Int32);
        parameters.Add("SizeBytes", song.SizeBytes, DbType.Int32);
        parameters.Add("UploadedAt", song.UploadedAt, DbType.Date);
        parameters.Add("GenreId", song.GenreId, DbType.Int64);
        parameters.Add("Duration", song.Duration, DbType.Time);

        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.UpdateSong, parameters);
    }

    public async Task Delete(long id)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.DeleteSong, new { id });
    }

    public async Task<long> GetTotalBytesSumUploadsByUser(long userId)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<long>(SqlScriptsResource.GetUserTotalUploadsSize,
            new { UserId = userId });
    }

    public async Task<IReadOnlyCollection<Song>> GetSongsUploadedByUserId(long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb, GenreDb, Song>(SqlScriptsResource.GetSongsUploadedByUserId,
            (songDb, genreDb) =>
            {
                var song = new Song(songDb.UserId, songDb.Name, songDb.Path, songDb.AuthorName, songDb.Year,
                    songDb.GenreId, songDb.SizeBytes, songDb.UploadedAt, songDb.Duration)
                {
                    Id = songDb.Id,
                    Genre = new Genre(genreDb.Name) { Id = genreDb.Id }
                };
                return song;
            },
            new { UserId = userId });
        return songs.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Song>> GetSongsForPlaylistToAdd(long playlistId, long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb>(SqlScriptsResource.GetSongsToAddToPlaylist, new { playlistId, userId });
        return songs.Select(songDb => new Song(songDb.UserId, songDb.Name, songDb.Path, songDb.AuthorName, songDb.Year,
            songDb.GenreId, songDb.SizeBytes, songDb.UploadedAt, songDb.Duration) { Id = songDb.Id }).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Song>> GetFavouriteUserSongs(long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb, GenreDb, Song>(SqlScriptsResource.GetFavouriteSongs,
            (songDb, genreDb) =>
            {
                var song = new Song(songDb.UserId, songDb.Name, songDb.Path, songDb.AuthorName, songDb.Year,
                    songDb.GenreId, songDb.SizeBytes, songDb.UploadedAt, songDb.Duration)
                {
                    Id = songDb.Id,
                    Genre = new Genre(genreDb.Name) { Id = genreDb.Id }
                };
                return song;
            },
            new { UserId = userId });
        return songs.ToList().AsReadOnly();
    }

    public async Task CreateFavouriteSong(long id, long userId)
    {
        using var connection = _context.CreateConnection();
        
        await connection.ExecuteAsync(SqlScriptsResource.CreateFavouriteSong, new { id, userId });
    }

    public async Task DeleteFavouriteSong(long id, long userId)
    {
        using var connection = _context.CreateConnection();
        
        await connection.ExecuteAsync(SqlScriptsResource.DeleteFavouriteSong, new { id, userId });
    }

    public async Task<IReadOnlyCollection<Song>> GetFavouriteAndUploadedUserSongs(long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb, GenreDb, Song>(SqlScriptsResource.GetFavouriteAndUploadedUserSongs,
            (songDb, genreDb) =>
            {
                var song = new Song(songDb.UserId, songDb.Name, songDb.Path, songDb.AuthorName, songDb.Year,
                    songDb.GenreId, songDb.SizeBytes, songDb.UploadedAt, songDb.Duration)
                {
                    Id = songDb.Id,
                    Genre = new Genre(genreDb.Name) { Id = genreDb.Id }
                };
                return song;
            },
            new { UserId = userId });
        return songs.ToList().AsReadOnly();
    }
}