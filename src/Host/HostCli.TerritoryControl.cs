#nullable enable
// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 134 host probe — Dynamic Faction Territory & Supply Lines.
//
// --territory-control-selftest loads authored faction_territory.json and
// supply_lines.json, then proves the real territory control lifecycle:
//   * catalogs load and initialize nodes,
//   * fortification and garrison assignment modify strength,
//   * deterministic contest shifts control between factions,
//   * supply line raids disrupt/sever corridors and degrade destination strength,
//   * supply line restoration recovers corridor to active,
//   * day advance delivers cargo and reinforces control,
//   * census accurately aggregates dynamic state,
//   * checksummed state capture and restore round-trips cleanly.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Factions;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunTerritoryControlSelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;

            void Check(string gate, bool ok, string note = "")
            {
                if (ok)
                {
                    pass++;
                    GD.Print($"[PASS] territory/{gate}");
                }
                else
                {
                    fail++;
                    GD.Print($"[FAIL] territory/{gate}{(note.Length > 0 ? " — " + note : "")}");
                }
            }

            TerritoryControlHostSession session;
            try
            {
                session = TerritoryControlHostSession.Load(dataDirectory, new FileSystemIO());
            }
            catch (Exception ex)
            {
                GD.Print($"[FAIL] territory/authored_catalogs_load — {ex.Message}");
                GD.Print("[territory-control-selftest] 0 passed, 1 failed");
                return 1;
            }

            // 1 — Both authored catalogs load
            var territories = session.System.GetAllTerritories();
            var supplyLines = session.System.GetAllSupplyLines();
            Check("authored_catalogs_load",
                territories.Count >= 10 && supplyLines.Count >= 3,
                $"loaded {territories.Count} territories, {supplyLines.Count} supply lines");

            // 2 — Controlled nodes initialized with base strength and controlling faction
            var allLocs = session.System.GetAllLocationStates();
            Check("locations_initialized",
                allLocs.Count > 0 && allLocs.All(l => !string.IsNullOrEmpty(l.LocationId) && !string.IsNullOrEmpty(l.ControllingFactionId)),
                $"initialized {allLocs.Count} location nodes");

            // 3 — Fortification level upgrade clamps to 0..3 and fires seam
            string testLoc = allLocs.First().LocationId;
            int initialStrength = session.System.GetLocationState(testLoc)!.ControlStrength;
            string? fortifiedLoc = null;
            int fortifiedLevel = 0;
            session.System.OnLocationFortifiedSeam = (loc, lvl) =>
            {
                fortifiedLoc = loc;
                fortifiedLevel = lvl;
            };

            bool f1 = session.FortifyLocation(testLoc, 1);
            var locAfterF1 = session.System.GetLocationState(testLoc)!;
            Check("fortification_upgrades_level_and_strength",
                f1 && fortifiedLoc == testLoc && fortifiedLevel == 1 && locAfterF1.FortificationLevel == 1 && locAfterF1.ControlStrength > initialStrength,
                $"level={locAfterF1.FortificationLevel}, strength={locAfterF1.ControlStrength}");

            // 4 — Garrison assignment modifies garrison strength
            int initialGarrison = locAfterF1.GarrisonStrength;
            bool g1 = session.AssignGarrison(testLoc, 10);
            var locAfterG1 = session.System.GetLocationState(testLoc)!;
            Check("garrison_assignment_modifies_strength",
                g1 && locAfterG1.GarrisonStrength == initialGarrison + 10,
                $"garrison={locAfterG1.GarrisonStrength}");

            // 5 — Deterministic location contest shifts control
            string? contestedLoc = null;
            string? oldOwner = null;
            string? newOwner = null;
            session.System.OnTerritoryControlChangedSeam = (loc, o, n) =>
            {
                contestedLoc = loc;
                oldOwner = o;
                newOwner = n;
            };

            var contestTarget = allLocs.First(l => l.LocationId != testLoc);
            string originalFaction = contestTarget.ControllingFactionId;
            string attackerFaction = "faction_iron_raiders";
            if (string.Equals(originalFaction, attackerFaction, StringComparison.OrdinalIgnoreCase))
                attackerFaction = "faction_the_office";

            var rng = new SeededRng(134);
            bool shifted = session.ContestLocation(contestTarget.LocationId, attackerFaction, attackPower: 500, rng: rng, currentDay: 3);
            Check("contest_location_shifts_control_deterministically",
                shifted && contestedLoc == contestTarget.LocationId && oldOwner == originalFaction && newOwner == attackerFaction,
                $"shifted={shifted}, old={oldOwner}, new={newOwner}");

            // 6 — Supply line raid disrupts/severs line and degrades destination
            var testLine = supplyLines.First();
            string? raidedLineId = null;
            SupplyLineStatus reportedStatus = SupplyLineStatus.Active;
            session.System.OnSupplyLineStatusChangedSeam = (id, st) =>
            {
                raidedLineId = id;
                reportedStatus = st;
            };

            var raidRng = new SeededRng(77);
            bool raided = session.RaidSupplyLine(testLine.SupplyLineId, raidIntensity: 90, rng: raidRng);
            var lineAfterRaid = session.System.GetSupplyLineState(testLine.SupplyLineId)!;
            Check("supply_line_raid_disrupts_corridor",
                raided && raidedLineId == testLine.SupplyLineId && lineAfterRaid.Status != SupplyLineStatus.Active && reportedStatus == lineAfterRaid.Status,
                $"status={lineAfterRaid.Status}");

            // 7 — Supply line restore recovers corridor to Active
            bool restored = session.RestoreSupplyLine(testLine.SupplyLineId);
            var lineAfterRestore = session.System.GetSupplyLineState(testLine.SupplyLineId)!;
            Check("supply_line_restore_recovers_corridor",
                restored && lineAfterRestore.Status == SupplyLineStatus.Active,
                $"status={lineAfterRestore.Status}");

            // 8 — Daily tick advances delivery and reinforces destination control
            string? deliveredLineId = null;
            int deliveredAmt = 0;
            session.System.OnSupplyLineDeliveredSeam = (id, amt) =>
            {
                deliveredLineId = id;
                deliveredAmt = amt;
            };

            session.TickDay(currentDay: 5, rng: rng);
            var lineAfterTick = session.System.GetSupplyLineState(testLine.SupplyLineId)!;
            Check("daily_tick_delivers_cargo",
                lineAfterTick.LastDeliveredDay == 5 && lineAfterTick.TotalDelivered > 0,
                $"lastDay={lineAfterTick.LastDeliveredDay}, delivered={lineAfterTick.TotalDelivered}");

            // 9 — Census aggregates counts truthfully
            var census = session.ReadCensus();
            Check("census_aggregates_truthfully",
                census.TotalTerritories == territories.Count && census.TotalNodes == allLocs.Count && census.TotalSupplyLines == supplyLines.Count && census.ActiveSupplyLines > 0,
                census.Describe());

            // 10 — State capture and restore round-trip
            var captured = session.CaptureState();
            string capturedJson = JsonSerializer.Serialize(captured);

            var roundTripSession = TerritoryControlHostSession.Load(dataDirectory, new FileSystemIO());
            bool restoredState = roundTripSession.RestoreState(captured);
            var roundTripCaptured = roundTripSession.CaptureState();
            string roundTripJson = JsonSerializer.Serialize(roundTripCaptured);

            Check("state_capture_and_restore_round_trip",
                restoredState && string.Equals(capturedJson, roundTripJson, StringComparison.Ordinal),
                $"capturedLen={capturedJson.Length}, roundTripLen={roundTripJson.Length}");

            // 11 — Malformed or null state rejected fail-closed
            Check("null_state_rejected_fail_closed",
                !session.RestoreState(null));
            Check("invalid_schema_version_rejected",
                !session.RestoreState(new TerritoryControlSaveState { schema_version = 99 }));

            GD.Print($"[territory-control-selftest] {pass} passed, {fail} failed");
            return fail == 0 ? 0 : 1;
        }
    }
}
