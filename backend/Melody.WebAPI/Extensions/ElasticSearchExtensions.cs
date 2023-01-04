using Melody.Infrastructure.ElasticSearch;
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

        var client = new ElasticClient(settings);
        services.AddSingleton<IElasticClient>(client);

        CreateIndex(client, defaultIndex);
    }

    private static void CreateIndex(IElasticClient client, string indexName)
    {
        client.Indices.Create(indexName,
            index => index.Map<SongElastic>(x => x.AutoMap())
        );
    }
}