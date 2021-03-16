using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarketIntelligency.Infrastructure.CosmosDB
{
    public static class CosmosDbExtension
    {
        /// <summary>
        /// Add and configure the CosmosDbO client to use with repository services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureOptions">A delegate to configure the <see cref="CosmosDbOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddCosmosDb(this IServiceCollection services, Action<CosmosDbOptions> configureOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions is null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
            services.AddSingleton<ICosmosDbClient, CosmosDbClient>();
            return services;
        }
    }
}
