using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    public class MoralChoiceSystemTests
    {
        private static MoralChoiceSystem Sys(int seed = 42) => new MoralChoiceSystem(new SeededRng(seed));

        private static MoralChoiceQuestDefinition Quest(
            string id = "quest_moral_test",
            params (int moral, int empathy)[] deltas)
        {
            (int moral, int empathy)[] source = deltas.Length == 0
                ? new (int moral, int empathy)[] { (moral: 10, empathy: 1), (moral: 5, empathy: 0), (moral: 0, empathy: 0), (moral: -5, empathy: 0) }
                : deltas;
            var choices = source
                .Select(d => new MoralChoiceOption
                {
                    MoralDelta = d.moral,
                    EmpathyDelta = d.empathy,
                    Epitaph = $"chose {d.moral}"
                })
                .ToList();
            return new MoralChoiceQuestDefinition
            {
                Id = id,
                DisplayName = id,
                Category = "trust",
                Choices = choices
            };
        }

        [Fact]
        public void InitialStateNeutralAndEmpty()
        {
            var sys = Sys();
            Assert.Equal(0, sys.MoralScore);
            Assert.Equal(0, sys.EmpathyPoints);
            Assert.Equal(0, sys.QuestsResolved);
            Assert.Equal(MoralPathBand.Neutral, sys.CurrentBand);
            Assert.False(sys.IsListener);
            Assert.False(sys.IsConfidant);
            Assert.Equal(MoralEndingKind.BalancedSurvivor, sys.SelectEnding());
        }

        [Theory]
        [InlineData(-500, MoralPathBand.VeryEvil)]
        [InlineData(-200, MoralPathBand.VeryEvil)]
        [InlineData(-100, MoralPathBand.VeryEvil)]
        [InlineData(-99, MoralPathBand.Evil)]
        [InlineData(-50, MoralPathBand.Evil)]
        [InlineData(-49, MoralPathBand.SlightlyEvil)]
        [InlineData(-1, MoralPathBand.SlightlyEvil)]
        [InlineData(0, MoralPathBand.Neutral)]
        [InlineData(1, MoralPathBand.SlightlyPositive)]
        [InlineData(49, MoralPathBand.SlightlyPositive)]
        [InlineData(50, MoralPathBand.Positive)]
        [InlineData(99, MoralPathBand.Positive)]
        [InlineData(100, MoralPathBand.VeryPositive)]
        [InlineData(200, MoralPathBand.VeryPositive)]
        [InlineData(500, MoralPathBand.VeryPositive)]
        public void BandEdgesPinned(int score, MoralPathBand expected)
        {
            Assert.Equal(expected, MoralChoiceSystem.BandForScore(score));
        }

        [Fact]
        public void ResolveAppliesDeltasAndRaisesEvent()
        {
            var sys = Sys();
            MoralChoiceResolution? raised = null;
            sys.OnQuestResolved += r => raised = r;

            var resolution = sys.Resolve(Quest("quest_moral_share_child"), 0, "loc_urban_ruins_block_9", 12);

            Assert.Equal(10, sys.MoralScore);
            Assert.Equal(1, sys.EmpathyPoints);
            Assert.Equal(1, sys.QuestsResolved);
            Assert.Same(resolution, raised);
            Assert.Equal("quest_moral_share_child", resolution.questId);
            Assert.Equal(12, resolution.resolvedDay);
            Assert.Equal("up", resolution.impactMark);
            Assert.InRange(resolution.outcomeRoll, 0, 99);
            Assert.InRange(resolution.propagatesOnDay, 13, 15);
            Assert.True(sys.IsResolved("quest_moral_share_child"));
        }

        [Fact]
        public void ResolveIsIdempotentPerQuest()
        {
            var sys = Sys();
            var first = sys.Resolve(Quest("quest_moral_a"), 0, "loc_x", 5);

            var second = sys.Resolve(Quest("quest_moral_a"), 3, "loc_x", 40);

            Assert.Equal(1, sys.QuestsResolved);
            Assert.Equal(10, sys.MoralScore);
            Assert.Equal(first.outcomeRoll, second.outcomeRoll);
            Assert.Equal(first.propagatesOnDay, second.propagatesOnDay);
            Assert.Equal(first.choiceIndex, second.choiceIndex);
        }

        [Fact]
        public void ScoreClampsAtCapAndFiresLegendOncePerDirection()
        {
            var sys = Sys();
            var fired = new List<string>();
            sys.OnThresholdEventFired += fired.Add;

            sys.Resolve(Quest("quest_moral_cap_a", (250, 0)), 0, "", 1);
            Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore);
            Assert.Contains(MoralChoiceSystem.EventLegendPositive, fired);

            sys.Resolve(Quest("quest_moral_cap_b", (250, 0)), 0, "", 2);
            Assert.Equal(MoralChoiceSystem.MaxScore, sys.MoralScore);
            Assert.Single(fired.Where(e => e == MoralChoiceSystem.EventLegendPositive));

            sys.Resolve(Quest("quest_moral_cap_c", (-400, 0)), 0, "", 3);
            sys.Resolve(Quest("quest_moral_cap_d", (-400, 0)), 0, "", 4);
            Assert.Equal(MoralChoiceSystem.MinScore, sys.MoralScore);
            Assert.Single(fired.Where(e => e == MoralChoiceSystem.EventLegendNegative));
        }

        [Fact]
        public void ResolveRejectsNonCanonicalQuestId()
        {
            var sys = Sys();
            Assert.Throws<ArgumentException>(() => sys.Resolve(Quest("qst_moral_share_child"), 0, "", 1));
        }

        [Fact]
        public void ResolveRejectsOutOfRangeChoice()
        {
            var sys = Sys();
            var quest = Quest();
            Assert.Throws<ArgumentOutOfRangeException>(() => sys.Resolve(quest, -1, "", 1));
            Assert.Throws<ArgumentOutOfRangeException>(() => sys.Resolve(quest, 4, "", 1));
        }

        [Fact]
        public void ImpactMarksFollowDeltaSign()
        {
            var sys = Sys();
            Assert.Equal("up", sys.Resolve(Quest("quest_moral_up", (7, 0)), 0, "", 1).impactMark);
            Assert.Equal("flat", sys.Resolve(Quest("quest_moral_flat", (0, 0)), 0, "", 2).impactMark);
            Assert.Equal("down", sys.Resolve(Quest("quest_moral_down", (-7, 0)), 0, "", 3).impactMark);
        }

        [Fact]
        public void SameSeedSameRolls()
        {
            var ids = new[] { "quest_moral_r1", "quest_moral_r2", "quest_moral_r3" };
            var a = Sys(977);
            var b = Sys(977);

            for (int i = 0; i < ids.Length; i++)
            {
                var ra = a.Resolve(Quest(ids[i]), 0, "", i + 1);
                var rb = b.Resolve(Quest(ids[i]), 0, "", i + 1);
                Assert.Equal(ra.outcomeRoll, rb.outcomeRoll);
                Assert.Equal(ra.propagatesOnDay, rb.propagatesOnDay);
            }
        }

        [Fact]
        public void ReconcileFiresExtremeBandEventsOnce()
        {
            var sys = Sys();
            var fired = new List<string>();
            sys.OnThresholdEventFired += fired.Add;

            sys.Resolve(Quest("quest_moral_saint", (120, 0)), 0, "", 1);
            sys.Reconcile(2);
            Assert.Contains(MoralChoiceSystem.EventContractRaised, fired);
            Assert.Contains(MoralChoiceSystem.EventPatrolDefense, fired);

            sys.Reconcile(3);
            sys.Resolve(Quest("quest_moral_fall", (-250, 0)), 0, "", 4);
            sys.Reconcile(5);
            Assert.Contains(MoralChoiceSystem.EventBountyIssued, fired);

            sys.Resolve(Quest("quest_moral_return", (250, 0)), 0, "", 6);
            sys.Reconcile(7);

            // Insertion order: both VeryPositive events on entry, bounty on the
            // fall to VeryEvil; the return crossing re-fires nothing.
            Assert.Equal(
                new[] { MoralChoiceSystem.EventContractRaised, MoralChoiceSystem.EventPatrolDefense,
                        MoralChoiceSystem.EventBountyIssued },
                fired);
        }

        [Fact]
        public void ReconcileFiresContractAtPositiveBand()
        {
            var sys = Sys();
            var fired = new List<string>();
            sys.OnThresholdEventFired += fired.Add;

            sys.Resolve(Quest("quest_moral_savior", (60, 0)), 0, "", 1);
            sys.Reconcile(2);

            Assert.Equal(new[] { MoralChoiceSystem.EventContractTaken }, fired);
        }

        [Fact]
        public void ReconcileIgnoresOutOfOrderDays()
        {
            var sys = Sys();
            sys.Reconcile(10);
            sys.Reconcile(5);
            Assert.Equal(10, sys.State.lastReconciledDay);
        }

        [Fact]
        public void EndingStorykeeperOverridesBand()
        {
            var sys = Sys(31);
            for (int i = 0; i < 25; i++)
            {
                sys.Resolve(Quest($"quest_moral_arc_{i}", (-8, 2)), 0, "loc_arc", i + 1);
            }

            Assert.Equal(MoralChoiceSystem.MinScore, sys.MoralScore);
            Assert.Equal(50, sys.EmpathyPoints);
            Assert.Equal(MoralPathBand.VeryEvil, sys.CurrentBand);
            Assert.Equal(MoralEndingKind.Storykeeper, sys.SelectEnding());
        }

        [Theory]
        [InlineData(150, 19, MoralEndingKind.CommunityBuilder)]
        [InlineData(-150, 19, MoralEndingKind.NeutralSurvivor)]
        [InlineData(0, 19, MoralEndingKind.BalancedSurvivor)]
        [InlineData(150, 20, MoralEndingKind.SaintOfWasteland)]
        [InlineData(-150, 25, MoralEndingKind.Warlord)]
        [InlineData(-60, 25, MoralEndingKind.SurvivorKing)]
        [InlineData(-10, 20, MoralEndingKind.NeutralSurvivor)]
        [InlineData(0, 20, MoralEndingKind.BalancedSurvivor)]
        [InlineData(30, 20, MoralEndingKind.CommunityBuilder)]
        [InlineData(55, 22, MoralEndingKind.Savior)]
        [InlineData(150, 30, MoralEndingKind.SaintOfWasteland)]
        public void EndingSelectionRules(int score, int quests, MoralEndingKind expected)
        {
            Assert.Equal(expected, MoralChoiceSystem.SelectEnding(score, 0, quests));
        }

        [Fact]
        public void StorykeeperNeedsBothThresholds()
        {
            Assert.Equal(MoralEndingKind.Warlord, MoralChoiceSystem.SelectEnding(-150, 45, 24));
            Assert.Equal(MoralEndingKind.Warlord, MoralChoiceSystem.SelectEnding(-150, 44, 25));
            Assert.Equal(MoralEndingKind.Storykeeper, MoralChoiceSystem.SelectEnding(-150, 45, 25));
        }

        [Fact]
        public void SaveRoundTripPreservesLedger()
        {
            var sys = Sys(7);
            sys.Resolve(Quest("quest_moral_a", (30, 3)), 0, "loc_a", 5);
            sys.Resolve(Quest("quest_moral_b", (30, 3)), 0, "loc_b", 6);
            sys.Resolve(Quest("quest_moral_c", (250, 4)), 0, "loc_c", 7);
            sys.Reconcile(8);
            var snap = sys.CaptureState();

            var restored = Sys(1234);
            restored.RestoreState(snap);

            Assert.Equal(sys.MoralScore, restored.MoralScore);
            Assert.Equal(sys.EmpathyPoints, restored.EmpathyPoints);
            Assert.Equal(3, restored.QuestsResolved);
            Assert.Equal(sys.State.lastReconciledDay, restored.State.lastReconciledDay);
            Assert.Equal(sys.State.bandAtLastReconcile, restored.State.bandAtLastReconcile);
            Assert.Equal(sys.State.firedThresholdEvents, restored.State.firedThresholdEvents);
            for (int i = 0; i < snap.resolutions.Count; i++)
            {
                Assert.Equal(snap.resolutions[i].questId, restored.Resolutions[i].questId);
                Assert.Equal(snap.resolutions[i].outcomeRoll, restored.Resolutions[i].outcomeRoll);
                Assert.Equal(snap.resolutions[i].propagatesOnDay, restored.Resolutions[i].propagatesOnDay);
                Assert.Equal(snap.resolutions[i].impactMark, restored.Resolutions[i].impactMark);
            }

            restored.Resolve(Quest("quest_moral_a", (30, 3)), 0, "loc_a", 99);
            Assert.Equal(sys.MoralScore, restored.MoralScore);
            Assert.Equal(3, restored.QuestsResolved);
        }

        [Fact]
        public void CapturedStateIsDetached()
        {
            var sys = Sys();
            sys.Resolve(Quest("quest_moral_a", (30, 3)), 0, "loc_a", 5);
            var snap = sys.CaptureState();

            snap.resolutions.Clear();
            snap.firedThresholdEvents.Clear();
            snap.moralScore = -77;

            Assert.Equal(1, sys.QuestsResolved);
            Assert.Equal(30, sys.MoralScore);
        }

        [Fact]
        public void AvailabilityWindow()
        {
            var windowed = new MoralChoiceQuestDefinition { Id = "quest_moral_window", MinDay = 10, MaxDay = 30 };
            Assert.False(MoralChoiceSystem.IsAvailableOnDay(windowed, 9));
            Assert.True(MoralChoiceSystem.IsAvailableOnDay(windowed, 10));
            Assert.True(MoralChoiceSystem.IsAvailableOnDay(windowed, 30));
            Assert.False(MoralChoiceSystem.IsAvailableOnDay(windowed, 31));

            var openEnded = new MoralChoiceQuestDefinition { Id = "quest_moral_late", MinDay = 200, MaxDay = 0 };
            Assert.False(MoralChoiceSystem.IsAvailableOnDay(openEnded, 199));
            Assert.True(MoralChoiceSystem.IsAvailableOnDay(openEnded, 200));
            Assert.True(MoralChoiceSystem.IsAvailableOnDay(openEnded, 5000));

            var malformed = new MoralChoiceQuestDefinition { Id = "quest_moral_bad", MinDay = 30, MaxDay = 10 };
            Assert.False(MoralChoiceSystem.IsAvailableOnDay(malformed, 20));
        }
    }
}
