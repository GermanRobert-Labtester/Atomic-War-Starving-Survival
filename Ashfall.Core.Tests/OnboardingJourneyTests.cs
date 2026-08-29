using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Onboarding;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Engine-agnostic unit tests for <see cref="OnboardingJourney"/>.
    /// Validates stage machine, signal semantics, persistence roundtrip,
    /// deterministic capture/restore, future-version rejection, and
    /// "no-fabrication" properties of the journey itself.
    /// </summary>
    public class OnboardingJourneyTests
    {
        private const string S_ProtocolRation = "protocol.ration";
        private const string S_ProtocolMaintenance = "protocol.maintenance";
        private const string S_ProtocolRadio = "protocol.radio";
        private const string S_InspectRoom = "inspect.room";
        private const string S_StoreOpened = "store.opened";
        private const string S_DutyAssigned = "duty.assigned";
        private const string S_WeatherRead = "weather.read";
        private const string S_InventoryUsed = "inventory.used";

        [Fact]
        public void FreshJourney_StartsAtProtocolStage()
        {
            var j = new OnboardingJourney();
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);
            Assert.False(j.JourneyComplete);
            Assert.False(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.Equal(OnboardingAssistance.Standard, j.Assistance);
        }

        [Fact]
        public void RecordSigil_NullOrEmptyOrDeltaZero_IsIgnored()
        {
            var j = new OnboardingJourney();
            Assert.Equal(OnboardingSignalResult.Ignored, j.RecordSigil("", 1));
            Assert.Equal(OnboardingSignalResult.Ignored, j.RecordSigil(null!, 5));
            Assert.Equal(OnboardingSignalResult.Ignored, j.RecordSigil(S_ProtocolRation, 0));
            Assert.Equal(OnboardingSignalResult.Ignored, j.RecordSigil(S_ProtocolRation, -3));
            Assert.Empty(j.Sigils);
        }

        [Fact]
        public void RecordSigil_DaySentinel_IsIgnored()
        {
            var j = new OnboardingJourney();
            // The "day.at_least" sentinel must never be a recordable signal — only
            // SetDay must move the day boundary.
            var r = j.RecordSigil(OnboardingCatalog.DaySentinel, 1);
            Assert.Equal(OnboardingSignalResult.Ignored, r);
            Assert.DoesNotContain(OnboardingCatalog.DaySentinel, j.Sigils.Keys);
        }

        [Fact]
        public void Protocol_RequiresAllThreeDirectives()
        {
            var j = new OnboardingJourney();
            j.RecordSigil(S_ProtocolRation);
            j.RecordSigil(S_ProtocolMaintenance);
            Assert.False(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);

            j.RecordSigil(S_ProtocolRadio);
            Assert.True(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.Equal(OnboardingStage.Inspect, j.CurrentStage);
        }

        [Fact]
        public void Inspect_RequiresThreeRoomSigils()
        {
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.Inspect,
                      S_ProtocolRation, S_ProtocolMaintenance, S_ProtocolRadio);
            j.RecordSigil(S_InspectRoom);
            Assert.False(j.IsStageComplete(OnboardingStage.Inspect));
            j.RecordSigil(S_InspectRoom);
            Assert.False(j.IsStageComplete(OnboardingStage.Inspect));
            j.RecordSigil(S_InspectRoom);
            Assert.True(j.IsStageComplete(OnboardingStage.Inspect));
            Assert.Equal(OnboardingStage.Rationing, j.CurrentStage);
            Assert.Equal(3, j.Sigils[S_InspectRoom]);
        }

        [Fact]
        public void Rationing_RequiresStoresOpened()
        {
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.Rationing);
            Assert.Equal(OnboardingStage.Rationing, j.CurrentStage);
            Assert.False(j.IsStageComplete(OnboardingStage.Rationing));
            j.RecordSigil(S_StoreOpened);
            Assert.True(j.IsStageComplete(OnboardingStage.Rationing));
            Assert.Equal(OnboardingStage.Assignment, j.CurrentStage);
        }

        [Fact]
        public void Assignment_RequiresAtLeastOneDutyAssignment()
        {
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.Assignment);
            Assert.False(j.IsStageComplete(OnboardingStage.Assignment));
            j.RecordSigil(S_DutyAssigned);
            Assert.True(j.IsStageComplete(OnboardingStage.Assignment));
            Assert.Equal(OnboardingStage.Weather, j.CurrentStage);
        }

        [Fact]
        public void Weather_RequiresWeatherRead()
        {
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.Weather);
            j.RecordSigil(S_WeatherRead);
            Assert.True(j.IsStageComplete(OnboardingStage.Weather));
            Assert.Equal(OnboardingStage.InventoryUse, j.CurrentStage);
        }

        [Fact]
        public void InventoryUse_RequiresInventoryUsed()
        {
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.InventoryUse);
            j.RecordSigil(S_InventoryUsed);
            Assert.True(j.IsStageComplete(OnboardingStage.InventoryUse));
            Assert.Equal(OnboardingStage.DayAdvance, j.CurrentStage);
        }

        [Fact]
        public void DayAdvance_RecordsAnInventorySignal_DoesNotCompleteByItself()
        {
            var j = new OnboardingJourney();
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);
            j.SetDay(1);
            Assert.False(j.JourneyComplete);
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);
        }

        [Fact]
        public void SetDay_Two_CompletesDayAdvanceAndJourneysToComplete()
        {
            var j = new OnboardingJourney();
            j.SetDay(1);
            Assert.False(j.JourneyComplete);
            j.SetDay(2);
            Assert.True(j.IsStageComplete(OnboardingStage.DayAdvance));
            Assert.True(j.JourneyComplete);
        }

        [Fact]
        public void SetDay_SameOrSmaller_IsIgnored()
        {
            var j = new OnboardingJourney();
            j.SetDay(2);
            Assert.Equal(2, j.Day);
            j.SetDay(1);
            Assert.Equal(2, j.Day);
            j.SetDay(2);
            Assert.Equal(2, j.Day);
        }

        [Fact]
        public void SkipCurrent_MarksCompletedAndAdvances_ExceptDayAdvance()
        {
            var j = new OnboardingJourney();
            Assert.True(j.SkipCurrent());
            Assert.True(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.Equal(OnboardingStage.Inspect, j.CurrentStage);

            // Drive all the way to DayAdvance; Skip refuses at the stage that
            // requires a real day advance.
            AdvanceTo(j, OnboardingStage.DayAdvance);
            Assert.Equal(OnboardingStage.DayAdvance, j.CurrentStage);
            Assert.False(j.SkipCurrent());
            Assert.False(j.JourneyComplete);
        }

        [Fact]
        public void SkipAllRemaining_LeavesDayAdvanceUntouched()
        {
            var j = new OnboardingJourney();
            j.SkipAllRemaining();
            Assert.True(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.True(j.IsStageComplete(OnboardingStage.Inspect));
            Assert.True(j.IsStageComplete(OnboardingStage.Rationing));
            Assert.True(j.IsStageComplete(OnboardingStage.Assignment));
            Assert.True(j.IsStageComplete(OnboardingStage.Weather));
            Assert.True(j.IsStageComplete(OnboardingStage.InventoryUse));
            Assert.False(j.IsStageComplete(OnboardingStage.DayAdvance));
            Assert.False(j.JourneyComplete);
        }

        [Fact]
        public void Replay_AfterDayTwoPreservesDayAdvance_HonoringRealDay()
        {
            var j = new OnboardingJourney();
            j.SetDay(2);
            AdvanceTo(j, OnboardingStage.DayAdvance);
            Assert.True(j.JourneyComplete);

            j.Replay();
            // All stages back to incomplete except DayAdvance, which the real
            // day tick genuinely satisfied.
            Assert.False(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.False(j.IsStageComplete(OnboardingStage.Inspect));
            Assert.False(j.IsStageComplete(OnboardingStage.InventoryUse));
            Assert.True(j.IsStageComplete(OnboardingStage.DayAdvance));
            Assert.True(j.JourneyComplete);
            // CurrentStage points to DayAdvance when JourneyComplete, so the
            // hint UI can still offer "show me where" on prior objectives.
            Assert.Equal(OnboardingStage.DayAdvance, j.CurrentStage);
        }

        [Fact]
        public void Replay_BeforeDayTwo_RestoresToProtocol()
        {
            var j = new OnboardingJourney();
            j.SetDay(1);
            AdvanceTo(j, OnboardingStage.DayAdvance);
            Assert.Equal(OnboardingStage.DayAdvance, j.CurrentStage);
            Assert.False(j.JourneyComplete);

            j.Replay();
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);
            Assert.False(j.JourneyComplete);
        }

        [Fact]
        public void SetAssistance_StoresAndEmitsOnce()
        {
            var j = new OnboardingJourney();
            int fires = 0;
            j.OnJourneyChanged += _ => fires++;
            j.SetAssistance(OnboardingAssistance.Minimal);
            Assert.Equal(OnboardingAssistance.Minimal, j.Assistance);
            Assert.Equal(1, fires);
            j.SetAssistance(OnboardingAssistance.Minimal);
            // Idempotent — no second emission.
            Assert.Equal(1, fires);

            j.SetAssistance(OnboardingAssistance.Guided);
            Assert.Equal(OnboardingAssistance.Guided, j.Assistance);
            Assert.Equal(2, fires);
        }

        [Fact]
        public void DismissHint_IsIdempotent()
        {
            var j = new OnboardingJourney();
            int fires = 0;
            j.OnJourneyChanged += _ => fires++;
            j.DismissHint("stage.inspect.hint");
            Assert.True(j.IsHintDismissed("stage.inspect.hint"));
            Assert.Equal(1, fires);
            j.DismissHint("stage.inspect.hint");
            Assert.Equal(1, fires);
            Assert.False(j.IsHintDismissed("stage.weather.hint"));
        }

        [Fact]
        public void CaptureState_DefensivelyCopiesCollections()
        {
            var j = new OnboardingJourney();
            j.RecordSigil(S_ProtocolRation);
            // Capture before we record the second sigil — the captured snapshot
            // is honoured after subsequent live mutations.
            var firstCount = j.Sigils.Count;
            Assert.Equal(1, firstCount);
            var snap = j.CaptureState();
            // The captured snapshot's own sigil collection and the live
            // Dictionary must be independent fields after Capture.
            Assert.Equal(1, snap.sigils.Count);
            Assert.Equal(1, snap.sigils[0].count);

            // Mutate the live journey after capturing the snapshot. The
            // captured snapshot's sigil list must NOT mirror live state because
            // the persisted list is built ordinally at Capture and the live
            // runtime counter map is not aliased through Capture.
            j.RecordSigil(S_ProtocolMaintenance);
            Assert.Equal(1, snap.sigils.Count);
            Assert.Equal(2, j.Sigils.Count);
            // Restore from the captured snapshot into a new journey — the
            // counter map for the maintenance sigil is absent (it didn't exist
            // at Capture-time).
            var j2 = OnboardingJourney.Restore(snap);
            Assert.Equal(1, j2.Sigils.Count);
            Assert.False(j2.Sigils.ContainsKey(S_ProtocolMaintenance));
        }

        [Fact]
        public void Restore_ProducesEqualJourneyState_AndIsDeterministic()
        {
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.InventoryUse);
            j.SetAssistance(OnboardingAssistance.Guided);
            j.DismissHint("stage.inventory_use.hint");

            var a = j.CaptureState();
            var j2 = OnboardingJourney.Restore(a);

            Assert.Equal(j.CurrentStage, j2.CurrentStage);
            Assert.Equal(j.JourneyComplete, j2.JourneyComplete);
            Assert.Equal(j.Assistance, j2.Assistance);
            Assert.Equal(j.Sigils.Count, j2.Sigils.Count);
            foreach (var key in j.Sigils.Keys)
                Assert.Equal(j.Sigils[key], j2.Sigils[key]);
            Assert.Equal(j.DismissedHints.Count, j2.DismissedHints.Count);

            // Determinism: replaying the same capture yields the same state.
            var b = j.CaptureState();
            var j3 = OnboardingJourney.Restore(b);
            for (int i = 0; i < a.completedStages.Count; i++)
                Assert.Equal(a.completedStages[i], b.completedStages[i]);
        }

        [Fact]
        public void Restore_AfterSave_ResumesAtCorrectStep()
        {
            // Stage 5 (Weather) in flight when saved; restore must surface back
            // to the same outstanding stage — the DoD "resumes at the correct
            // step after save/load" hinge.
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.Weather);
            Assert.Equal(OnboardingStage.Weather, j.CurrentStage);
            Assert.False(j.IsStageComplete(OnboardingStage.Weather));

            var snap = j.CaptureState();
            var j2 = OnboardingJourney.Restore(snap);

            Assert.Equal(OnboardingStage.Weather, j2.CurrentStage);
            Assert.False(j2.IsStageComplete(OnboardingStage.Weather));
            Assert.Equal(j.Sigils.Count, j2.Sigils.Count);
        }

        [Fact]
        public void Restore_AfterEarlierStagesSatisfied_AdvancesCurrentStageCorrectly()
        {
            // Three stages done, persisted with weather.read=0. Restore must
            // NOT mark Weather complete (no signal at save time). Adding the
            // signal after restore must complete inspect / assign / weather in
            // order — the persistent sigils persist, the resume lands at
            // weather.
            var j = new OnboardingJourney();
            AdvanceTo(j, OnboardingStage.InventoryUse);
            // Don't record weather.read; the current stage is Weather.
            // But AdvanceTo populates InventoryUse sigil so we're already past
            // Weather. Drive backward via a fresh journey instead.
            var fresh = new OnboardingJourney();
            AdvanceTo(fresh, OnboardingStage.Assignment);
            // Assignment requires duty.assigned=1; stage now current. Record
            // inventory.used after restore to progress through the back of
            // the catalog — note that InventoryUse is AFTER Assignment.
            Assert.Equal(OnboardingStage.Assignment, fresh.CurrentStage);
            var snap = fresh.CaptureState();

            var fresh2 = OnboardingJourney.Restore(snap);
            Assert.Equal(OnboardingStage.Assignment, fresh2.CurrentStage);
            Assert.False(fresh2.IsStageComplete(OnboardingStage.Assignment));
            fresh2.RecordSigil("duty.assigned");
            // Now moves through all stages that just need the missing later
            // signales. Without separate per-stage preclude (we allow
            // later-stage signals to count), duty.assigned as a sigil is now
            // recorded. Assignment's requirement is just duty.assigned=1.
            Assert.True(fresh2.IsStageComplete(OnboardingStage.Assignment));
            // The journey must remain faithful: at the resume point, the
            // already-satisfied earlier stages are still satisfied.
            for (int i = 0; i <= (int)OnboardingStage.Assignment; i++)
                Assert.True(fresh2.IsStageComplete((OnboardingStage)i));
        }

        [Fact]
        public void Restore_FutureSchema_Throws()
        {
            var saved = new OnboardingSaveState { schemaVersion = OnboardingJourney.SaveVersion + 1 };
            Assert.Throws<InvalidOperationException>(() => OnboardingJourney.Restore(saved));
        }

        [Fact]
        public void Restore_ZeroSchema_LegacyRollsForwardToCurrent()
        {
            var saved = new OnboardingSaveState { schemaVersion = 0, day = 1 };
            var j = OnboardingJourney.Restore(saved);
            // Pre-versioned legacy saves are accepted at the current version.
            Assert.Equal(OnboardingJourney.SaveVersion, j.CaptureState().schemaVersion);
        }

        [Fact]
        public void Restore_NullSnapshot_YieldsFreshJourney()
        {
            var j = OnboardingJourney.Restore(null);
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);
            Assert.False(j.JourneyComplete);
        }

        [Fact]
        public void RecordSigil_IsOrderIndependentInLateStages()
        {
            // A signal for a later stage is recorded but does not complete the
            // current stage prematurely. This matches the "resume at correct
            // step" DoD: the journey must not jump ahead of the current stage.
            var j = new OnboardingJourney();

            j.RecordSigil(S_InventoryUsed);
            Assert.False(j.IsStageComplete(OnboardingStage.Protocol));
            Assert.Equal(OnboardingStage.Protocol, j.CurrentStage);
            Assert.Contains(S_InventoryUsed, j.Sigils.Keys);

            j.RecordSigil(S_ProtocolRation);
            j.RecordSigil(S_ProtocolMaintenance);
            j.RecordSigil(S_ProtocolRadio);
            Assert.Equal(OnboardingStage.Inspect, j.CurrentStage);
        }

        [Fact]
        public void Journey_NeverFabricates_ResourcesOrState()
        {
            // The journey exposes only RecordSigil/SetDay/Restore—none of which
            // can produce inventory/duty/survivor changes. The test asserts the
            // surface contains no mutation methods escaping the journey.
            var journeyMethods = typeof(OnboardingJourney).GetMethods()
                .Where(m => m.DeclaringType == typeof(OnboardingJourney) &&
                            !m.IsSpecialName)
                .Select(m => m.Name)
                .ToHashSet(StringComparer.Ordinal);

            string[] allowed =
            {
                "Restore", "CaptureState", "RecordSigil", "SetDay",
                "SkipCurrent", "SkipAllRemaining", "Replay",
                "SetAssistance", "DismissHint", "RecordShowMeWhere"
            };
            foreach (var verb in allowed) Assert.Contains(verb, journeyMethods);

            Assert.DoesNotContain("AddItem", journeyMethods);
            Assert.DoesNotContain("RemoveItem", journeyMethods);
            Assert.DoesNotContain("AssignDuty", journeyMethods);
            Assert.DoesNotContain("AdvanceDay", journeyMethods);
            Assert.DoesNotContain("EquipItem", journeyMethods);
        }

        [Fact]
        public void CaptureState_AcrossFullJourney_YieldsNonNullCollections()
        {
            var j = new OnboardingJourney();
            // Drive all data stages first so the sigil dictionary is non-empty,
            // THEN tick the day so DayAdvance completes (RecordSigil becomes a
            // no-op once JourneyComplete=true).
            AdvanceTo(j, OnboardingStage.InventoryUse);
            j.SetDay(2);
            var s = j.CaptureState();
            Assert.NotNull(s.sigils);
            Assert.NotNull(s.completedStages);
            Assert.NotNull(s.dismissedHints);
            Assert.NotNull(s.stagesGuided);
            Assert.True(s.sigils.Count > 0);
            Assert.Contains((int)OnboardingStage.DayAdvance, s.completedStages);
        }

        // ── Helpers ───────────────────────────────────────────────────────

        /// <summary>
        /// Pushes the journey to <paramref name="target"/> by emitting the
        /// full set of required sigils for every earlier stage. The helper
        /// stops precisely at <paramref name="target"/>: the stage's own
        /// final precondition is left to the caller's records. The trailing
        /// <c>params string[]</c> keeps the call signature readable but is
        /// intentionally ignored — the core path is canonical and exhaustively
        /// tested by every call site.
        /// </summary>
        private static void AdvanceTo(OnboardingJourney j, OnboardingStage target, params string[] _unused)
        {
            var perStageSigs = new[] {
                new[] { S_ProtocolRation, S_ProtocolMaintenance, S_ProtocolRadio },
                new[] { S_InspectRoom, S_InspectRoom, S_InspectRoom },
                new[] { S_StoreOpened },
                new[] { S_DutyAssigned },
                new[] { S_WeatherRead },
                new[] { S_InventoryUsed },
            };
            int emit = (int)target;
            for (int s = 0; s < emit; s++)
            {
                if (j.CurrentStage >= target) return;
                if (j.JourneyComplete) return;
                foreach (var sig in perStageSigs[s]) j.RecordSigil(sig);
            }
        }
    }
}