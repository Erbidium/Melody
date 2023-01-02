using Nest;

namespace Melody.Infrastructure.ElasticSearch;

public static class QueryContainerDescriptorExtensions
{
    public static QueryContainer SearchSongsReleasedLaterThanYear(this QueryContainerDescriptor<SongElastic> descriptor, int startYear)
    {
        return descriptor.Range(selector => selector
            .Field(song => song.Year)
            .GreaterThanOrEquals(startYear));
    }
    
    public static QueryContainer SearchSongsReleasedEarlierThanYear(this QueryContainerDescriptor<SongElastic> descriptor, int endYear)
    {
        return descriptor.Range(selector => selector
            .Field(song => song.Year)
            .LessThanOrEquals(endYear));
    }
    
    public static QueryContainer SearchSongsWithDurationInRange(this QueryContainerDescriptor<SongElastic> descriptor, int durationInMinutes, int deltaSeconds)
    {
        var startDuration = durationInMinutes * 60 - deltaSeconds > 0
            ? durationInMinutes * 60 - deltaSeconds
            : 60;

        var endDuration = durationInMinutes * 60 + 30;
        return descriptor.Range(selector => selector
            .Field(song => song.DurationInSeconds)
            .GreaterThanOrEquals(startDuration)
            .LessThanOrEquals(endDuration));
    }
    
    public static QueryContainer SearchSongsWithAuthorName(this QueryContainerDescriptor<SongElastic> descriptor, string author)
    {
        return descriptor.Match(selector => selector
            .Field(song => song.AuthorName)
            .Query(author)
            .Fuzziness(Fuzziness.Auto));
    }

    public static QueryContainer SearchSongsWithGenreId(this QueryContainerDescriptor<SongElastic> descriptor, long genreId)
    {
        return descriptor.Term(selector => selector
            .Field(song => song.GenreId)
            .Value(genreId));
    }
}