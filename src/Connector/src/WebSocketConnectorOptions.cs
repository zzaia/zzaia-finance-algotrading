using Zzaia.Finance.Core.Models.EnumerationAggregate;
using Zzaia.Finance.Core.Models.MarketAgregate;
using System;
using System.Collections.Generic;

namespace Zzaia.Finance.Connector
{
    public class WebSocketConnectorOptions
    {
        /// <summary>
        /// Exchange or vendors reference name;
        /// </summary>
        public ExchangeName ExchangeName { get; set; }

        /// <summary>
        /// The collection of data to be used as argument;
        /// </summary>
        public IEnumerable<Market> DataIn { get; set; }

        /// <summary>
        /// The collection of types of data to publish;
        /// </summary>
        public IEnumerable<Type> DataOut { get; set; }

        /// <summary>
        /// This is the time tolerance multiplier, the bigger the more tolerant will be the connector;
    }
}
