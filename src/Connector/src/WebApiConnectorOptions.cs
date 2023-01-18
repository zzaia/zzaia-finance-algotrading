using Zzaia.Finance.Core.Models.EnumerationAggregate;
using System;
using System.Collections.Generic;

namespace Zzaia.Finance.Connector
{
    public class WebApiConnectorOptions
    {
        /// <summary>
        /// Exchange or vendors reference name;
        /// </summary>
        public ExchangeName ExchangeName { get; set; }

        /// <summary>
        /// This is the period between requests;
        /// </summary>
        public TimeFrame TimeFrame { get; set; }

        /// <summary>
        /// The dictionary of data to be used as argument;
        /// </summary>
        public Dictionary<dynamic, dynamic> DataProfile { get; set; }

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

        /// <summary>
        ///     Gets or sets the maximum number of concurrent tasks enabled by this System.Threading.Tasks.ParallelOptions
        ///     instance, default is the maximum int value.
        /// </summary>
        public int MaxDegreeOfParallelism { get; set; } = int.MaxValue;
    }
}