namespace Aimer.Core.Models
{
    public class PdfDocument
    {
        public  required Guid Id{get; init;}= Guid.NewGuid();
        public required string Name { get; set; }
        
         public required byte[] Content { get; init; }
         public DateTime CreatedAt { get; init; }= DateTime.UtcNow;
         public long SizeInBytes => Content.Length;
         public  string? Source { get; set; }
    }
}