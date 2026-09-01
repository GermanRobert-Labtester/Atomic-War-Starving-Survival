using System;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Classifies the gameplay origin or cause of an inventory mutation.
    /// </summary>
    public enum InventoryMutationSource
    {
        Unknown = 0,
        Consume = 1,
        Produce = 2,
        Loot = 3,
        Trade = 4,
        Spoilage = 5,
        Migration = 6,
        Admin = 7
    }

    /// <summary>
    /// Records the provenance and causality of a single inventory mutation.
    /// </summary>
    public readonly struct InventoryProvenanceRecord
    {
        public string ItemId { get; }
        public int Delta { get; }
        public InventoryMutationSource Source { get; }
        public int Day { get; }
        public string Context { get; }

        public InventoryProvenanceRecord(string itemId, int delta, InventoryMutationSource source, int day = 0, string? context = null)
        {
            ItemId = ItemAliases.ToCanonical(itemId);
            Delta = delta;
            Source = source;
            Day = day;
            Context = context ?? string.Empty;
        }

        public override string ToString() => $"[Day {Day} | {Source}] {ItemId} {(Delta >= 0 ? "+" : "")}{Delta} ({Context})";
    }
}
