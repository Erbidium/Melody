using LanguageExt.Common;
using Melody.Core.Entities;
using Melody.Core.Interfaces;
using Nest;

namespace Melody.Infrastructure.ElasticSearch;

public class Recommender : IRecommender
{
    private readonly IElasticClient _elasticClient;
    
    public Recommender(IElasticClient elasticClient)
    {
        _elasticClient = elasticClient;
    }

    public async Task<Result<List<long>>> GetRecommendedSongsIds(RecommendationsPreferences recommendationsPreferences, int page = 1, int pageSize = 10)
    {
        var queryContainers = new List<QueryContainer>();
        var descriptor = new QueryContainerDescriptor<SongElastic>();

        if (recommendationsPreferences.StartYear.HasValue)
        {
            queryContainers.Add(descriptor.SearchSongsReleasedLaterThanYear(recommendationsPreferences.StartYear.Value));
        }
        if (recommendationsPreferences.EndYear.HasValue)
        {
            queryContainers.Add(descriptor.SearchSongsReleasedEarlierThanYear(recommendationsPreferences.EndYear.Value));
        }
        
        const int deltaSeconds = 30;
        if (recommendationsPreferences.AverageDurationInMinutes.HasValue)
        {
            queryContainers.Add(descriptor.SearchSongsWithDurationInRange(
                recommendationsPreferences.AverageDurationInMinutes.Value,
                deltaSeconds));
        }

        if (!string.IsNullOrWhiteSpace(recommendationsPreferences.AuthorName))
        {
            queryContainers.Add(descriptor.SearchSongsWithAuthorName(recommendationsPreferences.AuthorName));
        }
        
        queryContainers.Add(descriptor.SearchSongsWithGenreId(recommendationsPreferences.GenreId));

        var searchRequest = new SearchDescriptor<SongElastic>();
        searchRequest
            .Index("songs")
            .From((page - 1) * pageSize)
            .Size(pageSize)
            .Query(q => q.Bool(b => b.Must(queryContainers.ToArray())));
        
        var songsElasticResponse = await _elasticClient.SearchAsync<SongElastic>(searchRequest);

        return !songsElasticResponse.IsValid 
            ? new Result<List<long>>(new Exception("Error while creating recommendations"))
            : songsElasticResponse.Documents.Select(s => s.Id).ToList();
    }
}