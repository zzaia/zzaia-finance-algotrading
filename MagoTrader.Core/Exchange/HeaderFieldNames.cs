using System;
using System.Collections.Generic;
using System.Text;

namespace MarketMaker.Core.Exchange
{
    /// <summary>
    /// Presents a collection of header names specified in the RFC 2616,
    /// section 14. Header Field Definitions.
    /// Checkout at <see cref="https://tools.ietf.org/html/rfc2616#section-14"/>.
    /// </summary>
    public class HeaderFieldNames
    {
        public const string Accept = "Accept";
        public const string Authorization = "Authorization";
        public const string UserAgent = "User-Agent";
    }
}
