using FluentMigrator.Runner;
using FluentValidation;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Auth.Stores;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Interfaces;
using Melody.Infrastructure.Data.Migrations;
using Melody.Infrastructure.Data.Records;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.Extensions;
using Melody.WebAPI.MappingProfiles;
using Melody.WebAPI.Middlewares;
using Melody.WebAPI.Services;
using Melody.WebAPI.Validators.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();

builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

builder.Services.AddScoped<TokenService>();

builder.Services.AddScoped<IUserStore<UserIdentity>, UserStore>();
builder.Services.AddScoped<IUserRoleStore<UserIdentity>, UserStore>();
builder.Services.AddScoped<IUserEmailStore<UserIdentity>, UserStore>();
builder.Services.AddScoped<IUserPasswordStore<UserIdentity>, UserStore>();

builder.Services.AddScoped<IRoleStore<RoleIdentity>, RoleStore>();

builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserProfile)));

builder.Services.AddValidatorsFromAssemblyContaining<NewUserDtoValidator>();

builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
.AddFluentMigratorCore()
        .ConfigureRunner(c => c.AddSqlServer2016()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("MelodyDBConnection"))
            .ScanIn(Assembly.GetAssembly(typeof(DapperContext))).For.Migrations());

builder.Services.AddControllers();

builder.Services.AddIdentity<UserIdentity, RoleIdentity>();
builder.Services.AddAuthentication(options =>
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
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration
                ["Jwt:Key"]))
        };
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MigrateDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Sounds")),
    RequestPath = new PathString("/Sounds")
});

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
