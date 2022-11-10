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
        using var connection = _context.CreateConnection();
        var rowsInserted = await connection.ExecuteAsync(SqlScriptsResource.CreateRole, new
        {
            role.Name,
            role.NormalizedName,
        });
        return rowsInserted == 1;
    }

    public async Task<bool> DeleteAsync(long roleId)
    {
        using var connection = _context.CreateConnection();
        var rowsDeleted = await connection.ExecuteAsync(SqlScriptsResource.DeleteRole, new { Id = roleId });
        return rowsDeleted == 1;
    }

    public async Task<RoleIdentity> FindByIdAsync(long roleId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RoleIdentity>(SqlScriptsResource.CreateRole, new { Id = roleId });
    }

    public async Task<RoleIdentity> FindByNameAsync(string normalizedName)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<RoleIdentity>(SqlScriptsResource.GetRoleByName, new { NormalizedName = normalizedName });
    }

    public async Task<bool> UpdateAsync(RoleIdentity role)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(SqlScriptsResource.UpdateRole, new
        {
            role.Name,
            role.NormalizedName,
            role.Id
        });
        return true;
    }
}
