using MarketIntelligency.Core.Models.MarketAgregate;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace MarketIntelligency.DataEventManager.MediatorAggregate
{
    /// <summary>
    /// Extension methods for the Data Event Manager.
    /// </summary>
    public static class DataEventManagerExtension
    {
        /// <summary>
        /// Adds services and options for the data event manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="managerOptions">A delegate to configure the <see cref="EventManagerOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddEventManager(this IServiceCollection services,
             Action<EventManagerOptions> managerOptions, params Assembly[] assemblies)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }
            services.AddSingleton<Publisher>();
            services.AddSingleton<INotificationHandler<EventSource<OrderBook>>, EventHubHandler>();
            services.AddMediatR(assemblies);

            return services;
        }
    }
}
