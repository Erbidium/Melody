using System.Reflection;
using System.Text;
using FluentValidation;
using Melody.Core.Interfaces;
using Melody.Core.Services;
using Melody.Infrastructure;
using Melody.Infrastructure.Auth.Services;
using Melody.Infrastructure.Auth.Stores;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Interfaces;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.MappingProfiles;
using Melody.WebAPI.Validators.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Melody.WebAPI.Extensions;

public static class ServiceCollectionExtensions
{
    public static void RegisterCustomServices(this IServiceCollection services)
    {
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<ISongService, SongService>();
        services.AddScoped<ISongFileStorage, SongFileStorage>();
    }

    public static void RegisterCustomRepositories(this IServiceCollection services)
    {
        services.AddScoped<ISongRepository, SongRepository>();
        services.AddScoped<IPlaylistRepository, PlaylistRepository>();
        services.AddScoped<IGenreRepository, GenreRepository>();
        services.AddScoped<Infrastructure.Data.Interfaces.IUserRepository, UserRepository>();
        services.AddScoped<Core.Interfaces.IUserRepository, UserRepository>();
        services.AddScoped<IRoleRepository, RoleRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
    }

    public static void AddAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetAssembly(typeof(SongProfile)));
    }

    public static void AddValidation(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<UserRegisterValidator>();
    }

    public static void AddIdentityStores(this IServiceCollection services)
    {
        services.AddScoped<IUserStore<UserIdentity>, UserStore>();
        services.AddScoped<IUserRoleStore<UserIdentity>, UserStore>();
        services.AddScoped<IUserEmailStore<UserIdentity>, UserStore>();
        services.AddScoped<IUserPasswordStore<UserIdentity>, UserStore>();
        services.AddScoped<IRoleStore<RoleIdentity>, RoleStore>();
    }

    public static void AddJwtBearerAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["Jwt:Issuer"],
                    ValidAudience = configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration
                        ["Jwt:Key"]))
                };
            });
    }
}