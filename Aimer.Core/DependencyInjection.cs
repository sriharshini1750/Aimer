using Aimer.Core.CQRS.Middleware.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Aimer.Core.Services;

namespace Aimer.Core
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCore(this IServiceCollection services)
        // Register core services here
        {
            services.AddMediatR(med => med.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddSingleton<IChunkingService, PdfSentenceChunkingService>();
            return services;
        }
    }
}