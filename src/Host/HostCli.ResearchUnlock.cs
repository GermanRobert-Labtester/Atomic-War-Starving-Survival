// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 141 (Research Downstream Unlocks Bridge).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Research;

namespace AtomicWar.GodotApp
{
    public static class HostCliResearchUnlock
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Research Unlock Bridge Self-Test (Plan 141) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var inventory = new Ashfall.Core.Inventory.Inventory();
                var host = ResearchUnlockHostSession.Create(dataDir, inventory);
                if (host.Bridge.Catalog.Count >= 30)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({host.Bridge.Catalog.Count} definitions).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Expected >= 30 unlocks, got {host.Bridge.Catalog.Count}.");
                }

                // Check 2: Catalog variety across all 6 unlock types
                var types = host.Bridge.Catalog.Select(u => u.unlock_type.ToLowerInvariant()).Distinct().ToList();
                if (types.Contains("item") && types.Contains("recipe") && types.Contains("shelter") &&
                    types.Contains("expedition") && types.Contains("combat") && types.Contains("medical"))
                {
                    GD.Print("[PASS] Check 2: Catalog contains all 6 unlock types (item, recipe, shelter, expedition, combat, medical).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Missing unlock types. Found: {string.Join(", ", types)}");
                }

                // Check 3: Breakthrough item grant to player inventory
                int granted = host.ProcessResearchCompletion("knowledge_water_advanced");
                if (granted > 0 && inventory.CountById("item_water_filter_advanced") >= 1)
                {
                    GD.Print("[PASS] Check 3: Breakthrough item granted directly to inventory.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Breakthrough item not granted. Count={inventory.CountById("item_water_filter_advanced")}.");
                }

                // Check 4: Recipe unlocking
                if (host.HasRecipe("purify_water"))
                {
                    GD.Print("[PASS] Check 4: Recipe 'purify_water' unlocked in bridge state.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Recipe 'purify_water' was not unlocked.");
                }

                // Check 5: Capability enabling
                int radGranted = host.ProcessResearchCompletion("knowledge_radiation_shielding");
                if (radGranted > 0 && host.HasCapability("shelter_upgrade_lead_lining"))
                {
                    GD.Print("[PASS] Check 5: Shelter capability 'shelter_upgrade_lead_lining' enabled.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 5: Capability 'shelter_upgrade_lead_lining' was not enabled.");
                }

                // Check 6: Idempotency (re-processing completed node does not duplicate)
                int repeatGranted = host.ProcessResearchCompletion("knowledge_water_advanced");
                if (repeatGranted == 0 && inventory.CountById("item_water_filter_advanced") == 1)
                {
                    GD.Print("[PASS] Check 6: Re-processing already-completed node is idempotent (0 new grants).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Idempotency failed. Repeat grants={repeatGranted}.");
                }

                // Check 7: Retroactive synchronization from completed nodes
                var freshInv = new Ashfall.Core.Inventory.Inventory();
                var freshHost = ResearchUnlockHostSession.Create(dataDir, freshInv);
                var completedNodes = new[] { "knowledge_air_filtration", "knowledge_solar_advanced" };
                int synced = freshHost.SynchronizeCompletedResearch(completedNodes);
                if (synced >= 4 && freshInv.CountById("item_air_filter_hepa") >= 1 && freshHost.HasCapability("shelter_upgrade_solar_roof"))
                {
                    GD.Print($"[PASS] Check 7: Retroactive synchronization granted {synced} unlocks for saved nodes.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Retroactive sync failed. Synced={synced}.");
                }

                // Check 8: State capture / restore round-trip
                var captured = host.CaptureState();
                var restoredHost = ResearchUnlockHostSession.Create(dataDir, new Ashfall.Core.Inventory.Inventory());
                restoredHost.RestoreState(captured);
                if (restoredHost.HasRecipe("purify_water") && restoredHost.HasCapability("shelter_upgrade_lead_lining"))
                {
                    GD.Print("[PASS] Check 8: State capture and restore preserves recipes and capabilities.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: State capture/restore failed to preserve state.");
                }

                // Check 9: Save store serialization
                string json = ResearchUnlockSaveStore.TryCapturePersisted(captured);
                var fromJson = ResearchUnlockSaveStore.TryRestorePersisted(json);
                if (fromJson != null && fromJson.grantedUnlockIds.Count == captured.grantedUnlockIds.Count)
                {
                    GD.Print("[PASS] Check 9: ResearchUnlockSaveStore serialization round-trip succeeds.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: Save store serialization failed.");
                }

                // Check 10: Census consistency
                var census = host.Census;
                if (census.TotalCatalogUnlocks >= 30 && census.GrantedUnlocksCount > 0 && census.UnlockedRecipesCount > 0)
                {
                    GD.Print($"[PASS] Check 10: Census reflects valid state (catalog={census.TotalCatalogUnlocks}, granted={census.GrantedUnlocksCount}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 10: Census returned invalid values.");
                }

                // Check 11: Binding to ResearchSystem event
                var testResearch = new ResearchSystem();
                var testInv = new Ashfall.Core.Inventory.Inventory();
                var testHost = ResearchUnlockHostSession.Create(dataDir, testInv);
                testHost.BindResearchSystem(testResearch);

                var nodeDef = new ResearchKnowledgeDef(
                    "knowledge_gas_mask_improved",
                    "Improved Gas Masks",
                    "engineering",
                    "Double-canister respirator masks",
                    5,
                    null,
                    "item_gas_mask_improved");
                testResearch.Register(nodeDef);
                testResearch.StartResearch(nodeDef.id, 1);
                testResearch.CompleteResearch(nodeDef.id);

                if (testInv.CountById("item_gas_mask_improved") >= 1)
                {
                    GD.Print("[PASS] Check 11: ResearchSystem completion event automatically triggered downstream unlock.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: ResearchSystem event did not trigger unlock.");
                }

                // Check 12: Resilience against malformed or empty node IDs
                int emptyGranted = host.ProcessResearchCompletion("");
                int nullGranted = host.ProcessResearchCompletion(null!);
                int bogusGranted = host.ProcessResearchCompletion("nonexistent_node_xyz");
                if (emptyGranted == 0 && nullGranted == 0 && bogusGranted == 0)
                {
                    GD.Print("[PASS] Check 12: Malformed/nonexistent node IDs rejected safely with 0 grants.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: Malformed node IDs produced unexpected grants.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[ERROR] Research Unlock Bridge Self-Test threw exception: {ex}");
            }

            GD.Print($"=== Research Unlock Bridge Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
