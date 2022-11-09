using Dapper;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;

namespace Melody.Infrastructure.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly DapperContext _context;
    public UserRepository(DapperContext context)
    {
        _context = context;
    }
    public async Task<bool> CreateAsync(UserIdentity user)
    {
        const string sql = @"
            INSERT INTO Users
            VALUES (@Id, @UserName, @NormalizedUserName, @Email, @NormalizedEmail, @EmailConfirmed, @PasswordHash, @PhoneNumber);
        ";
        using var connection = _context.CreateConnection();
        var rowsInserted = await connection.ExecuteAsync(sql, new
        {
            user.Id,
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.PhoneNumber,
        });
        return rowsInserted == 1;
    }

    public async Task<bool> DeleteAsync(long userId)
    {
        const string sql = @"
            DELETE
            FROM Users
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        var rowsDeleted = await connection.ExecuteAsync(sql, new { Id = userId });
        return rowsDeleted == 1;
    }

    public async Task<UserIdentity> FindByEmailAsync(string normalizedEmail)
    {
        const string sql = @"
            SELECT * 
            FROM Users
            WHERE NormalizedEmail = @NormalizedEmail;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserIdentity>(sql, new { NormalizedEmail = normalizedEmail });
    }

    public async Task<UserIdentity> FindByIdAsync(long userId)
    {
        const string sql = @"
            SELECT *
            FROM Users
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserIdentity>(sql, new { Id = userId });
    }

    public async Task<UserIdentity> FindByNameAsync(string normalizedUserName)
    {
        const string sql = @"
            SELECT *
            FROM Users
            WHERE NormalizedUserName = @NormalizedUserName;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserIdentity>(sql, new { NormalizedUserName = normalizedUserName });
    }

    public async Task<UserRole> FindUserRoleAsync(long userId, long roleId)
    {
        const string sql = @"
            SELECT *
            FROM UserRoles
            WHERE UserId = @UserId AND RoleId = @RoleId;
        ";
        using var connection = _context.CreateConnection();
        var userRole = await connection.QuerySingleOrDefaultAsync<UserRole>(sql, new
        {
            UserId = userId,
            RoleId = roleId
        });
        return userRole;
    }

    public async Task<IEnumerable<RoleIdentity>> GetRolesAsync(long userId)
    {
        const string sql = @"
            SELECT Roles.*
            FROM Roles
            INNER JOIN Users ON Users.RoleId = Roles.Id
            WHERE Users.Id = @UserId;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<RoleIdentity>(sql, new { UserId = userId });
    }

    public async Task<IEnumerable<UserIdentity>> GetUsersInRoleAsync(string roleName)
    {
        const string sql = @"
            SELECT Users.*
            FROM Users
            INNER JOIN Roles ON Users.RoleId = Roles.Id
            WHERE Roles.NormalizedName = @NormalizedName;
        ";
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<UserIdentity>(sql, new { NormalizedName = roleName });
    }

    public async Task<bool> UpdateAsync(UserIdentity user)
    {
        const string updateUserSql = @"
            UPDATE Users
            SET UserName = @UserName, 
                NormalizedUserName = @NormalizedUserName, 
                Email = @Email, 
                NormalizedEmail = @NormalizedEmail, 
                EmailConfirmed = @EmailConfirmed, 
                PasswordHash = @PasswordHash,  
                PhoneNumber = @PhoneNumber,
                IsBanned = @IsBanned
                IsDeleted = @IsDeleted
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(updateUserSql, new
        {
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.PhoneNumber,
            user.IsBanned,
            user.IsDeleted
        });
        return true;
    }

    public async Task<bool> UpdateAsync(UserIdentity user, IList<UserRole> roles)
    {
        const string updateUserSql = @"
            UPDATE Users
            SET UserName = @UserName, 
                NormalizedUserName = @NormalizedUserName, 
                Email = @Email, 
                NormalizedEmail = @NormalizedEmail, 
                EmailConfirmed = @EmailConfirmed, 
                PasswordHash = @PasswordHash,  
                PhoneNumber = @PhoneNumber,
                IsBanned = @IsBanned
                IsDeleted = @IsDeleted
            WHERE Id = @Id;
        ";
        using var connection = _context.CreateConnection();
        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(updateUserSql, new
        {
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.PhoneNumber,
            user.IsBanned,
            user.IsDeleted
        }, transaction);
        if (roles?.Count > 0)
        {
            const string deleteRolesSql = @"
                DELETE
                FROM UserRoles
                WHERE UserId = @UserId;
            ";
            await connection.ExecuteAsync(deleteRolesSql, new { UserId = user.Id }, transaction);
            const string insertRolesSql = @"
                INSERT INTO UserRoles (UserId, RoleId)
                VALUES (@UserId, @RoleId);
            ";
            await connection.ExecuteAsync(insertRolesSql, roles.Select(x => new {
                UserId = user.Id,
                x.RoleId
            }), transaction);
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
}
