using Aimer.Core.Models;

namespace Aimer.Core.CQRS.Middleware.Interfaces
{
    public interface IVectorEmbeddingService
    {
         public Task<List<EmbeddedChunks>> CreateEmbeddedChunks(IReadOnlyList<ExtractedChunks> chunks, CancellationToken cancellationToken);
    }
}