using Aimer.Core.CQRS.Middleware.Interfaces;
using Aimer.Core.Models;

namespace Aimer.Infrastructure.Storage.Documents
{
    public class PdfDocumentRepoService: IPdfDocumentRepoService
    {
         private readonly string _storageFolder;

    public PdfDocumentRepoService()
    {
        _storageFolder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
            "AimerData", "pdfs");

        Directory.CreateDirectory(_storageFolder);
    }
        
        public async Task<string> UploadPdfDocumentAsync(PdfDocument document)
        {
            var filePath = Path.Combine(_storageFolder, $"{document.Id}.pdf");
            await File.WriteAllBytesAsync(filePath, document.Content);
            return document.Id.ToString();
        }
    }
}