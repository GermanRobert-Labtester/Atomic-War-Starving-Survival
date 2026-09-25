// SPDX-License-Identifier: MIT
// ASHFALL CI Gate: First-Hour Onboarding Wiring (UI/UX audit 2026-09-25 follow-up).
//
// The live first-hour journey completes only from real recorded commands
// ("sigils"). This gate is the scripted-playtest backstop: it fails if a live
// stage's required sigil has no producer in production source, if a stage
// ships without hint copy or a show-me-where route, or if an event-driven
// contextual lesson has no authored presentation. Those are exactly the silent
// failures a playtest experiences as "the hint never appears".
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Localization;
using Ashfall.Core.Onboarding;

namespace Ashfall.Core.Tests.UI
{
    public sealed class OnboardingWiringGateTests
    {
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

        private static string AllProductionSource()
        {
            string root = FindRepoRoot();
            var files = Directory.GetFiles(Path.Combine(root, "src"), "*.cs", SearchOption.AllDirectories)
                .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
                .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}bin{Path.DirectorySeparatorChar}"))
                // HostCli.* files are selftest/dev surfaces, not live producers.
                .Where(f => !Path.GetFileName(f).StartsWith("HostCli", StringComparison.Ordinal))
                .Where(f => !Path.GetFileName(f).EndsWith("SelfTest.cs", StringComparison.Ordinal));
            return string.Join("\n", files.Select(File.ReadAllText));
        }

        [Fact]
        public void EveryFirstHourStageSigil_HasAProductionRecorder()
        {
            string source = AllProductionSource();
            var missing = new List<string>();

            foreach (var stage in OnboardingCatalog.FirstHourOrder)
            {
                var def = OnboardingCatalog.DefFor(OnboardingProfile.FirstHour, stage);
                foreach (var requirement in def.Requirements)
                {
                    if (!source.Contains($"ObserveSigil(\"{requirement.Sigil}\")", StringComparison.Ordinal))
                        missing.Add($"{stage}:{requirement.Sigil}");
                }
            }

            Assert.True(missing.Count == 0,
                "First-hour stage sigils with no production ObserveSigil(...) call: "
                + string.Join(", ", missing));
        }

        [Fact]
        public void EveryFirstHourStage_HasHintCopyAndShowMeWhereRoute()
        {
            foreach (var stage in OnboardingCatalog.FirstHourOrder)
            {
                var def = OnboardingCatalog.DefFor(OnboardingProfile.FirstHour, stage);
                Assert.False(string.IsNullOrWhiteSpace(def.Title),
                    $"stage {stage} has no hint title");
                Assert.False(string.IsNullOrWhiteSpace(def.Objective),
                    $"stage {stage} has no hint objective");
                Assert.False(string.IsNullOrWhiteSpace(def.ShowMeWhereRoute),
                    $"stage {stage} has no show-me-where route");
            }
        }

        [Fact]
        public void ContextualLessons_HaveAuthoredPresentation()
        {
            string root = FindRepoRoot();
            string tutorial = File.ReadAllText(
                Path.Combine(root, "src", "UI", "TutorialPanel.cs"));

            // The presentation switch must handle each lesson identity (the
            // body fallback is the authored copy; no silent generic default).
            Assert.Contains("OnboardingLessonLocalization.ProtectionBeforeDispatchId", tutorial, StringComparison.Ordinal);
            Assert.Contains("OnboardingLessonLocalization.SevereWeatherPrepId", tutorial, StringComparison.Ordinal);

            // And each identity starts from a non-empty trigger constant.
            Assert.False(string.IsNullOrWhiteSpace(OnboardingLessonLocalization.ProtectionBeforeDispatchId));
            Assert.False(string.IsNullOrWhiteSpace(OnboardingLessonLocalization.SevereWeatherPrepId));
        }

        [Fact]
        public void ContextualLessons_FireAtMostOncePerCampaign()
        {
            var journey = OnboardingJourney.CreateFirstHour();

            Assert.True(journey.RequestContextualTutorial(
                OnboardingLessonLocalization.ProtectionBeforeDispatchId));
            Assert.False(journey.RequestContextualTutorial(
                OnboardingLessonLocalization.ProtectionBeforeDispatchId));
            Assert.True(journey.RequestContextualTutorial(
                OnboardingLessonLocalization.SevereWeatherPrepId));

            Assert.Equal(2, journey.ContextualTutorialQueue.Count);
            Assert.True(journey.AcknowledgeContextualTutorial(
                OnboardingLessonLocalization.ProtectionBeforeDispatchId));
            Assert.Single(journey.ContextualTutorialQueue);
            Assert.False(journey.AcknowledgeContextualTutorial(
                OnboardingLessonLocalization.ProtectionBeforeDispatchId));
        }
    }
}
