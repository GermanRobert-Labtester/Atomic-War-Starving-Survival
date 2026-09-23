// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Economy
{
    public enum ContrabandClassification
    {
        Unregulated = 1,
        Restricted = 2,
        BannedWeaponry = 3,
        Narcotics = 4,
        MilitaryHardware = 5
    }

    public enum BlackMarketActionType
    {
        Buy = 1,
        Sell = 2,
        Fence = 3,
        ContractEscrow = 4,
        ContractPayout = 5
    }

    public sealed class BlackMarketTradeQuote
    {
        public bool IsViable { get; }
        public BlackMarketActionType Action { get; }
        public int Quantity { get; }
        public int UnitPriceChits { get; }
        public int GrossValueChits { get; }
        public int FencingFeeChits { get; }
        public int NetFundsDelta { get; }
        public int GeneratedHeatPermille { get; }
        public int ProjectedHeatPermille { get; }
        public string ReasonKey { get; }
        public string RejectionReason { get; }
        public string Summary { get; }

        public BlackMarketTradeQuote(
            bool isViable,
            BlackMarketActionType action,
            int quantity,
            int unitPriceChits,
            int grossValueChits,
            int fencingFeeChits,
            int netFundsDelta,
            int generatedHeatPermille,
            int projectedHeatPermille,
            string reasonKey,
            string rejectionReason,
            string summary)
        {
            IsViable = isViable;
            Action = action;
            Quantity = Math.Max(0, quantity);
            UnitPriceChits = Math.Max(0, unitPriceChits);
            GrossValueChits = Math.Max(0, grossValueChits);
            FencingFeeChits = Math.Max(0, fencingFeeChits);
            NetFundsDelta = netFundsDelta;
            GeneratedHeatPermille = Math.Max(0, generatedHeatPermille);
            ProjectedHeatPermille = Math.Clamp(projectedHeatPermille, 0, 1000);
            ReasonKey = reasonKey ?? string.Empty;
            RejectionReason = rejectionReason ?? string.Empty;
            Summary = summary ?? string.Empty;
        }

        public static BlackMarketTradeQuote Rejected(BlackMarketActionType action, string rejectionReason)
        {
            return new BlackMarketTradeQuote(
                isViable: false,
                action: action,
                quantity: 0,
                unitPriceChits: 0,
                grossValueChits: 0,
                fencingFeeChits: 0,
                netFundsDelta: 0,
                generatedHeatPermille: 0,
                projectedHeatPermille: 0,
                reasonKey: string.Empty,
                rejectionReason: rejectionReason ?? "Transaction rejected.",
                summary: "Transaction rejected."
            );
        }
    }

    /// <summary>
    /// XP-04-F2 / UNBLOCK-02 §3.1 / §5.4 / §18.2: Black Market Contraband Valuation & Fence Engine.
    /// Manages black market buy/sell/fence quotes, contraband classification multipliers,
    /// trust-scaled fencing fees (20-35%), heat accumulation, and raid threshold gating.
    /// Connects to canonical FundsLedger with closed reason keys.
    /// Engine-free, integer/permille determinism.
    /// </summary>
    public static class BlackMarketContrabandEngine
    {
        public const int MaxHeatPermille = 1000;
        public const int RaidLockoutHeatPermille = 800;
        public const int BaseFencingFeeMaxPermille = 350; // 35% fee for 0 trust
        public const int MinFencingFeePermille = 200;     // 20% fee for 1000 trust

        public static int CalculateFencingFeePermille(int fenceTrustPermille)
        {
            fenceTrustPermille = Math.Clamp(fenceTrustPermille, 0, 1000);
            int discount = (fenceTrustPermille * 150) / 1000;
            return BaseFencingFeeMaxPermille - discount;
        }

        public static (int multiplierPermille, int baseHeatPermille) GetClassificationParameters(ContrabandClassification classification)
        {
            return classification switch
            {
                ContrabandClassification.MilitaryHardware => (2000, 100),
                ContrabandClassification.Narcotics => (1600, 75),
                ContrabandClassification.BannedWeaponry => (1500, 60),
                ContrabandClassification.Restricted => (1200, 30),
                ContrabandClassification.Unregulated => (1000, 10),
                _ => (1000, 10)
            };
        }

        public static BlackMarketTradeQuote QuoteBuy(
            string itemId,
            int quantity,
            int baseUnitValueChits,
            ContrabandClassification classification,
            int currentHeatPermille)
        {
            if (quantity <= 0 || baseUnitValueChits <= 0)
                return BlackMarketTradeQuote.Rejected(BlackMarketActionType.Buy, "Invalid quantity or unit value.");

            if (currentHeatPermille >= RaidLockoutHeatPermille)
                return BlackMarketTradeQuote.Rejected(BlackMarketActionType.Buy, "Underground market locked down; enforcer raids imminent.");

            var (mult, baseHeat) = GetClassificationParameters(classification);
            int unitPrice = Math.Max(1, (baseUnitValueChits * mult) / 1000);
            int grossValue = unitPrice * quantity;
            int generatedHeat = Math.Min(300, (baseHeat * quantity) / 2);
            int projectedHeat = Math.Min(MaxHeatPermille, currentHeatPermille + generatedHeat);

            return new BlackMarketTradeQuote(
                isViable: true,
                action: BlackMarketActionType.Buy,
                quantity: quantity,
                unitPriceChits: unitPrice,
                grossValueChits: grossValue,
                fencingFeeChits: 0,
                netFundsDelta: -grossValue, // Debit from player
                generatedHeatPermille: generatedHeat,
                projectedHeatPermille: projectedHeat,
                reasonKey: FundsLedger.ReasonBmBuy,
                rejectionReason: string.Empty,
                summary: $"Buy {quantity}x {itemId} for {grossValue} chits (Heat +{generatedHeat / 10}%)."
            );
        }

        public static BlackMarketTradeQuote QuoteSell(
            string itemId,
            int quantity,
            int baseUnitValueChits,
            ContrabandClassification classification,
            int currentHeatPermille)
        {
            if (quantity <= 0 || baseUnitValueChits <= 0)
                return BlackMarketTradeQuote.Rejected(BlackMarketActionType.Sell, "Invalid quantity or unit value.");

            if (currentHeatPermille >= RaidLockoutHeatPermille)
                return BlackMarketTradeQuote.Rejected(BlackMarketActionType.Sell, "Underground market locked down; enforcer raids imminent.");

            var (mult, baseHeat) = GetClassificationParameters(classification);
            // Wholesale merchant buys at 75% of classified value
            int unitPrice = Math.Max(1, (baseUnitValueChits * mult * 750) / 1000000);
            int grossValue = unitPrice * quantity;
            int generatedHeat = Math.Min(300, (baseHeat * quantity) / 2);
            int projectedHeat = Math.Min(MaxHeatPermille, currentHeatPermille + generatedHeat);

            return new BlackMarketTradeQuote(
                isViable: true,
                action: BlackMarketActionType.Sell,
                quantity: quantity,
                unitPriceChits: unitPrice,
                grossValueChits: grossValue,
                fencingFeeChits: 0,
                netFundsDelta: grossValue, // Credit to player
                generatedHeatPermille: generatedHeat,
                projectedHeatPermille: projectedHeat,
                reasonKey: FundsLedger.ReasonBmSell,
                rejectionReason: string.Empty,
                summary: $"Sell {quantity}x {itemId} for {grossValue} chits (Heat +{generatedHeat / 10}%)."
            );
        }

        public static BlackMarketTradeQuote QuoteFence(
            string itemId,
            int quantity,
            int baseUnitValueChits,
            ContrabandClassification classification,
            int currentHeatPermille,
            int fenceTrustPermille)
        {
            if (quantity <= 0 || baseUnitValueChits <= 0)
                return BlackMarketTradeQuote.Rejected(BlackMarketActionType.Fence, "Invalid quantity or unit value.");

            if (currentHeatPermille >= RaidLockoutHeatPermille)
                return BlackMarketTradeQuote.Rejected(BlackMarketActionType.Fence, "Underground market locked down; enforcer raids imminent.");

            var (mult, baseHeat) = GetClassificationParameters(classification);
            int unitPrice = Math.Max(1, (baseUnitValueChits * mult) / 1000);
            int grossValue = unitPrice * quantity;

            int feePermille = CalculateFencingFeePermille(fenceTrustPermille);
            int fencingFee = Math.Max(1, (grossValue * feePermille) / 1000);
            int netPayout = Math.Max(0, grossValue - fencingFee);

            // Fencing illicit/flagged goods causes higher heat
            int generatedHeat = Math.Min(400, (baseHeat * quantity * 1500) / 1000);
            int projectedHeat = Math.Min(MaxHeatPermille, currentHeatPermille + generatedHeat);

            return new BlackMarketTradeQuote(
                isViable: true,
                action: BlackMarketActionType.Fence,
                quantity: quantity,
                unitPriceChits: unitPrice,
                grossValueChits: grossValue,
                fencingFeeChits: fencingFee,
                netFundsDelta: netPayout, // Net payout to player
                generatedHeatPermille: generatedHeat,
                projectedHeatPermille: projectedHeat,
                reasonKey: FundsLedger.ReasonBmFence,
                rejectionReason: string.Empty,
                summary: $"Fence {quantity}x {itemId}: gross {grossValue} chits, fee {fencingFee} chits ({feePermille / 10}%), net {netPayout} chits."
            );
        }

        public static FundsResult ExecuteTransaction(
            FundsLedger ledger,
            BlackMarketTradeQuote quote,
            string counterpartyId,
            int day,
            ref int currentHeatPermille)
        {
            if (ledger == null)
                throw new ArgumentNullException(nameof(ledger));
            if (quote == null || !quote.IsViable)
                throw new ArgumentException("Cannot execute an invalid or rejected trade quote.", nameof(quote));

            FundsResult result;
            if (quote.NetFundsDelta < 0)
            {
                // Debit from player
                int debitAmount = Math.Abs(quote.NetFundsDelta);
                result = ledger.TryDebit(debitAmount, quote.ReasonKey, counterpartyId, day);
            }
            else
            {
                // Credit to player
                result = ledger.TryCredit(quote.NetFundsDelta, quote.ReasonKey, counterpartyId, day);
            }

            if (result.Success)
            {
                currentHeatPermille = Math.Clamp(currentHeatPermille + quote.GeneratedHeatPermille, 0, MaxHeatPermille);
            }

            return result;
        }
    }
}
