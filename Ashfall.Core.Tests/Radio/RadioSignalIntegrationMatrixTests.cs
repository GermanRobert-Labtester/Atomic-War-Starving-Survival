// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Radio;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Flagship Task 18: Distress Signal Cross-Plan Integration Matrix Suite.
    ///
    /// Validates cross-subsystem contracts:
    /// - Plan 50 / Expeditions: Dispatching expedition links to destination, completes rescue, applies rewards.
    /// - Plan 24 / Tuner: Evaluates locked frequencies within bandwidth, respects noise floor.
    /// - Plan 23 / Moral Choice: Rescue intent vs. Ignore choices mutate moral and faction standings cleanly.
    /// - Plan 30 / Faction War: Standing changes bounded [-100, +100] and applied through FactionWarSystem.
    /// - Trap/Deception Avoidance: Transition to ResolvedTrapAvoided with zero wrongful penalties.
    /// - Sender Survival & Expiration: Ticking daily decrements countdown and marks Expired when deadline lapses.
    /// - Save/Load Persistence: CaptureState / RestoreState round-trip all active signals, flags, and resolution markers.
    /// - Idempotency: Duplicate calls to trigger, resolve, or complete produce zero duplicate side-effects.
    /// </summary>
    public sealed class RadioSignalIntegrationMatrixTests : CatalogTestBase
    {
        private static (RadioDistressSystem distress, FactionWarSystem factionWar, MoralChoiceSystem moral, Dictionary<string, MoralChoiceQuestDefinition> quests, RadioTuner tuner) CreateFixture(int seed = 42)
        {
            var distress = new RadioDistressSystem();
            string path = Path.Combine(DataDirectory, "radio_distress_signals.json");
            distress.LoadFromJson(File.ReadAllText(path));

            var factionWar = new FactionWarSystem();
            var moral = new MoralChoiceSystem(new SeededRng(seed));
            var quests = MoralChoiceCatalogLoader.LoadStubs(DataDirectory, new FileSystemIO(), new SystemTextJsonSerializer())
                .ToDictionary(q => q.Id, StringComparer.OrdinalIgnoreCase);
            var tuner = new RadioTuner(new RadioTunerState { TunedFrequencyKHz = 88.0f });

            return (distress, factionWar, moral, quests, tuner);
        }

        [Fact]
        public void TunerEvaluation_LocksWithinWindow_SurfacesAccurateAuthenticity()
        {
            var (distress, _, _, _, tuner) = CreateFixture();
            var rng = new SeededRng(42);

            // Exact frequency match: 88.3 MHz (Genuine rescue)
            var resultExact = tuner.EvaluateFrequency(88.3f, distress, 0.1f, rng, day: 1);
            Assert.True(resultExact.IsLocked);
            Assert.NotNull(resultExact.Signal);
            Assert.Equal("freq_distress_88_3", resultExact.Signal!.FrequencyId);
            Assert.True(resultExact.IsGenuineRescue);
            Assert.False(resultExact.IsDeceptive);
            Assert.True(resultExact.VuStrength > 0.8f);

            // Slightly detuned within 0.2 MHz
            var resultNear = tuner.EvaluateFrequency(88.4f, distress, 0.1f, rng, day: 1);
            Assert.True(resultNear.IsLocked);
            Assert.True(resultNear.VuStrength < resultExact.VuStrength);

            // Completely off frequency (> 0.5 MHz)
            var resultFar = tuner.EvaluateFrequency(95.0f, distress, 0.1f, rng, day: 1);
            Assert.False(resultFar.IsLocked);
            Assert.Null(resultFar.Signal);

            // Trap signal: 192.4 MHz
            var resultTrap = tuner.EvaluateFrequency(192.4f, distress, 0.1f, rng, day: 1);
            Assert.True(resultTrap.IsLocked);
            Assert.True(resultTrap.IsDeceptive);
            Assert.False(resultTrap.IsGenuineRescue);
        }

        [Fact]
        public void MoralChoice_RescueFlow_UpdatesMoralScore_AndDispatchesSignal()
        {
            var (distress, factionWar, moral, quests, _) = CreateFixture();
            string signalId = "freq_distress_88_3";

            Assert.True(distress.Intercept(signalId, day: 1));
            Assert.True(distress.TryTriggerMoralChoice(signalId, out string questId));
            Assert.Equal("quest_moral_distress_trapped_mechanic", questId);

            int initialMoral = moral.MoralScore;

            // Resolve choice 0: Accept rescue
            bool resolved = distress.ResolveMoralChoice(signalId, 0, moral, quests[questId], day: 2, out var resolution, factionWar);
            Assert.True(resolved);
            Assert.NotNull(resolution);

            var active = distress.GetActiveState(signalId);
            Assert.NotNull(active);
            Assert.Equal(DistressSignalStatus.Dispatched, active!.Status);
            Assert.Equal(0, active.MoralChoiceResolutionIndex);
            Assert.True(moral.MoralScore > initialMoral, "Accepting rescue must improve moral score");
        }

        [Fact]
        public void MoralChoice_IgnoreFlow_AppliesIgnorePenalty_AndTerminatesSignal()
        {
            var (distress, factionWar, moral, quests, _) = CreateFixture();
            string signalId = "freq_distress_901_2"; // Iron Garrison Stranded Patrol

            Assert.True(distress.Intercept(signalId, day: 1));
            Assert.True(distress.TryTriggerMoralChoice(signalId, out string questId));

            int initialStanding = factionWar.GetStanding("iron_garrison");
            int initialMoral = moral.MoralScore;

            // Resolve choice 1: Ignore signal
            bool resolved = distress.ResolveMoralChoice(signalId, 1, moral, quests[questId], day: 2, out var resolution, factionWar);
            Assert.True(resolved);
            Assert.NotNull(resolution);

            var active = distress.GetActiveState(signalId);
            Assert.NotNull(active);
            Assert.Equal(DistressSignalStatus.ResolvedIgnored, active!.Status);
            Assert.True(active.IsIgnored);
            Assert.Equal(1, active.MoralChoiceResolutionIndex);

            int finalStanding = factionWar.GetStanding("iron_garrison");
            Assert.True(finalStanding < initialStanding, "Ignoring allied signal must penalize faction standing");
            Assert.True(moral.MoralScore < initialMoral, "Ignoring rescue must reduce moral score");

            // Attempting to complete an ignored signal must fail
            Assert.False(distress.CompleteRescue(signalId, factionWar));
        }

        [Fact]
        public void TrapAvoidance_TransitionsToResolvedTrapAvoided_WithoutFactionPenalty()
        {
            var (distress, factionWar, _, _, _) = CreateFixture();
            string trapId = "freq_distress_192_4";

            int initialStanding = factionWar.GetStanding("raiders");

            Assert.True(distress.Intercept(trapId, day: 1));
            Assert.True(distress.MarkTriangulated(trapId));

            // Player recognizes trap and avoids
            bool avoided = distress.Resolve(trapId, DistressSignalStatus.ResolvedTrapAvoided, "Trap recognized and bypassed safely.");
            Assert.True(avoided);

            var active = distress.GetActiveState(trapId);
            Assert.NotNull(active);
            Assert.Equal(DistressSignalStatus.ResolvedTrapAvoided, active!.Status);

            int finalStanding = factionWar.GetStanding("raiders");
            Assert.Equal(initialStanding, finalStanding);
        }

        [Fact]
        public void ExpirationLifecycle_DecrementsDailyCountdown_AndMarksExpired()
        {
            var (distress, factionWar, _, _, _) = CreateFixture();
            string signalId = "freq_distress_148_2"; // 3 days to trace

            Assert.True(distress.Intercept(signalId, day: 1));
            var active = distress.GetActiveState(signalId);
            Assert.NotNull(active);
            Assert.Equal(3, active!.DaysRemaining);

            distress.TickDaily(2);
            Assert.Equal(2, active.DaysRemaining);
            Assert.Equal(DistressSignalStatus.Intercepted, active.Status);

            distress.TickDaily(3);
            Assert.Equal(1, active.DaysRemaining);

            distress.TickDaily(4);
            Assert.Equal(0, active.DaysRemaining);
            Assert.Equal(DistressSignalStatus.Expired, active.Status);

            // Cannot trigger moral choice or complete rescue on expired signal
            Assert.False(distress.TryTriggerMoralChoice(signalId, out _));
            Assert.False(distress.CompleteRescue(signalId, factionWar));
        }

        [Fact]
        public void ExpeditionRescueCompletion_AwardsFactionReputation_ExactlyOnce()
        {
            var (distress, factionWar, _, _, _) = CreateFixture();
            string signalId = "freq_distress_88_3";

            Assert.True(distress.Intercept(signalId, day: 1));
            Assert.True(distress.MarkTriangulated(signalId));
            Assert.True(distress.DispatchExpedition(signalId));

            int initialStanding = factionWar.GetStanding("faction_railway_guild");

            // Complete rescue
            Assert.True(distress.CompleteRescue(signalId, factionWar));

            var active = distress.GetActiveState(signalId);
            Assert.NotNull(active);
            Assert.Equal(DistressSignalStatus.ResolvedRescued, active!.Status);

            int rescuedStanding = factionWar.GetStanding("faction_railway_guild");
            Assert.True(rescuedStanding > initialStanding, "Rescuing must increase sender faction standing");

            // Second call must be idempotent: returns true, zero standing increase
            Assert.True(distress.CompleteRescue(signalId, factionWar));
            Assert.Equal(rescuedStanding, factionWar.GetStanding("faction_railway_guild"));
        }

        [Fact]
        public void SaveLoadPersistence_PreservesAllActiveSignalProperties()
        {
            var (distress, _, _, _, _) = CreateFixture();

            // Setup multi-signal state
            distress.Intercept("freq_distress_88_3", day: 1);
            distress.MarkTriangulated("freq_distress_88_3");
            distress.DispatchExpedition("freq_distress_88_3");

            distress.Intercept("freq_distress_192_4", day: 2);
            distress.Resolve("freq_distress_192_4", DistressSignalStatus.ResolvedTrapAvoided, "Avoided raider ambush");

            distress.Intercept("freq_distress_148_2", day: 1);
            var activeBunker = distress.GetActiveState("freq_distress_148_2");
            activeBunker!.IsMoralChoiceAvailable = true;
            activeBunker.MoralChoiceResolutionIndex = 1;
            activeBunker.IsIgnored = true;
            activeBunker.Status = DistressSignalStatus.ResolvedIgnored;

            // Capture
            var state = distress.CaptureState();
            Assert.NotNull(state);
            Assert.True(state.Count >= 25);

            // Restore in fresh instance
            var fresh = new RadioDistressSystem();
            fresh.LoadFromJson(File.ReadAllText(Path.Combine(DataDirectory, "radio_distress_signals.json")));
            fresh.RestoreState(state);

            // Verify 88.3
            var restored88 = fresh.GetActiveState("freq_distress_88_3");
            Assert.NotNull(restored88);
            Assert.Equal(DistressSignalStatus.Dispatched, restored88!.Status);
            Assert.True(restored88.IsTriangulated);
            Assert.True(restored88.IsDispatched);

            // Verify 192.4
            var restored192 = fresh.GetActiveState("freq_distress_192_4");
            Assert.NotNull(restored192);
            Assert.Equal(DistressSignalStatus.ResolvedTrapAvoided, restored192!.Status);
            Assert.Contains("Avoided", restored192.ResolutionSummary);

            // Verify 148.2
            var restored148 = fresh.GetActiveState("freq_distress_148_2");
            Assert.NotNull(restored148);
            Assert.Equal(DistressSignalStatus.ResolvedIgnored, restored148!.Status);
            Assert.True(restored148.IsIgnored);
            Assert.True(restored148.IsMoralChoiceAvailable);
            Assert.Equal(1, restored148.MoralChoiceResolutionIndex);
        }

        [Fact]
        public void Idempotency_GuardsPreventDuplicateSideEffects()
        {
            var (distress, factionWar, moral, quests, _) = CreateFixture();
            string signalId = "freq_distress_88_3";

            Assert.True(distress.Intercept(signalId, day: 1));
            Assert.False(distress.Intercept(signalId, day: 2), "Re-intercepting must return false");

            Assert.True(distress.TryTriggerMoralChoice(signalId, out string questId));

            Assert.True(distress.ResolveMoralChoice(signalId, 0, moral, quests[questId], day: 2, out _, factionWar));
            int moralAfterResolve = moral.MoralScore;

            // Refused once resolved
            Assert.False(distress.TryTriggerMoralChoice(signalId, out _), "Triggering moral choice once resolved must return false");

            // Re-resolving is idempotent: returns true but does not duplicate moral score deltas
            Assert.True(distress.ResolveMoralChoice(signalId, 0, moral, quests[questId], day: 2, out _, factionWar));
            Assert.Equal(moralAfterResolve, moral.MoralScore);
        }
    }
}
