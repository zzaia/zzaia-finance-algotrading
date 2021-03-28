using MarketIntelligency.Application.SA0001;
using MarketIntelligency.Core.Models.ExchangeAggregate;
using MarketIntelligency.Exchange.MercadoBitcoin.Trade;
using Microsoft.Extensions.Configuration;
using System;
using System.Net.Http;
using System.Threading;
using Xunit;

namespace MarketIntelligency.Test.Exchange.MercadoBitcoin
{
    /// <summary>
    /// ## Instructions for the test ## 
    /// ATTENTION: THIS TEST WILL MAKE REAL TRADES!;
    /// Have the following requirements:
    /// [] The exhange secret and id locally in secrets.json;
    /// [] Set the _unlockTest to true;
    /// /// </summary>
    public class TradeApiClientShould
    {
        private readonly TradeApiClient _client;
        private readonly ClientCredential _clientCredential;
        private bool _unlockTest;
        public TradeApiClientShould()
        {
            var htttpClient = new HttpClient();
            _client = new TradeApiClient(htttpClient);
            _client.SetBaseAddress(new Uri("https://www.mercadobitcoin.net"));
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("secrets.json")
               .AddUserSecrets<Startup>()
               .Build();
            _clientCredential = new ClientCredential();
            configuration.Bind("Exchange:MercadoBitcoin:Private", _clientCredential);
            _unlockTest = false;
        }

        [Fact]
        public async void PlaceMarketBuyOrder()
        {
            if (_unlockTest)
            {
                //Arrange:
                string tickerPair = "BTCBRL";
                string cost = "0.0000000001";
                var cancellationToken = new CancellationToken();

                //Act:
                var response = await _client.PlaceMarketBuyOrderAsync(_clientCredential, tickerPair, cost, cancellationToken);

                //Assert:
                Assert.True(response.Success);
                Assert.NotNull(response);
                Assert.True(response.Output.Success);
                Assert.NotNull(response.Output.Data);
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async void PlaceMarketSellOrder()
        {
            if (_unlockTest)
            {
                //Arrange:
                string tickerPair = "BTCBRL";
                string cost = "0.0000000001";
                var cancellationToken = new CancellationToken();

                //Act:
                var response = await _client.PlaceMarketSellOrderAsync(_clientCredential, tickerPair, cost, cancellationToken);

                //Assert:
                Assert.True(response.Success);
                Assert.NotNull(response);
                Assert.True(response.Output.Success);
                Assert.NotNull(response.Output.Data);
            }
            else
            {
                Assert.True(true);
            }
        }

        [Fact]
        public async void PlaceAndCancelOrder()
        {
            if (_unlockTest)
            {
                //Arrange:
                string tickerPair = "BRLBTC";
                string cost = "0.0000000001";
                var cancellationToken = new CancellationToken();

                var responseFromPlaceOrder = await _client.PlaceMarketSellOrderAsync(_clientCredential, tickerPair, cost, cancellationToken);

                if (responseFromPlaceOrder.Success)
                {
                    int orderId = responseFromPlaceOrder.Output.Data.Id;
                    //Act:
                    var response = await _client.CancelOrderAsync(_clientCredential, tickerPair, orderId, cancellationToken);

                    //Assert:
                    Assert.True(response.Success);
                    Assert.NotNull(response);
                    Assert.True(response.Output.Success);
                    Assert.NotNull(response.Output.Data);
                    Assert.True(response.Output.Data.Status == 3);
                }
                else Assert.True(false);

            }
            else
            {
                Assert.True(true);
            }
        }
    }
}
