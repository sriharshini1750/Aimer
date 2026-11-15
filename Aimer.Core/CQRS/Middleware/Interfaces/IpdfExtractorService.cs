using Aimer.Core.Models;

namespace Aimer.Core.CQRS.Middleware.Interfaces
{
    public interface IPdfExtractorService
    {
         public IReadOnlyList<ExtractedPage> ExtractTextAsync(byte[] Content);
    }
}