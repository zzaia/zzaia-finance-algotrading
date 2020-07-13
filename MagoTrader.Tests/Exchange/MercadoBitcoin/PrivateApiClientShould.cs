using MagoTrader.Core.Exchange;
using MagoTrader.Exchange.MercadoBitcoin.Models;
using MagoTrader.Exchange.MercadoBitcoin.Private;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Net.Http;
using Xunit;

namespace MagoTrader.Tests.Exchange.MercadoBitcoin
{
    public class PrivateApiClientShould
    {
        private readonly PrivateApiClient _client;
        private readonly IConfiguration _configuration;
        public PrivateApiClientShould()
        {
            var htttpClient = new HttpClient();
            var logger = new Logger<PrivateApiClient>(new LoggerFactory());
            _client = new PrivateApiClient(htttpClient, logger);
            _client.SetBaseAddress(new Uri("https://www.mercadobitcoin.net"));
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("secrets.json")
                .AddUserSecrets<MagoTrader.ServerApp.Startup>()
                .Build();
        }

        [Fact]
        public async void GetSytemsMessages()
        {
            //Arrange:
            ClientCredential clientCredential = new ClientCredential();
            _configuration.Bind("Exchange:MercadoBitcoin:Private", clientCredential);
            string messageLevel = SystemMessageTypeEnum.WARNING.ToString();

            //Act:
            var response = await _client.GetListOfSystemMessagesAsync(clientCredential, messageLevel);

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
            ClientCredential clientCredential = new ClientCredential();
            _configuration.Bind("Exchange:MercadoBitcoin:Private", clientCredential);

            //Act:
            var response = await _client.GetAccountInformationAsync(clientCredential);

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
            ClientCredential clientCredential = new ClientCredential();
            _configuration.Bind("Exchange:MercadoBitcoin:Private", clientCredential);
            int orderId = 81629700;
            string tickerPair = "BRLBTC";

            //Act:
            var response = await _client.GetOrderByIdAsync(clientCredential, orderId, tickerPair);

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
            ClientCredential clientCredential = new ClientCredential();
            _configuration.Bind("Exchange:MercadoBitcoin:Private", clientCredential);
            string tickerPair = "BRLBTC";
            string statusList = "[2,3,4]";
            bool hasFills = true;

            //Act:
            var response = await _client.GetListOfOrdersAsync(clientCredential, tickerPair, statusList, hasFills);

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
            ClientCredential clientCredential = new ClientCredential();
            _configuration.Bind("Exchange:MercadoBitcoin:Private", clientCredential);
            string tickerPair = "BRLBTC";
            bool fullQuantity = true;

            //Act:
            var response = await _client.GetCompleteOrderBookByTickerPairAsync(clientCredential, tickerPair, fullQuantity);

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
