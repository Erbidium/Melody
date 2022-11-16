using FluentMigrator.Runner;
using Melody.Infrastructure.Data.Context;
using Melody.Infrastructure.Data.Migrations;
using Melody.Infrastructure.Data.Records;
using Melody.WebAPI.Extensions;
using Melody.WebAPI.Middlewares;
using Microsoft.Extensions.FileProviders;
using System.Reflection;

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