using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;

namespace MarketIntelligency.Infrastructure.CosmosDB.ChangeFeed
{
    public class ChangedCommand<T> : IRequest where T : class
    {
        public ChangedCommand(IReadOnlyCollection<T> changes, CancellationToken cancellationToken)
        {
            Changes = changes ?? throw new ArgumentNullException(nameof(changes));
            CancellationToken = cancellationToken;
        }
        public IReadOnlyCollection<T> Changes { get; set; }
        public CancellationToken CancellationToken { get; set; }
    }
}