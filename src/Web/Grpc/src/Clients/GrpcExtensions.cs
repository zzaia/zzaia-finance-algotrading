using Grpc.Net.ClientFactory;
using MarketIntelligency.Web.Grpc.Clients.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Net.Http;

namespace MarketIntelligency.Web.Grpc.Clients
{
    /// <summary>
    /// Extension methods for the BankOtcExchange Grpc client.
    /// </summary>
    public static class GrpcExtensions
    {
        /// <summary>
        /// Adds services and options for the BankOtcExchange Grpc Grpc client.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <param name="authOptions">A delegate to configure the <see cref="OAuth2Options"/>.</param>
        /// <param name="grpcOptions">A delegate to configure the <see cref="GrpcClientFactoryOptions"/>.</param>
        /// <returns></returns>
        public static IServiceCollection AddControlGrpcClient(this IServiceCollection services, IHostEnvironment env, Action<OAuth2Options> authOptions, Action<GrpcClientFactoryOptions> grpcOptions)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (authOptions is null)
            {
                throw new ArgumentNullException(nameof(authOptions));
            }

            if (grpcOptions is null)
            {
                throw new ArgumentNullException(nameof(grpcOptions));
            }

            OAuth2Options authOptionsTarget = new OAuth2Options();
            authOptions.Invoke(authOptionsTarget);

            if (env.IsEnvironment("Test"))
            {
                services.AddScoped<IControlGrpc, MockControlGrpc>();
            }
            else
            {
                services.AddSingleton<IGrpcAuthentication, GrpcAuthentication>(ctx =>
                {
                    IHttpClientFactory clientFactory = ctx.GetRequiredService<IHttpClientFactory>();
                    HttpClient httpClient = clientFactory.CreateClient();
                    return new GrpcAuthentication(httpClient, clientId: authOptionsTarget.ClientId, clientSecret: authOptionsTarget.ClientSecret, tokenIssuer: authOptionsTarget.Issuer, scope: authOptionsTarget.Scope);
                });

                services.AddGrpcClient<Protos.ControlGrpc.ControlGrpcClient>(grpcOptions);
                services.AddScoped<IControlGrpc, ControlGrpc>();
            }
            return services;
        }
    }
}