// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Economy
{
    /// <summary>Side-effect-free command availability returned to the host/UI.</summary>
    public readonly struct BlackMarketActionPreview
    {
        public readonly bool IsAvailable;
        public readonly string ActionId;
        public readonly string SyndicateId;
        public readonly string EntryId;
        public readonly string DebtId;
        public readonly string ItemId;
        public readonly int Quantity;
        public readonly long SettlementUnits;
        public readonly int DueDay;
        public readonly string ReasonId;
        public readonly string Message;

        public BlackMarketActionPreview(bool isAvailable, string actionId, string syndicateId,
            string entryId, string debtId, string itemId, int quantity, long settlementUnits,
            int dueDay, string reasonId, string message)
        {
            IsAvailable = isAvailable;
            ActionId = actionId ?? string.Empty;
            SyndicateId = syndicateId ?? string.Empty;
            EntryId = entryId ?? string.Empty;
            DebtId = debtId ?? string.Empty;
            ItemId = itemId ?? string.Empty;
            Quantity = quantity;
            SettlementUnits = settlementUnits;
            DueDay = dueDay;
            ReasonId = reasonId ?? string.Empty;
            Message = message ?? string.Empty;
        }
    }

    /// <summary>Authoritative result of one composed black-market command.</summary>
    public readonly struct BlackMarketActionResult
    {
        public readonly bool Success;
        public readonly string ActionId;
        public readonly string SyndicateId;
        public readonly string EntryId;
        public readonly string DebtId;
        public readonly string ItemId;
        public readonly int Quantity;
        public readonly long WalletDelta;
        public readonly int InventoryDelta;
        public readonly long SettlementUnits;
        public readonly int DueDay;
        public readonly string ReasonId;
        public readonly string Message;

        public BlackMarketActionResult(bool success, string actionId, string syndicateId,
            string entryId, string debtId, string itemId, int quantity, long walletDelta,
            int inventoryDelta, long settlementUnits, int dueDay, string reasonId, string message)
        {
            Success = success;
            ActionId = actionId ?? string.Empty;
            SyndicateId = syndicateId ?? string.Empty;
            EntryId = entryId ?? string.Empty;
            DebtId = debtId ?? string.Empty;
            ItemId = itemId ?? string.Empty;
            Quantity = quantity;
            WalletDelta = walletDelta;
            InventoryDelta = inventoryDelta;
            SettlementUnits = settlementUnits;
            DueDay = dueDay;
            ReasonId = reasonId ?? string.Empty;
            Message = message ?? string.Empty;
        }
    }

    /// <summary>
    /// Stateless transaction coordinator for Plan 211. Policy remains in
    /// <see cref="BlackMarketSystem"/>; funds remain in
    /// <see cref="HoldfastTradeSession"/>; goods remain in
    /// <see cref="Inventory.Inventory"/>. This type only composes their
    /// existing mutation boundaries into one rollback-safe command.
    /// </summary>
    public sealed class BlackMarketSettlementService
    {
        public const string BuyAction = "buy";
        public const string SellAction = "sell";
        public const string LoanAction = "take_loan";
        public const string RepayAction = "repay";

        private readonly BlackMarketSystem _blackMarket;
        private readonly HoldfastTradeSession _wallet;
        private readonly Inventory.Inventory _inventory;
        private readonly ItemCatalog _items;

        public BlackMarketSettlementService(BlackMarketSystem blackMarket,
            HoldfastTradeSession wallet, Inventory.Inventory inventory, ItemCatalog items)
        {
            _blackMarket = blackMarket ?? throw new ArgumentNullException(nameof(blackMarket));
            _wallet = wallet ?? throw new ArgumentNullException(nameof(wallet));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public long WalletValue => _wallet.Value;

        public int InventoryCount(string itemId) =>
            string.IsNullOrEmpty(itemId) ? 0 : _inventory.CountById(itemId);

        public BlackMarketActionPreview PreviewBuy(string syndicateId, string entryId, int quantity, int day)
        {
            var quote = _blackMarket.PreviewBuy(syndicateId, entryId, quantity, day);
            if (!quote.Valid) return Unavailable(BuyAction, syndicateId, entryId, string.Empty,
                string.Empty, quantity, quote.RejectReason);

            var entry = _blackMarket.Catalog.FindEntry(entryId);
            if (entry == null) return Unavailable(BuyAction, syndicateId, entryId, string.Empty,
                string.Empty, quantity, "unknown_entry");
            var definition = _items.Get(entry.item_id);
            if (definition == null) return Unavailable(BuyAction, syndicateId, entryId, string.Empty,
                entry.item_id, quantity, "item_not_in_inventory_catalog");
            if (!TryRoundBuy(quote.TotalValue, out long cost))
                return Unavailable(BuyAction, syndicateId, entryId, string.Empty,
                    entry.item_id, quantity, "invalid_settlement_value");
            if (!_wallet.CanDebitValue(cost))
                return Unavailable(BuyAction, syndicateId, entryId, string.Empty,
                    entry.item_id, quantity, "insufficient_funds");

            var validation = _inventory.ValidateTransaction(
                new InventoryBill().AddGrant(definition, quantity));
            if (!validation.IsValid)
                return Unavailable(BuyAction, syndicateId, entryId, string.Empty,
                    entry.item_id, quantity, InventoryReason(validation));

            return Available(BuyAction, syndicateId, entryId, string.Empty, entry.item_id,
                quantity, cost, 0, $"Buy {quantity} for {cost} value.");
        }

        public BlackMarketActionResult Buy(string syndicateId, string entryId, int quantity, int day)
        {
            var preview = PreviewBuy(syndicateId, entryId, quantity, day);
            if (!preview.IsAvailable) return Failed(preview);
            var definition = _items.Get(preview.ItemId)!;
            var bill = new InventoryBill().AddGrant(definition, quantity);

            var quote = _blackMarket.Buy(syndicateId, entryId, quantity, day, q =>
            {
                if (!TryRoundBuy(q.TotalValue, out long cost) || cost != preview.SettlementUnits)
                    return false;
                return _wallet.TryDebitValueForSettlement(cost,
                    () => _inventory.TryExecuteTransaction(bill));
            });
            if (!quote.Valid)
                return Failed(BuyAction, syndicateId, entryId, string.Empty, preview.ItemId,
                    quantity, quote.RejectReason);

            _wallet.NotifyExternalValueSettlement();
            return Succeeded(BuyAction, syndicateId, entryId, string.Empty, preview.ItemId,
                quantity, -preview.SettlementUnits, quantity, preview.SettlementUnits, 0,
                $"Bought {quantity} × {preview.ItemId} for {preview.SettlementUnits} value.");
        }

        public BlackMarketActionPreview PreviewSell(string syndicateId, string entryId, int quantity, int day)
        {
            var quote = _blackMarket.PreviewSell(syndicateId, entryId, quantity, day);
            if (!quote.Valid) return Unavailable(SellAction, syndicateId, entryId, string.Empty,
                string.Empty, quantity, quote.RejectReason);

            var entry = _blackMarket.Catalog.FindEntry(entryId);
            if (entry == null) return Unavailable(SellAction, syndicateId, entryId, string.Empty,
                string.Empty, quantity, "unknown_entry");
            var definition = _items.Get(entry.item_id);
            if (definition == null) return Unavailable(SellAction, syndicateId, entryId, string.Empty,
                entry.item_id, quantity, "item_not_in_inventory_catalog");
            if (!TryRoundSell(quote.TotalValue, out long proceeds))
                return Unavailable(SellAction, syndicateId, entryId, string.Empty,
                    entry.item_id, quantity, "invalid_settlement_value");
            if (!_wallet.CanCreditValue(proceeds))
                return Unavailable(SellAction, syndicateId, entryId, string.Empty,
                    entry.item_id, quantity, "wallet_capacity");

            var validation = _inventory.ValidateTransaction(
                new InventoryBill().AddCost(definition, quantity));
            if (!validation.IsValid)
                return Unavailable(SellAction, syndicateId, entryId, string.Empty,
                    entry.item_id, quantity, InventoryReason(validation));

            return Available(SellAction, syndicateId, entryId, string.Empty, entry.item_id,
                quantity, proceeds, 0, $"Sell {quantity} for {proceeds} value.");
        }

        public BlackMarketActionResult Sell(string syndicateId, string entryId, int quantity, int day)
        {
            var preview = PreviewSell(syndicateId, entryId, quantity, day);
            if (!preview.IsAvailable) return Failed(preview);
            var definition = _items.Get(preview.ItemId)!;
            var bill = new InventoryBill().AddCost(definition, quantity);

            var quote = _blackMarket.Sell(syndicateId, entryId, quantity, day, q =>
            {
                if (!TryRoundSell(q.TotalValue, out long proceeds) || proceeds != preview.SettlementUnits)
                    return false;
                return _wallet.TryCreditValueForSettlement(proceeds,
                    () => _inventory.TryExecuteTransaction(bill));
            });
            if (!quote.Valid)
                return Failed(SellAction, syndicateId, entryId, string.Empty, preview.ItemId,
                    quantity, quote.RejectReason);

            _wallet.NotifyExternalValueSettlement();
            return Succeeded(SellAction, syndicateId, entryId, string.Empty, preview.ItemId,
                quantity, preview.SettlementUnits, -quantity, preview.SettlementUnits, 0,
                $"Sold {quantity} × {preview.ItemId} for {preview.SettlementUnits} value.");
        }

        public BlackMarketActionPreview PreviewLoan(string syndicateId, long units, int day, int durationDays)
        {
            if (units <= 0 || units > int.MaxValue)
                return Unavailable(LoanAction, syndicateId, string.Empty, string.Empty,
                    string.Empty, 0, "amount_must_be_positive");
            var quote = _blackMarket.PreviewLoan(syndicateId, units, day, durationDays);
            if (!quote.Valid) return Unavailable(LoanAction, syndicateId, string.Empty, string.Empty,
                string.Empty, 0, quote.RejectReason);
            if (!_wallet.CanCreditValue(units))
                return Unavailable(LoanAction, syndicateId, string.Empty, string.Empty,
                    string.Empty, 0, "wallet_capacity");
            return Available(LoanAction, syndicateId, string.Empty, string.Empty, string.Empty,
                0, units, quote.DueDay, $"Borrow {units} value; due day {quote.DueDay}.");
        }

        public BlackMarketActionResult TakeLoan(string syndicateId, long units, int day, int durationDays)
        {
            var preview = PreviewLoan(syndicateId, units, day, durationDays);
            if (!preview.IsAvailable) return Failed(preview);
            var debt = _blackMarket.TakeLoan(syndicateId, units, day, durationDays,
                _ => _wallet.TryCreditValueForSettlement(units));
            if (debt == null)
                return Failed(LoanAction, syndicateId, string.Empty, string.Empty, string.Empty,
                    0, "settlement_failed");

            _wallet.NotifyExternalValueSettlement();
            return Succeeded(LoanAction, syndicateId, string.Empty, debt.debtId, string.Empty,
                0, units, 0, units, debt.dueDay,
                $"Borrowed {units} value from {syndicateId}; due day {debt.dueDay}.");
        }

        public BlackMarketActionPreview PreviewRepay(string debtId, long units, int day)
        {
            if (units <= 0 || units > int.MaxValue)
                return Unavailable(RepayAction, string.Empty, string.Empty, debtId,
                    string.Empty, 0, "amount_must_be_positive");
            var quote = _blackMarket.PreviewRepay(debtId, units, day);
            if (!quote.Valid) return Unavailable(RepayAction, string.Empty, string.Empty, debtId,
                string.Empty, 0, quote.RejectReason);
            if (!TryRoundRepay(quote.AppliedUnits, out long applied))
                return Unavailable(RepayAction, string.Empty, string.Empty, debtId,
                    string.Empty, 0, "invalid_settlement_value");
            if (!_wallet.CanDebitValue(applied))
                return Unavailable(RepayAction, string.Empty, string.Empty, debtId,
                    string.Empty, 0, "insufficient_funds");
            return Available(RepayAction, string.Empty, string.Empty, debtId, string.Empty,
                0, applied, 0, $"Repay {applied} value.");
        }

        public BlackMarketActionResult Repay(string debtId, long units, int day)
        {
            var preview = PreviewRepay(debtId, units, day);
            if (!preview.IsAvailable) return Failed(preview);
            bool repaid = _blackMarket.RepayDebt(debtId, units, day, q =>
            {
                if (!TryRoundRepay(q.AppliedUnits, out long applied) || applied != preview.SettlementUnits)
                    return false;
                return _wallet.TryDebitValueForSettlement(applied);
            });
            if (!repaid)
                return Failed(RepayAction, string.Empty, string.Empty, debtId, string.Empty,
                    0, "settlement_failed");

            _wallet.NotifyExternalValueSettlement();
            return Succeeded(RepayAction, string.Empty, string.Empty, debtId, string.Empty,
                0, -preview.SettlementUnits, 0, preview.SettlementUnits, 0,
                $"Repaid {preview.SettlementUnits} value.");
        }

        private static bool TryRoundBuy(float value, out long units) =>
            TryRound(value, roundUp: true, out units);

        private static bool TryRoundSell(float value, out long units) =>
            TryRound(value, roundUp: false, out units);

        private static bool TryRoundRepay(float value, out long units) =>
            TryRound(value, roundUp: true, out units);

        private static bool TryRound(float value, bool roundUp, out long units)
        {
            units = 0;
            if (float.IsNaN(value) || float.IsInfinity(value) || value <= 0f) return false;
            double rounded = roundUp ? Math.Ceiling(value) : Math.Floor(value);
            if (rounded <= 0d || rounded > long.MaxValue) return false;
            units = (long)rounded;
            return true;
        }

        private static string InventoryReason(InventoryTransactionValidationResult validation)
        {
            return validation.Status switch
            {
                InventoryTransactionStatus.InsufficientQuantity => "insufficient_inventory",
                InventoryTransactionStatus.ExceedsCapacity => "inventory_capacity",
                InventoryTransactionStatus.ExceedsWeight => "inventory_weight",
                _ => "inventory_rejected"
            };
        }

        public static string ReasonText(string reasonId)
        {
            return reasonId switch
            {
                "unknown_entry" => "That stock entry no longer exists.",
                "quantity_must_be_positive" => "Choose a quantity of at least one.",
                "amount_must_be_positive" => "Choose an amount of at least one.",
                "contact_not_discovered" => "This underworld contact is not available.",
                "access_tier_too_low" => "Your access tier is too low for this stock.",
                "out_of_stock" => "That item is out of stock.",
                "insufficient_stock" => "The syndicate does not have that quantity.",
                "unpriced" => "The canonical market has no price for this item.",
                "unpriced_entry" => "The canonical market has no price for this item.",
                "item_not_in_inventory_catalog" => "This item cannot enter Holdfast storage.",
                "invalid_settlement_value" => "The quoted value cannot be settled.",
                "insufficient_funds" => "Holdfast does not have enough value.",
                "inventory_capacity" => "Holdfast storage has no free slot for this purchase.",
                "inventory_weight" => "This purchase would exceed the storage weight limit.",
                "insufficient_inventory" => "Holdfast storage does not contain that quantity.",
                "inventory_rejected" => "Holdfast storage rejected the goods transfer.",
                "wallet_capacity" => "The wallet cannot accept that value safely.",
                "unknown_syndicate" => "That syndicate no longer exists.",
                "credit_limit_exceeded" => "The requested loan exceeds this syndicate's credit limit.",
                "active_loan_exists" => "This syndicate already has an active loan on the ledger.",
                "unknown_debt" => "That debt is not on the ledger.",
                "debt_not_active" => "That debt is no longer active.",
                "settlement_failed" => "The transaction could not be committed; nothing changed.",
                _ => string.IsNullOrEmpty(reasonId) ? "Action unavailable." : reasonId.Replace('_', ' ')
            };
        }

        private static BlackMarketActionPreview Available(string actionId, string syndicateId,
            string entryId, string debtId, string itemId, int quantity, long units, int dueDay,
            string message) =>
            new BlackMarketActionPreview(true, actionId, syndicateId, entryId, debtId, itemId,
                quantity, units, dueDay, string.Empty, message);

        private static BlackMarketActionPreview Unavailable(string actionId, string syndicateId,
            string entryId, string debtId, string itemId, int quantity, string reasonId) =>
            new BlackMarketActionPreview(false, actionId, syndicateId, entryId, debtId, itemId,
                quantity, 0, 0, reasonId, ReasonText(reasonId));

        private static BlackMarketActionResult Failed(BlackMarketActionPreview preview) =>
            Failed(preview.ActionId, preview.SyndicateId, preview.EntryId, preview.DebtId,
                preview.ItemId, preview.Quantity, preview.ReasonId);

        private static BlackMarketActionResult Failed(string actionId, string syndicateId,
            string entryId, string debtId, string itemId, int quantity, string reasonId) =>
            new BlackMarketActionResult(false, actionId, syndicateId, entryId, debtId, itemId,
                quantity, 0, 0, 0, 0, reasonId, ReasonText(reasonId));

        private static BlackMarketActionResult Succeeded(string actionId, string syndicateId,
            string entryId, string debtId, string itemId, int quantity, long walletDelta,
            int inventoryDelta, long settlementUnits, int dueDay, string message) =>
            new BlackMarketActionResult(true, actionId, syndicateId, entryId, debtId, itemId,
                quantity, walletDelta, inventoryDelta, settlementUnits, dueDay, string.Empty, message);
    }
}
