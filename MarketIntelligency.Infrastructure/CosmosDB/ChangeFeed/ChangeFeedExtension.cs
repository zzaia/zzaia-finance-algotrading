using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarketIntelligency.Infrastructure.CosmosDB.ChangeFeed
{
    public static class ChangeFeedExtension
    {
        /// <summary>
        /// Add and configure the CosmosDbO client to use with repository services.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="configureOptions">A delegate to configure the <see cref="ChangeFeedOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddChangeFeed<Ttype>(this IServiceCollection services, Action<ChangeFeedOptions> configureOptions) where Ttype : class
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions is null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddHostedService((s) =>
            {
                var mediator = (IMediator)s.GetService(typeof(IMediator));
                var client = (ICosmosDbClient)s.GetService(typeof(ICosmosDbClient));
                return new ChangeFeedProcessor<Ttype>(configureOptions, mediator, client);
            });
            return services;
        }
    }
}