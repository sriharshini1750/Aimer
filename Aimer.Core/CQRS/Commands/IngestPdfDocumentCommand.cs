using Aimer.Core.Models;
using MediatR;

namespace Aimer.Core.CQRS.Commands;

   
 public record IngestPdfDocumentCommand(PdfDocument PdfDocument):IRequest<String>;
    
