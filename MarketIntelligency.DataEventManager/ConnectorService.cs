using MarketIntelligency.Core.Interfaces.ExchangeAggregate;
using MarketIntelligency.Core.Models;
using MarketIntelligency.Core.Models.EnumerationAggregate;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.DataEventManager
{
    public class ConnectorService
    {
        private readonly IExchange _exchange;
        private readonly IMediator _mediator;

        public ConnectorService(IExchange exchange, IMediator mediator)
        {
            _exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        private async Task ConnectToRest<T, TResult>(T parameter, CancellationToken cancelationToken, TimeFrame timeFrame, Func<T, ObjectResult<TResult>> method) where TResult : class
        {
            while (!cancelationToken.IsCancellationRequested)
            {
                try
                {
                    var response = method.Invoke(parameter);
                    if (response.Succeed)
                    {
                        var eventToPublish = new EventSource<TResult>(response.Output);
                        await _mediator.Publish(eventToPublish);
                    }
                    else
                    {

                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}