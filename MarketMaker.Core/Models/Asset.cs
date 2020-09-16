using MarketMaker.Core.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarketMaker.Core.Models
{
    public class Asset
    {
        public AssetTickerEnum Ticker { get; set; }
        public string Name { get; set; }
        public WithdrawAndDeposit Withdraw { get; set; }
        public WithdrawAndDeposit Deposit { get; set; }
        public Asset(string name, AssetTickerEnum ticker, 
                                  decimal depositMinimumLimit,
                                  decimal depositMaximumLimit, 
                                  decimal depositFee, 
                                  decimal withdrawMinimumLimit,
                                  decimal withdrawMaximumLimit, 
                                  decimal withdrawFee)
        {
            Name = name;
            Ticker = ticker;
            Withdraw = new WithdrawAndDeposit
            {
                MaximumLimit = depositMaximumLimit,
                MinimumLimit = depositMinimumLimit,
                Fee = withdrawFee 
            };
            Deposit = new WithdrawAndDeposit
            {
                MaximumLimit = withdrawMaximumLimit,
                MinimumLimit = withdrawMinimumLimit,
                Fee = depositFee 
            };
        }
    }
}
