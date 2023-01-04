using System.Reflection;
using FluentMigrator.Runner;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.DbEntites;
using Melody.Infrastructure.Data.Migrations;
using Melody.WebAPI.Extensions;
using Melody.WebAPI.Middlewares;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddSingleton<DapperContext>();
builder.Services.AddSingleton<Database>();

builder.Services.RegisterCustomRepositories();
builder.Services.RegisterCustomServices();
builder.Services.AddIdentityStores();
builder.Services.AddAutoMapper();
builder.Services.AddValidation();

builder.Services.AddElasticsearch(builder.Configuration);

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
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    }); 
});

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