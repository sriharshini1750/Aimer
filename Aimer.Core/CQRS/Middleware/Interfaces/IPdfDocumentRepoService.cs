using Aimer.Core.Models;

namespace Aimer.Core.CQRS.Middleware.Interfaces
{
    public interface IPdfDocumentRepoService
    {
         public Task<string> UploadPdfDocumentAsync(PdfDocument pdf);
    }
}