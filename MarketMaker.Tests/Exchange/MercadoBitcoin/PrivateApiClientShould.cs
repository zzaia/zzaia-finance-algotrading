using MarketMaker.Core.Exchange;
using MarketMaker.Exchange.MercadoBitcoin.Models;
using MarketMaker.Exchange.MercadoBitcoin.Private;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MarketMaker.Tests.Exchange.MercadoBitcoin
{
    public class PrivateApiClientShould
    {
        private readonly PrivateApiClient _client;
        private readonly ClientCredential _clientCredential;

        public PrivateApiClientShould()
        {
            var htttpClient = new HttpClient();
            var logger = new Logger<PrivateApiClient>(new LoggerFactory());
            _client = new PrivateApiClient(htttpClient, logger);
            _client.SetBaseAddress(new Uri("https://www.mercadobitcoin.net"));
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("secrets.json")
                .AddUserSecrets<Startup>()
                .Build();
            _clientCredential = new ClientCredential();
            configuration.Bind("Exchange:MercadoBitcoin:Private", _clientCredential);
        }

        [Fact]
        public async void GetSytemsMessages()
        {
            //Arrange:
            string messageLevel = SystemMessageTypeEnum.WARNING.ToString();

            //Act:
            var response = await _client.GetListOfSystemMessagesAsync(_clientCredential, messageLevel);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.True(response.Output.IsSuccess());
            Assert.NotNull(response.Output.Data.Messages);
        }

        [Fact]
        public async void GetAccountInformation()
        {
            //Arrange:

            //Act:
            var response = await _client.GetAccountInformationAsync(_clientCredential);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.True(response.Output.IsSuccess());
            Assert.NotNull(response.Output.Data.Assets);
            Assert.NotNull(response.Output.Data.WithdrawLimits);

        }

        [Fact]
        public async void GetOrderInformation()
        {
            //Arrange:
            int orderId = 81629700;
            string tickerPair = "BRLBTC";

            //Act:
            var response = await _client.GetOrderByIdAsync(_clientCredential, orderId, tickerPair);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.True(response.Output.IsSuccess());
            Assert.NotNull(response.Output.Data);
            Assert.NotNull(response.Output.Data.Order.CreatedAt);

        }

        [Fact]
        public async void GetListOfOrders()
        {
            //Arrange:
            string tickerPair = "BRLBTC";
            string statusList = "[2,3,4]";
            bool hasFills = true;

            //Act:
            var response = await _client.GetListOfOrdersAsync(_clientCredential, tickerPair, statusList, hasFills);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.True(response.Output.IsSuccess());
            Assert.NotNull(response.Output.Data);
            Assert.True(response.Output.Data.Orders.Any());

        }

        [Fact]
        public async void GetCompleteOrderBook()
        {
            //Arrange:
            string tickerPair = "BRLBTC";
            bool fullQuantity = true;

            //Act:
            var response = await _client.GetCompleteOrderBookByTickerPairAsync(_clientCredential, tickerPair, fullQuantity);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.True(response.Output.IsSuccess());
            Assert.NotNull(response.Output.Data);
            Assert.True(response.Output.Data.Orderbook.Asks.Any());
            Assert.True(response.Output.Data.Orderbook.Bids.Any());
            Assert.Equal(500, response.Output.Data.Orderbook.Bids.Count());
            Assert.Equal(500, response.Output.Data.Orderbook.Asks.Count());
        }
    }
}
