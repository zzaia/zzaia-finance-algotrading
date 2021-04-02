using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Options;
using System;

namespace MarketIntelligency.Infrastructure.CosmosDB
{
    public class CosmosDbClient : ICosmosDbClient
    {
        private readonly CosmosClient _cosmosClient;
        private Container _container;
        private Container _leaseContainer;
        public CosmosDbClient() { }

        public CosmosDbClient(IOptionsMonitor<CosmosDbOptions> options)
        {
            var curentOptions = options.CurrentValue ?? throw new ArgumentNullException(nameof(options));
            _cosmosClient = new CosmosClient(curentOptions.AccountEndpoint, GetCosmosClientOptions());
            _container = _cosmosClient.GetContainer(curentOptions.DatabaseId, curentOptions.ContainerId);
            _leaseContainer = _cosmosClient.GetContainer(curentOptions.DatabaseId, curentOptions.LeaseContainerId);
        }

        public Container GetContainer()
        {
            return _container;
        }

        public Container GetLeaseContainer()
        {
            return _leaseContainer;
        }

        private CosmosClientOptions GetCosmosClientOptions()
        {
            var clientOptions = new CosmosClientOptions()
            {
                SerializerOptions = new CosmosSerializationOptions()
                {
                    IgnoreNullValues = true,
                    PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                }
            };
            return clientOptions;
        }
    }
}