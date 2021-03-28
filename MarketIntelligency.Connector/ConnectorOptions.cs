using MarketIntelligency.Core.Models.EnumerationAggregate;
using System;
using System.Collections.Generic;

namespace MarketIntelligency.Connector
{
    public class ConnectorOptions
    {
        /// <summary>
        /// Exchange or vendors reference name;
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// This is the period between requests;
        /// </summary>
        public TimeFrame TimeFrame { get; set; }

        /// <summary>
        /// The collection of data to be used as argument;
        /// </summary>
        public IEnumerable<dynamic> DataIn { get; set; }

        /// <summary>
        /// The collection of types of data to publish;
        /// </summary>
        public IEnumerable<Type> DataOut { get; set; }

        /// <summary>
        /// This is the time tolerance multiplier, the bigger the more tolerant will be the connector;
        /// </summary>
        public int Tolerance { get; set; } = 2;

        /// <summary>
        /// This is the time resolution, the bigger the more precise will be the connector;
        /// </summary>
        public int Resolution { get; set; } = 2000;
    }
}