// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Expansion 25 (The Iron Road — rail track maintenance).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Rail;

namespace AtomicWar.GodotApp
{
    public static class HostCliRailTrackMaintenance
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Rail Track Maintenance Self-Test (Expansion 25) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: locomotive weight table.
                if (RailTrackMaintenanceEngine.GetLocomotiveWeightTons(LocomotiveClass.ManualHandcar) == 2
                    && RailTrackMaintenanceEngine.GetLocomotiveWeightTons(LocomotiveClass.ArmoredBattleTrain) == 250)
                {
                    GD.Print("[PASS] Check 1: Locomotive weight table matches the authored classes.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 1: Locomotive weight table mismatch.");

                // Check 2: pristine segment passes a light train.
                var pristine = new TrackSegmentState { SegmentId = "seg_a" };
                var r2 = RailTrackMaintenanceEngine.EvaluateTrackFeasibility(pristine, LocomotiveClass.LightSteamShunter, 10);
                if (r2.Outcome == RailFeasibilityOutcome.Passable && r2.DerailmentRiskPermille < RailTrackMaintenanceEngine.DerailmentThresholdPermille)
                {
                    GD.Print($"[PASS] Check 2: Pristine segment is passable (risk {r2.DerailmentRiskPermille}\u2030).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 2: Pristine segment unexpectedly refused (outcome {r2.Outcome}).");

                // Check 3: debris blocks the segment.
                var blocked = new TrackSegmentState { SegmentId = "seg_d", IsBlockedByDebris = true };
                if (RailTrackMaintenanceEngine.EvaluateTrackFeasibility(blocked, LocomotiveClass.ManualHandcar, 0).Outcome == RailFeasibilityOutcome.GaugeSpreadRefusal)
                {
                    GD.Print("[PASS] Check 3: Debris-blocked segment is refused.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 3: Debris-blocked segment was not refused.");

                // Check 4: broken gauge is refused.
                var broken = new TrackSegmentState { SegmentId = "seg_b", GaugeStability = TrackGaugeStability.BrokenGaugeRefusal };
                if (RailTrackMaintenanceEngine.EvaluateTrackFeasibility(broken, LocomotiveClass.LightSteamShunter, 0).Outcome == RailFeasibilityOutcome.GaugeSpreadRefusal)
                {
                    GD.Print("[PASS] Check 4: Broken gauge is refused.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 4: Broken gauge was not refused.");

                // Check 5: bridge overload is refused.
                var lightBridge = new TrackSegmentState { SegmentId = "seg_c", MaxBridgeLoadTons = 50, BridgeIntegrityPermille = 1000 };
                if (RailTrackMaintenanceEngine.EvaluateTrackFeasibility(lightBridge, LocomotiveClass.DieselFreightRig, 40).Outcome == RailFeasibilityOutcome.BridgeLoadRefusal)
                {
                    GD.Print("[PASS] Check 5: Bridge overload is refused.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 5: Bridge overload was not refused.");

                // Check 6: a run wears track and fatigues the bridge.
                var worn = new TrackSegmentState { SegmentId = "seg_e", TrackWearPermille = 100, BridgeIntegrityPermille = 1000 };
                RailTrackMaintenanceEngine.ApplyTrainWear(worn, LocomotiveClass.DieselFreightRig, 60);
                if (worn.TrackWearPermille > 100 && worn.BridgeIntegrityPermille < 1000)
                {
                    GD.Print($"[PASS] Check 6: Run wear {worn.TrackWearPermille}\u2030, bridge {worn.BridgeIntegrityPermille}\u2030.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 6: Run wear not applied (wear {worn.TrackWearPermille}, bridge {worn.BridgeIntegrityPermille}).");

                // Check 7: heavy wear induces gauge spread.
                var spread = new TrackSegmentState { SegmentId = "seg_f", TrackWearPermille = 790 };
                RailTrackMaintenanceEngine.ApplyTrainWear(spread, LocomotiveClass.ArmoredBattleTrain, 0);
                if (spread.GaugeStability != TrackGaugeStability.PristineStandard)
                {
                    GD.Print($"[PASS] Check 7: Heavy wear induced '{spread.GaugeStability}'.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 7: Heavy wear did not induce gauge spread.");

                // Check 8: workgang maintenance repairs gauge/wear/bridge and unblocks.
                var repairable = new TrackSegmentState
                {
                    SegmentId = "seg_g",
                    TrackWearPermille = 900,
                    BridgeIntegrityPermille = 400,
                    GaugeStability = TrackGaugeStability.SevereDistortion,
                    IsBlockedByDebris = true
                };
                RailTrackMaintenanceEngine.PerformMaintenance(repairable, 1000, 100);
                if (repairable.TrackWearPermille < 900 && repairable.BridgeIntegrityPermille > 400
                    && !repairable.IsBlockedByDebris && repairable.GaugeStability == TrackGaugeStability.MinorSpread)
                {
                    GD.Print($"[PASS] Check 8: Maintenance restored segment (wear {repairable.TrackWearPermille}, gauge {repairable.GaugeStability}).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 8: Maintenance did not restore (wear {repairable.TrackWearPermille}, gauge {repairable.GaugeStability}).");

                // Check 9: ledger seeds from topology and reports a census.
                var ledger = new RailTrackMaintenanceLedger();
                int seeded = ledger.SeedFromTopology(new[] { ("seg_1", true, 200f), ("seg_2", false, 120f) });
                var census = ledger.GetCensus();
                if (seeded == 2 && census.SegmentCount == 2 && census.BlockedSegments == 0)
                {
                    GD.Print($"[PASS] Check 9: Ledger seeded {seeded} segments ({census.SegmentCount} tracked).");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 9: Ledger seeding incorrect (seeded {seeded}, tracked {census.SegmentCount}).");

                // Check 10: ledger run + maintenance + capture/restore round-trip.
                ledger.ApplyRun("seg_1", LocomotiveClass.ArmoredBattleTrain, 100);
                var state = ledger.CaptureState();
                var restored = new RailTrackMaintenanceLedger();
                restored.RestoreState(state);
                bool roundTrip = restored.TryGetSegment("seg_1", out var seg)
                    && seg.TrackWearPermille == ledger.Segments["seg_1"].TrackWearPermille;
                if (roundTrip)
                {
                    GD.Print("[PASS] Check 10: Ledger capture/restore round-trips wear.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 10: Ledger round-trip lost wear state.");

                // Check 11: schema gate refuses a newer payload and accepts legacy.
                bool newerRejected = false;
                try
                {
                    var newer = ledger.CaptureState();
                    newer.SchemaVersion = 99;
                    restored.RestoreState(newer);
                }
                catch (InvalidOperationException) { newerRejected = true; }
                bool legacyAccepted = true;
                try
                {
                    var legacy = ledger.CaptureState();
                    legacy.SchemaVersion = 0;
                    restored.RestoreState(legacy);
                }
                catch (Exception) { legacyAccepted = false; }
                if (newerRejected && legacyAccepted)
                {
                    GD.Print("[PASS] Check 11: Schema gate rejects newer and accepts legacy v1.");
                    passed++;
                }
                else GD.PrintErr($"[FAIL] Check 11: Schema gate incorrect (newerRejected={newerRejected}, legacyAccepted={legacyAccepted}).");

                // Check 12: host wiring — dispatch hook + save section.
                string rail = ReadRepoFile("src", "Main.Plans190_193.cs");
                string main = ReadRepoFile("src", "Main.RailTrackMaintenance.cs");
                string registry = ReadRepoFile("Assets", "Ashfall.Core", "Save", "SaveSectionRegistry.cs");
                if (rail.Contains("RecordRailRunFromTrain(_railway, trainId, segmentId)")
                    && main.Contains("SeedRailMaintenanceFromTopology")
                    && registry.Contains("rail_track_maintenance")
                    && registry.Contains("rail_track_maintenance_save.json"))
                {
                    GD.Print("[PASS] Check 12: Host records run wear on dispatch and registers the maintenance section.");
                    passed++;
                }
                else GD.PrintErr("[FAIL] Check 12: Rail maintenance host wiring or save section missing.");
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during rail maintenance self-test: {ex}");
            }

            GD.Print($"=== Rail Track Maintenance Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static string ReadRepoFile(params string[] parts)
        {
            try
            {
                string dir = AppContext.BaseDirectory;
                for (int i = 0; i < 8 && dir != null; i++)
                {
                    string candidate = Path.Combine(dir, Path.Combine(parts));
                    if (File.Exists(candidate)) return File.ReadAllText(candidate);
                    dir = Directory.GetParent(dir)?.FullName ?? string.Empty;
                }
            }
            catch (Exception) { }
            return string.Empty;
        }
    }
}
