using System.Data;
using Dapper;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;

namespace Melody.Infrastructure.Data.Repositories;

public class RefreshTokenRepository : IRefreshTokenRepository
{
    private readonly DapperContext _context;

    public RefreshTokenRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateOrUpdateAsync(string token, long userId)
    {
        var entry = await FindByUserIdAsync(userId);

        using var connection = _context.CreateConnection();
        if (entry is null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int64);
            parameters.Add("Token", token, DbType.String);

            await connection.ExecuteAsync(SqlScriptsResource.CreateRefreshToken, parameters);
        }
        else
        {
            await connection.ExecuteAsync(SqlScriptsResource.UpdateRefreshToken,
                new { Token = token, Id = userId });
        }

        return true;
    }

    public async Task<bool> DeleteByUserIdAsync(long userId)
    {
        using var connection = _context.CreateConnection();
        var rowsDeleted =
            await connection.ExecuteAsync(SqlScriptsResource.DeleteRefreshTokenByUserId,
                new { userId });
        return rowsDeleted == 1;
    }

    public async Task<bool> DeleteByValueAsync(string token)
    {
        using var connection = _context.CreateConnection();
        var rowsDeleted =
            await connection.ExecuteAsync(SqlScriptsResource.DeleteRefreshToken,
                new { token });
        return rowsDeleted == 1;
    }

    public async Task<RefreshTokenDb?> FindByTokenValueAsync(string token)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RefreshTokenDb>(SqlScriptsResource.FindRefreshTokenByValue,
            new { Token = token });
    }

    public async Task<RefreshTokenDb?> FindByUserIdAsync(long userId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RefreshTokenDb>(SqlScriptsResource.FindRefreshTokenByUserId,
            new { UserId = userId });
    }
}