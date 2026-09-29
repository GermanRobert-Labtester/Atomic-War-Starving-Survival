// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Clock;
using Ashfall.Core.Crafting;
using Ashfall.Core.Economy;
using Ashfall.Core.Endgame;
using Ashfall.Core.Events;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Flags;
using Ashfall.Core.Legacy;
using Ashfall.Core.Medical;
using Ashfall.Core.Muster;
using Ashfall.Core.Narrative;
using Ashfall.Core.Save;
using Ashfall.Core.Settings;
using Ashfall.Core.Shelter;
using Ashfall.Core.Survivors;
using Ashfall.Core.UtilityAI;
using Ashfall.Core.Verdict;
using Ashfall.Core.Warlords;
using Ashfall.Core.World;
using Ashfall.Core.YearOfAsh;
using AtomicWar.GodotApp.Narrative;
using AtomicWar.GodotApp.Settings;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.YearOfAsh;
using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {

        /// <summary>
        /// Expansion hub save gate: build the hub session, unlock the waystation,
        /// walk a Standing Record site, grant the Crossing vouch, plant + water a
        /// greenhouse plot, capture, write through the codec to a temp path, reload
        /// into a fresh session, restore, and verify each surface reproduces. Then
        /// tamper the file and verify the checksum refuses it.
        /// </summary>
        public static int RunExpansionHubSaveSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_expansion_hub_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

            int failures = 0;
            void Check(bool condition, string name)
            {
                if (condition) GD.Print("[PASS] " + name);
                else
                {
                    GD.Print("[FAIL] " + name);
                    failures++;
                }
            }

            try
            {
                var session = ExpansionHostSession.Create(dataDirectory);
                session.UnlockWaystation();
                session.AssignWaystationWatch(new[] { "elena_vasquez" });
                session.UnlockRecord();
                session.ArriveAtSite("loc_cut_kilometre_19");
                session.EnterSiteRoom("room_km19_post");
                session.InspectSiteRoom("room_km19_post");
                session.GrantVouch("npc_osran_kell");
                session.EnsureGreenhousePlots(3);
                session.PlantGreenhouse(0, "item_seed_tuber", 12);
                session.WaterGreenhouse(0, 60f);
                session.TickGreenhouse(13);
                session.LoadDefaultBackerPool();
                session.Arbitration.CallStanding("quest_crossing_the_terms", 13);
                session.Arbitration.DeclareBacker("quest_crossing_the_terms", CrossingIds.NpcOsran);
                session.Arbitration.DeclareBacker("quest_crossing_the_terms", CrossingIds.NpcMattis);
                session.Arbitration.DeclareBacker("quest_crossing_the_terms", "npc_halden_mire");
                session.Ledger.PresentContract(CrossingIds.NpcWyn, 12f, 30, 0.2f, "the pledged grain");
                session.Ledger.PresentContract(CrossingIds.NpcWyn, 12f, 30, 0.2f, "the pledged grain");
                session.Ledger.SignContract(CrossingIds.NpcWyn, 13);

                var save = session.CaptureSave(13);
                Check(!string.IsNullOrEmpty(save.Checksum), "capture stamps checksum");
                Check(save.saveVersion == ExpansionHubSave.CurrentSaveVersion, "saveVersion current");
                Check(save.waystation.unlocked, "envelope carries waystation unlock");
                Check(save.layouts.expansionUnlocked, "envelope carries record unlock");
                Check(save.vouch.vouchedBy == "npc_osran_kell", "envelope carries the vouch");
                Check(save.greenhouse.plots != null && save.greenhouse.plots.Count == 3,
                    "envelope carries greenhouse plots");

                Check(ExpansionHubSaveStore.TrySave(save, tmpPath), "save written via codec");

                var fresh = ExpansionHostSession.Create(dataDirectory);
                var loaded = ExpansionHubSaveStore.TryLoad(tmpPath);
                Check(loaded != null, "save loads back");
                if (loaded != null)
                {
                    fresh.RestoreSave(loaded);
                    Check(fresh.Waystation.Unlocked, "waystation unlock restored");
                    Check(fresh.Vouch.HasAccess, "vouch restored");
                    Check(fresh.Layouts.State.expansionUnlocked, "record unlock restored");
                    Check(fresh.Greenhouse.PlotCount == 3, "greenhouse plots restored");
                    Check(fresh.Greenhouse.State.plots.Count > 0
                        && fresh.Greenhouse.State.plots[0].seedItemId == "item_seed_tuber",
                        "planted seed restored");
                    Check(fresh.Arbitration.State.rulingsCalled >= 1, "arbitration rulings restored");
                    Check(fresh.Arbitration.IsRulingActive("quest_crossing_the_terms"),
                        "arbitration active ruling restored");
                    Check(fresh.Ledger.GetContract(CrossingIds.NpcWyn)?.signed == true,
                        "ledger contract restored as signed");
                }

                // Tamper: flip the sim day in the raw text. Checksum must refuse it.
                string raw = File.ReadAllText(tmpPath);
                string tampered = raw.Replace("\"simDay\":13", "\"simDay\":1");
                Check(tampered != raw, "tamper actually changed the payload");
                if (tampered != raw)
                {
                    File.WriteAllText(tmpPath, tampered);
                    Check(ExpansionHubSaveStore.TryLoad(tmpPath) == null, "tampered save rejected (checksum)");
                }
            }
            catch (Exception e)
            {
                Check(false, "selftest threw: " + e.Message);
            }
            finally
            {
                TryDeleteTempFile(tmpPath);
            }

            return EmitSummary("expansion_hub_save_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
