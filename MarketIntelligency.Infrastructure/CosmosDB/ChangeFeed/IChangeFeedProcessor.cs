using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Infrastructure.CosmosDB.ChangeFeed
{
    public interface IChangeFeedProcessor<T>
    {
        Task HandleChangesAsync(IReadOnlyCollection<T> changes, CancellationToken cancellationToken);
    }
}