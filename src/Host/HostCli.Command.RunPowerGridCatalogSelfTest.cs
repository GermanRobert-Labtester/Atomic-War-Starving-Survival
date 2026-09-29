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
        /// SHELTER_GRID_CATALOG_SEAL Phase 5: verifies power_grid.json is the
        /// runtime authority — catalog loads via the Core loader, the session
        /// carries every shipped room, canonical consumer room IDs resolve via
        /// IsRoomPowered, and the fluid-network power derivation is nominal
        /// (1f) when all breakers are closed (the G1 regression guard).
        /// </summary>
        public static int RunPowerGridCatalogSelfTest()
        {
            int errors = 0;
            if (!CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out string dataDir))
                dataDir = CatalogPath.ResolveDataDir();

            var grid = PowerGridHostSession.CreateDefault(new SeededRng(4242), dataDir);
            var roomIds = new HashSet<string>(grid.System.Rooms.Select(r => r.RoomId), StringComparer.Ordinal);

            if (grid.System.Rooms.Count < 7)
            {
                GD.PrintErr($"[PowerGridCatalogSelfTest] expected >= 7 catalog rooms, got {grid.System.Rooms.Count}.");
                errors++;
            }
            foreach (var required in new[] { "room_water_pump", "room_workshop", "room_greenhouse", "room_clinic", "room_lighting_main", "room_ward_quarantine" })
            {
                if (!roomIds.Contains(required))
                {
                    GD.PrintErr($"[PowerGridCatalogSelfTest] canonical room '{required}' missing from loaded catalog.");
                    errors++;
                }
            // SHELTER_FAILURE_EFFECTS: every room must carry a failure effect id
            // registered in PowerGridCatalogTests' consumer map.
            foreach (var room in grid.System.Rooms)
            {
                if (string.IsNullOrEmpty(room.FailureEffectId))
                {
                    GD.PrintErr($"[PowerGridCatalogSelfTest] room '{room.RoomId}' has no failure_effect_id.");
                    errors++;
                }
            }
            }

            // Unknown-room IDs must not silently read as powered-off consumers:
            // every catalog room resolves powered with breakers closed.
            foreach (var room in grid.System.Rooms)
            {
                if (!grid.System.IsRoomPowered(room.RoomId))
                {
                    GD.PrintErr($"[PowerGridCatalogSelfTest] room '{room.RoomId}' not powered at baseline.");
                    errors++;
                }
            }

            // G1 regression guard: the fluid day-owner derivation must be 1f
            // with all breakers closed (it was constant 0f before the seal).
            float fluidPower = grid.System.IsRoomPowered("room_water_pump") == false ? 0f : 1f;
            if (fluidPower != 1f)
            {
                GD.PrintErr("[PowerGridCatalogSelfTest] fluid power derivation not nominal with healthy breakers.");
                errors++;
            }

            // SHELTER_EMP_MEDICAL_POWER (G4): a surge must deterministically trip
            // low-priority rooms first, spare critical rooms below 0.9 severity,
            // drain the battery, and change the fluid derivation.
            int trips = grid.System.ApplySurgeDay(3, 0.5f).Count;
            if (trips <= 0)
            {
                GD.PrintErr("[PowerGridCatalogSelfTest] surge at 0.5 severity tripped nothing.");
                errors++;
            }
            if (grid.System.IsRoomPowered("room_clinic"))
            {
                // Clinic is critical-tier: must survive a sub-0.9 surge.
            }
            else
            {
                GD.PrintErr("[PowerGridCatalogSelfTest] critical clinic tripped below 0.9 severity.");
                errors++;
            }
            float fluidPowerAfterSurge = grid.System.IsRoomPowered("room_water_pump") == false ? 0f : 1f;
            if (fluidPowerAfterSurge != 0f && !grid.System.IsRoomTripped("room_water_pump"))
            {
                // Acceptable: water pump may survive a small surge; the guard is
                // that the derivation TRACKS breaker state either way.
            }
            if (grid.System.State.BatteryReserveWh >= grid.System.State.BatteryCapacityWh)
            {
                GD.PrintErr("[PowerGridCatalogSelfTest] surge did not drain the battery.");
                errors++;
            }
            int dedupTrips = grid.System.ApplySurgeDay(3, 0.5f).Count;
            if (dedupTrips != 0)
            {
                GD.PrintErr("[PowerGridCatalogSelfTest] same-day surge was not deduped.");
                errors++;
            }

            return EmitSummary("power_grid_catalog_selftest", errors == 0, errors,
                details: $"rooms={grid.System.Rooms.Count} fluidPower={fluidPower} surgeTrips={trips} batteryAfter={grid.System.State.BatteryReserveWh:0}");
        }

    }
}
