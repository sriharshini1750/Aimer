using Aimer.Console.IActions;
using Aimer.Core.CQRS.Commands;
using Aimer.Core.Models;
using MediatR;

namespace Aimer.Console.Actions
{
    public class IngestPdfAction : IAction
    {
        private readonly IMediator _mediator;

        public IngestPdfAction(IMediator mediator)
        {
            _mediator = mediator;
        }

        public string Title => "Ingest PDF";

        public async Task RunAsync(CancellationToken cancellationToken = default)
        {
        System.Console.Write("Path to PDF: ");
        var path = System.Console.ReadLine()?.Trim().Trim('"', '\'').Replace("\\ ", " ") ?? "";

        if (!File.Exists(path))
        {
            System.Console.WriteLine("❌ File not found.");
            return;
        }

        var bytes = await File.ReadAllBytesAsync(path, cancellationToken);
        var pdf=new PdfDocument
        {
            Id = Guid.NewGuid(),
            Name = Path.GetFileName(path),
            Content = bytes,
            CreatedAt = DateTime.UtcNow,
            Source="vburhv"
        };
        var command = new IngestPdfDocumentCommand(pdf);
        var result = await _mediator.Send(command, cancellationToken);

        System.Console.WriteLine(result);
        }
    }
}