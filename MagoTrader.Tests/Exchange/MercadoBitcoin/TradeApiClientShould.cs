using MarketMaker.Core.Exchange;
using MarketMaker.Exchange.MercadoBitcoin.Trade;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using Xunit;

namespace MarketMaker.Tests.Exchange.MercadoBitcoin
{
    /// <summary>
    /// ## Instructions for the test ## 
    /// ATTENTION: THIS TEST WILL MAKE REAL TRADES;
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
            var logger = new Logger<TradeApiClient>(new LoggerFactory());
            _client = new TradeApiClient(htttpClient, logger);
            _client.SetBaseAddress(new Uri("https://www.mercadobitcoin.net"));
            var configuration = new ConfigurationBuilder()
               .AddJsonFile("secrets.json")
               .AddUserSecrets<MarketMaker.ServerApp.Startup>()
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
                //Act:
                var response = await _client.PlaceMarketBuyOrderAsync(_clientCredential, tickerPair, cost);

                //Assert:
                Assert.True(response.Success);
                Assert.NotNull(response);
                Assert.True(response.Output.IsSuccess());
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

                //Act:
                var response = await _client.PlaceMarketSellOrderAsync(_clientCredential, tickerPair, cost);

                //Assert:
                Assert.True(response.Success);
                Assert.NotNull(response);
                Assert.True(response.Output.IsSuccess());
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
                var responseFromPlaceOrder = await _client.PlaceMarketSellOrderAsync(_clientCredential, tickerPair, cost);
                if (responseFromPlaceOrder.Success)
                {
                    int orderId = responseFromPlaceOrder.Output.Data.Id;
                    //Act:
                    var response = await _client.CancelOrderAsync(_clientCredential, tickerPair, orderId);

                    //Assert:
                    Assert.True(response.Success);
                    Assert.NotNull(response);
                    Assert.True(response.Output.IsSuccess());
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
