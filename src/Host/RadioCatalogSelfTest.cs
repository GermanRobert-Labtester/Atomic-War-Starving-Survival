// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Godot;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Self-test runner for AF-B1 / Plan 60 (--radio-catalog-selftest).
    /// Enforces JSON station authority, schedule slot queries, typed signal strength,
    /// unknown ID preservation, and zero hardcoded station definitions in Core.
    /// </summary>
    public static class RadioCatalogSelfTest
    {
        public static int Run(string dataDirectory)
        {
            GD.Print("── RADIO CATALOG SELF-TEST (AF-B1 / Plan 60) ──");
            string actualDataDir = dataDirectory ?? string.Empty;
            string stationsFile = Path.Combine(actualDataDir, RadioStationCatalogLoader.StationsFileName);
            if (!File.Exists(stationsFile))
            {
                actualDataDir = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
                stationsFile = Path.Combine(actualDataDir, RadioStationCatalogLoader.StationsFileName);
            }

            int failures = 0;

            // 1. File existence
            if (!File.Exists(stationsFile))
            {
                GD.PrintErr($"[FAIL] Missing authoritative file: {stationsFile}");
                return 1;
            }
            GD.Print($"[PASS] Found authoritative stations file: {stationsFile}");

            // 2. Load via RadioStationCatalogLoader
            var catalog = new RadioStationCatalog();
            int loaded = 0;
            try
            {
                loaded = RadioStationCatalogLoader.LoadAndRegister(catalog, actualDataDir);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception during RadioStationCatalogLoader execution: {ex.Message}");
                return 1;
            }

            if (loaded < 6)
            {
                GD.PrintErr($"[FAIL] Expected at least 6 canonical stations, loaded {loaded}");
                failures++;
            }
            else
            {
                GD.Print($"[PASS] Loaded {loaded} stations from JSON.");
            }

            // 3. Verify canonical station IDs
            string[] canonicalIds = new[]
            {
                RadioStationCatalog.StationCivilDefense,
                RadioStationCatalog.StationGarrisonOverlord,
                RadioStationCatalog.StationVitrifiedCrater,
                RadioStationCatalog.StationOpenClassroom,
                RadioStationCatalog.StationNumbersSigint,
                RadioStationCatalog.StationAutomatedRelay
            };

            foreach (var id in canonicalIds)
            {
                var st = catalog.GetStation(id);
                if (st == null)
                {
                    GD.PrintErr($"[FAIL] Canonical station '{id}' missing from catalog.");
                    failures++;
                    continue;
                }

                if (st.FrequencyMhz < 0.1f || st.FrequencyMhz > 1000.0f)
                {
                    GD.PrintErr($"[FAIL] Station '{id}' has invalid frequency {st.FrequencyMhz} MHz.");
                    failures++;
                }

                if (st.Schedule == null || st.Schedule.Count == 0)
                {
                    GD.PrintErr($"[FAIL] Station '{id}' has no schedule slots defined.");
                    failures++;
                }
            }

            // 4. Schedule Slot Queries (GetCurrentSlot / GetNextSlot)
            var cd = catalog.GetStation(RadioStationCatalog.StationCivilDefense);
            if (cd != null)
            {
                var morningSlot = cd.GetCurrentSlot(campaignDay: 1, hour: 8);
                if (morningSlot == null || morningSlot.ProgramType != "CivilianNews")
                {
                    GD.PrintErr($"[FAIL] Civil Defense 08:00 slot expected CivilianNews, got: {morningSlot?.ProgramType ?? "null"}");
                    failures++;
                }
                else
                {
                    GD.Print("[PASS] Civil Defense current slot query resolved correctly.");
                }

                var nextSlot = cd.GetNextSlot(campaignDay: 1, hour: 8);
                if (nextSlot == null || nextSlot.SlotId == morningSlot?.SlotId)
                {
                    GD.PrintErr($"[FAIL] Civil Defense next slot query failed or returned identical slot: {nextSlot?.SlotId ?? "null"}");
                    failures++;
                }
                else
                {
                    GD.Print($"[PASS] Civil Defense next slot query resolved to {nextSlot.SlotId}.");
                }
            }

            // 5. Signal Strength & Degradation Reasons
            var factors = new RadioReceptionFactors
            {
                DistanceKm = 120f,
                WeatherAttenuation01 = 0.5f,
                IsBrownout = true,
                ReceiverCondition01 = 0.6f,
                IsJammed = true
            };
            var signal = catalog.ComputeSignalStrength(RadioStationCatalog.StationCivilDefense, factors);
            if (signal.Reasons.Count < 5)
            {
                GD.PrintErr($"[FAIL] Expected at least 5 degradation reasons, got {signal.Reasons.Count}: [{string.Join(", ", signal.Reasons)}]");
                failures++;
            }
            else
            {
                GD.Print($"[PASS] Computed signal strength: {signal.QualityBand} ({signal.EffectiveStrength01:F2}) with reasons: {string.Join(", ", signal.Reasons)}");
            }

            // 6. Unknown ID Preservation on Overrides Roundtrip
            catalog.ResetOverrides();
            catalog.SetStationState("station_mod_expansion_unknown", RadioStationState.Silent);
            var exported = catalog.ExportOverrides();
            if (!exported.ContainsKey("station_mod_expansion_unknown"))
            {
                GD.PrintErr("[FAIL] ExportOverrides dropped unknown station ID.");
                failures++;
            }
            else
            {
                var catalog2 = new RadioStationCatalog();
                catalog2.ImportOverrides(exported);
                if (catalog2.GetStationState("station_mod_expansion_unknown") != RadioStationState.Silent)
                {
                    GD.PrintErr("[FAIL] ImportOverrides did not preserve unknown station state.");
                    failures++;
                }
                else
                {
                    GD.Print("[PASS] Unknown station state override preserved across export/import.");
                }
            }

            // 7. Core Source Gate: No Hardcoded Station Defaults
            string coreFile = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "Ashfall.Core", "Radio", "RadioStationCatalog.cs");
            if (File.Exists(coreFile))
            {
                string text = File.ReadAllText(coreFile);
                if (text.Contains("RegisterDefaults") || text.Contains("Central Civil Defense Radio"))
                {
                    GD.PrintErr("[FAIL] Hardcoded station definitions detected in RadioStationCatalog.cs");
                    failures++;
                }
                else
                {
                    GD.Print("[PASS] Core source gate verified: 0 hardcoded station definitions.");
                }
            }

            if (failures == 0)
            {
                GD.Print("── RADIO_CATALOG_SELFTEST: PASS (All checks clean) ──");
                return 0;
            }

            GD.PrintErr($"── RADIO_CATALOG_SELFTEST: FAILED with {failures} error(s) ──");
            return 1;
        }
    }
}
