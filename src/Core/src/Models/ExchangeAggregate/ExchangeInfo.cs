using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Zzaia.Finance.Core.Models.ExchangeAggregate
{
    public class ExchangeInfo
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public ExchangeName Name { get; set; }

        [Required]
        public IEnumerable<Market> Markets { get; set; }
        public IEnumerable<Market> Futures { get; set; }
        public IEnumerable<Asset> Assets { get; set; }
        public IEnumerable<OperationInfo> Operations { get; set; }
        public Country Country { get; set; }
        public CultureInfo Culture { get; set; }
        public IEnumerable<TimeFrame> Timeframes { get; set; }
        public ExchangeUris Uris { get; set; }
        public RequiredCredentials RequiredCredentials { get; set; }
        public ExchangeLimitRate LimitRate { get; set; }
        public ExchangeOptions Options { get; set; }

    }
}