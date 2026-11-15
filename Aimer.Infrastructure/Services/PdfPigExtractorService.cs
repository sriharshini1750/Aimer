using Aimer.Core.CQRS.Middleware.Interfaces;
using Aimer.Core.Models;
using pdfDocExtractor=UglyToad.PdfPig.PdfDocument;

namespace Aimer.Infrastructure.Services
{
    public class PdfPigExtractorService:IPdfExtractorService
    {

        public IReadOnlyList<ExtractedPage> ExtractTextAsync(byte[] Content)
        {
            var extractedPages=new List<ExtractedPage>();
            var pdfDoc=pdfDocExtractor.Open(Content);
            foreach(var page in pdfDoc.GetPages())
            {
                var text=page.Text;
                if(string.IsNullOrWhiteSpace(text))
                    continue;
                var pageNo=page.Number;
                extractedPages.Add(new ExtractedPage { PageNumber = pageNo, Text = text });
            }
            return extractedPages;
        }
    }
}