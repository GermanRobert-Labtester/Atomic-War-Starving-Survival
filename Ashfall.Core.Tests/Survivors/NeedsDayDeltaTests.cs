// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Localization;
using Ashfall.Core.Onboarding;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    /// <summary>
    /// P010 — day-over-day needs trend read model. These are pure Core tests:
    /// the tracker owns no simulation rule, mutates nothing, and its baseline is
    /// transient (never persisted).
    /// </summary>
    public sealed class NeedsDayDeltaTests
    {
        private static SurvivorNeedsState Survivor(string id) => new SurvivorNeedsState
        {
            Id = id,
            Hunger = 20f,
            Thirst = 30f,
            Fatigue = 10f,
            Morale = 55f,
            Warmth = 80f
        };

        [Fact]
        public void CaptureThenModify_ReturnsSignedDelta()
        {
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(3, new List<SurvivorNeedsState> { survivor });

            survivor.Hunger += 19f;   // worsened
            survivor.Morale -= 4f;    // worsened
            survivor.Warmth -= 12f;   // colder

            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Hunger, out float hunger));
            Assert.Equal(19f, hunger, 3);
            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Morale, out float morale));
            Assert.Equal(-4f, morale, 3);
            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Warmth, out float warmth));
            Assert.Equal(-12f, warmth, 3);
            Assert.Equal(3, tracker.BaselineDay);
        }

        [Fact]
        public void NoBaseline_ReturnsFalse()
        {
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");

            Assert.False(tracker.HasBaseline);
            Assert.False(tracker.TryGetDelta(survivor, NeedKind.Thirst, out float delta));
            Assert.Equal(0f, delta, 3);
        }

        [Fact]
        public void Clear_DropsBaseline()
        {
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(2, new List<SurvivorNeedsState> { survivor });
            Assert.True(tracker.HasBaseline);

            tracker.Clear();

            Assert.False(tracker.HasBaseline);
            Assert.Equal(-1, tracker.BaselineDay);
            Assert.False(tracker.TryGetDelta(survivor, NeedKind.Fatigue, out _));
        }

        [Fact]
        public void Recapture_ReplacesBaseline_SoRetryCannotCarryFailedDay()
        {
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(5, new List<SurvivorNeedsState> { survivor });
            survivor.Thirst += 50f; // simulated failed day
            tracker.Capture(5, new List<SurvivorNeedsState> { survivor }); // retry snapshot

            survivor.Thirst += 18f; // the successful day

            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Thirst, out float delta));
            Assert.Equal(18f, delta, 3);
        }

        [Fact]
        public void UntrackedNeedOrUnknownSurvivor_ReturnsFalse()
        {
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(1, new List<SurvivorNeedsState> { survivor });

            Assert.False(tracker.TryGetDelta(survivor, NeedKind.Health, out _));
            Assert.False(tracker.TryGetDelta(Survivor("sv2"), NeedKind.Hunger, out _));
            Assert.False(tracker.TryGetDelta(null!, NeedKind.Hunger, out _));
        }

        [Fact]
        public void Capture_SkipsNullAndIdlessEntries()
        {
            var tracker = new NeedsDayDeltaTracker();
            var roster = new List<SurvivorNeedsState> { null!, new SurvivorNeedsState { Id = string.Empty }, Survivor("sv1") };
            tracker.Capture(1, roster);

            Assert.True(tracker.TryGetDelta(Survivor("sv1"), NeedKind.Hunger, out _));
        }

        [Fact]
        public void NeedsProfile_ExposesCriticalPredicates()
        {
            // Task 7 — the predicate family is symmetric with IsWarmthCritical so
            // panels can stop comparing raw critical fields.
            var p = new NeedsProfile();

            Assert.True(p.IsHungerCritical(p.hungerCritical));
            Assert.False(p.IsHungerCritical(p.hungerCritical - 1f));
            Assert.True(p.IsHealthCritical(p.healthCritical));
            Assert.False(p.IsHealthCritical(p.healthCritical + 1f));
            Assert.True(p.IsHealthWarn(p.healthWarn));
            Assert.False(p.IsHealthWarn(p.healthWarn + 1f));
            Assert.True(p.IsThirstCritical(p.thirstCritical));
            Assert.False(p.IsThirstCritical(p.thirstCritical - 1f));
            Assert.True(p.IsFatigueCritical(p.fatigueCritical));
            Assert.False(p.IsFatigueCritical(p.fatigueCritical - 1f));
            Assert.True(p.IsMoraleCritical(p.moraleCritical));
            Assert.False(p.IsMoraleCritical(p.moraleCritical + 1f));
            Assert.True(p.IsWarmthCritical(p.warmthCritical));
            Assert.False(p.IsWarmthCritical(p.warmthCritical + 1f));
        }

        [Fact]
        public void NeedsSystem_Profile_ExposesEnforcedCriticalValues()
        {
            var needs = new NeedsSystem();

            Assert.Equal(90f, needs.Profile.hungerCritical, 3);
            Assert.Equal(90f, needs.Profile.thirstCritical, 3);
            Assert.Equal(20f, needs.Profile.warmthCritical, 3);
        }

        [Fact]
        public void NeedsSystem_Profile_ExposesAuthoredWarnBands()
        {
            // Task 6 — the HUD/feedback warn bands live on the authored profile
            // instead of presentation literals.
            var profile = new NeedsSystem().Profile;

            Assert.Equal(70f, profile.hungerWarn, 3);
            Assert.Equal(70f, profile.thirstWarn, 3);
            Assert.Equal(70f, profile.fatigueWarn, 3);
            Assert.Equal(90f, profile.fatigueCritical, 3);
            Assert.Equal(30f, profile.moraleWarn, 3);
            Assert.Equal(15f, profile.moraleCritical, 3);
        }

        [Fact]
        public void Delta_IsClampedToTheAuthoredNeedRange()
        {
            // Task 9 — a corrupt baseline or long gap must not render an
            // implausible delta. The need range is 0..100.
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(1, new List<SurvivorNeedsState> { survivor });

            survivor.Hunger = 100f;
            survivor.Thirst = 0f;
            // Rewrite the baseline to an out-of-range value via a second capture.
            tracker.Capture(1, new List<SurvivorNeedsState>
            {
                new SurvivorNeedsState { Id = "sv1", Hunger = 0f, Thirst = 100f }
            });

            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Hunger, out float hunger));
            Assert.Equal(100f, hunger, 3);
            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Thirst, out float thirst));
            Assert.Equal(-100f, thirst, 3);
        }

        [Fact]
        public void DaySpan_TracksBaselineAge()
        {
            var tracker = new NeedsDayDeltaTracker();
            Assert.Equal(0, tracker.DaySpan(5));

            tracker.Capture(3, new List<SurvivorNeedsState> { Survivor("sv1") });
            Assert.Equal(1, tracker.DaySpan(4));
            Assert.Equal(2, tracker.DaySpan(5));
        }

        [Fact]
        public void DaySpan_NonPositiveAge_ClampsToOne()
        {
            // Task 7 — a same-day or rewound clock must not yield 0/negative.
            var tracker = new NeedsDayDeltaTracker();
            tracker.Capture(5, new List<SurvivorNeedsState> { Survivor("sv1") });
            Assert.Equal(1, tracker.DaySpan(5));
            Assert.Equal(1, tracker.DaySpan(4));
            Assert.Equal(1, tracker.DaySpan(0));
        }

        [Fact]
        public void NeedsDayDeltaFormat_IsCultureInvariant()
        {
            // Task 6 — the signed formatter must not follow the current culture
            // (e.g. tr-TR) or emit a locale-specific sign.
            var previous = System.Globalization.CultureInfo.CurrentCulture;
            try
            {
                System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("tr-TR");
                Assert.Equal("+19", NeedsDayDeltaFormat.Signed(19f));
                Assert.Equal("-4", NeedsDayDeltaFormat.Signed(-4f));
                Assert.Equal("0", NeedsDayDeltaFormat.Signed(-0.4f));
            }
            finally
            {
                System.Globalization.CultureInfo.CurrentCulture = previous;
            }
        }

        [Fact]
        public void Tracker_IsTransient_NotReferencedByAnySaveState()
        {
            // Task 10 — the day-delta baseline must never enter persistence.
            string root = RepoRoot();
            string core = Path.Combine(root, "Assets", "Ashfall.Core");
            foreach (var file in Directory.EnumerateFiles(core, "*.cs", SearchOption.AllDirectories))
            {
                if (!file.Contains("Save", StringComparison.Ordinal)) continue;
                string text = File.ReadAllText(file);
                Assert.DoesNotContain("NeedsDayDeltaTracker", text, StringComparison.Ordinal);
            }
        }

        [Fact]
        public void Signed_RoundsAndRejectsNonFiniteDeltas()
        {
            // Hardening: the shared formatter is now the single authority for
            // drift text; guard the -0 and NaN/Infinity leaks directly.
            Assert.Equal("0", NeedsDayDeltaFormat.Signed(0f));
            Assert.Equal("0", NeedsDayDeltaFormat.Signed(-0.4f));
            Assert.Equal("+19", NeedsDayDeltaFormat.Signed(19.4f));
            Assert.Equal("-4", NeedsDayDeltaFormat.Signed(-4.4f));
            Assert.Equal("0", NeedsDayDeltaFormat.Signed(float.NaN));
            Assert.Equal("0", NeedsDayDeltaFormat.Signed(float.PositiveInfinity));
            Assert.Equal("0", NeedsDayDeltaFormat.Signed(float.NegativeInfinity));
            // Overflow-safe: a huge finite value clamps instead of an undefined cast.
            Assert.Equal("+100", NeedsDayDeltaFormat.Signed(float.MaxValue));
            Assert.Equal("-100", NeedsDayDeltaFormat.Signed(float.MinValue));
        }

        [Fact]
        public void NeedsProfile_Predicates_RespectTheirBoundaries()
        {
            var p = new NeedsProfile();
            Assert.False(p.IsHungerCritical(p.hungerCritical - 1f));
            Assert.True(p.IsHungerCritical(p.hungerCritical));
            Assert.False(p.IsHealthCritical(p.healthCritical + 1f));
            Assert.True(p.IsHealthCritical(p.healthCritical));
            Assert.True(p.IsHealthWarn(p.healthWarn));
            Assert.False(p.IsHealthWarn(p.healthWarn + 1f));
            Assert.False(p.IsThirstCritical(p.thirstCritical - 1f));
            Assert.True(p.IsThirstCritical(p.thirstCritical));
            Assert.False(p.IsFatigueCritical(p.fatigueCritical - 1f));
            Assert.True(p.IsFatigueCritical(p.fatigueCritical));
            Assert.True(p.IsMoraleCritical(p.moraleCritical));
            Assert.False(p.IsMoraleCritical(p.moraleCritical + 1f));
            Assert.True(p.IsWarmthCritical(p.warmthCritical));
            Assert.False(p.IsWarmthCritical(p.warmthCritical + 1f));
        }

        [Fact]
        public void Capture_SameUnchangedStateTwice_YieldsZeroDeltas()
        {
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(1, new List<SurvivorNeedsState> { survivor });
            tracker.Capture(1, new List<SurvivorNeedsState> { survivor });

            Assert.True(tracker.TryGetDelta(survivor, NeedKind.Hunger, out float delta));
            Assert.Equal(0f, delta, 3);
            Assert.Equal(1, tracker.BaselineDay);
        }

        [Fact]
        public void TrackedKinds_CoverEveryRenderedNeed_AndRejectTheRest()
        {
            // Task 9 — guards against a NeedKind being rendered by a panel but
            // silently absent from the tracker's Tracked array (or vice versa).
            var tracker = new NeedsDayDeltaTracker();
            var survivor = Survivor("sv1");
            tracker.Capture(1, new List<SurvivorNeedsState> { survivor });

            foreach (var kind in new[]
            {
                NeedKind.Hunger, NeedKind.Thirst, NeedKind.Fatigue, NeedKind.Morale, NeedKind.Warmth
            })
            {
                Assert.True(tracker.TryGetDelta(survivor, kind, out _), $"tracked kind {kind}");
            }

            foreach (var kind in new[]
            {
                NeedKind.Health, NeedKind.Hygiene, NeedKind.Numbness, NeedKind.RadiationAnxiety
            })
            {
                Assert.False(tracker.TryGetDelta(survivor, kind, out _), $"untracked kind {kind}");
            }
        }

        [Fact]
        public void AcuteRadiationLesson_IsRequestedAtMostOncePerCampaign()
        {
            // Task 4 — the persisted onboarding authority dedups the lesson even
            // if the production trigger fires on every HUD refresh.
            var journey = OnboardingJourney.CreateFirstHour();

            Assert.True(journey.RequestContextualTutorial(OnboardingLessonLocalization.AcuteRadiationId));
            Assert.False(journey.RequestContextualTutorial(OnboardingLessonLocalization.AcuteRadiationId));
            Assert.Equal(1, journey.ContextualTutorialQueue.Count(id => id == OnboardingLessonLocalization.AcuteRadiationId));
        }

        [Theory]
        [InlineData(19f, "+19")]
        [InlineData(-4f, "-4")]
        [InlineData(0f, "0")]
        [InlineData(0.4f, "0")]
        [InlineData(-0.4f, "0")]
        [InlineData(1.5f, "+2")]
        [InlineData(-1.5f, "-2")]
        public void NeedsDayDeltaFormat_RoundsAwayFromZero_AndAvoidsNegativeZero(float value, string expected)
        {
            Assert.Equal(expected, NeedsDayDeltaFormat.Signed(value));
        }

        [Theory]
        [InlineData(float.NaN)]
        [InlineData(float.PositiveInfinity)]
        [InlineData(float.NegativeInfinity)]
        public void NeedsDayDeltaFormat_NonFinite_ReturnsZero(float value)
        {
            // Task 12 — a corrupt baseline must never leak "NaN"/"Infinity"
            // into the HUD drift row.
            Assert.Equal("0", NeedsDayDeltaFormat.Signed(value));
        }

        private static string RepoRoot()
        {
            var dir = new DirectoryInfo(Path.GetFullPath(Directory.GetCurrentDirectory()));
            while (dir != null)
            {
                if (Directory.Exists(Path.Combine(dir.FullName, "src"))
                    && Directory.Exists(Path.Combine(dir.FullName, "Assets")))
                    return dir.FullName;
                dir = dir.Parent!;
            }
            throw new DirectoryNotFoundException("repository root not found");
        }
    }
}
