using MarketIntelligency.Core.Models.MarketAgregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.DataEventManager
{
    public class EventHubHandler : INotificationHandler<EventSource<OrderBook>>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventHubHandler> _logger;

        public EventHubHandler(IMediator mediator, ILogger<EventHubHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Handle all notifications, to notify handlers that need specific commands.
        /// </summary>
        public async Task Handle(EventSource<OrderBook> eventSource, CancellationToken cancellationToken)
        {
            Console.WriteLine($"### Received {eventSource.Content.Asks.FirstOrDefault().Item1} and {eventSource.Content.Asks.FirstOrDefault().Item2} at time {DateTimeOffset.UtcNow}");
            await Task.Delay(1000);
        }
    }
}