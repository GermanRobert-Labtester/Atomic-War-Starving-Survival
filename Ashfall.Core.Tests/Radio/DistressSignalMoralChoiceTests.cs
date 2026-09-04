// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 23: Distress Signal Moral Choice Integration Suite.
    ///
    /// Validates:
    /// - Genuine rescue signals trigger moral choice availability upon reaching clarity.
    /// - False flags, raider traps, and automated beacons are strictly excluded from rescue moral choices.
    /// - Rescue vs. Ignore choices produce authored moral deltas and distinct outcomes.
    /// - Idempotent lifecycle: Repeated triggers and resolution calls produce zero duplicate choices or consequences.
    /// - Save/load durability: Choice availability, resolution index, and consequences survive capture/restore.
    /// </summary>
    public sealed class DistressSignalMoralChoiceTests : CatalogTestBase
    {
        private static (RadioDistressSystem distress, MoralChoiceSystem moral, Dictionary<string, MoralChoiceQuestDefinition> quests) CreateFixture()
        {
            var distress = new RadioDistressSystem();
            string path = Path.Combine(DataDirectory, "radio_distress_signals.json");
            distress.LoadFromJson(File.ReadAllText(path));

            var moral = new MoralChoiceSystem(new SeededRng(42));
            var quests = MoralChoiceCatalogLoader.LoadStubs(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer())
                .ToDictionary(q => q.Id, StringComparer.OrdinalIgnoreCase);

            return (distress, moral, quests);
        }

        [Fact]
        public void GenuineRescueSignal_TriggersMoralChoice_WhenClarityReached()
        {
            var (distress, _, _) = CreateFixture();

            // 88.3 MHz - Trapped Mechanic at Rail Depot
            string signalId = "freq_distress_88_3";

            // 1. Inactive: cannot trigger moral choice
            Assert.False(distress.TryTriggerMoralChoice(signalId, out _));

            // 2. Intercept on Day 1: clarity begins at 0.35f
            Assert.True(distress.Intercept(signalId, day: 1));

            // 3. Now clarity threshold (>= 0.25f) is met: moral choice becomes available
            Assert.True(distress.TryTriggerMoralChoice(signalId, out string moralChoiceId));
            Assert.Equal("quest_moral_distress_trapped_mechanic", moralChoiceId);

            var active = distress.GetActiveState(signalId);
            Assert.NotNull(active);
            Assert.True(active!.IsMoralChoiceAvailable);
        }

        [Fact]
        public void FalseFlagsAndTraps_NeverTriggerRescueMoralChoice()
        {
            var (distress, _, _) = CreateFixture();

            // 192.4 MHz - Raider Lure (Trap)
            string trapId = "freq_distress_192_4";
            distress.Intercept(trapId, day: 1);
            distress.MarkTriangulated(trapId);

            Assert.False(distress.TryTriggerMoralChoice(trapId, out string trapChoiceId));
            Assert.Empty(trapChoiceId);

            // 333.6 MHz - Impersonated Settlement Call (False Flag)
            string falseFlagId = "freq_distress_333_6";
            distress.Intercept(falseFlagId, day: 1);
            distress.MarkTriangulated(falseFlagId);

            Assert.False(distress.TryTriggerMoralChoice(falseFlagId, out string ffChoiceId));
            Assert.Empty(ffChoiceId);

            // 392.7 MHz - Automated Weather Station (Automated loop)
            string autoId = "freq_distress_392_7";
            distress.Intercept(autoId, day: 1);
            distress.MarkTriangulated(autoId);

            Assert.False(distress.TryTriggerMoralChoice(autoId, out string autoChoiceId));
            Assert.Empty(autoChoiceId);
        }

        [Fact]
        public void MoralChoice_ProvidesAuthoredRescueAndIgnoreOptions()
        {
            var (_, _, quests) = CreateFixture();

            string questId = "quest_moral_distress_trapped_mechanic";
            Assert.True(quests.TryGetValue(questId, out var quest));
            Assert.NotNull(quest);
            Assert.Equal(2, quest!.Choices.Count);

            // Choice 0: Rescue
            var rescue = quest.Choices[0];
            Assert.True(rescue.MoralDelta > 0, "Rescue must have positive moral delta");
            Assert.True(rescue.EmpathyDelta > 0, "Rescue must have positive empathy delta");
            Assert.Contains("rescue", rescue.Label, StringComparison.OrdinalIgnoreCase);

            // Choice 1: Ignore
            var ignore = quest.Choices[1];
            Assert.True(ignore.MoralDelta < 0, "Ignore must have negative moral delta");
            Assert.False(string.IsNullOrWhiteSpace(ignore.Epitaph));
        }

        [Fact]
        public void MoralChoiceResolution_RescueOption_UpdatesStateAndDispatchesExpedition()
        {
            var (distress, moral, quests) = CreateFixture();
            string signalId = "freq_distress_88_3";

            distress.Intercept(signalId, day: 1);
            Assert.True(distress.TryTriggerMoralChoice(signalId, out string moralChoiceId));
            var questDef = quests[moralChoiceId];

            int scoreBefore = moral.MoralScore;

            // Resolve choice 0 (Rescue)
            bool success = distress.ResolveMoralChoice(signalId, 0, moral, questDef, day: 2, out var resolution);
            Assert.True(success);
            Assert.NotNull(resolution);
            Assert.Equal(0, resolution!.choiceIndex);

            // Moral score increased
            Assert.True(moral.MoralScore > scoreBefore);

            // Active state transitioned to Dispatched
            var active = distress.GetActiveState(signalId);
            Assert.Equal(DistressSignalStatus.Dispatched, active!.Status);
            Assert.Equal(0, active.MoralChoiceResolutionIndex);
            Assert.False(active.IsIgnored);
        }

        [Fact]
        public void MoralChoiceResolution_IgnoreOption_AppliesPenaltyAndLocksSignal()
        {
            var (distress, moral, quests) = CreateFixture();
            string signalId = "freq_distress_88_3";

            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);
            var questDef = quests[moralChoiceId];

            int scoreBefore = moral.MoralScore;

            // Resolve choice 1 (Ignore)
            bool success = distress.ResolveMoralChoice(signalId, 1, moral, questDef, day: 2, out var resolution);
            Assert.True(success);
            Assert.NotNull(resolution);
            Assert.Equal(1, resolution!.choiceIndex);

            // Moral score decreased
            Assert.True(moral.MoralScore < scoreBefore);

            // Active state marked ResolvedIgnored
            var active = distress.GetActiveState(signalId);
            Assert.Equal(DistressSignalStatus.ResolvedIgnored, active!.Status);
            Assert.True(active.IsResolved);
            Assert.True(active.IsIgnored);
            Assert.Equal(1, active.MoralChoiceResolutionIndex);
            Assert.Contains("sender_death", active.ResolutionSummary);
        }

        [Fact]
        public void MoralChoiceLifecycle_IsStrictlyIdempotent()
        {
            var (distress, moral, quests) = CreateFixture();
            string signalId = "freq_distress_88_3";

            distress.Intercept(signalId, day: 1);

            // Repeated trigger calls do not duplicate availability
            for (int i = 0; i < 5; i++)
            {
                Assert.True(distress.TryTriggerMoralChoice(signalId, out _));
            }

            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);
            var questDef = quests[moralChoiceId];

            // Resolve choice
            distress.ResolveMoralChoice(signalId, 0, moral, questDef, day: 2, out var firstRes);
            int scoreAfterFirst = moral.MoralScore;
            int empathyAfterFirst = moral.EmpathyPoints;

            // Repeated resolution calls return stored outcome with zero double-application
            for (int i = 0; i < 5; i++)
            {
                bool again = distress.ResolveMoralChoice(signalId, 0, moral, questDef, day: 2, out var againRes);
                Assert.True(again);
                Assert.Equal(firstRes!.choiceIndex, againRes!.choiceIndex);
                Assert.Equal(scoreAfterFirst, moral.MoralScore);
                Assert.Equal(empathyAfterFirst, moral.EmpathyPoints);
            }
        }

        [Fact]
        public void DistressSignalSaveLoad_PreservesMoralChoiceState()
        {
            var (distress, moral, quests) = CreateFixture();
            string signalId = "freq_distress_88_3";

            distress.Intercept(signalId, day: 1);
            distress.TryTriggerMoralChoice(signalId, out string moralChoiceId);
            distress.ResolveMoralChoice(signalId, 0, moral, quests[moralChoiceId], day: 2, out _);

            // Capture state
            var saved = distress.CaptureState();
            Assert.NotEmpty(saved);

            // Restore into clean instance
            var freshDistress = new RadioDistressSystem();
            string path = Path.Combine(DataDirectory, "radio_distress_signals.json");
            freshDistress.LoadFromJson(File.ReadAllText(path));
            freshDistress.RestoreState(saved);

            var restored = freshDistress.GetActiveState(signalId);
            Assert.NotNull(restored);
            Assert.True(restored!.IsMoralChoiceAvailable);
            Assert.Equal(0, restored.MoralChoiceResolutionIndex);
            Assert.Equal(DistressSignalStatus.Dispatched, restored.Status);
        }
    }
}
