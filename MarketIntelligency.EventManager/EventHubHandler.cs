using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.MarketAgregate;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.EventManager
{
    public class EventHubHandler : INotificationHandler<INotification>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<EventHubHandler> _logger;
        private readonly Guid _id;
        public EventHubHandler(IMediator mediator, ILogger<EventHubHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _id = Guid.NewGuid();
        }
        /// <summary>
        /// Handle all notifications, to notify handlers that need specific commands.
        /// </summary>
        public async Task Handle(INotification eventSource, CancellationToken cancellationToken)
        {
            if (eventSource.GetType() == typeof(EventSource<OrderBook>))
            {
                var source = (EventSource<OrderBook>)eventSource;
                Console.WriteLine($"### Received, {_id} {source.Content.Asks.FirstOrDefault().Item1} and {source.Content.Asks.FirstOrDefault().Item2} at time {DateTimeOffset.UtcNow}");
            }
            else
            {
                Console.WriteLine("Notification without stream");
            }
            await Task.Delay(1000);
        }
    }
}