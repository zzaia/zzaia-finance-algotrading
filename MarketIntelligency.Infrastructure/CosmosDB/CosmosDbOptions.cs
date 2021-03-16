namespace MarketIntelligency.Infrastructure.CosmosDB
{
    public class CosmosDbOptions
    {
        public CosmosDbOptions()
        {
            AccountEndpoint = "AccountEndpoint=https://localhost:8081/;AccountKey=C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==;";
            DatabaseId = "transferoDatabaseName";
            ContainerId = "transferoCollectionId";
        }
        public string AccountAuthSecret { get; set; }
        public string AccountEndpoint { get; set; }
        public string DatabaseId { get; set; }
        public string ContainerId { get; set; }
        public string LeaseContainerId { get; set; }
    }
}