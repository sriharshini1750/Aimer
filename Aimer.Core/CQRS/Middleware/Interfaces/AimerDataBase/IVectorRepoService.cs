using Aimer.Core.Models;

namespace Aimer.Core.CQRS.Middleware.Interfaces.AimerDataBase
{
    public interface IVectorRepoService
    {
         public Task<int> SaveChunksAsync(Guid documentId,
        string? source,IReadOnlyList<EmbeddedChunks> embeddedChunks,CancellationToken cancellationToken = default);
    }
}