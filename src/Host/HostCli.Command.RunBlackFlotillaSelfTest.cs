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
        /// The Black Flotilla / Maritime (Expansion 09) headless gate. Drives the
        /// live MaritimeHostSession surface: deep-lore catalog loading, dive-site
        /// data presence, deterministic procedural scavenge, stealth-dive
        /// room/air/noise/compromise progression, psychological contamination,
        /// visit-state depletion, and a checksummed save round-trip. Pure
        /// host + Core — no UI nodes.
        /// </summary>
        public static int RunBlackFlotillaSelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();

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

            string tmpPath = Path.Combine(
                Path.GetTempPath(), "ashfall_black_flotilla_selftest_" + Guid.NewGuid().ToString("N") + ".json"); // DETERMINISM_ALLOWLIST: Selftest scratch file path

            try
            {
                var io = new FileSystemIO();
                var json = new SystemTextJsonSerializer();

                // 1. Catalog / data loading: deep-lore locations + dive-site data file.
                var locations = Ashfall.Core.Maritime.DeepLoreLocationCatalogLoader.Load(dataDirectory, io, json);
                Check(locations.Count >= 10, "deep_lore_locations.json loads " + locations.Count + " locations");
                var library = Ashfall.Core.Maritime.DeepLoreLocationCatalogLoader.FindById(locations, "location_municipal_library");
                Check(library != null && library.lootTable.Count > 0, "deep-lore location carries a loot table");
                string divePath = io.Combine(dataDirectory, "dive_sites.json");
                Check(io.FileExists(divePath), "dive_sites.json present in the data authority");
                var diveSites = Ashfall.Core.Maritime.DiveSiteCatalogLoader.Load(dataDirectory, io, json);
                Check(diveSites != null && diveSites.dive_sites != null && diveSites.dive_sites.Count >= 4,
                    "dive_sites.json defines 4+ sites");
                if (diveSites != null && diveSites.dive_sites != null)
                {
                    bool sovereignFound = Ashfall.Core.Maritime.DiveSiteCatalogLoader.FindById(
                        diveSites, "site_exp09_ss_sovereign") != null;
                    Check(sovereignFound, "canonical wreck site site_exp09_ss_sovereign present");
                }

                // 2. Host wiring: the four engine-agnostic maritime systems are alive.
                var session = MaritimeHostSession.Create(dataDirectory);
                Check(session.Dive != null && session.Scavenge != null && session.Psychology != null,
                    "maritime host wires dive + scavenge + psychological systems");
                Check(session.LootNodes.Count >= 4, "host seeds loot nodes");

                // 3. Deterministic procedural scavenge (same seed → identical rolls).
                var table = new List<Ashfall.Core.Maritime.VariableLootNode>();
                table.AddRange(session.LootNodes);
                var s1 = new Ashfall.Core.Maritime.ProceduralScavengeSystem(new SeededRng(9909));
                var s2 = new Ashfall.Core.Maritime.ProceduralScavengeSystem(new SeededRng(9909));
                s1.SetCurrentDay(30);
                s2.SetCurrentDay(30);
                var r1 = s1.RollLootTable("loc_selftest_a", table, 2f, false);
                var r2 = s2.RollLootTable("loc_selftest_a", table, 2f, false);
                Check(r1.Count == r2.Count, "scavenge deterministic (same seed, same roll count)");
                bool identical = r1.Count == r2.Count;
                for (int i = 0; i < r1.Count && identical; i++)
                    identical = r1[i].ItemId == r2[i].ItemId && r1[i].Quantity == r2[i].Quantity;
                Check(identical, "scavenge deterministic (rolls identical)");

                // 4. Dive-room progression + air / noise / compromised state.
                Check(!session.Dive!.IsActive, "dive starts idle");
                session.StartDive("diver_selftest", "operator_selftest");
                Check(session.Dive.IsActive, "dive launches");
                Check(Math.Abs(session.Dive.AirSupplySeconds - 120f) < 0.001f, "dive starts at full air (120s)");
                session.TickDive(60f);
                Check(Math.Abs(session.Dive.AirSupplySeconds - 60f) < 0.001f, "air consumed on tick");
                session.CrankDiveCompressor();
                Check(Math.Abs(session.Dive.AirSupplySeconds - 90f) < 0.001f, "compressor crank restores air");
                bool advanced = session.Dive.AdvanceToNextRoom(50);
                Check(advanced && session.Dive.CurrentRoomIndex == 1 && session.Dive.NoiseLevel == 50,
                    "advance to companionway with noise 50");
                session.Dive.AdvanceToNextRoom(40);
                Check(session.Dive.NoiseLevel == 90 && session.Dive.IsCompromised,
                    "noise >= 80 compromises the dive");
                session.Dive.AdvanceToNextRoom(40);
                Check(session.Dive.NoiseLevel == 100, "noise clamps at 100");
                Check(!session.Dive.AdvanceToNextRoom(40), "cannot advance past the deep hold");

                // 5. Contamination / psychological state.
                session.ContaminateDemo("survivor_selftest", "location_sunshine_daycare");
                Check(session.Psychology!.HasContamination("survivor_selftest",
                        Ashfall.Core.Maritime.PsychologicalContaminationSystem.Contam_ChildCotTrauma),
                    "daycare visit applies child-cot trauma");
                Check(session.Psychology.IsActionBlocked("survivor_selftest", "action_teach_child"),
                    "contamination blocks a work action");

                // 6. Depletion / visit state.
                session.ScavengeDemo("location_municipal_library");
                Check(session.Scavenge!.GetVisitCount("location_municipal_library") >= 1,
                    "scavenge visit state recorded (depletion tracking)");

                // 7. Save capture/restore round-trip through the checksummed envelope.
                var save = session.CaptureSave();
                Check(save != null && save.Dive != null && save.Scavenge != null && save.Psychology != null,
                    "maritime save captures all three engine states");
                save!.Checksum = SaveChecksum.Compute(save);
                File.WriteAllText(tmpPath, json.Serialize(save));
                var loaded = json.Deserialize<MaritimeHostSave>(File.ReadAllText(tmpPath));
                Check(loaded != null && loaded.Checksum == SaveChecksum.Compute(loaded),
                    "checksummed envelope round-trips");
                if (loaded != null)
                {
                    var fresh = new MaritimeHostSession();
                    fresh.RestoreSave(loaded);
                    Check(fresh.Dive.IsActive == session.Dive.IsActive, "dive state restored");
                    Check(Math.Abs(fresh.Dive.NoiseLevel - session.Dive.NoiseLevel) < 0.001f, "noise restored");
                    Check(fresh.Scavenge.GetVisitCount("location_municipal_library") ==
                          session.Scavenge.GetVisitCount("location_municipal_library"), "visit state restored");
                    Check(fresh.Psychology.HasContamination("survivor_selftest",
                            Ashfall.Core.Maritime.PsychologicalContaminationSystem.Contam_ChildCotTrauma),
                        "contamination restored from save");
                }
            }
            catch (Exception e)
            {
                Check(false, "black flotilla selftest threw: " + e.Message);
            }
            finally
            {
                TryDeleteTempFile(tmpPath);
            }

            return EmitSummary("black_flotilla_selftest", failures == 0, failures == 0 ? 0 : 1, details: failures == 0 ? "PASS" : $"FAIL ({failures})");
        }

    }
}
