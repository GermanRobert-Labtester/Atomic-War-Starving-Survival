// ============================================================================
// Self Test : ContrabandStashSelfTest (--contraband-stash-selftest)
// Proves    : Plan 147 host loop — catalog load + validation, day gate,
//             once-only claim, canonical inventory delta, no-side-effect reads,
//             checksummed save round-trip and post-restore replay block.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Narrative;
using Godot;

namespace AtomicWar.GodotApp
{
    public static class ContrabandStashSelfTest
    {
        public static string Run(string dataDirectory)
        {
            try
            {
                // ── Resolve the data authority ──
                string dataDir = dataDirectory ?? string.Empty;
                if (string.IsNullOrEmpty(dataDir))
                    CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir);
                if (string.IsNullOrEmpty(dataDir))
                    return "[FAIL] could not resolve the StreamingAssets data directory";

                string catalogPath = Path.Combine(dataDir, "narrative", "bunker_contraband_barter.json");
                if (!new FileSystemIO().FileExists(catalogPath))
                    return $"[FAIL] contraband catalog not found at {catalogPath}";

                string catalogJson = new FileSystemIO().ReadAllText(catalogPath);

                // ── 1. Validation gates the load ──
                var validation = ContrabandCatalogValidator.ValidateJson(catalogJson);
                if (!validation.IsValid)
                    return "[FAIL] authored catalog failed validation: " + string.Join("; ", validation.Errors);
                if (validation.EntryCount != 20)
                    return $"[FAIL] expected 20 entries, found {validation.EntryCount}";
                GD.Print("[PASS] catalog validation (20 entries)");

                var catalog = BunkerContrabandCatalog.LoadFromJson(catalogJson);

                // ── 2. System construction over canonical inventory ──
                var itemCatalog = ItemCatalogLoader.LoadCatalog(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
                var inventory = new Inventory();
                var system = new ContrabandStashSystem(
                    catalog, inventory, id => itemCatalog.Get(id), new GodotLog());

                foreach (var activation in ContrabandStashSystem.DefaultActivations())
                {
                    if (!system.TryRegisterActivation(activation))
                        return $"[FAIL] default activation rejected: {activation.entryId}";
                }
                GD.Print("[PASS] three tiered activations registered");

                // ── 3. Day gate blocks early claim ──
                var early = system.TryClaimStash("contraband_century_seed_grain_vial", 5);
                if (early.Status != ActionResult.StatusKind.Blocked)
                    return "[FAIL] tier-3 stash claim before its day gate was not blocked";
                GD.Print("[PASS] tier-3 day gate blocks early claim");

                // ── 4. Claim grants canonical items exactly once ──
                var claim = system.TryClaimStash("contraband_card_deck_pinned_kings", 3);
                if (claim.Status != ActionResult.StatusKind.Success)
                    return $"[FAIL] tier-1 claim failed: {claim.FailureCode}";
                if (inventory.CountById("item_playing_cards") != 1)
                    return "[FAIL] canonical item_playing_cards grant missing";
                var reclaim = system.TryClaimStash("contraband_card_deck_pinned_kings", 4);
                if (reclaim.Status != ActionResult.StatusKind.Blocked ||
                    reclaim.FailureCode != "contraband_already_claimed")
                    return "[FAIL] once-only claim not enforced";
                GD.Print("[PASS] claim grants canonical items and is once-only");

                // ── 5. No side effects on query ──
                int slotsBefore = 0;
                foreach (var s in inventory.Slots) if (s.Amount > 0) slotsBefore++;
                _ = system.ListDiscoverable(50);
                _ = system.IsDiscoverable("contraband_unrationed_sugar_brick", 50);
                int slotsAfter = 0;
                foreach (var s in inventory.Slots) if (s.Amount > 0) slotsAfter++;
                if (slotsBefore != slotsAfter)
                    return "[FAIL] catalog query mutated inventory";
                GD.Print("[PASS] queries are side-effect free");

                // ── 6. Checksummed save round-trip ──
                var captured = system.CaptureState();
                if (!ContrabandSaveStore.TrySave(captured))
                    return "[FAIL] save store write failed";
                var loaded = ContrabandSaveStore.TryLoad();
                if (loaded == null)
                    return "[FAIL] save store load returned null";
                if (!loaded.claimedDayByEntry.ContainsKey("contraband_card_deck_pinned_kings"))
                    return "[FAIL] claim not preserved in save section";

                var reloaded = new ContrabandStashSystem(
                    catalog, new Inventory(), id => itemCatalog.Get(id), new GodotLog());
                foreach (var activation in ContrabandStashSystem.DefaultActivations())
                    reloaded.TryRegisterActivation(activation);
                reloaded.RestoreState(loaded);

                var replay = reloaded.TryClaimStash("contraband_card_deck_pinned_kings", 12);
                if (replay.Status != ActionResult.StatusKind.Blocked)
                    return "[FAIL] save/reload reopened a claimed stash";
                if (reloaded.ListDiscoverable(100).Count != 2)
                    return "[FAIL] expected exactly 2 remaining discoverable stashes after restore";
                GD.Print("[PASS] checksummed save round-trip preserves once-only claims");

                return "CONTRABAND_STASH_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
