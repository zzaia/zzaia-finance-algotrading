using MarketIntelligency.Core.Models.EnumerationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MarketIntelligency.Core.Models.MarketAgregate
{
    public class Market
    {
        public string Ticker { get; }
        public Asset Quote { get; }
        public Asset Base { get; }
        public Market(Asset baseTicker, Asset quoteTicker)
        {
            Base = baseTicker;
            Quote = quoteTicker;
            Ticker = $"{baseTicker.DisplayName}/{quoteTicker.DisplayName}";
        }

        public Market(string pair)
        {
            var assets = Enumeration.GetAll<Asset>().ToList();
            List<Asset> startAssets = new();
            List<Asset> endAssets = new();
            foreach (var currency in assets)
            {
                if (pair.StartsWith(currency.DisplayName, StringComparison.InvariantCultureIgnoreCase))
                {
                    startAssets.Add(currency);
                }
                if (pair.EndsWith(currency.DisplayName, StringComparison.InvariantCultureIgnoreCase))
                {
                    endAssets.Add(currency);
                }
            }

            foreach (var startCurrency in startAssets)
            {
                foreach (var endCurrency in endAssets)
                {
                    if (pair.Equals(startCurrency.DisplayName + endCurrency.DisplayName, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Base = startCurrency;
                        Quote = endCurrency;
                        Ticker = startCurrency.DisplayName + "/" + endCurrency.DisplayName;
                    }
                }
            }
        }
    }
}