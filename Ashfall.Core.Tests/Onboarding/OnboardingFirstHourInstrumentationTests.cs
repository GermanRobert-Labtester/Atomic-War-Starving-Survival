// SPDX-License-Identifier: MIT
// P001–P004 — first-hour instrumentation and tutorial ordering (2026-10-01).
//
// Pins the Core authorities the host and the funnel tool share:
//   * the published first-hour verb mapping (PlayerCommandCode),
//   * the sigil → stage → verb reverse lookup,
//   * the terminal-dispatch prerequisite gate (Water→Dose),
//   * the hint_shown / hint_dismissed metric vocabulary,
//   * a source gate that the production host actually emits the verbs, counts
//     hints, and binds the tutorial dispatch gate (the silent-failure backstop
//     a headless run cannot see).
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Onboarding;
using Ashfall.Core.PlayerCommand;
using Ashfall.Core.Telemetry;

namespace Ashfall.Core.Tests.Onboarding
{
    public sealed class OnboardingFirstHourInstrumentationTests
    {
        // ── P002 — verb vocabulary ────────────────────────────────────────

        [Theory]
        [InlineData(OnboardingStage.Water, "water.start")]
        [InlineData(OnboardingStage.Power, "power.breaker")]
        [InlineData(OnboardingStage.Food, "food.consume")]
        [InlineData(OnboardingStage.Duty, "duty.assign")]
        [InlineData(OnboardingStage.Dose, "dose.open")]
        [InlineData(OnboardingStage.Research, "research.start")]
        [InlineData(OnboardingStage.Expedition, "expedition.dispatch")]
        public void FirstHourVerbForStage_PublishesStableVerb(OnboardingStage stage, string expected)
        {
            Assert.Equal(expected, OnboardingCatalog.FirstHourVerbForStage(stage));
        }

        [Fact]
        public void FirstHourVerbForStage_UnknownStageIsEmpty()
        {
            Assert.Equal(string.Empty, OnboardingCatalog.FirstHourVerbForStage(OnboardingStage.Protocol));
            Assert.Equal(string.Empty, OnboardingCatalog.FirstHourVerbForStage(OnboardingStage.Weather));
        }

        [Fact]
        public void TryGetFirstHourVerbForSigil_ResolvesOnlyLiveRequirementSigils()
        {
            foreach (var stage in OnboardingCatalog.FirstHourOrder)
            {
                var def = OnboardingCatalog.DefFor(OnboardingProfile.FirstHour, stage);
                Assert.True(OnboardingCatalog.TryGetFirstHourVerbForSigil(
                    def.Requirements[0].Sigil, out var resolved, out string verb),
                    $"{def.Requirements[0].Sigil} did not resolve");
                Assert.Equal(stage, resolved);
                Assert.Equal(OnboardingCatalog.FirstHourVerbForStage(stage), verb);
            }

            // Unrelated/blocked-attempt sigils must not manufacture a verb.
            Assert.False(OnboardingCatalog.TryGetFirstHourVerbForSigil(
                "power.breaker_reset_missing_item", out _, out _));
            Assert.False(OnboardingCatalog.TryGetFirstHourVerbForSigil(
                "protocol.ration", out _, out _));
            Assert.False(OnboardingCatalog.TryGetFirstHourVerbForSigil("", out _, out _));
        }

        // ── P003 — tutorial ordering gate ─────────────────────────────────

        [Fact]
        public void ExpeditionDispatchPrerequisite_BlocksUntilDoseComplete()
        {
            var j = OnboardingJourney.CreateFirstHour();
            Assert.Equal(OnboardingStage.Water, j.ExpeditionDispatchPrerequisite());

            j.RecordSigil("water.treatment_started");
            Assert.Equal(OnboardingStage.Power, j.ExpeditionDispatchPrerequisite());

            j.RecordSigil("power.breaker_toggled");
            Assert.Equal(OnboardingStage.Food, j.ExpeditionDispatchPrerequisite());

            j.RecordSigil("food.ration_consumed");
            Assert.Equal(OnboardingStage.Duty, j.ExpeditionDispatchPrerequisite());

            j.RecordSigil("duty.assigned");
            Assert.Equal(OnboardingStage.Dose, j.ExpeditionDispatchPrerequisite());

            // Dose is the fifth prerequisite: satisfying it unlocks dispatch
            // even though Research/Expedition have not been reached yet.
            j.RecordSigil("dose.read");
            Assert.Null(j.ExpeditionDispatchPrerequisite());
        }

        [Fact]
        public void ExpeditionDispatchPrerequisite_LegacyProfileIsUnlocked()
        {
            var legacy = new OnboardingJourney();
            Assert.Equal(OnboardingProfile.Legacy, legacy.Profile);
            Assert.Null(legacy.ExpeditionDispatchPrerequisite());

            legacy.RecordSigil("protocol.ration");
            legacy.RecordSigil("protocol.maintenance");
            legacy.RecordSigil("protocol.radio");
            Assert.Null(legacy.ExpeditionDispatchPrerequisite());
        }

        [Fact]
        public void ExpeditionDispatchPrerequisite_CompletedJourneyIsUnlocked()
        {
            var j = OnboardingJourney.CreateFirstHour();
            // First-hour completion is signal-driven and terminal only on the
            // Expedition dispatch (SkipAllRemaining deliberately stops there).
            j.RecordSigil("water.treatment_started");
            j.RecordSigil("power.breaker_toggled");
            j.RecordSigil("food.ration_consumed");
            j.RecordSigil("duty.assigned");
            j.RecordSigil("dose.read");
            j.RecordSigil("research.started");
            j.RecordSigil("expedition.dispatched");

            Assert.True(j.JourneyComplete);
            Assert.Null(j.ExpeditionDispatchPrerequisite());
        }

        // ── P004 — hint metric vocabulary ─────────────────────────────────

        [Fact]
        public void HintActions_AreKnownToRecorderVocabulary()
        {
            Assert.True(PlaySessionActions.IsKnown(PlaySessionActions.HintShown));
            Assert.True(PlaySessionActions.IsKnown(PlaySessionActions.HintDismissed));
            Assert.Equal("hint_shown", PlaySessionActions.HintShown);
            Assert.Equal("hint_dismissed", PlaySessionActions.HintDismissed);
        }

        // ── P002 — end-to-end funnel measurement ──────────────────────────

        [Fact]
        public void VerbAndSigil_BothMeasurable_AndFunnelCompletesStage()
        {
            var recorder = new PlaySessionRecorder("sess_verbs");
            var funnel = new FirstHourFunnel();
            recorder.PlaySessionEventRecordedSeam += evt => funnel.ProcessEvent(evt);

            // The host emits the sigil and the published verb for each stage.
            var sigil = OnboardingCatalog.DefFor(
                OnboardingProfile.FirstHour, OnboardingStage.Water).Requirements[0].Sigil;
            recorder.RecordSigil(sigil, 1);
            recorder.Record(OnboardingCatalog.FirstHourVerbForStage(OnboardingStage.Water),
                "water", "observed", 1);

            Assert.True(funnel.IsStepCompleted("first_water"));

            var report = PlaySessionReport.Aggregate(recorder.Drain());
            Assert.Equal(1, report.ActionHistogram["water.start"]);
            Assert.Equal(1, report.ActionHistogram[PlaySessionActions.Sigil]);
        }

        // ── Production wiring source gate ─────────────────────────────────

        [Fact]
        public void ProductionHost_EmitsVerbs_CountsHints_AndBindsDispatchGate()
        {
            string root = FindRepoRoot();
            string onboarding = File.ReadAllText(Path.Combine(root, "src", "Main.Onboarding.cs"));
            string expeditions = File.ReadAllText(Path.Combine(root, "src", "Main.Expeditions.cs"));
            string host = File.ReadAllText(Path.Combine(root, "src", "Host", "ExpeditionHostSession.cs"));

            // P002 — the observed sigil also emits its published verb.
            Assert.Contains("OnboardingCatalog.TryGetFirstHourVerbForSigil", onboarding, StringComparison.Ordinal);
            Assert.Contains("RecordPlayMetricFirstHourVerb", onboarding, StringComparison.Ordinal);

            // P004 — hint presentation/dismissal counters per stage.
            Assert.Contains("OnHintShown", onboarding, StringComparison.Ordinal);
            Assert.Contains("OnHintDismissedForStage", onboarding, StringComparison.Ordinal);
            Assert.Contains("PlaySessionActions.HintShown", onboarding, StringComparison.Ordinal);
            Assert.Contains("PlaySessionActions.HintDismissed", onboarding, StringComparison.Ordinal);

            // P003 — the tutorial-ordering gate is bound into the dispatch host.
            Assert.Contains("TutorialDispatchGate = TutorialExpeditionBlockReason", expeditions, StringComparison.Ordinal);
            Assert.Contains("TutorialDispatchGate", host, StringComparison.Ordinal);
            Assert.Contains("tutorial_step_locked", onboarding, StringComparison.Ordinal);
        }

        // ── P005 — "N days on this stage" (persisted stage entry day) ──────

        [Fact]
        public void DaysOnCurrentStage_CountsFromEntryDay_AndSurvivesRestore()
        {
            var j = OnboardingJourney.CreateFirstHour();
            Assert.Equal(0, j.DaysOnCurrentStage);

            j.SetDay(3);
            Assert.Equal(2, j.DaysOnCurrentStage);

            var restored = OnboardingJourney.Restore(j.CaptureState());
            Assert.Equal(3, restored.Day);
            Assert.Equal(2, restored.DaysOnCurrentStage);

            // Advancing the stage stamps the entry day; the count resets.
            restored.RecordSigil("water.treatment_started");
            Assert.Equal(OnboardingStage.Power, restored.CurrentStage);
            Assert.Equal(0, restored.DaysOnCurrentStage);

            restored.SetDay(5);
            Assert.Equal(2, restored.DaysOnCurrentStage);
        }

        [Fact]
        public void DaysOnCurrentStage_LegacyZeroField_FallsBackToCurrentDay()
        {
            var saved = OnboardingJourney.CreateFirstHour().CaptureState();
            saved.day = 4;
            saved.stageStartDay = 0; // legacy / omitted additive field
            var restored = OnboardingJourney.Restore(saved);
            Assert.Equal(4, restored.StageStartDay);
            Assert.Equal(0, restored.DaysOnCurrentStage);
        }

        // ── P006/P007 — HUD progress + visible skip-tutorial wiring ─────────

        [Fact]
        public void ProductionHost_ProjectsHudProgress_AndExposesSkipAll()
        {
            string root = FindRepoRoot();
            string onboarding = File.ReadAllText(Path.Combine(root, "src", "Main.Onboarding.cs"));
            string hud = File.ReadAllText(Path.Combine(root, "src", "UI", "GameHudOverlay.cs"));
            string panel = File.ReadAllText(Path.Combine(root, "src", "UI", "OnboardingHintPanel.cs"));
            string journey = File.ReadAllText(Path.Combine(root, "Assets", "Ashfall.Core", "Onboarding", "OnboardingJourney.cs"));

            // P006 — the HUD exposes a progress projection and the host feeds it
            // from the catalog order; the HUD keeps no onboarding knowledge.
            Assert.Contains("UpdateOnboardingProgress", hud, StringComparison.Ordinal);
            Assert.Contains("RefreshOnboardingHud", onboarding, StringComparison.Ordinal);
            Assert.Contains("UpdateOnboardingProgress", onboarding, StringComparison.Ordinal);
            Assert.Contains("onboarding.hud.progress", onboarding, StringComparison.Ordinal);

            // P005 — the nudge is driven by the persisted days-on-stage read.
            Assert.Contains("DaysOnCurrentStage", onboarding, StringComparison.Ordinal);
            Assert.Contains("onboarding.status.days_on_stage", onboarding, StringComparison.Ordinal);

            // P007 — the panel raises a skip-all request bound to the existing
            // (previously dead) SkipAllOnboardingStages seam.
            Assert.Contains("OnSkipAllRequested", panel, StringComparison.Ordinal);
            Assert.Contains("panel.OnSkipAllRequested += SkipAllOnboardingStages", onboarding, StringComparison.Ordinal);
            Assert.Contains("SkipAllRemaining", journey, StringComparison.Ordinal);
        }

        // ── P008 — recorded 7-stage floor + registered CI smoke ─────────────

        [Fact]
        public void FirstHourCatalog_RetainsRecordedSevenStageFloor()
        {
            Assert.Equal(7, OnboardingCatalog.FirstHourOrder.Length);
            Assert.Equal(OnboardingCatalog.FirstHourOrder.Length,
                OnboardingCatalog.FirstHourOrder.Distinct().Count());
            foreach (var stage in OnboardingCatalog.FirstHourOrder)
                Assert.NotEqual(string.Empty, OnboardingCatalog.FirstHourVerbForStage(stage));
        }

        [Fact]
        public void CiGate_GuardsOnboardingJourneySelftestFloor()
        {
            string root = FindRepoRoot();
            string manifest = File.ReadAllText(Path.Combine(root, "docs", "ci", "CI_GATE_MANIFEST.json"));
            Assert.Contains("\"first_hour_onboarding_journey\"", manifest, StringComparison.Ordinal);
            Assert.Contains("--onboarding-journey-selftest", manifest, StringComparison.Ordinal);
            Assert.Contains("ONBOARDING_JOURNEY_SELFTEST PASS", manifest, StringComparison.Ordinal);
        }

        private static string FindRepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                    && Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException(
                "Could not locate repository root from " + Directory.GetCurrentDirectory());
        }
    }
}