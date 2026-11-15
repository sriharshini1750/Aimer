namespace Aimer.Core.Models
{
    public class VectorEmbeddings
    {
                public Guid Ids { get; set; } 

        public Guid DocumentId { get; set; } 
        public string? Text { get; set; }
        public float[]? Embedding { get; set; }
        public DateTime CreatedAt { get; set; }
        public int PageNo{get;set;}
        public int ChunkNo { get; set; }
        public string? Source { get; set; }

    }
}