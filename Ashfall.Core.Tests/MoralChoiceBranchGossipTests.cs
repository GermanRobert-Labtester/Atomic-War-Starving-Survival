using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    public sealed class MoralChoiceBranchGossipTests : CatalogTestBase
    {
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        // ── Chain catalog loader ────────────────────────────────────────

        [Fact]
        public void ChainCatalogLoadsFourBranches()
        {
            var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(4, data.Branches.Count);
            Assert.Contains(data.Branches, b => b.Id == "branch_mercy_road");
            Assert.Contains(data.Branches, b => b.Id == "branch_iron_way");
            Assert.Contains(data.Branches, b => b.Id == "branch_listener_thread");
            Assert.Contains(data.Branches, b => b.Id == "branch_broken_compact");
        }

        [Fact]
        public void ChainCatalogHasQuestGates()
        {
            var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.NotEmpty(data.QuestGates);
            Assert.All(data.QuestGates, g =>
            {
                Assert.False(string.IsNullOrWhiteSpace(g.QuestId));
                Assert.False(string.IsNullOrWhiteSpace(g.Branch));
            });
        }

        [Fact]
        public void ChainCatalogHasEchoQuests()
        {
            var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.NotEmpty(data.EchoQuests);
            Assert.All(data.EchoQuests, e =>
            {
                Assert.False(string.IsNullOrWhiteSpace(e.QuestId));
                Assert.False(string.IsNullOrWhiteSpace(e.TriggeredBy));
                Assert.True(e.MinDaysAfter > 0);
            });
        }

        [Fact]
        public void ChainCatalogLockoutRulesArePermanent()
        {
            var data = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.True(data.LockoutRules.LockoutIsPermanent);
            Assert.True(data.LockoutRules.LockoutFiresJournalEntry);
        }

        [Fact]
        public void ChainCatalogMissingFileReturnsEmpty()
        {
            var data = MoralChoiceChainCatalogLoader.Load("/no/such/dir", s_files, s_json);
            Assert.Empty(data.Branches);
            Assert.Empty(data.QuestGates);
        }

        // ── Branching quest catalog loader ──────────────────────────────

        [Fact]
        public void BranchingQuestsLoadAllFourChains()
        {
            var quests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(100, quests.Count);
            Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_mercy_")));
            Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_iron_")));
            Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_listen_")));
            Assert.Equal(25, quests.Count(q => q.Id.StartsWith("quest_moral_chain_betray_")));
        }

        [Fact]
        public void BranchingQuestsAllChainsComplete()
        {
            var quests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(100, quests.Count);
            foreach (string prefix in new[] { "mercy", "iron", "listen", "betray" })
            {
                for (int i = 1; i <= 25; i++)
                {
                    Assert.Contains(quests, q => q.Id == $"quest_moral_chain_{prefix}_{i:D2}");
                }
            }
        }

        [Fact]
        public void BranchingQuestsHaveValidChoices()
        {
            var quests = MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.All(quests, q =>
            {
                Assert.InRange(q.Choices.Count, 3, 4);
                Assert.False(string.IsNullOrWhiteSpace(q.DisplayName));
                Assert.False(string.IsNullOrWhiteSpace(q.Discovery));
                foreach (var c in q.Choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(c.Label));
                    Assert.False(string.IsNullOrWhiteSpace(c.Epitaph));
                }
            });
        }

        // ── Expansion quest catalog loader ──────────────────────────────

        [Fact]
        public void ExpansionQuestsLoadFiftyQuests()
        {
            var quests = MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(50, quests.Count);
            Assert.All(quests, q => Assert.StartsWith("quest_moral_", q.Id));
        }

        [Fact]
        public void ExpansionQuestIdsMatchStaticList()
        {
            var quests = MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json);
            var catalogIds = quests.Select(q => q.Id).ToHashSet();
            var staticIds = MoralChoiceIds.AllExpansion.ToHashSet();
            Assert.True(catalogIds.SetEquals(staticIds),
                "Expansion catalog and MoralChoiceIds.AllExpansion must match");
        }

        // ── Gossip catalog loader ───────────────────────────────────────

        [Fact]
        public void GossipCatalogLoadsAllBands()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.NotEmpty(data.CampChatter.VeryPositive);
            Assert.NotEmpty(data.CampChatter.Positive);
            Assert.NotEmpty(data.CampChatter.Neutral);
            Assert.NotEmpty(data.CampChatter.Evil);
            Assert.NotEmpty(data.CampChatter.VeryEvil);
        }

        [Fact]
        public void GossipCatalogHasNpcGreetings()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.NotEmpty(data.NpcGreetingShifts.VeryPositive);
            Assert.NotEmpty(data.NpcGreetingShifts.Neutral);
            Assert.NotEmpty(data.NpcGreetingShifts.VeryEvil);
        }

        [Fact]
        public void GossipCatalogHasDecayRules()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(30, data.GossipDecay.DecayIntervalDays);
            Assert.Equal(60, data.GossipDecay.FullDecayDays);
            Assert.Equal(10, data.GossipDecay.DramaticResetThreshold);
        }

        // ── Faction reactions catalog loader ────────────────────────────

        [Fact]
        public void FactionReactionsLoadAllThresholdEvents()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Contains("moral_event_bounty_issued", data.ThresholdReactions.Keys);
            Assert.Contains("moral_event_contract_taken", data.ThresholdReactions.Keys);
            Assert.Contains("moral_event_contract_raised", data.ThresholdReactions.Keys);
            Assert.Contains("moral_event_legend_positive", data.ThresholdReactions.Keys);
            Assert.Contains("moral_event_legend_negative", data.ThresholdReactions.Keys);
        }

        [Fact]
        public void FactionReactionsHaveDialogue()
        {
            var data = MoralChoiceFactionReactionsCatalogLoader.Load(DataDirectory, s_files, s_json);
            var bounty = data.ThresholdReactions["moral_event_bounty_issued"];
            Assert.NotEmpty(bounty.PeacekeeperDialogue);
            Assert.NotEmpty(bounty.RaiderDialogue);
            Assert.False(string.IsNullOrWhiteSpace(bounty.JournalEntry));
        }

        // ── Flag catalog loader ─────────────────────────────────────────

        [Fact]
        public void FlagCatalogLoadsTenFlags()
        {
            var data = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(10, data.Flags.Count);
            Assert.All(data.Flags, f =>
            {
                Assert.StartsWith("flag_", f.Id);
                Assert.False(string.IsNullOrWhiteSpace(f.DisplayName));
            });
        }

        [Fact]
        public void FlagCatalogIdsMatchStaticList()
        {
            var data = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json);
            var catalogFlagIds = data.Flags.Select(f => f.Id).ToHashSet();
            // AllFlags has 11 entries (10 from JSON + FlagMessengerKept from code).
            // The JSON flags should be a subset of AllFlags.
            foreach (var id in catalogFlagIds)
            {
                Assert.Contains(id, MoralChoiceIds.AllFlags);
            }
        }

        // ── Branch tracking in MoralChoiceSystem ────────────────────────

        [Fact]
        public void BranchTracking_LocksOutOpposingBranches()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            var lockedBranches = new List<string>();
            sys.OnBranchLocked += lockedBranches.Add;

            // Resolve 3 Mercy Road entry quests → should lock Iron Way + Broken Compact
            var mercyEntry = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_01",
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices2(10, 2)
            };
            sys.Resolve(mercyEntry, 0, "", 10);

            var mercyEntry2 = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_02",
                DisplayName = "test",
                Category = "comfort",
                Choices = MakeChoices2(10, 2)
            };
            sys.Resolve(mercyEntry2, 0, "", 20);

            Assert.Equal(2, sys.GetBranchProgress(MoralChoiceIds.BranchMercyRoad));
            Assert.False(sys.IsBranchLocked(MoralChoiceIds.BranchIronWay));

            var mercyEntry3 = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_chain_mercy_03",
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices2(10, 2)
            };
            sys.Resolve(mercyEntry3, 0, "", 30);

            Assert.Equal(3, sys.GetBranchProgress(MoralChoiceIds.BranchMercyRoad));
            Assert.True(sys.IsBranchLocked(MoralChoiceIds.BranchIronWay));
            Assert.True(sys.IsBranchLocked(MoralChoiceIds.BranchBrokenCompact));
            Assert.False(sys.IsBranchLocked(MoralChoiceIds.BranchMercyRoad));
            Assert.False(sys.IsBranchLocked(MoralChoiceIds.BranchListenerThread));

            Assert.Contains(MoralChoiceIds.BranchIronWay, lockedBranches);
            Assert.Contains(MoralChoiceIds.BranchBrokenCompact, lockedBranches);
        }

        [Fact]
        public void BranchTracking_LockedBranchBlocksAccessibility()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            // Lock Iron Way by resolving 3 mercy entry quests
            ResolveEntryQuests(sys, "mercy", 3);

            Assert.True(sys.IsBranchLocked(MoralChoiceIds.BranchIronWay));
            Assert.False(sys.IsChainQuestAccessible("quest_moral_chain_iron_01", 100));
        }

        [Fact]
        public void BranchTracking_GateRequiresMoralThreshold()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            // quest_moral_chain_mercy_04 requires min_moral 15 and prior quest_03
            // Without resolving prerequisites, gate should fail
            Assert.False(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));

            // Resolve prerequisites and boost moral score
            ResolveEntryQuests(sys, "mercy", 3);
            ResolveChainQuests(sys, "mercy", 3); // resolve 01, 02, 03

            // Now boost moral to 15+
            var boost = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_boost",
                DisplayName = "boost",
                Category = "share",
                Choices = MakeChoices2(20, 0)
            };
            sys.Resolve(boost, 0, "", 50);

            // Gate should now pass (moral >= 15, prerequisites resolved)
            Assert.True(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));
        }

        [Fact]
        public void BranchTracking_GateRequiresPriorQuestResolved()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            // quest_moral_chain_mercy_04 requires quest_moral_chain_mercy_03 resolved
            // Without resolving it, gate should fail even with high moral
            var boost = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_gate_boost",
                DisplayName = "boost",
                Category = "share",
                Choices = MakeChoices2(50, 0)
            };
            sys.Resolve(boost, 0, "", 1);
            Assert.True(sys.MoralScore >= 15);

            // Gate should fail because prerequisite quest not resolved
            Assert.False(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));

            // Now resolve the prerequisite chain (entry quests 01-03)
            ResolveEntryQuests(sys, "mercy", 3);

            // Gate should now pass (prerequisites resolved + moral >= 15)
            Assert.True(sys.IsChainQuestAccessible("quest_moral_chain_mercy_04", 100));
        }

        [Fact]
        public void BranchTracking_BranchLockFlagsAreSet()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            ResolveEntryQuests(sys, "mercy", 3);

            Assert.True(sys.HasFlag(MoralChoiceIds.FlagIronWayLocked));
            Assert.True(sys.HasFlag(MoralChoiceIds.FlagBrokenCompactLocked));
            Assert.False(sys.HasFlag(MoralChoiceIds.FlagMercyRoadLocked));
        }

        // ── Echo quest availability ─────────────────────────────────────

        [Fact]
        public void EchoQuests_AvailableAfterTriggerAndDelay()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            // Resolve quest_moral_share_child with choice 0 (best option)
            var childQuest = new MoralChoiceQuestDefinition
            {
                Id = MoralChoiceIds.ShareChild,
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices2(10, 2)
            };
            sys.Resolve(childQuest, 0, "loc_test", 10);

            // Echo quest_moral_echo_child_returns needs choice 0 + 30 days
            // At day 30 (10 + 20), should NOT be available yet
            var available = sys.FindAvailableEchoQuests(30);
            Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_child_returns");

            // At day 40 (10 + 30), should be available
            available = sys.FindAvailableEchoQuests(40);
            Assert.Contains(available, e => e.QuestId == "quest_moral_echo_child_returns");
        }

        [Fact]
        public void EchoQuests_NotAvailableForWrongChoice()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            // Resolve quest_moral_share_child with choice 3 (refuse)
            var childQuest = new MoralChoiceQuestDefinition
            {
                Id = MoralChoiceIds.ShareChild,
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices4(10, 2, 5, 1, 0, 0, -5, 0)
            };
            sys.Resolve(childQuest, 3, "loc_test", 10);

            // Echo child_returns needs choice 0 — should NOT fire for choice 3
            var available = sys.FindAvailableEchoQuests(100);
            Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_child_returns");

            // Echo child_steals needs choice 3 — should fire
            Assert.Contains(available, e => e.QuestId == "quest_moral_echo_child_steals");
        }

        [Fact]
        public void EchoQuests_MarkFiredPreventsRefire()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            var childQuest = new MoralChoiceQuestDefinition
            {
                Id = MoralChoiceIds.ShareChild,
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices2(10, 2)
            };
            sys.Resolve(childQuest, 0, "loc_test", 10);

            sys.MarkEchoQuestFired("quest_moral_echo_child_returns");

            var available = sys.FindAvailableEchoQuests(100);
            Assert.DoesNotContain(available, e => e.QuestId == "quest_moral_echo_child_returns");
        }

        // ── Gossip runtime ──────────────────────────────────────────────

        [Fact]
        public void GossipRuntime_ReturnsCorrectBandChatter()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));

            var positive = runtime.GetCampChatter(MoralPathBand.VeryPositive);
            Assert.NotEmpty(positive);

            var evil = runtime.GetCampChatter(MoralPathBand.VeryEvil);
            Assert.NotEmpty(evil);

            var neutral = runtime.GetCampChatter(MoralPathBand.Neutral);
            Assert.NotEmpty(neutral);
        }

        [Fact]
        public void GossipRuntime_PickReturnsNonEmpty()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));

            Assert.False(string.IsNullOrWhiteSpace(runtime.PickCampChatter(MoralPathBand.Positive)));
            Assert.False(string.IsNullOrWhiteSpace(runtime.PickNpcGreeting(MoralPathBand.Evil)));
            Assert.False(string.IsNullOrWhiteSpace(runtime.PickWhisper(MoralPathBand.VeryEvil)));
        }

        [Fact]
        public void GossipRuntime_DecayToNeutralAfterFullDecay()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));

            var sys = new MoralChoiceSystem(new SeededRng(42));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_gossip_test",
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices2(30, 0)
            };
            sys.Resolve(quest, 0, "", 10);

            Assert.Equal(MoralPathBand.SlightlyPositive, sys.CurrentBand);

            // After full decay from the day gossip actually propagates (not
            // from resolvedDay — the wasteland hasn't heard yet on day 10),
            // gossip should be neutral.
            int propagatesOnDay = sys.Resolutions[0].propagatesOnDay;
            var effective = runtime.GetEffectiveGossipBand(sys, propagatesOnDay + 60);
            Assert.Equal(MoralPathBand.Neutral, effective);
        }

        [Fact]
        public void GossipRuntime_DecayOneLevelAfterInterval()
        {
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));

            var sys = new MoralChoiceSystem(new SeededRng(42));
            // Use a small moral delta (< dramatic threshold of 10) so decay kicks in
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_gossip_decay_test",
                DisplayName = "test",
                Category = "share",
                Choices = new List<MoralChoiceOption>
                {
                    new() { MoralDelta = 60, EmpathyDelta = 0, Epitaph = "big" },
                    new() { MoralDelta = 8, EmpathyDelta = 0, Epitaph = "small" },
                    new() { MoralDelta = 0, EmpathyDelta = 0, Epitaph = "none" },
                    new() { MoralDelta = -5, EmpathyDelta = 0, Epitaph = "bad" }
                }
            };
            // Resolve with choice 1 (delta=8, below dramatic threshold of 10)
            sys.Resolve(quest, 1, "", 10);

            // Score 8 → SlightlyPositive
            Assert.Equal(MoralPathBand.SlightlyPositive, sys.CurrentBand);

            // After 30 days (decay interval), non-dramatic → decay one level to Neutral
            var effective = runtime.GetEffectiveGossipBand(sys, 10 + 31);
            Assert.Equal(MoralPathBand.Neutral, effective);
        }

        [Fact]
        public void GossipRuntime_StaysNeutralBeforePropagation()
        {
            // A resolution's consequences must not reach camp chatter before
            // MoralChoiceResolution.propagatesOnDay — resolvedDay + 1..3 with
            // this seed. Checking the very next day (resolvedDay + 1) must
            // still show Neutral if propagatesOnDay lands later than that.
            var data = MoralChoiceGossipCatalogLoader.Load(DataDirectory, s_files, s_json);
            var runtime = new MoralChoiceGossipRuntime(data, new SeededRng(42));

            var sys = new MoralChoiceSystem(new SeededRng(1));
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_gossip_propagation_test",
                DisplayName = "test",
                Category = "share",
                Choices = MakeChoices2(60, 0)
            };
            sys.Resolve(quest, 0, "", 10);

            var resolution = sys.Resolutions[0];
            Assert.True(resolution.propagatesOnDay > resolution.resolvedDay,
                "propagatesOnDay must be strictly after resolvedDay for this test to be meaningful");

            // The instant the choice resolves, gossip has nothing to work
            // with yet — the wasteland has not heard.
            var immediately = runtime.GetEffectiveGossipBand(sys, resolution.resolvedDay);
            Assert.Equal(MoralPathBand.Neutral, immediately);

            // One day before propagation (if there's a gap to test), still neutral.
            if (resolution.propagatesOnDay - 1 > resolution.resolvedDay)
            {
                var stillWaiting = runtime.GetEffectiveGossipBand(sys, resolution.propagatesOnDay - 1);
                Assert.Equal(MoralPathBand.Neutral, stillWaiting);
            }

            // On the day it propagates, gossip reflects the actual band.
            var propagated = runtime.GetEffectiveGossipBand(sys, resolution.propagatesOnDay);
            Assert.Equal(sys.CurrentBand, propagated);
        }

        // ── Save round-trip with new fields ─────────────────────────────

        [Fact]
        public void SaveRoundTrip_PreservesBranchTracking()
        {
            var sys = new MoralChoiceSystem(new SeededRng(42));
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, s_files, s_json);
            sys.InitializeChainData(chainData);

            ResolveEntryQuests(sys, "mercy", 3);
            sys.SetFlag(MoralChoiceIds.FlagMessengerKept);
            sys.MarkEchoQuestFired("quest_moral_echo_child_returns");

            var snap = sys.CaptureState();

            var restored = new MoralChoiceSystem(new SeededRng(99));
            restored.InitializeChainData(chainData);
            restored.RestoreState(snap);

            Assert.True(restored.IsBranchLocked(MoralChoiceIds.BranchIronWay));
            Assert.True(restored.IsBranchLocked(MoralChoiceIds.BranchBrokenCompact));
            Assert.Equal(3, restored.GetBranchProgress(MoralChoiceIds.BranchMercyRoad));
            Assert.True(restored.HasFlag(MoralChoiceIds.FlagMessengerKept));
            Assert.Contains("quest_moral_echo_child_returns", restored.State.firedEchoQuests);
        }

        // ── Static IDs ──────────────────────────────────────────────────

        [Fact]
        public void StaticIds_AllChainHasOneHundredEntries()
        {
            Assert.Equal(100, MoralChoiceIds.AllChain.Length);
            Assert.Equal(25, MoralChoiceIds.ChainMercy.Length);
            Assert.Equal(25, MoralChoiceIds.ChainIron.Length);
            Assert.Equal(25, MoralChoiceIds.ChainListen.Length);
            Assert.Equal(25, MoralChoiceIds.ChainBetray.Length);
        }

        [Fact]
        public void StaticIds_AllExpansionHasFiftyEntries()
        {
            Assert.Equal(50, MoralChoiceIds.AllExpansion.Length);
            Assert.All(MoralChoiceIds.AllExpansion, id =>
                Assert.StartsWith("quest_moral_", id));
        }

        [Fact]
        public void StaticIds_AllFlagsHasElevenEntries()
        {
            Assert.Equal(11, MoralChoiceIds.AllFlags.Length);
            Assert.All(MoralChoiceIds.AllFlags, id =>
                Assert.StartsWith("flag_", id));
        }

        [Fact]
        public void StaticIds_AllBranchesHasFourEntries()
        {
            Assert.Equal(4, MoralChoiceIds.AllBranches.Length);
        }

        [Fact]
        public void StaticIds_ChainQuestsFollowNamingPattern()
        {
            Assert.All(MoralChoiceIds.ChainMercy, id =>
                Assert.Matches(@"^quest_moral_chain_mercy_\d{2}$", id));
            Assert.All(MoralChoiceIds.ChainIron, id =>
                Assert.Matches(@"^quest_moral_chain_iron_\d{2}$", id));
            Assert.All(MoralChoiceIds.ChainListen, id =>
                Assert.Matches(@"^quest_moral_chain_listen_\d{2}$", id));
            Assert.All(MoralChoiceIds.ChainBetray, id =>
                Assert.Matches(@"^quest_moral_chain_betray_\d{2}$", id));
        }

        // ── Helpers ─────────────────────────────────────────────────────

        private static List<MoralChoiceOption> MakeChoices(params (int moral, int empathy)[] deltas)
        {
            return deltas.Select(d => new MoralChoiceOption
            {
                MoralDelta = d.moral,
                EmpathyDelta = d.empathy,
                Epitaph = $"chose {d.moral}"
            }).ToList();
        }

        private static List<MoralChoiceOption> MakeChoices2(int m1, int e1) =>
            MakeChoices((m1, e1), (m1 / 2, e1 > 0 ? e1 - 1 : 0), (0, 0), (-m1 / 2, 0));

        private static List<MoralChoiceOption> MakeChoices4(int m1, int e1, int m2, int e2, int m3, int e3, int m4, int e4) =>
            MakeChoices((m1, e1), (m2, e2), (m3, e3), (m4, e4));

        private static void ResolveEntryQuests(MoralChoiceSystem sys, string chain, int count)
        {
            string[] prefixes = { "mercy", "iron", "listen", "betray" };
            string prefix = chain.Length <= 4 ? chain : chain switch
            {
                "mercy" => "mercy",
                "iron" => "iron",
                "listen" => "listen",
                "betray" => "betray",
                _ => chain
            };

            string[] categories = { "share", "comfort", "share" };
            for (int i = 1; i <= count; i++)
            {
                var quest = new MoralChoiceQuestDefinition
                {
                    Id = $"quest_moral_chain_{prefix}_{i:D2}",
                    DisplayName = "test",
                    Category = categories[(i - 1) % categories.Length],
                    Choices = MakeChoices4(10, 2, 5, 1, 0, 0, -5, 0)
                };
                sys.Resolve(quest, 0, "", i * 10);
            }
        }

        private static void ResolveChainQuests(MoralChoiceSystem sys, string chain, int count)
        {
            ResolveEntryQuests(sys, chain, count);
        }
    }
}
