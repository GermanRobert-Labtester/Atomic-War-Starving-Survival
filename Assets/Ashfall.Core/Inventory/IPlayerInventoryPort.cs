using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Narrow port for gameplay systems to query, consume, or produce items
    /// against the authoritative player inventory. Supports atomic transactions and provenance.
    /// </summary>
    public interface IPlayerInventoryPort
    {
        int CountById(string itemId);
        bool HasSufficient(string itemId, int count);
        bool TryConsume(string itemId, int count, Action? onCommitted = null);
        bool TryConsumeBill(IReadOnlyDictionary<string, int> bill, Action? onCommitted = null);
        bool TryProduce(string itemId, int count, ItemDefinition? def = null);
        InventoryTransactionQuote QuoteTransaction(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null);
        InventoryTransaction BeginTransaction(InventoryBill bill, Func<string, ItemDefinition?>? lookup = null);
        bool TryExecuteTransaction(InventoryBill bill, Action? onCommitted = null, Func<string, ItemDefinition?>? lookup = null);
        IReadOnlyList<InventorySlot> GetSlots();
    }
}
