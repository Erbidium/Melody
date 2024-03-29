﻿using System.Data;
using Ardalis.GuardClauses;
using Dapper;
using Melody.Core.Entities;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Nest;

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
        var parameters = new DynamicParameters();
        parameters.Add("UserName", user.UserName, DbType.String);
        parameters.Add("NormalizedUserName", user.NormalizedUserName, DbType.String);
        parameters.Add("Email", user.Email, DbType.String);
        parameters.Add("NormalizedEmail", user.NormalizedEmail, DbType.String);
        parameters.Add("EmailConfirmed", user.EmailConfirmed, DbType.Boolean);
        parameters.Add("PasswordHash", user.PasswordHash, DbType.String);
        parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String);
        parameters.Add("PhoneNumber", user.PhoneNumber, DbType.String);

        using var connection = _context.CreateConnection();
        user.Id = await connection.ExecuteScalarAsync<long>(SqlScriptsResource.CreateUser, parameters);
        return true;
    }

    public async Task<bool> DeleteAsync(long userId)
    {
        using var connection = _context.CreateConnection();
        var rowsDeleted = await connection.ExecuteAsync(SqlScriptsResource.DeleteUser, new { Id = userId });
        return rowsDeleted == 1;
    }

    public async Task<bool> CreateOrUpdateUserRecommendationsPreferences(
        RecommendationsPreferences recommendationsPreferences)
    {
        var entry = await GetUserRecommendationsPreferences(recommendationsPreferences.UserId);

        using var connection = _context.CreateConnection();
        if (entry is null)
        {
            var parameters = new DynamicParameters();
            parameters.Add("UserId", recommendationsPreferences.UserId, DbType.Int64);

            await connection.ExecuteAsync(SqlScriptsResource.CreateRecommendationsPreferences, new
            {
                recommendationsPreferences.UserId,
                recommendationsPreferences.AuthorName,
                recommendationsPreferences.StartYear,
                recommendationsPreferences.EndYear,
                recommendationsPreferences.GenreId,
                recommendationsPreferences.AverageDurationInMinutes
            });
        }
        else
        {
            await connection.ExecuteAsync(SqlScriptsResource.UpdateRecommendationsPreferences, new
            {
                recommendationsPreferences.UserId,
                recommendationsPreferences.AuthorName,
                recommendationsPreferences.StartYear,
                recommendationsPreferences.EndYear,
                recommendationsPreferences.GenreId,
                recommendationsPreferences.AverageDurationInMinutes
            });
        }

        return true;
    }

    public async Task<RecommendationsPreferences?> GetUserRecommendationsPreferences(long userId)
    {
        using var connection = _context.CreateConnection();
        var dbEntry =
            await connection.QuerySingleOrDefaultAsync<RecommendationsPreferencesDb>(
                SqlScriptsResource.GetRecommendationsPreferences, new { userId });
        return dbEntry is null
            ? null
            : new RecommendationsPreferences(
                dbEntry.UserId,
                dbEntry.GenreId,
                dbEntry.AuthorName,
                dbEntry.StartYear,
                dbEntry.EndYear,
                dbEntry.AverageDurationInMinutes
            );
    }

    public async Task<UserIdentity> FindByEmailAsync(string normalizedEmail)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserIdentity>(SqlScriptsResource.GetUserByEmail,
            new { NormalizedEmail = normalizedEmail });
    }

    public async Task<UserIdentity> FindByIdAsync(long userId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserIdentity>(SqlScriptsResource.GetUserById,
            new { Id = userId });
    }

    public async Task<UserIdentity> FindByNameAsync(string normalizedUserName)
    {
        using var connection = _context.CreateConnection();
        return await connection.QuerySingleOrDefaultAsync<UserIdentity>(SqlScriptsResource.GetUserByName,
            new { NormalizedUserName = normalizedUserName });
    }

    public async Task<UserRole> FindUserRoleAsync(long userId, long roleId)
    {
        using var connection = _context.CreateConnection();
        var userRole = await connection.QuerySingleOrDefaultAsync<UserRole>(SqlScriptsResource.GetUserRoles, new
        {
            UserId = userId,
            RoleId = roleId
        });
        return userRole;
    }

    public async Task<IEnumerable<RoleIdentity>> GetRolesAsync(long userId)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<RoleIdentity>(SqlScriptsResource.GetRoles, new { UserId = userId });
    }

    public async Task<IEnumerable<UserIdentity>> GetUsersInRoleAsync(string roleName)
    {
        using var connection = _context.CreateConnection();
        return await connection.QueryAsync<UserIdentity>(SqlScriptsResource.GetUsersInRole,
            new { NormalizedName = roleName });
    }

    public async Task<bool> SetUserBannedStatus(bool isBanned, long userId)
    {
        using var connection = _context.CreateConnection();

        var rowsAffected = await connection.ExecuteAsync(SqlScriptsResource.SetUserBanStatus, new { userId, isBanned });
        return rowsAffected == 1;
    }

    public async Task<bool> UpdateAsync(UserIdentity user)
    {
        using var connection = _context.CreateConnection();
        await connection.ExecuteAsync(SqlScriptsResource.UpdateUser, new
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
        using var connection = _context.CreateConnection();
        connection.Open();
        using var transaction = connection.BeginTransaction();
        await connection.ExecuteAsync(SqlScriptsResource.UpdateUser, new
        {
            user.UserName,
            user.NormalizedUserName,
            user.Email,
            user.NormalizedEmail,
            user.EmailConfirmed,
            user.PasswordHash,
            user.PhoneNumber,
            user.IsBanned,
            user.IsDeleted,
            user.Id
        }, transaction);
        if (roles?.Count > 0)
        {
            await connection.ExecuteAsync(SqlScriptsResource.DeleteRoles, new { UserId = user.Id }, transaction);
            await connection.ExecuteAsync(SqlScriptsResource.InsertRoles, roles.Select(x => new
            {
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

    public async Task<IReadOnlyCollection<User>> GetUsersWithoutAdministratorRole(string searchText, int page = 1,
        int pageSize = 10)
    {
        Guard.Against.NegativeOrZero(page, nameof(page));
        Guard.Against.NegativeOrZero(pageSize, nameof(pageSize));

        using var connection = _context.CreateConnection();

        var users = await connection.QueryAsync<UserIdentity>(SqlScriptsResource.GetUsersWithoutAdminRole,
            new { Offset = (page - 1) * pageSize, pageSize, SearchText = searchText.Trim().ToLower() });
        return users.Select(record => new User(record.UserName, record.Email, record.PhoneNumber, record.IsBanned)
                { Id = record.Id }).ToList()
            .AsReadOnly();
    }
}