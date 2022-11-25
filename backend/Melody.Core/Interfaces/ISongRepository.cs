﻿using Melody.Core.Entities;
using Melody.SharedKernel.Interfaces;

namespace Melody.Infrastructure.Data.Repositories;

public interface ISongRepository : IRepository<Song, long>
{
    public Task<long> GetTotalBytesSumUploadsByUser(long userId);
    public Task<IReadOnlyCollection<Song>> GetSongsUploadedByUserId(long userId);
}