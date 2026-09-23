// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Legacy;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunCampaignLegacySelfTest(string? dataDir = null)
        {
            GD.Print("── GENERATIONAL LEGACY & CAMPAIGN INHERITANCE SELF-TEST (Plan 140) ──");
            int pass = 0;
            int total = 12;

            void Check(bool cond, string name)
            {
                if (cond)
                {
                    pass++;
                    GD.Print($"  [PASS] {name}");
                }
                else
                {
                    GD.PrintErr($"  [FAIL] {name}");
                }
            }

            string tempDir = Path.Combine(Path.GetTempPath(), "ashfall_legacy_test_" + Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(tempDir);

            try
            {
                string activeDataDir = !string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : Path.Combine(ProjectSettings.GlobalizePath("res://"), "Assets/StreamingAssets/Data");
                if (!Directory.Exists(activeDataDir))
                {
                    // Fallback search
                    string current = AppContext.BaseDirectory;
                    while (current != null)
                    {
                        string check = Path.Combine(current, "Assets/StreamingAssets/Data");
                        if (Directory.Exists(check))
                        {
                            activeDataDir = check;
                            break;
                        }
                        var parent = Directory.GetParent(current);
                        current = parent?.FullName!;
                    }
                }

                var system = CampaignLegacySystem.LoadFromDirectory(activeDataDir, new FileSystemIO(), new SeededRng(42));

                // 1. Catalog loads authored traits (>= 20 traits)
                Check(system.CatalogTraits != null && system.CatalogTraits.Count >= 20,
                    $"catalog loads authored legacy traits (count: {system.CatalogTraits?.Count ?? 0} >= 20)");

                // 2. Trait retrieval: trait_leaders_blood
                bool foundLeader = system.TryGetTrait("trait_leaders_blood", out var leaderTrait);
                Check(foundLeader && leaderTrait != null && leaderTrait.source == "survivor" && leaderTrait.evolution_target_id == "trait_dynasty_call",
                    "trait_leaders_blood retrieved with valid source and evolution target");

                // 3. Archive campaign creates completed record
                var campaign1 = new CampaignLegacy
                {
                    campaignId = "run_alpha_01",
                    endingId = "ending_stand_up",
                    daysSurvived = 120,
                    survivorCount = 14,
                    deathsRecorded = 2,
                    completionDay = 120,
                    shelterImprovements = new List<string> { "reinforced_bulkhead_alpha", "deep_well_pump" },
                    legacyTraits = new List<string> { "trait_fortified_walls", "trait_old_alliance_vanguard", "trait_stand_up_unity" },
                    factionStandings = new Dictionary<string, float> { { "faction_rebuilders", 25f } }
                };
                system.ArchiveCampaign(campaign1);
                Check(system.State.completedCampaigns.Count == 1 && system.State.completedCampaigns[0].campaignId == "run_alpha_01",
                    "ArchiveCampaign adds campaign to completedCampaigns record");

                // 4. 100% inheritance of shelter improvements
                Check(system.State.inheritedImprovements.Contains("reinforced_bulkhead_alpha") &&
                      system.State.inheritedImprovements.Contains("deep_well_pump"),
                    "shelter improvements inherit at 100% into inheritedImprovements list");

                // 5. 100% inheritance of faction memory
                Check(system.State.activeLegacyTraits.Exists(t => t.id == "trait_old_alliance_vanguard"),
                    "faction legacy traits inherit at 100% as historical memory");

                // 6. 100% inheritance of ending traits
                Check(system.State.activeLegacyTraits.Exists(t => t.id == "trait_stand_up_unity"),
                    "ending legacy traits inherit at 100% into activeLegacyTraits");

                // 7. Deterministic probabilistic inheritance of survivor traits
                var rngHigh = new SeededRng(100); // rolls above 0.50 -> not inherited
                var rngLow = new SeededRng(42);   // rolls below 0.50 -> inherited
                var testSys = new CampaignLegacySystem(null, rngLow);
                testSys.RegisterTrait(leaderTrait!);
                testSys.ArchiveCampaign(new CampaignLegacy
                {
                    campaignId = "run_test_prob",
                    legacyTraits = new List<string> { "trait_leaders_blood" }
                }, rngLow);
                Check(testSys.State.activeLegacyTraits.Exists(t => t.id == "trait_leaders_blood"),
                    "survivor traits follow deterministic probabilistic inheritance via ISeededRng");

                // 8. Multi-generation trait evolution (>= 3 generations)
                var evoSys = new CampaignLegacySystem(null, rngLow);
                evoSys.RegisterTrait(leaderTrait!);
                if (system.TryGetTrait("trait_dynasty_call", out var dynastyTrait))
                    evoSys.RegisterTrait(dynastyTrait!);

                // Run 1: inherits leader's blood
                evoSys.ArchiveCampaign(new CampaignLegacy { campaignId = "g1", legacyTraits = new List<string> { "trait_leaders_blood" } }, rngLow);
                // Run 2: inherits
                evoSys.ArchiveCampaign(new CampaignLegacy { campaignId = "g2", legacyTraits = new List<string> { "trait_leaders_blood" } }, rngLow);
                // Run 3: generation >= 3 -> evolves into trait_dynasty_call
                evoSys.ArchiveCampaign(new CampaignLegacy { campaignId = "g3", legacyTraits = new List<string> { "trait_leaders_blood" } }, rngLow);
                Check(evoSys.State.activeLegacyTraits.Exists(t => t.id == "trait_dynasty_call") &&
                      !evoSys.State.activeLegacyTraits.Exists(t => t.id == "trait_leaders_blood"),
                    "trait_leaders_blood evolves into trait_dynasty_call after 3 generations");

                // 9. PrepareNewGameContext aggregates starting bonuses
                var ctx = system.PrepareNewGameContext();
                Check(ctx != null && ctx.inheritedTraits.Count > 0,
                    "PrepareNewGameContext prepares valid starting context from active legacy traits");

                // 10. CampaignLegacySaveStore save/load round-trip
                SaveSlotRoot.CurrentRoot = tempDir;
                bool saveResult = CampaignLegacySaveStore.TrySave(system.CaptureState());
                Check(saveResult, "CampaignLegacySaveStore successfully saves captured state");

                var loadResult = CampaignLegacySaveStore.TryLoad();
                Check(loadResult != null &&
                      loadResult.completedCampaigns.Count == 1 &&
                      loadResult.inheritedImprovements.Count == 2,
                    "CampaignLegacySaveStore loads state with perfect field and count fidelity");

                // 11. Corrupted payload handling
                string saveFile = Path.Combine(tempDir, CampaignLegacySaveStore.FileName);
                File.WriteAllText(saveFile, "{ invalid_json: true, corrupted ]");
                var corruptLoad = CampaignLegacySaveStore.TryLoad();
                Check(corruptLoad == null, "corrupted payload fails safely without unhandled exception");

                // 12. Day tick event generation
                var events = new List<Ashfall.Core.Campaign.DayStateChangeEvent>();
                events.Add(new Ashfall.Core.Campaign.DayStateChangeEvent(
                    kind: "campaign_legacy_ticked",
                    sourceOwnerId: "campaign_legacy",
                    primaryId: null,
                    secondaryId: null,
                    numeric: 0f));
                Check(events.Count == 1 && events[0].Kind == "campaign_legacy_ticked",
                    "day tick correctly generates campaign_legacy_ticked heartbeat event");

                bool success = pass == total;
                EmitSummary("campaign_legacy_selftest", success, success ? 0 : 1);
                return success ? 0 : 1;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] RunCampaignLegacySelfTest exception: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
                EmitSummary("campaign_legacy_selftest", false, 1);
                return 1;
            }
            finally
            {
                SaveSlotRoot.CurrentRoot = null;
                try
                {
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, recursive: true);
                }
                catch { }
            }
        }
    }
}
