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
        using var connection = _context.CreateConnection();

        var songs = await connection.QueryAsync<SongDb>(SqlScriptsResource.GetAllSongs);
        return songs.Select(record => new Song(record.UserId, record.Name, record.Path, record.AuthorName, record.Year,
            record.GenreId, record.SizeBytes, record.UploadedAt, record.Id)).ToList().AsReadOnly();
    }

    public async Task<Song?> GetById(long id)
    {
        using var connection = _context.CreateConnection();

        var record = await connection.QuerySingleOrDefaultAsync<SongDb>(SqlScriptsResource.GetSongById, new { id });
        return record == null
            ? null
            : new Song(record.UserId, record.Name, record.Path, record.AuthorName, record.Year, record.GenreId,
                record.SizeBytes, record.UploadedAt, record.Id);
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
        parameters.Add("UploadedAt", song.UploadedAt, DbType.Date);
        parameters.Add("GenreId", song.GenreId, DbType.Int64);

        using var connection = _context.CreateConnection();

        var id = await connection.ExecuteScalarAsync<long>(SqlScriptsResource.CreateSong, parameters);

        return new Song(song.UserId, song.Name, song.Path, song.AuthorName, song.Year, song.GenreId, song.SizeBytes,
            song.UploadedAt, id);
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
}