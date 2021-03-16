using MediatR;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MarketIntelligency.Infrastructure.CosmosDB.ChangeFeed
{
    public class ChangeFeedProcessor<T> : IHostedService where T : class
    {
        private readonly IMediator _mediator;
        private readonly ICosmosDbClient _client;
        private ChangeFeedOptions _options;
        private ChangeFeedProcessor _changeFeedProcessor;
        public ChangeFeedProcessor(Action<ChangeFeedOptions> options, IMediator mediator, ICosmosDbClient client)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));
            var changeFeedModel = new ChangeFeedOptions();
            options.Invoke(changeFeedModel);
            _options = changeFeedModel;
            _options.InstanceName ??= Guid.NewGuid().ToString();
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            Container leaseContainer = _client.GetLeaseContainer();
            Container monitoredContainer = _client.GetContainer();

            _changeFeedProcessor = monitoredContainer
                .GetChangeFeedProcessorBuilder<T>(_options.ProcessorName, HandleChangesAsync)
                    .WithInstanceName(_options.InstanceName)
                    .WithLeaseContainer(leaseContainer)
                    .Build();

            await _changeFeedProcessor.StartAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _changeFeedProcessor.StopAsync();
        }

        /// <summary>
        /// The delegate receives batches of changes as they are generated in the change feed and publish to the mediator pattern.
        /// </summary>
        public async Task HandleChangesAsync(IReadOnlyCollection<T> changes, CancellationToken cancellationToken)
        {
            var command = new ChangedCommand<T>(changes, cancellationToken);
            await _mediator.Send(command);
        }
    }
}