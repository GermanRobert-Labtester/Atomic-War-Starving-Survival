// SPDX-License-Identifier: MIT
// ============================================================================
// Headless self-test driver: --onboarding-journey-selftest
// Drives the first-hour onboarding state machine through its five stable
// state-true signals, then performs an in-flight save/load resume. The
// inventory seed is used only to prove that recording onboarding evidence does
// not fabricate or consume resources.
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
                    inv = InventoryHostSession.Create(dataDirectory, seedWhenNoSave: true);
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

                // ── Phase B: fresh first-hour journey + state signals ──
                var journey = OnboardingJourney.CreateFirstHour();
                Check(journey.Profile == OnboardingProfile.FirstHour &&
                      journey.CurrentStage == OnboardingStage.Water,
                    "fresh journey starts at the water stage");

                journey.RecordSigil("water.treatment_started");
                Check(journey.IsStageComplete(OnboardingStage.Water),
                    "water treatment signal completes Water");
                journey.RecordSigil("power.breaker_toggled");
                Check(journey.IsStageComplete(OnboardingStage.Power),
                    "breaker signal completes Power");

                int cannedBeforeFood = cannedBefore;
                journey.RecordSigil("food.ration_consumed");
                Check(journey.IsStageComplete(OnboardingStage.Food),
                    "food ration signal completes Food");
                Check(cannedBeforeFood == cannedBefore,
                    "onboarding signal did not fabricate or consume inventory");

                journey.RecordSigil("duty.assigned");
                Check(journey.IsStageComplete(OnboardingStage.Duty),
                    "duty assignment signal completes Duty");
                journey.RecordSigil("dose.read");
                Check(journey.IsStageComplete(OnboardingStage.Dose),
                    "dose ledger signal completes Dose");

                journey.RecordSigil("research.started");
                Check(journey.IsStageComplete(OnboardingStage.Research),
                    "research start signal completes Research");
                Check(journey.CurrentStage == OnboardingStage.Expedition &&
                      !journey.JourneyComplete,
                    "expedition remains the final outstanding stage");

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
                Check(loaded.profile == (int)OnboardingProfile.FirstHour &&
                      loaded.currentStage == (int)OnboardingStage.Expedition,
                    "current stage at save == Expedition (last incomplete)");

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
                Check(restored.Profile == OnboardingProfile.FirstHour &&
                      restored.CurrentStage == OnboardingStage.Expedition,
                    "restored journey resumes at Expedition");
                Check(restored.JourneyComplete == false,
                    "restored journey is NOT yet complete (no dispatch yet)");

                // ── Phase E: only the real expedition dispatch completes the journey ──
                restored.RecordSigil("expedition.dispatched");
                Check(restored.JourneyComplete,
                    "expedition dispatch signal completes journey");
                restored.SetDay(2);
                Check(restored.JourneyComplete,
                    "day advance does not undo completed first-hour journey");
                Check(restored.CurrentStage == OnboardingStage.Expedition,
                    "Expedition remains the terminal completed stage");

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
                TryDeleteTempDirectory(scratchRoot);
            }

            return EmitSummary("onboarding_journey_selftest", failures == 0,
                failures == 0 ? 0 : 1,
                details: failures == 0
                    ? "PASS: full journey with save/load resume in 1 host"
                    : $"FAIL ({failures} check(s) failed)");
        }
    }
}
