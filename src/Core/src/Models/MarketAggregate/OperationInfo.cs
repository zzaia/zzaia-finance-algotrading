using Zzaia.Finance.Core.Models.EnumerationAggregate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Zzaia.Finance.Core.Models.MarketAgregate
{
    public class OperationInfo
    {
        public OperationInfo() { }
        public OperationInfo(string operationType, Asset asset,
                                                   decimal minAmount = 10,
                                                   decimal maxAmount = 10000,
                                                   string profile = "")
        {

            Type = Types.IsValid(operationType) ? operationType : throw new ArgumentException(nameof(operationType));
            Asset = asset ?? throw new ArgumentException(nameof(asset));
            AmountMin = minAmount;
            AmountMax = maxAmount;
            ProfileLevel = profile;
            Fees = new List<Fee>();
        }
        public OperationInfo(string operationType, decimal minAmount = 10,
                                                   decimal maxAmount = 10000,
                                                   string profile = "")
        {

            Type = Types.IsValid(operationType) ? operationType : throw new ArgumentException(nameof(operationType));
            AmountMin = minAmount;
            AmountMax = maxAmount;
            ProfileLevel = profile;
            Fees = new List<Fee>();
        }

        public string Type { get; set; }
        public Asset Asset { get; set; }
        public string ProfileLevel { get; set; }
        public decimal AmountMax { get; set; }
        public decimal AmountMin { get; set; }
        public IList<Fee> Fees { get; set; }
        public class Types
        {
            public const string Deposit = "deposit";
            public const string Withdrawal = "withdrawal";
            public const string Maker = "maker";
            public const string Taker = "taker";
            public static bool IsValid(string type)
            {
                return Deposit.Equals(type)
                    || Withdrawal.Equals(type)
                    || Taker.Equals(type)
                    || Maker.Equals(type);
            }
        }
        public bool HasFees()
        {
            var notNull = Fees != null;
            var notEmpty = Fees.Count > 0;
            return notNull && notEmpty;
        }
        public bool NotHasFees()
        {
            return !HasFees();
        }
        public bool HasNetworkFee()
        {
            if (HasFees())
            {
                var hasTokenFee = Fees.Any(each => each.Type == Fee.Types.Network);
                return hasTokenFee;
            }
            return false;
        }
        public bool NotHasNetworkFee()
        {
            return !HasNetworkFee();
        }
        /// <summary>
        /// Add Fee by properties to collection of fees.
        /// </summary>
        /// <param name="flat">The flat fee property to add.</param>
        /// <param name="percentage">The percentage fee property to add.</param>
        /// <param name="type">The fee type property to add.</param>
        /// <returns>The operation information</returns>
        public OperationInfo AddFee(decimal flat, decimal percentage, string type = Fee.Types.Exchange)
        {
            this.Fees.Add(new Fee(flat, percentage, type));
            return this;
        }
        /// <summary>
        /// Add Fee object to fees collection.
        /// </summary>
        /// <param name="fee">The fee to add.</param>
        /// <returns>The operation information</returns>
        public OperationInfo AddFee(Fee fee)
        {
            this.Fees.Add(fee);
            return this;
        }
        /// <summary>
        /// Compute Fee based on flat and percentage fee values.
        /// </summary>
        /// <param name="amountGross">The amount to calculate the fee.</param>
        /// <returns>The calculated amount of fee from the input amount.</returns>
        public decimal ComputeFee(decimal amountGross)
        {
            var feeCalculated = (amountGross * Fees.Select(a => a.Percentage).Sum()) + Fees.Select(a => a.Flat).Sum();
            return RoundBasedOnCurrency(feeCalculated);
        }
        /// <summary>
        /// Compute gross amount based on a net amount value.
        /// </summary>
        /// <param name="amountNet">The net amount to calculate the gross amount.</param>
        /// <returns>The calculated gross amount</returns>
        public decimal ComputeAmountGross(decimal amountNet)
        {
            var computedAmountGross = (Fees.Select(a => a.Flat).Sum() + amountNet) / (1 - Fees.Select(a => a.Percentage).Sum());
            return RoundBasedOnCurrency(computedAmountGross);
        }
        /// <summary>
        /// Compute net amount based on a gross amount value.
        /// </summary>
        /// <param name="amountGross">The gross amount to calculate the net amount.</param>
        /// <returns>The calculated net amount</returns>
        public decimal ComputeAmountNet(decimal amountGross)
        {
            return amountGross - ComputeFee(amountGross);
        }
        /// <summary>
        /// Inclusive inequality assessment.
        /// </summary>
        /// <param name="amountGross">The amount to assess.</param>
        /// <returns>True if the gross amount is in bound between minimum and maximum amount inclusively. False otherwise.</returns>
        public bool IsInBound(decimal amountGross)
        {
            return amountGross >= AmountMin && amountGross <= AmountMax;
        }
        public bool IsOutBound(decimal amountGross)
        {
            return !IsInBound(amountGross);
        }
        /// <summary>
        /// Inclusive inequality assessment.
        /// </summary>
        /// <param name="amountGross">The amount to assess.</param>
        /// <returns>True if the gross amount is under minimum amount inclusively. False otherwise.</returns>
        public bool IsUnderMin(decimal amountGross)
        {
            return amountGross < AmountMin;
        }
        /// <summary>
        /// Inclusive inequality assessment.
        /// </summary>
        /// <param name="amountGross">The amount to assess.</param>
        /// <returns>True if the gross amount is over maximum amount inclusively. False otherwise.</returns>
        public bool IsOverMax(decimal amountGross)
        {
            return amountGross > AmountMax;
        }
        private decimal RoundBasedOnCurrency(decimal value)
        {
            if (Asset.BRZ.Equals(Asset))
            {
                return Math.Round(value, 4, MidpointRounding.ToEven);
            }
            else
            {
                return Math.Round(value, 2, MidpointRounding.ToEven);
            }
        }
    }
}
