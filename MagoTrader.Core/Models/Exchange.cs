using System;

namespace MagoTrader.Core.Models
{
    public class Exchange
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Ticker> AllTickers { get; set; }

    }
}