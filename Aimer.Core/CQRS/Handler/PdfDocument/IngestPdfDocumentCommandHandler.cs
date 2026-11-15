using Aimer.Core.CQRS.Commands;
using Aimer.Core.CQRS.Middleware.Interfaces;
using Aimer.Core.CQRS.Middleware.Interfaces.AimerDataBase;
using Aimer.Core.Models;
using MediatR;
using Microsoft.Extensions.AI;

namespace Aimer.Core.CQRS.Handler.PdfDocument
{
    public class IngestPdfDocumentCommandHandler : IRequestHandler<IngestPdfDocumentCommand, string>
    {
        private readonly IPdfDocumentRepoService _pdfDocumentRepoService;
        private readonly IPdfExtractorService _pdfExtractorService;
        private readonly IChunkingService _chunkingService;
        private readonly IVectorEmbeddingService _vectorEmbeddingService;
        private readonly IVectorRepoService _vectorRepoService;
        public IngestPdfDocumentCommandHandler(IPdfDocumentRepoService pdfDocumentRepoService,IPdfExtractorService pdfExtractorService,IChunkingService chunkingService, IVectorEmbeddingService vectorEmbeddingService, IVectorRepoService vectorRepoService)
        {
            _pdfDocumentRepoService = pdfDocumentRepoService;
            _pdfExtractorService= pdfExtractorService;
            _chunkingService = chunkingService;
            _vectorEmbeddingService = vectorEmbeddingService;
            _vectorRepoService = vectorRepoService;
        }

        public async Task<string> Handle(IngestPdfDocumentCommand request, CancellationToken cancellationToken)
        {
            // Implement the handling logic here
            await _pdfDocumentRepoService.UploadPdfDocumentAsync(request.PdfDocument);

            var extractedPages =  _pdfExtractorService.ExtractTextAsync(request.PdfDocument.Content);
            Console.WriteLine($"Extracted {extractedPages.Count} pages from the PDF document.");
            var extractedChunks= await _chunkingService.ChunkAsync(extractedPages);
            var embeddedChunks = await CreateEmbeddedChunks(extractedChunks, cancellationToken);
                       Console.WriteLine($"Saved {embeddedChunks.Count} chunks to the vector repository.");

           var x = await _vectorRepoService.SaveChunksAsync(request.PdfDocument.Id, request.PdfDocument.Source, embeddedChunks, cancellationToken);
           Console.WriteLine($"Saved {x} chunks to the vector repository.");

            // Console.WriteLine("embedded chunks: " + embeddedChunks[0].Embedding[0]);
            // Console.WriteLine($"Chunks: {extractedChunks.Count}, Embedded: {embeddedChunks.Count}");
            // Console.WriteLine($"Vector length: {embeddedChunks[0].Embedding.Length}");
            return "PDF document ingested successfully.";
        }
         public async Task<List<EmbeddedChunks>> CreateEmbeddedChunks(IReadOnlyList<ExtractedChunks> chunks, CancellationToken cancellationToken)
        {
            var embeddedChunksList = new List<EmbeddedChunks>();
            foreach (var batch in chunks.Chunk(32))
    {
        var inputs = batch.Select(c => "search_document: " + c.Text);
        var embeddings = await _vectorEmbeddingService.CreateEmbeddedChunks(batch, cancellationToken);

        for (var i = 0; i < batch.Length; i++)
        {
            var embChunkList=new EmbeddedChunks
            {
                Chunks=batch[i],
                Embedding=embeddings[i].Embedding
            };
            embeddedChunksList.Add(embChunkList);
            // TODO: create an EmbeddedChunk from batch[i] and embeddings[i].Vector.ToArray()
            //       and add it to `embedded`
        }
    }
            return embeddedChunksList;
        }
    }
}