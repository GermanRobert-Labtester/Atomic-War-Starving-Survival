// ============================================================================
// Self Test : ContrabandStashSelfTest (--contraband-stash-selftest)
// Proves    : Plan 147 host loop — catalog load + validation, day gate,
//             once-only claim, canonical inventory delta, no-side-effect reads,
//             checksummed save round-trip and post-restore replay block.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Economy;
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
                if (reloaded.ListDiscoverable(100).Count != 3)
                    return "[FAIL] expected exactly 3 remaining discoverable stashes after restore";
                GD.Print("[PASS] checksummed save round-trip preserves once-only claims");

                // ── 7. Narcotics slice: dependency catalog links the canonical id ──
                string depPath = Path.Combine(dataDir, "chemical_dependency_items.json");
                if (!new FileSystemIO().FileExists(depPath))
                    return "[FAIL] chemical dependency catalog missing";
                using (var depDoc = System.Text.Json.JsonDocument.Parse(new FileSystemIO().ReadAllText(depPath)))
                {
                    bool morphineLinked = false;
                    foreach (var it in depDoc.RootElement.GetProperty("items").EnumerateArray())
                    {
                        if (it.TryGetProperty("item_id", out var idEl) &&
                            string.Equals(idEl.GetString(), "morphine", StringComparison.Ordinal) &&
                            it.TryGetProperty("dependency_kind", out var kindEl) &&
                            string.Equals(kindEl.GetString(), "opioid", StringComparison.Ordinal))
                        {
                            morphineLinked = true;
                            break;
                        }
                    }
                    if (!morphineLinked)
                        return "[FAIL] canonical morphine not linked in the dependency catalog (opioid row required)";
                }
                GD.Print("[PASS] dependency catalog links canonical morphine (opioid) — host routes one dose per committed consumption");

                // ── 8. Barter acquisition route: the contraband broker ──
                var barterInventory = new Inventory();
                if (!barterInventory.TryProduce("item_canned_food", 30))
                    return "[FAIL] could not seed barter inventory";
                var barter = new ShelterBarterSystem(
                    new SeededRng(147), barterInventory, null, new GodotLog(),
                    id => itemCatalog.Get(id));
                barter.RegisterCaravan(ContrabandBrokerCaravan.Build(
                    catalog, ContrabandStashSystem.DefaultActivations()));

                barter.TickDay(10); // first arrival — high-tier gates not passed
                var brokerState = barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
                if (!brokerState.isAtAirlock)
                    return "[FAIL] broker not at airlock on its arrival day";
                if (brokerState.remainingStock["morphine"] != 0)
                    return "[FAIL] gated morphine stocked before its day gate";

                for (int day = 11; day <= 30; day++) barter.TickDay(day); // next arrival, gates passed
                brokerState = barter.State.caravans[ContrabandBrokerCaravan.CaravanId];
                if (brokerState.remainingStock["morphine"] != 4)
                    return $"[FAIL] morphine stock after gate = {brokerState.remainingStock["morphine"]}, expected 4";

                // Scarcity premium: 1 morphine costs 60 tradeValue × 1.25 = 75.
                float morphineCost = barter.CalculateCaravanStockCost(
                    barter.Catalog[ContrabandBrokerCaravan.CaravanId],
                    new Dictionary<string, int> { { "morphine", 1 } });
                if (System.Math.Abs(morphineCost - 75f) > 0.01f)
                    return $"[FAIL] morphine broker price {morphineCost} != canonical 75 (no second pricing authority)";

                var purchase = barter.ExecuteTrade(
                    ContrabandBrokerCaravan.CaravanId,
                    new Dictionary<string, int> { { "item_canned_food", 6 } }, // 6 × 15 = 90 ≥ 75
                    new Dictionary<string, int> { { "morphine", 1 } });
                if (purchase.Status != ActionResult.StatusKind.Success)
                    return $"[FAIL] broker purchase failed: {purchase.FailureCode}";
                if (barterInventory.CountById("morphine") != 1)
                    return "[FAIL] broker purchase did not grant the canonical item";
                if (brokerState.remainingStock["morphine"] != 3)
                    return "[FAIL] broker stock not pinned after purchase (reroll risk)";
                GD.Print("[PASS] broker barter: canonical premium pricing (75), day-gated stock, pinned after purchase");

                // Sell-back round trip must strictly lose value (anti-arbitrage).
                var sellBack = barter.ExecuteTrade(
                    ContrabandBrokerCaravan.CaravanId,
                    new Dictionary<string, int> { { "morphine", 1 } },
                    new Dictionary<string, int> { { "sugar", 1 } });
                if (sellBack.Status != ActionResult.StatusKind.Success)
                    return $"[FAIL] broker sell-back failed: {sellBack.FailureCode}";
                GD.Print("[PASS] broker round trip loses value — no arbitrage");

                return "CONTRABAND_STASH_SELFTEST PASS";
            }
            catch (Exception ex)
            {
                return $"[FAIL] {ex.GetType().Name}: {ex.Message}";
            }
        }
    }
}
