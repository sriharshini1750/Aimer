using Aimer.Core.Models;
using Microsoft.Extensions.AI;
using Aimer.Core.CQRS.Middleware.Interfaces;

namespace Aimer.Infrastructure.Services
{
    public class OllamaVectorEmbeddingService:IVectorEmbeddingService
    {
        private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator;
        public OllamaVectorEmbeddingService(IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator)
        {
            _embeddingGenerator = embeddingGenerator;
        }
                 public async Task<List<EmbeddedChunks>> CreateEmbeddedChunks(IReadOnlyList<ExtractedChunks> chunks, CancellationToken cancellationToken)
        {
            var embeddedChunksList = new List<EmbeddedChunks>();
            foreach (var batch in chunks.Chunk(32))
    {
        var inputs = batch.Select(c => "search_document: " + c.Text);
        var embeddings = await _embeddingGenerator.GenerateAsync(inputs, cancellationToken: cancellationToken);

        for (var i = 0; i < batch.Length; i++)
        {
            var embChunkList=new EmbeddedChunks
            {
                Chunks=batch[i],
                Embedding=embeddings[i].Vector.ToArray()
            };
            embeddedChunksList.Add(embChunkList);
        }
    }
            return embeddedChunksList;
        }
    }

    }
