using Aimer.Core.CQRS.Commands;
using Aimer.Core.Models;
using MediatR;
using Aimer.Console.IActions;
namespace Aimer.Console.Actions;

public class UploadPdfAction(IMediator mediator):IAction
{
    public string Title => "Upload PDF";

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
        var command = new UploadPdfDocument(pdf);
        var result = await mediator.Send(command, cancellationToken);

        System.Console.WriteLine(result);
    }
}