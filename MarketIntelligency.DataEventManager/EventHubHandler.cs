using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.DataEventManager
{
    public class EventHubHandler : INotificationHandler<INotification>
    {
        private readonly IMediator _mediator;

        public EventHubHandler(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }
        /// <summary>
        /// Handle all notifications, to notify handlers that need specific commands.
        /// </summary>
        public async Task Handle(INotification notification, CancellationToken cancellationToken)
        {

        }
    }
}
