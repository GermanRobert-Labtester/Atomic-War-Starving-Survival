using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// The Quartermaster's Line: in-universe dispatch log triggered by verified
    /// runtime events. Every entry references true state (quantities, values,
    /// faction status). Flavor is an overlay; it never touches save files,
    /// transaction logic, or domain assertions.
    /// </summary>
    public sealed class HoldfastDispatchLog
    {
        private const int MaxEntries = 64;

        private readonly HoldfastFlavorCatalog _flavor;
        private readonly List<string> _entries = new List<string>();
        private string _lastDispatch = string.Empty;

        public IReadOnlyList<string> Entries => _entries;
        public string LastDispatch => _lastDispatch;

        public HoldfastDispatchLog(HoldfastFlavorCatalog flavor)
        {
            _flavor = flavor ?? HoldfastFlavorCatalog.Load(string.Empty);
        }

        public void OnSessionOpened(string sessionType)
        {
            Emit("Ledger reopened (" + sessionType + "). The quartermaster's desk is ready.");
        }

        public void OnFirstPurchase(string itemId, int quantity, long totalValue, string factionId)
        {
            var voice = _flavor.GetFactionVoice(factionId);
            Emit("First requisition this session: " + quantity + " × " + ItemRef(itemId) +
                 " for " + totalValue + ". " + voice.voice);
        }

        public void OnPurchase(string itemId, int quantity, long totalValue, string factionId)
        {
            var voice = _flavor.GetFactionVoice(factionId);
            Emit(quantity + " × " + ItemRef(itemId) + " released. " + totalValue + " deducted. " + voice.voice);
        }

        public void OnSale(string itemId, int quantity, long totalValue, string factionId)
        {
            var voice = _flavor.GetFactionVoice(factionId);
            Emit(quantity + " × " + ItemRef(itemId) + " accepted. " + totalValue + " credited. " + voice.sold);
        }

        public void OnHoldingEmptied(string itemId, string factionId)
        {
            var voice = _flavor.GetFactionVoice(factionId);
            Emit("The last " + ItemRef(itemId) + " has left the shelf. " + voice.voice);
        }

        public void OnStockLow(string itemId, int remaining, string factionId)
        {
            var voice = _flavor.GetFactionVoice(factionId);
            Emit("Stock of " + ItemRef(itemId) + " is now " + remaining + ". " + voice.voice);
        }

        public void OnStockEmpty(string itemId, string factionId)
        {
            var voice = _flavor.GetFactionVoice(factionId);
            Emit("No holdings of " + ItemRef(itemId) + " remain. " + voice.voice);
        }

        public void OnRejected(HoldfastTradeResult result, string factionId)
        {
            if (result == null) return;
            var voice = _flavor.GetFactionVoice(factionId);
            string detail = result.Failure switch
            {
                HoldfastTradeFailure.InvalidQuantity => "Quantity must be at least one.",
                HoldfastTradeFailure.InsufficientFunds => "Available value is below the listed worth.",
                HoldfastTradeFailure.InsufficientStock => "The selected counterparty has no stock at that quantity.",
                HoldfastTradeFailure.InsufficientInventory => "No holdings of this item are available for transfer.",
                HoldfastTradeFailure.InventoryCapacity => "The inventory cannot hold that quantity.",
                HoldfastTradeFailure.InvalidPrice => "The listed value cannot be represented safely.",
                HoldfastTradeFailure.UnknownItem => "The selected item is not in the Holdfast catalog.",
                HoldfastTradeFailure.UnknownFaction => "No valid Holdfast counterparty is selected.",
                HoldfastTradeFailure.UnavailableOrRestricted => "This supply remains reserved under current Holdfast restrictions.",
                _ => "Transaction declined."
            };

            Emit("Requisition refused: " + detail + " " + voice.rejected);
        }

        public void OnSaveCommitted(string path)
        {
            Emit("Ledger committed to " + System.IO.Path.GetFileName(path) + ". The old state is sealed.");
        }

        public void OnReloaded(string path)
        {
            Emit("Ledger reopened from " + System.IO.Path.GetFileName(path) + ". Previous state restored.");
        }

        public void OnQuarantine(string corruptPath)
        {
            Emit("Corrupt ledger quarantined to " + System.IO.Path.GetFileName(corruptPath) +
                 ". A fresh session has been opened.");
        }

        public void OnNewLedger()
        {
            Emit("New ledger started. Prior records archived. The desk is clean.");
        }

        private void Emit(string text)
        {
            _lastDispatch = text;
            _entries.Add(text);
            if (_entries.Count > MaxEntries)
                _entries.RemoveAt(0);
        }

        private static string ItemRef(string itemId)
        {
            return string.IsNullOrEmpty(itemId) ? "the item" : itemId.Replace("item_", "");
        }
    }
}
