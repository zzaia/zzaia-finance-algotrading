using MagoTrader.Core.Exchange;
using MagoTrader.Exchange.MercadoBitcoin.Models;
using MagoTrader.Exchange.MercadoBitcoin.Private;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
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
        public async void PostSytemsMessages()
        {
            //Arrange:
            ClientCredential clientCredential = new ClientCredential();
            _configuration.Bind("Exchange:MercadoBitcoin:Private", clientCredential);

            //Act:
            var response = await _client.PostSystemMessagesAsync(clientCredential, SystemMessageTypeEnum.ERROR);

            //Assert:
            Assert.True(response.Success);
            Assert.NotNull(response);
            Assert.True(response.Output.IsSuccess());
        }

    }
}
