using LanguageExt.ClassInstances;
using Melody.Infrastructure.Data.DbEntites;
using Nest;

namespace Melody.WebAPI.Extensions;

public static class ElasticSearchExtensions
{
    public static void AddElasticsearch(
        this IServiceCollection services, IConfiguration configuration)
    {
        var url = configuration["ELKConfiguration:url"];
        var defaultIndex = configuration["ELKConfiguration:index"];

        var settings = new ConnectionSettings(new Uri(url))
            .PrettyJson()
            .DefaultIndex(defaultIndex);

        AddDefaultMappings(settings);

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);
    }

    private static void AddDefaultMappings(ConnectionSettings settings)
    {
        settings
            .DefaultMappingFor<SongDb>(m => m
                .Ignore(s => s.Id)
                .Ignore(s => s.Path)
                .Ignore(s => s.SizeBytes)
                .Ignore(s => s.UserId)
            );
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        client.Indices.Create(indexName,
            index => index.Map<SongDb>(x => x.AutoMap())
        );
    }
}