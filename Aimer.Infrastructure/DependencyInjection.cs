using Aimer.Core.CQRS.Middleware.Interfaces;
using Aimer.Core.CQRS.Middleware.Interfaces.AimerDataBase;
using Aimer.Infrastructure.Storage.Documents;
using Aimer.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Configuration;
using OllamaSharp;
using MongoDB.Driver;
using Aimer.Infrastructure.Storage.Sqlite;

namespace Aimer.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection  AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPdfDocumentRepoService, PdfDocumentRepoService>();
            services.AddSingleton<IPdfExtractorService, PdfPigExtractorService>();
            services.AddSingleton<IEmbeddingGenerator<string, Embedding<float>>>(
                new OllamaApiClient(new Uri("http://localhost:11434"), "nomic-embed-text"));

            var connectionString = configuration["MongoDb:ConnectionString"]
                ?? throw new InvalidOperationException("MongoDb:ConnectionString is missing. Set it with dotnet user-secrets.");
            var databaseName = configuration["MongoDb:DatabaseName"] ?? "aimer";

            services.AddSingleton<IMongoClient>(new MongoClient(connectionString));
            services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(databaseName));
            services.AddSingleton<IVectorRepoService, VectorRepoService>();
            services.AddSingleton<IVectorEmbeddingService, OllamaVectorEmbeddingService>();

            return services;
        }
    }
}