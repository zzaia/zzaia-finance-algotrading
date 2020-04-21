using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MagoTrader.Core.Models;

namespace MagoTrader.Core.Exchange
{
    public class ExchangeInfo
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public ExchangeNameEnum Name { get; set; }

        [Required]
        public IEnumerable<Market> Markets { get; set; }
        public IEnumerable<Market> Futures { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
        public IEnumerable<Asset> Fiats { get; set; }
        public IEnumerable<CountryEnum> Countries { get; set; }
        public IEnumerable<TimeFrame> Timeframes{ get; set; }
        public ExchangeUrls Urls { get; set; }
        public RequiredCredentials RequiredCredentials { get; set; }
        public ExchangeOptions Options { get; set; }
        public MarketFee TradingFee { get; set; }
        public ExchangeLimitRate LimitRate { get; set; }

    }
}