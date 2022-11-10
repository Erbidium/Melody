using Dapper;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Melody.Infrastructure.Data.Records;
using System.Data;

namespace Melody.Infrastructure.Data.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly DapperContext _context;
        public RefreshTokenRepository(DapperContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateOrUpdateAsync(string token, long userId)
        {
            var entry = await FindAsync(token);

            using var connection = _context.CreateConnection();
            if (entry == null)
            {
                var insertQuery = "INSERT INTO UserRefreshTokens(UserId, RefreshToken) VALUES (@UserId, @Token)";
                var parameters = new DynamicParameters();
                parameters.Add("UserId", userId, DbType.Int64);
                parameters.Add("RefreshToken", token, DbType.String);

                await connection.ExecuteAsync(insertQuery, parameters);
            }
            else {
                await connection.ExecuteAsync("UPDATE UserRefreshTokens SET RefreshToken = @Token, WHERE UserId = @Id;", new { Token = token, Id = userId });
            }
            return true;
        }

        public async Task<bool> DeleteAsync(string Token)
        {
            using var connection = _context.CreateConnection();
            var rowsDeleted = await connection.ExecuteAsync("DELETE FROM UserRefreshTokens WHERE RefreshToken = @Token;", new { Token });
            return rowsDeleted == 1;
        }

        public async Task<RefreshTokenDb> FindAsync(string token)
        {
            var query = "SELECT * FROM UserRefreshTokens WHERE RefreshToken = @Token";
            using var connection = _context.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<RefreshTokenDb>(query, new { Token = token });
        }
    }
}
