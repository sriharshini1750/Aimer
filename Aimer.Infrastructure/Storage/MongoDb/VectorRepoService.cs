using Aimer.Core.CQRS.Middleware.Interfaces.AimerDataBase;
using Aimer.Core.Models;
using MongoDB.Driver;

namespace Aimer.Infrastructure.Storage.MongoDb
{
    public class VectorRepoService:IVectorRepoService
    {
        private readonly IMongoDatabase _mongoDatabase;
     private readonly IMongoCollection<VectorEmbeddings> _chunks;

        public VectorRepoService(IMongoDatabase mongoClient)
        {
            _mongoDatabase = mongoClient;
            _chunks = _mongoDatabase.GetCollection<VectorEmbeddings>("chunks");
        }



        public async Task<int> SaveChunksAsync(Guid documentId, string? source, IReadOnlyList<EmbeddedChunks> embeddedChunks, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"Saving {embeddedChunks.Count} embedded chunks to the vector repository.");
            var vectorEmbeddings = new List<VectorEmbeddings>();
            foreach (var chunk in embeddedChunks)
            {
                if (chunk.Embedding == null || chunk.Embedding.Length == 0)
                {
                    continue;
                }
                var vector=new VectorEmbeddings
                {
                    Ids = Guid.NewGuid(),
                    DocumentId = documentId,
                    Text = chunk.Chunks.Text,
                    Embedding = chunk.Embedding,
                    CreatedAt = DateTime.UtcNow,
                    PageNo = chunk.Chunks.PageNumber,
                    ChunkNo = chunk.Chunks.ChunkIndex,
                    Source = source
                };
                vectorEmbeddings.Add(vector);
            }


           await  _chunks.InsertManyAsync(vectorEmbeddings, cancellationToken: cancellationToken);
            return vectorEmbeddings.Count;
        }
    }
}