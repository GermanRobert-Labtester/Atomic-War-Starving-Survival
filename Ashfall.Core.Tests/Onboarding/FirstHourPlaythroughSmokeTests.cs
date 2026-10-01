// SPDX-License-Identifier: MIT
// ============================================================================
// First-hour playthrough smoke — the "first 30 minutes" end-to-end gate.
//
// Complements the per-stage OnboardingJourneyTests and the producer-parity
// OnboardingWiringGateTests with one continuous playthrough: a fresh FirstHour
// journey must complete when the real signals are recorded, survive a
// mid-journey save/restore without re-demanding finished stages, and expose a
// non-empty "show me where" route for every stage so the human smoke checklist
// in docs/qa/FIRST_HOUR_SMOKE_TEST.md maps to real surfaces.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Onboarding;
using Xunit;

namespace Ashfall.Core.Tests.Onboarding
{
    public sealed class FirstHourPlaythroughSmokeTests
    {
        private static readonly string[] FirstHourSigils =
        {
            "water.treatment_started",
            "power.breaker_toggled",
            "food.ration_consumed",
            "duty.assigned",
            "dose.read",
            "research.started",
            "expedition.dispatched",
        };

        [Fact]
        public void FreshFirstHour_CompletesWhenEveryRealSignalIsRecorded()
        {
            var journey = OnboardingJourney.CreateFirstHour();
            Assert.False(journey.JourneyComplete);

            for (int i = 0; i < FirstHourSigils.Length; i++)
            {
                journey.RecordSigil(FirstHourSigils[i]);
            }

            Assert.True(journey.JourneyComplete,
                "recording all six first-hour signals plus expedition must complete the journey");
            foreach (var stage in OnboardingCatalog.FirstHourOrder)
                Assert.True(journey.IsStageComplete(stage), $"stage {stage} must be complete");
        }

        [Fact]
        public void MidJourneySaveRestore_DoesNotReDemandFinishedStages()
        {
            var journey = OnboardingJourney.CreateFirstHour();
            for (int i = 0; i < 3; i++) journey.RecordSigil(FirstHourSigils[i]);

            var captured = journey.CaptureState();
            var restored = OnboardingJourney.Restore(captured);
            Assert.False(restored.JourneyComplete);
            // The reload resumes with exactly the progress captured, not a restart.
            Assert.Equal(journey.CompletedStages.Count, restored.CompletedStages.Count);

            for (int i = 3; i < FirstHourSigils.Length; i++) restored.RecordSigil(FirstHourSigils[i]);

            Assert.True(restored.JourneyComplete,
                "a reload must resume, not restart, the first-hour journey");
        }

        [Fact]
        public void EveryFirstHourStage_HasANonEmptyShowMeWhereRoute()
        {
            var seenRoutes = new HashSet<string>(StringComparer.Ordinal);
            foreach (var stage in OnboardingCatalog.FirstHourOrder)
            {
                var def = OnboardingCatalog.DefFor(OnboardingProfile.FirstHour, stage);
                Assert.False(string.IsNullOrWhiteSpace(def.ShowMeWhereRoute),
                    $"stage {stage} must name a real panel route");
                seenRoutes.Add(def.ShowMeWhereRoute);
            }
            Assert.True(seenRoutes.Count >= 5,
                "the first hour must span distinct surfaces, not one panel repeated");
        }

        [Fact]
        public void FirstHourOrderAndDefs_AreConsistent()
        {
            var defs = new Dictionary<OnboardingStage, OnboardingStageDef>();
            foreach (var def in OnboardingCatalog.FirstHour) defs[def.Id] = def;

            foreach (var stage in OnboardingCatalog.FirstHourOrder)
            {
                Assert.True(defs.ContainsKey(stage), $"FirstHourOrder lists {stage} without a def");
                Assert.False(string.IsNullOrWhiteSpace(defs[stage].Title));
                Assert.False(string.IsNullOrWhiteSpace(defs[stage].Objective));
                Assert.NotEmpty(defs[stage].Requirements);
            }
        }
    }
}
