using Aimer.Core.Models;

namespace Aimer.Core.CQRS.Middleware.Interfaces
{
    public interface IChunkingService
    {
         public Task<IReadOnlyList<ExtractedChunks>> ChunkAsync(IReadOnlyList<ExtractedPage> pages); 
    }
}