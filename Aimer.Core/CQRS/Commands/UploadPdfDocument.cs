using System.Net;
using Aimer.Core.Models;
using MediatR;

namespace Aimer.Core.CQRS.Commands;

    public record UploadPdfDocument(PdfDocument Pdf):IRequest<string>;

