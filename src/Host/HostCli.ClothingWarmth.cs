// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ClothingWarmthSelfTest
// Subsystem          : Plan 142 — Clothing & Warmth
// ============================================================================

using System;
using Ashfall.Core.Inventory;

namespace AtomicWar.GodotApp
{
    public static class HostCliClothingWarmth
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Clothing & Warmth Self-Test (Plan 142) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                var session = ClothingWarmthHostSession.Create();

                // Check 1: authored profile table
                if (session.System.Profiles.Count == 8
                    && session.System.Profiles.ContainsKey("item_winter_coat"))
                {
                    Console.WriteLine($"[PASS] Check 1: {session.System.Profiles.Count} clothing profiles registered.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Expected 8 profiles, got {session.System.Profiles.Count}.");
                }

                // Check 2: same-layer replacement
                session.Equip("surv_a", "item_ragged_coat");
                session.Equip("surv_a", "item_winter_coat");
                var afterReplace = session.GetEquipped("surv_a");
                if (afterReplace.Count == 1 && afterReplace[0].item_id == "item_winter_coat")
                {
                    Console.WriteLine("[PASS] Check 2: Same-layer equip replaced the previous outer item.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: Layer replacement failed (count={afterReplace.Count}).");
                }

                // Check 3: multi-layer stacking
                session.Equip("surv_a", "item_thermal_underwear");
                session.Equip("surv_a", "item_fur_boots");
                if (session.GetEquipped("surv_a").Count == 3)
                {
                    Console.WriteLine("[PASS] Check 3: Three distinct layers equipped (underwear/outer/accessory).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: Expected 3 layers, got {session.GetEquipped("surv_a").Count}.");
                }

                // Check 4: cold reduction present and capped
                float reduction = session.CalculateColdLossReduction("surv_a");
                if (reduction > 0.2f && reduction <= ClothingWarmthSystem.MaxColdMitigation)
                {
                    Console.WriteLine($"[PASS] Check 4: Cold-loss reduction {reduction:F3} within (0, {ClothingWarmthSystem.MaxColdMitigation}].");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: Cold-loss reduction out of range ({reduction}).");
                }

                // Check 5: condition scales the mitigation
                var worn = ClothingWarmthHostSession.Create();
                worn.Equip("surv_b", "item_arctic_survival_suit", condition: 1.0f);
                float pristine = worn.CalculateColdLossReduction("surv_b");
                worn.DegradeCondition("surv_b", 480f); // 20 days of wear
                float degraded = worn.CalculateColdLossReduction("surv_b");
                if (degraded < pristine)
                {
                    Console.WriteLine($"[PASS] Check 5: Condition wear reduced mitigation ({pristine:F3} → {degraded:F3}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: Degradation did not reduce mitigation ({pristine} → {degraded}).");
                }

                // Check 6: wetness reduces non-waterproof mitigation
                var wet = ClothingWarmthHostSession.Create();
                wet.Equip("surv_c", "item_winter_coat");
                float dryReduction = wet.CalculateColdLossReduction("surv_c");
                wet.ApplyWetness("surv_c", 1.0f);
                float soakedReduction = wet.CalculateColdLossReduction("surv_c");
                if (soakedReduction < dryReduction)
                {
                    Console.WriteLine($"[PASS] Check 6: Wetness reduced non-waterproof mitigation ({dryReduction:F3} → {soakedReduction:F3}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Wetness did not reduce mitigation ({dryReduction} → {soakedReduction}).");
                }

                // Check 7: waterproof outer limits wetness absorption
                var wp = ClothingWarmthHostSession.Create();
                wp.Equip("surv_d", "item_hazmat_cold_suit");
                wp.System.ApplyWetness("surv_d", 1.0f);
                float wpWet = wp.System.State.survivors["surv_d"].wetness;
                if (wpWet > 0.15f && wpWet < 0.30f)
                {
                    Console.WriteLine($"[PASS] Check 7: Waterproof outer reduced soak to {wpWet:F2} (80% absorption cut).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: Waterproof absorption unexpected ({wpWet}).");
                }

                // Check 8: drying lowers wetness
                wp.DryClothing("surv_d", 2f);
                float dried = wp.System.State.survivors["surv_d"].wetness;
                if (dried < wpWet)
                {
                    Console.WriteLine($"[PASS] Check 8: Drying lowered wetness ({wpWet:F2} → {dried:F2}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: Drying did not lower wetness ({wpWet} → {dried}).");
                }

                // Check 9: census projection
                var census = session.GetCensus();
                if (census.SurvivorsTracked == 1 && census.TotalEquippedItems == 3 && census.ProfilesRegistered == 8)
                {
                    Console.WriteLine($"[PASS] Check 9: Census reports {census.SurvivorsTracked} survivor, {census.TotalEquippedItems} items.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 9: Census mismatch ({census.SurvivorsTracked}/{census.TotalEquippedItems}/{census.ProfilesRegistered}).");
                }

                // Check 10: unequip
                bool removed = session.Unequip("surv_a", "item_fur_boots");
                if (removed && session.GetEquipped("surv_a").Count == 2)
                {
                    Console.WriteLine("[PASS] Check 10: Unequip removed the accessory layer.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 10: Unequip failed ({removed}, count={session.GetEquipped("surv_a").Count}).");
                }

                // Check 11: save/restore round-trip preserves layers, condition, wetness
                bool saved = session.TrySave();
                var reloaded = ClothingWarmthHostSession.Create();
                bool loaded = reloaded.TryLoad();
                var rec = reloaded.System.State.survivors.TryGetValue("surv_a", out var restoredRec) ? restoredRec : null;
                if (saved && loaded && rec != null && rec.equipped.Count == 2)
                {
                    Console.WriteLine("[PASS] Check 11: Save/restore preserved equipped layers.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Save/restore failed (saved={saved}, loaded={loaded}).");
                }

                // Check 12: save store contract names
                if (ClothingWarmthSaveStore.SectionName.Equals("clothing_warmth", StringComparison.Ordinal)
                    && ClothingWarmthSaveStore.FileName.Equals("clothing_warmth_save.json", StringComparison.Ordinal))
                {
                    Console.WriteLine("[PASS] Check 12: Save store contract names verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: Save store contract names mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Clothing & Warmth Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
