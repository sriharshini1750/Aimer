using MediatR;
using Aimer.Core.CQRS.Commands;
using Aimer.Core.CQRS.Middleware.Interfaces;
namespace Aimer.Core.CQRS.Handler.PdfDocument
{
    public class UploadPdfCommandHandler:IRequestHandler<UploadPdfDocument, string>
    {
        private readonly IPdfDocumentRepoService _pdfDocumentRepoService;
        public UploadPdfCommandHandler(IPdfDocumentRepoService pdfDocumentRepoService)
        {
            _pdfDocumentRepoService = pdfDocumentRepoService;
        }
        public async Task<string> Handle(UploadPdfDocument request, CancellationToken cancellationToken)
        {
            // Implement the logic to handle the upload of the PDF document here
            return await _pdfDocumentRepoService.UploadPdfDocumentAsync(request.Pdf);
        }
    }
}