using Zzaia.Finance.Web.Grpc.Models;
using Zzaia.Finance.Web.Grpc.Protos;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Zzaia.Finance.Web.Grpc
{
    public static class CommunicationExtensions
    {
        /// <summary>
        /// Adds services and options for the data event manager grpc unnary communication.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="baseAddress">Base address to event communication</param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcEventCommunication(this IServiceCollection services, string baseAddress = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (!string.IsNullOrEmpty(baseAddress))
            {
                services.AddGrpcClient<EventGrpc.EventGrpcClient>(opt => opt.Address = new Uri(baseAddress));
                services.AddHostedService<EventCommunicationHandler>();
            }

            services.AddGrpc();
            services.AddAutoMapper(configuration => configuration.AddProfile(new MappingProfile()));

            return services;
        }

        /// <summary>
        /// Adds services and options for the data event manager grpc stream connection.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="baseAddress">Base address to event communication</param>
        /// <returns></returns>
        public static IServiceCollection AddGrpcStreamCommunication(this IServiceCollection services, string baseAddress = null)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (!string.IsNullOrEmpty(baseAddress))
            {
                services.AddGrpcClient<StreamEventGrpc.StreamEventGrpcClient>(opt => opt.Address = new Uri(baseAddress));
                services.AddHostedService<StreamEventCommunicationHandler>();
            }

            services.AddGrpc();
            services.AddAutoMapper(configuration => configuration.AddProfile(new MappingProfile()));

            return services;
        }
    }
}
