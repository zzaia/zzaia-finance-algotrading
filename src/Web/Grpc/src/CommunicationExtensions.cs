using MarketIntelligency.Web.Grpc.Models;
using MarketIntelligency.Web.Grpc.Protos;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace MarketIntelligency.Web.Grpc
{
    public static class CommunicationExtensions
    {
        /// <summary>
        /// Adds services and options for the data event manager.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="managerOptions">A delegate to configure the <see cref="EventManagerOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcCommunication(this IServiceCollection services, string baseAddress = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (!string.IsNullOrEmpty(baseAddress))
            {
                services.AddGrpcClient<StreamEventGrpc.StreamEventGrpcClient>(opt => opt.Address = new Uri(baseAddress));
                services.AddHostedService<CommunicationHandler>();
            }

            services.AddGrpc();
            services.AddAutoMapper(configuration => configuration.AddProfile(new MappingProfile()));

            return services;
        }
    }
}
