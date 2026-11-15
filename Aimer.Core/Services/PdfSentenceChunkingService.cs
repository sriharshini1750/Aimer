using System.Text;
using System.Text.RegularExpressions;
using Aimer.Core.CQRS.Middleware.Interfaces;
using Aimer.Core.Models;

namespace Aimer.Core.Services
{
    public class PdfSentenceChunkingService:IChunkingService
    {
        private const int MaxChunkSize = 1500;
private const int OverlapSize = 200;
        public async Task<IReadOnlyList<ExtractedChunks>> ChunkAsync(IReadOnlyList<ExtractedPage> pages)
        {
            var chunks = new List<ExtractedChunks>();
        foreach (var page in pages)
         {
            if(page.Text==null)
                continue;
        var text = Regex.Replace(page.Text, @"\s+", " ").Trim();
        var sentences = Regex.Split(text, @"(?<=[.!?])\s+");

        var current = new StringBuilder();
        var chunkIndex = 0;

        foreach (var sentence in sentences)
        {
            if (string.IsNullOrWhiteSpace(sentence)) continue;

            if (current.Length + sentence.Length > MaxChunkSize && current.Length > 0)
            {
                // TODO 1: save `current` as a TextChunk (page.PageNumber, chunkIndex++, ...)
                //         then reset `current` so it starts with the last OverlapSize characterst
                var newChunk=new ExtractedChunks
                {
                    Text=current.ToString().Trim(),
                    ChunkIndex=chunkIndex++,
                    PageNumber=page.PageNumber
                };
                chunks.Add(newChunk);

                var chunkText = current.ToString();
                var overlap = chunkText.Length > OverlapSize ? chunkText[^OverlapSize..] : chunkText;

                current.Clear();
                current.Append(overlap).Append(' ');
            }

            current.Append(sentence).Append(' ');
        }
                if (current.Length > 0)
                {
                    var newChunk=new ExtractedChunks
                    {
                        Text=current.ToString().Trim(),
                        ChunkIndex=chunkIndex++,
                        PageNumber=page.PageNumber
                    };
                    chunks.Add(newChunk);
                }

        // TODO 2: save whatever is left in `current` (the last chunk of the page!)
    }            return chunks;
        }
    }
}