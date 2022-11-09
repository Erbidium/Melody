using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;

namespace Melody.Infrastructure.Data.Repositories;

public class RoleRepository : IRoleRepository
{
    private readonly DapperContext _context;
    public RoleRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateAsync(RoleIdentity role)
    {
        const string sql = @"
            INSERT INTO Roles
            VALUES (@Name, @NormalizedName);
        ";
        using var connection = _context.CreateConnection();
        var rowsInserted = await connection.ExecuteAsync(sql, new
        {
            role.Name,
            role.NormalizedName,
        });
        return rowsInserted == 1;
    }

    public async Task<bool> DeleteAsync(long roleId)
    {
        const string sql = @"
            DELETE
            FROM Roles
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        var rowsDeleted = await connection.ExecuteAsync(sql, new { Id = roleId });
        return rowsDeleted == 1;
    }

    public async Task<RoleIdentity> FindByIdAsync(long roleId)
    {
        const string sql = @"
            SELECT Id, Name, NormalizedName
            FROM Roles
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RoleIdentity>(sql, new { Id = roleId });
    }

    public async Task<RoleIdentity> FindByNameAsync(string normalizedName)
    {
        const string sql = @"
            SELECT Id, Name, NormalizedName
            FROM Roles
            WHERE NormalizedName = @NormalizedName;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RoleIdentity>(sql, new { NormalizedName = normalizedName });
    }

    public async Task<bool> UpdateAsync(RoleIdentity role)
    {
        const string updateRoleSql = @"
            UPDATE Roles
            SET Name = @Name, NormalizedName = @NormalizedName,
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(updateRoleSql, new
        {
            role.Name,
            role.NormalizedName,
            role.Id
        });
        return true;
    }
}
