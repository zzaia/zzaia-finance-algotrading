using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MagoTrader.Core.Models
{
    public class Exchange
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public List<Ticker> Tickers { get; set; }
        public Exchange()
        {
            Tickers = new List<Ticker>();
        }
    }
}