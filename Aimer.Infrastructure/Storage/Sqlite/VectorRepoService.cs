using Aimer.Core.CQRS.Middleware.Interfaces.AimerDataBase;
using Aimer.Core.Models;
using Microsoft.Data.Sqlite;

namespace Aimer.Infrastructure.Storage.Sqlite;

public class VectorRepoService : IVectorRepoService
{
    private readonly string _connectionString;

    public VectorRepoService()
    {
        var folder = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "AimerData");
        Directory.CreateDirectory(folder);

        _connectionString = new SqliteConnectionStringBuilder
        {
            DataSource = Path.Combine(folder, "aimer.db")
        }.ToString();

        EnsureCreated();
    }

    private void EnsureCreated()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = """
            CREATE TABLE IF NOT EXISTS chunks (
                id           INTEGER PRIMARY KEY AUTOINCREMENT,
                document_id  TEXT    NOT NULL,
                source       TEXT,
                page_number  INTEGER NOT NULL,
                chunk_index  INTEGER NOT NULL,
                text         TEXT    NOT NULL,
                embedding    BLOB    NOT NULL,
                ingested_at  TEXT    NOT NULL
            );
            CREATE INDEX IF NOT EXISTS ix_chunks_document_id ON chunks (document_id);
            """;
        command.ExecuteNonQuery();
    }

    public async Task<int> SaveChunksAsync(Guid documentId, string? source,
        IReadOnlyList<EmbeddedChunks> chunks, CancellationToken cancellationToken = default)
    {
        if (chunks.Count == 0) return 0;

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        using var transaction = connection.BeginTransaction();

        await using var command = connection.CreateCommand();
        command.Transaction = transaction;
        command.CommandText = """
            INSERT INTO chunks (document_id, source, page_number, chunk_index, text, embedding, ingested_at)
            VALUES ($documentId, $source, $pageNumber, $chunkIndex, $text, $embedding, $ingestedAt);
            """;

        var documentIdParam = command.Parameters.Add("$documentId", SqliteType.Text);
        var sourceParam     = command.Parameters.Add("$source", SqliteType.Text);
        var pageParam       = command.Parameters.Add("$pageNumber", SqliteType.Integer);
        var indexParam      = command.Parameters.Add("$chunkIndex", SqliteType.Integer);
        var textParam       = command.Parameters.Add("$text", SqliteType.Text);
        var embeddingParam  = command.Parameters.Add("$embedding", SqliteType.Blob);
        var ingestedParam   = command.Parameters.Add("$ingestedAt", SqliteType.Text);

        documentIdParam.Value = documentId.ToString();
        sourceParam.Value     = (object?)source ?? DBNull.Value;
        ingestedParam.Value   = DateTime.UtcNow.ToString("O");

        foreach (var chunk in chunks)
        {
            cancellationToken.ThrowIfCancellationRequested();

            pageParam.Value      = chunk.Chunks.PageNumber;
            indexParam.Value     = chunk.Chunks.ChunkIndex;
            textParam.Value      = chunk.Chunks.Text;
            embeddingParam.Value = ToBytes(chunk.Embedding);

            await command.ExecuteNonQueryAsync(cancellationToken);
        }

        transaction.Commit();
        return chunks.Count;
    }

    private static byte[] ToBytes(float[] vector)
    {
        var bytes = new byte[vector.Length * sizeof(float)];
        Buffer.BlockCopy(vector, 0, bytes, 0, bytes.Length);
        return bytes;
    }

    private static float[] FromBytes(byte[] bytes)
    {
        var vector = new float[bytes.Length / sizeof(float)];
        Buffer.BlockCopy(bytes, 0, vector, 0, bytes.Length);
        return vector;
    }

}