using MarketIntelligency.Core.Models.MarketAgregate;
using MarketIntelligency.DataEventManager;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Application.SA0001.Strategies
{
    public class MarketMakerHandler : INotificationHandler<EventSource<OrderBook>>
    {
        private readonly IMediator _mediator;
        private readonly ILogger<MarketMakerHandler> _logger;

        public MarketMakerHandler(IMediator mediator, ILogger<MarketMakerHandler> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Handle responsable to execute the strategy logic.
        /// </summary>
        public async Task Handle(EventSource<OrderBook> eventSource, CancellationToken cancellationToken)
        {
            /// <summary>
            /// Xisto & Cassar Strategy logic
            /// </summary>
            /// 
            Console.WriteLine("Received Ordebook");
            await Task.Delay(1000);
            var response = new Order();
            await _mediator.Send(response);
        }
    }
}