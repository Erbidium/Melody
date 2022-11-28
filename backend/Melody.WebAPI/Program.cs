using FluentMigrator.Runner;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Migrations;
using Melody.WebAPI.Extensions;
using Melody.WebAPI.Middlewares;
using Microsoft.Extensions.FileProviders;
using System.Reflection;
using Melody.Infrastructure.Data.DbEntites;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();

builder.Services.RegisterCustomRepositories();
builder.Services.RegisterCustomServices();
builder.Services.AddIdentityStores();
builder.Services.AddAutoMapper();
builder.Services.AddValidation();

builder.Services.AddLogging(c => c.AddFluentMigratorConsole())
    .AddFluentMigratorCore()
    .ConfigureRunner(c => c.AddSqlServer2016()
        .WithGlobalConnectionString(builder.Configuration.GetConnectionString("MelodyDBConnection"))
        .ScanIn(Assembly.GetAssembly(typeof(DapperContext))).For.Migrations());

builder.Services.AddControllers();

builder.Services.AddIdentity<UserIdentity, RoleIdentity>();
builder.Services.AddJwtBearerAuthentication(builder.Configuration);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();

var app = builder.Build();

app.MigrateDatabase();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseCors(opt => opt
    .AllowAnyMethod()
    .AllowAnyHeader()
    .AllowCredentials()
    .SetIsOriginAllowed(_ => true));

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();