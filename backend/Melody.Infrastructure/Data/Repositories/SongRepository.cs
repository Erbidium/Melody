using System.Data;
using Ardalis.GuardClauses;
using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.ElasticSearch;
using Nest;

namespace Melody.Infrastructure.Data.Repositories;

public class SongRepository : ISongRepository
{
    private readonly DapperContext _context;
    private readonly IElasticClient _elasticClient;
    private const string SongIndexName = "songs";

    public SongRepository(DapperContext context, IElasticClient elasticClient)
    {
        _context = context;
        _elasticClient = elasticClient;
    }

    public async Task<IReadOnlyCollection<Song>> GetAll(string searchText, int page = 1, int pageSize = 10)
    {
        Guard.Against.NegativeOrZero(page, nameof(page));
        Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

        using var connection = _context.CreateConnection();
        var songs = await connection.QueryAsync<SongDb, GenreDb, Song>(SqlScriptsResource.GetAllSongs,
            (songDb, genreDb) =>
            {
                var song = new Song(songDb.UserId, songDb.Name, songDb.Path, songDb.AuthorName, songDb.Year,
                    songDb.GenreId, songDb.SizeBytes, songDb.UploadedAt, songDb.Duration)
                {
                    Id = songDb.Id,
                    Genre = new Genre(genreDb.Name) { Id = genreDb.Id }
                };
                return song;
            }, new { Offset = (page - 1) * pageSize, pageSize, SearchText = searchText.Trim().ToLower() });
        return songs.ToList().AsReadOnly();
    }

    public async Task<Song?> GetById(long id)
    {
        using var connection = _context.CreateConnection();

        var record = await connection.QuerySingleOrDefaultAsync<SongDb>(SqlScriptsResource.GetSongById, new { id });
        return record is null
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

        var createdSong = new Song(song.UserId, song.Name, song.Path, song.AuthorName, song.Year, song.GenreId,
            song.SizeBytes,
            song.UploadedAt, song.Duration) { Id = id };

        var songElastic = new SongElastic
        {
            Id = song.Id,
            Name = song.Name,
            AuthorName = song.AuthorName,
            DurationInSeconds = (int)song.Duration.TotalSeconds,
            GenreId = song.GenreId,
            UploadedAt = song.UploadedAt,
            Year = song.Year
        };
        await _elasticClient.IndexAsync(songElastic, descriptor => descriptor.Index(SongIndexName).Id(id));

        return createdSong;
    }

    public async Task<bool> Delete(long id)
    {
        using var connection = _context.CreateConnection();

        var rowsDeleted = await connection.ExecuteAsync(SqlScriptsResource.DeleteSong, new { id });
        var response = await _elasticClient.DeleteAsync(DocumentPath<SongElastic>.Id(id).Index(SongIndexName));
        return rowsDeleted == 1 && response.IsValid;
    }

    public async Task<long> GetTotalBytesSumUploadsByUser(long userId)
    {
        using var connection = _context.CreateConnection();

        return await connection.ExecuteScalarAsync<long>(SqlScriptsResource.GetUserTotalUploadsSize,
            new { UserId = userId });
    }

    public async Task<IReadOnlyCollection<Song>> GetSongsUploadedByUserId(long userId, int page = 1, int pageSize = 10)
    {
        Guard.Against.NegativeOrZero(page, nameof(page));
        Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

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
            new { userId, Offset = (page - 1) * pageSize, pageSize });
        return songs.ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<Song>> GetSongsForPlaylistToAdd(long playlistId, long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb>(SqlScriptsResource.GetSongsToAddToPlaylist,
            new { playlistId, userId });
        return songs.Select(songDb => new Song(songDb.UserId, songDb.Name, songDb.Path, songDb.AuthorName, songDb.Year,
                songDb.GenreId, songDb.SizeBytes, songDb.UploadedAt, songDb.Duration) { Id = songDb.Id }).ToList()
            .AsReadOnly();
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

    public async Task<bool> DeleteFavouriteSong(long id, long userId)
    {
        using var connection = _context.CreateConnection();

        var rowsDeleted = await connection.ExecuteAsync(SqlScriptsResource.DeleteFavouriteSong, new { id, userId });
        return rowsDeleted == 1;
    }

    public async Task<IReadOnlyCollection<Song>> GetFavouriteAndUploadedUserSongs(long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb, GenreDb, Song>(
            SqlScriptsResource.GetFavouriteAndUploadedUserSongs,
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

    public async Task SaveNewSongListening(long id, long userId)
    {
        using var connection = _context.CreateConnection();

        await connection.ExecuteAsync(SqlScriptsResource.SaveNewSongListening, new { id, userId, Date = DateTime.Now });
    }

    public async Task<bool> DeleteUploadedSong(long id, long userId)
    {
        using var connection = _context.CreateConnection();

        var rowsDeleted = await connection.ExecuteAsync(SqlScriptsResource.DeleteUploadedSong, new { id, userId });

        var response = await _elasticClient.DeleteAsync(DocumentPath<SongElastic>.Id(id).Index(SongIndexName));
        return rowsDeleted == 1 && response.IsValid;
    }

    public async Task<IReadOnlyCollection<FavouriteSong>> GetSongsByIds(IList<long> ids, long userId)
    {
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<FavouriteSongFromPlaylistDb, GenreDb, FavouriteSong>(
            SqlScriptsResource.GetSongsByIds,
            (songDb, genreDb) =>
            {
                var song = new FavouriteSong(songDb.Name, songDb.AuthorName, songDb.GenreId, songDb.UploadedAt,
                    songDb.Duration, songDb.IsFavourite)
                {
                    Id = songDb.Id,
                    Genre = new Genre(genreDb.Name) { Id = genreDb.Id }
                };
                return song;
            },
            new { UserId = userId, Ids = ids.ToArray() });
        return songs.OrderBy(s => ids.IndexOf(s.Id)).ToList().AsReadOnly();
    }
}