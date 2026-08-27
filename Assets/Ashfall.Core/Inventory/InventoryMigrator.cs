using System;
using System.Collections.Generic;

namespace Ashfall.Core.Inventory
{
    /// <summary>
    /// Migrates legacy or siloed inventory stores into the single authoritative player inventory.
    /// Deduplicates existing holdings and preserves state invariants.
    /// </summary>
    public static class InventoryMigrator
    {
        /// <summary>
        /// Migrates items held in a legacy HoldfastTradeSaveState into the authoritative player inventory.
        /// Detects conflicts and merges quantities without duplicate double-counting.
        /// </summary>
        public static int MigrateHoldfastHeld(
            HoldfastTradeSaveState? legacyTradeState,
            Inventory targetInventory,
            Func<string, ItemDefinition?>? catalogLookup = null)
        {
            if (legacyTradeState?.held == null || legacyTradeState.held.Count == 0 || targetInventory == null)
                return 0;

            int migratedCount = 0;
            foreach (var kv in legacyTradeState.held)
            {
                string canonicalId = ItemAliases.ToCanonical(kv.Key);
                int legacyQty = kv.Value;
                if (legacyQty <= 0) continue;

                int currentQty = targetInventory.CountById(canonicalId);
                // If the target inventory already has items, only add the difference if legacy holds more
                if (currentQty < legacyQty)
                {
                    int delta = legacyQty - currentQty;
                    var def = catalogLookup?.Invoke(canonicalId) ?? new ItemDefinition
                    {
                        id = canonicalId,
                        displayName = canonicalId,
                        stackMax = 99,
                        weight = 1f
                    };
                    targetInventory.Add(def, delta);
                    migratedCount += delta;
                }
            }

            // Clear legacy held dictionary so subsequent saves do not duplicate
            legacyTradeState.held.Clear();
            return migratedCount;
        }
    }
}
