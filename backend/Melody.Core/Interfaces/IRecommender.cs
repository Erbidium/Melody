using LanguageExt.Common;
using Melody.Core.Entities;

namespace Melody.Core.Interfaces;

public interface IRecommender
{
    public Task<Result<List<long>>> GetRecommendedSongsIds(RecommendationsPreferences recommendationsPreferences, int page = 1, int pageSize = 10);
}