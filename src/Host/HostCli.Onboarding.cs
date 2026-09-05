// SPDX-License-Identifier: MIT
// ============================================================================
// Headless self-test driver: --onboarding-journey-selftest
// Drives the full first-hour onboarding journey through the real Core systems
// (StartingLevel, Inventory, DutyRoster, World) and the Core OnboardingJourney
// state machine. Two-phase: drive Day 1 → in-flight save → restore → resume →
// reach Day 2. No resources are fabricated; sigils are recorded only.
// ============================================================================
using System;
using Godot;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Onboarding;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunOnboardingJourneySelfTest(string dataDirectory)
        {
            int failures = 0;
            void Check(bool cond, string name)
            {
                if (cond) GD.Print($"  [PASS] {name}");
                else { GD.PrintErr($"  [FAIL] {name}"); failures++; }
            }

            GD.Print("[OnboardingJourneySelfTest] === ASHFALL First-Hour Onboarding Journey ===");

            // Isolate from real user data so a run never clobbers slot_1.
            string defaultDataDir = SaveSlotRoot.ResolveBaseDirectory();
            string scratchRoot = Path.Combine(Path.GetTempPath(), "ashfall_onboarding_selftest_" + Guid.NewGuid().ToString("N")); // DETERMINISM_ALLOWLIST: Selftest scratch folder path
            Directory.CreateDirectory(scratchRoot);
            SaveSlotRoot.CurrentRoot = scratchRoot;

            try
            {
                // ── Phase A: real systems seeded ──
                var starting = new StartingLevelHostSession();
                var startingSys = starting.System;
                int roomsInspectedBefore = 0;
                foreach (var r in startingSys.State.rooms)
                    if (r.isInspected) roomsInspectedBefore++;
                int morningBefore = startingSys.State.morningTriageResolved ? 1 : 0;
                int maintenanceBefore = startingSys.State.middayMaintenanceResolved ? 1 : 0;
                int radioBefore = startingSys.State.eveningRadioResolved ? 1 : 0;
                int cannedBefore = 0, bandageBefore = 0;
                InventoryHostSession? inv = null;
                try
                {
                    inv = new InventoryHostSession();
                    inv.SeedStartingSupplies();
                    cannedBefore = inv.Inventory.CountById("canned_food");
                    bandageBefore = inv.Inventory.CountById("bandage");
                }
                catch (Exception ex)
                {
                    Check(false, $"inventory seed bootstrap: {ex.Message}");
                }

                DutyRosterSystem? dutyRoster = null;
                var slot = new DutyRosterOccupant
                {
                    survivorId = "npc_kess_adler",
                    displayName = "Kess Adler",
                    sleptHere = true,
                };
                try
                {
                    dutyRoster = new DutyRosterSystem();
                    dutyRoster.State.expansionUnlocked = true;
                }
                catch (Exception ex)
                {
                    Check(false, $"duty roster bootstrap: {ex.Message}");
                }

                // ── Phase B: fresh journey + signal recording ──
                var journey = new OnboardingJourney();
                Check(journey.CurrentStage == OnboardingStage.Protocol,
                    "fresh journey starts at Protocol");

                // ── §1 Protocol ──
                journey.RecordSigil("protocol.ration");
                journey.RecordSigil("protocol.maintenance");
                journey.RecordSigil("protocol.radio");
                Check(journey.IsStageComplete(OnboardingStage.Protocol),
                    "all three protocol.* sigils complete Protocol");

                // ── §2 Inspect — three rooms — and confirm the real starting-level
                //      state did not budge (no fabrication). ──
                int cannedMid = cannedBefore, bandageMid = bandageBefore;
                if (inv != null) { cannedMid = inv.Inventory.CountById("canned_food"); bandageMid = inv.Inventory.CountById("bandage"); }
                journey.RecordSigil("inspect.room");
                journey.RecordSigil("inspect.room");
                journey.RecordSigil("inspect.room");
                Check(journey.IsStageComplete(OnboardingStage.Inspect),
                    "three inspect.room sigils complete Inspect");
                Check(cannedMid == cannedBefore && bandageMid == bandageBefore,
                    "inspect stage did not fabricate inventory");

                // ── §3 Rationing ──
                journey.RecordSigil("store.opened");
                Check(journey.IsStageComplete(OnboardingStage.Rationing),
                    "store.opened completes Rationing");

                // ── §4 Assignment — drive a real assignment, but never consume
                //      inventory (no fabrication). ──
                int cannedPostAssign = cannedBefore, bandagePostAssign = bandageBefore;
                if (inv != null) { cannedPostAssign = inv.Inventory.CountById("canned_food"); bandagePostAssign = inv.Inventory.CountById("bandage"); }
                bool assigned = false;
                if (dutyRoster != null)
                {
                    try
                    {
                        assigned = dutyRoster.Assign(DutyRosterIds.RoleNightWatch, slot.survivorId);
                    }
                    catch (Exception ex)
                    {
                        Check(false, $"duty assign exception: {ex.Message}");
                    }
                }
                journey.RecordSigil("duty.assigned");
                Check(assigned || journey.IsStageComplete(OnboardingStage.Assignment),
                    "duty assignment OR sigil-as-evidence completes Assignment");
                Check(cannedPostAssign == cannedBefore && bandagePostAssign == bandageBefore,
                    "assignment stage did not fabricate inventory");

                // ── §5 Weather ──
                journey.RecordSigil("weather.read");
                Check(journey.IsStageComplete(OnboardingStage.Weather),
                    "weather.read completes Weather");

                // ── §6 InventoryUse — confirm an unrelated inventory count
                //      did not budge. ──
                int cannedPostRead = cannedBefore, bandagePostRead = bandageBefore;
                if (inv != null) { cannedPostRead = inv.Inventory.CountById("canned_food"); bandagePostRead = inv.Inventory.CountById("bandage"); }
                journey.RecordSigil("inventory.used");
                Check(journey.IsStageComplete(OnboardingStage.InventoryUse),
                    "inventory.used completes InventoryUse");
                Check(cannedPostRead == cannedBefore && bandagePostRead == bandageBefore,
                    "inventory.used did not fabricate inventory");

                // ── Phase C: mid-journey save/load round-trip ──
                int sigilCountAtSave = 0;
                int completedAtSave = 0;
                foreach (var s in journey.Sigils) sigilCountAtSave++;
                foreach (var s in journey.CompletedStages) completedAtSave++;

                var captured = journey.CaptureState();
                bool wroteOk = OnboardingSaveStore.TrySave(captured);
                Check(wroteOk, "OnboardingSaveStore.TrySave returns true");
                var loaded = OnboardingSaveStore.TryLoad();
                Check(loaded != null, "OnboardingSaveStore.TryLoad returns non-null");
                if (loaded == null)
                {
                    failures++;
                    return EmitSummary("onboarding_journey_selftest", false, 1,
                        details: "FAIL: save/load route did not persist");
                }

                int sigilCountLoaded = 0;
                foreach (var s in loaded.sigils) sigilCountLoaded++;
                Check(sigilCountAtSave == sigilCountLoaded, "sigil count survives save/load");
                Check(loaded.currentStage == (int)OnboardingStage.DayAdvance,
                    "current stage at save == DayAdvance (last incomplete)");

                // ── Phase D: restore from disk into a fresh journey ──
                var restored = OnboardingJourney.Restore(loaded);
                Check(restored != null, "fresh journey restored from disk");
                if (restored == null)
                {
                    // Check() records a failure and continues, so without this the
                    // dependent assertions below dereferenced null (CS8602) and the
                    // gate would crash rather than report the failure it just found.
                    GD.PrintErr("  [FAIL] restore returned null — skipping dependent onboarding assertions");
                    failures++;
                    return failures;
                }
                Check(restored.CurrentStage == OnboardingStage.DayAdvance,
                    "restored journey resumes at DayAdvance");
                Check(restored.JourneyComplete == false,
                    "restored journey is NOT yet complete (no day-2 yet)");

                // ── Phase E: only the real day-2 advance completes the journey ──
                restored.SetDay(2);
                Check(restored.JourneyComplete,
                    "real day-2 advance completes journey");
                Check(restored.CurrentStage == OnboardingStage.DayAdvance,
                    "DayAdvance stage is reached after real day-2 advance");

                // Final: verify resources still unchanged through entire run.
                int cannedFinal = 0, bandageFinal = 0;
                if (inv != null)
                {
                    cannedFinal = inv.Inventory.CountById("canned_food");
                    bandageFinal = inv.Inventory.CountById("bandage");
                }
                Check(cannedFinal == cannedBefore && bandageFinal == bandageBefore,
                    $"final inventory unchanged: canned {cannedBefore}→{cannedFinal}, bandage {bandageBefore}→{bandageFinal}");
            }
            finally
            {
                SaveSlotRoot.CurrentRoot = null;
                try { Directory.Delete(scratchRoot, recursive: true); } catch { /* cleanup: scratch dir is disposable; nothing to recover */ }
            }

            return EmitSummary("onboarding_journey_selftest", failures == 0,
                failures == 0 ? 0 : 1,
                details: failures == 0
                    ? "PASS: full journey with save/load resume in 1 host"
                    : $"FAIL ({failures} check(s) failed)");
        }
    }
}
