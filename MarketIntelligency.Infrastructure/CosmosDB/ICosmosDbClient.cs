using Microsoft.Azure.Cosmos;

namespace MarketIntelligency.Infrastructure.CosmosDB
{
    public interface ICosmosDbClient
    {
        public Container GetLeaseContainer();
        public Container GetContainer();
    }
}