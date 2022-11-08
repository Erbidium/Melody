using FluentMigrator.Runner;
using FluentValidation;
using Melody.Core.Interfaces;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Migrations;
using Melody.Infrastructure.Data.Repositories;
using Melody.WebAPI.Extensions;
using Melody.WebAPI.MappingProfiles;
using Melody.WebAPI.Middlewares;
using Melody.WebAPI.Validators.User;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();
builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IPlaylistRepository, PlaylistRepository>();
builder.Services.AddScoped<IGenreRepository, GenreRepository>();
builder.Services.AddAutoMapper(Assembly.GetAssembly(typeof(UserProfile)));
builder.Services.AddValidatorsFromAssemblyContaining<NewUserDtoValidator>();

builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
.AddFluentMigratorCore()
        .ConfigureRunner(c => c.AddSqlServer2016()
            .WithGlobalConnectionString(builder.Configuration.GetConnectionString("MelodyDBConnection"))
            .ScanIn(Assembly.GetAssembly(typeof(DapperContext))).For.Migrations());

builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();
