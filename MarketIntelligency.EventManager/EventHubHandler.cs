using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.OrderBookAgregate;
using MarketIntelligency.Core.Utils;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.EventManager
{
    public class EventHubHandler : INotificationHandler<INotification>
    {
        private readonly IStreamSource _streamSource;
        private readonly IMediator _mediator;
        private readonly ILogger<EventHubHandler> _logger;
        public EventHubHandler(IMediator mediator, IStreamSource streamSource, ILogger<EventHubHandler> logger)
        {
            _streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
        /// <summary>
        /// Handle all notifications, to notify handlers that need specific commands.
        /// </summary>
        public async Task Handle(INotification eventSource, CancellationToken cancellationToken)
        {
            if (eventSource.GetType() == typeof(EventSource<OrderBook>))
            {
                _logger.LogInformation($"### Consuming event at {DateTimeUtils.CurrentUtcTimestamp()}");
                var source = (EventSource<OrderBook>)eventSource;
                _streamSource.Publish(source);
            }
            else
            {
                _logger.LogInformation("Notification without stream");
            }
            await Task.Delay(1000, cancellationToken);
        }
    }
}