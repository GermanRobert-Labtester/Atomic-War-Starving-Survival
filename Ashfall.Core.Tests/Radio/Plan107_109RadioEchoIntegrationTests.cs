// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.MoralChoice;
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    /// <summary>
    /// Wave 41 Batch 2 Cross-System Integration Test:
    /// Validates Plan 107 (Radio Distress Signals Expansion - 5 baseline → 25 authored broadcasts)
    /// alongside Plan 109 (Moral Choice Echo Quests Expansion - 32 baseline → 60 echo quests).
    /// </summary>
    public sealed class Plan107_109RadioEchoIntegrationTests : CatalogTestBase
    {
        private static readonly string[] BaselineSignalIds =
        {
            "freq_distress_217_4", // Checkpoint Kilo Automated Beacon
            "freq_distress_148_2", // Civilian Bunker 4-East
            "freq_distress_392_7", // Automated Weather Station Gamma
            "freq_distress_55_1",  // The Pianist's Last Broadcast
            "freq_distress_401_9"  // Military Convoy Echo-7
        };

        private static List<DistressSignalDefinition> LoadDistressCatalog(string dataDir)
        {
            string path = Path.Combine(dataDir, "radio_distress_signals.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var list = new List<DistressSignalDefinition>();
            foreach (var elem in doc.RootElement.GetProperty("radio_broadcasts").EnumerateArray())
            {
                var def = JsonSerializer.Deserialize<DistressSignalDefinition>(
                    elem.GetRawText(), SystemTextJsonSerializer.Options);
                if (def != null) list.Add(def);
            }
            return list;
        }

        [Fact]
        public void Plan107_RadioDistressSignals_LoadsAllTwentyFiveBroadcasts_WithValidGrammarAndFragments()
        {
            var signals = LoadDistressCatalog(DataDirectory);
            Assert.NotNull(signals);
            Assert.Equal(25, signals.Count);

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var validOutcomeTypes = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "survivor_community",
                "bait_trap",
                "knowledge",
                "narrative",
                "supply_cache",
                "false_flag",
                "encrypted",
                "child_voice",
                "military"
            };

            foreach (var sig in signals)
            {
                Assert.NotNull(sig);
                Assert.False(string.IsNullOrWhiteSpace(sig.FrequencyId), "FrequencyId must not be empty.");
                Assert.StartsWith("freq_distress_", sig.FrequencyId, StringComparison.OrdinalIgnoreCase);
                Assert.True(seenIds.Add(sig.FrequencyId), $"Duplicate frequency_id detected: {sig.FrequencyId}");

                Assert.False(string.IsNullOrWhiteSpace(sig.SourceName), $"SourceName missing for {sig.FrequencyId}");
                Assert.True(sig.FrequencyMhz > 0f, $"FrequencyMhz must be positive for {sig.FrequencyId}");
                Assert.Contains(sig.OutcomeTypeStr, validOutcomeTypes);
                Assert.True(sig.DaysToTrace >= 2, $"DaysToTrace must be >= 2 for {sig.FrequencyId}");

                Assert.NotNull(sig.MessageFragments);
                Assert.NotEmpty(sig.MessageFragments);
                Assert.Equal(sig.DaysToTrace, sig.MessageFragments.Count);

                float previousClarity = -1f;
                for (int i = 0; i < sig.MessageFragments.Count; i++)
                {
                    var frag = sig.MessageFragments[i];
                    Assert.NotNull(frag);
                    Assert.Equal(i + 1, frag.Day);
                    Assert.False(string.IsNullOrWhiteSpace(frag.Text), $"Fragment text missing for {sig.FrequencyId} Day {frag.Day}");
                    Assert.True(frag.Clarity >= 0f && frag.Clarity <= 1.0f, $"Clarity out of bounds for {sig.FrequencyId} Day {frag.Day}");
                    Assert.True(frag.Clarity >= previousClarity, $"Clarity must not regress for {sig.FrequencyId} Day {frag.Day}");
                    previousClarity = frag.Clarity;
                }
            }

            // Verify all 5 baseline signals remain preserved
            foreach (var baselineId in BaselineSignalIds)
            {
                Assert.Contains(baselineId, seenIds);
            }
        }

        [Fact]
        public void Plan107_RadioDistressSystem_ExecutesFullLifecycle_Deterministically()
        {
            var system = new RadioDistressSystem();
            var signals = LoadDistressCatalog(DataDirectory);

            foreach (var sig in signals)
            {
                system.RegisterSignal(sig);
            }

            Assert.True(system.TotalRegisteredSignals >= 25);

            // Test Checkpoint Kilo lifecycle
            string kiloId = "freq_distress_217_4";
            var def = system.GetDefinition(kiloId);
            Assert.NotNull(def);
            Assert.Equal("Checkpoint Kilo Automated Beacon", def!.SourceName);
            Assert.Equal(4, def.DaysToTrace);

            var active = system.GetActiveState(kiloId);
            Assert.NotNull(active);
            Assert.Equal(DistressSignalStatus.Inactive, active!.Status);

            bool intercepted = system.Intercept(kiloId, day: 12);
            Assert.True(intercepted);
            Assert.Equal(DistressSignalStatus.Intercepted, active.Status);
            Assert.Equal(12, active.InterceptedDay);

            bool triangulated = system.MarkTriangulated(kiloId);
            Assert.True(triangulated);
            Assert.Equal(DistressSignalStatus.Triangulated, active.Status);

            bool dispatched = system.DispatchExpedition(kiloId);
            Assert.True(dispatched);
            Assert.Equal(DistressSignalStatus.Dispatched, active.Status);

            bool resolved = system.Resolve(kiloId, DistressSignalStatus.ResolvedRescued, "Armory accessed and survivors secured.");
            Assert.True(resolved);
            Assert.Equal(DistressSignalStatus.ResolvedRescued, active.Status);
            Assert.True(active.IsResolved);
        }

        [Fact]
        public void Plan109_MoralChoiceEchoQuests_LoadsAllSixtyEchoQuests_WithValidTriggersAndBranches()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(chainData);
            Assert.NotNull(chainData.EchoQuests);
            Assert.Equal(60, chainData.EchoQuests.Count);

            var seenQuestIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var validBranches = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "branch_mercy_road",
                "branch_iron_way",
                "branch_listener_thread",
                "branch_broken_compact",
                "" // branch-agnostic echoes
            };

            foreach (var echo in chainData.EchoQuests)
            {
                Assert.NotNull(echo);
                Assert.False(string.IsNullOrWhiteSpace(echo.QuestId), "Echo QuestId must not be empty.");
                Assert.StartsWith("quest_moral_echo_", echo.QuestId, StringComparison.OrdinalIgnoreCase);
                Assert.True(seenQuestIds.Add(echo.QuestId), $"Duplicate echo questId detected: {echo.QuestId}");

                Assert.False(string.IsNullOrWhiteSpace(echo.TriggeredBy), $"TriggeredBy missing for {echo.QuestId}");
                Assert.True(echo.MinDaysAfter >= 0, $"MinDaysAfter must be >= 0 for {echo.QuestId}");
                Assert.Contains(echo.Branch ?? "", validBranches);
            }

            // Verify the 4 distinct branches have authored echo representations
            var branchCounts = chainData.EchoQuests
                .GroupBy(e => e.Branch ?? "")
                .ToDictionary(g => g.Key, g => g.Count());

            Assert.True(branchCounts.ContainsKey("branch_mercy_road") && branchCounts["branch_mercy_road"] >= 8);
            Assert.True(branchCounts.ContainsKey("branch_iron_way") && branchCounts["branch_iron_way"] >= 8);
            Assert.True(branchCounts.ContainsKey("branch_listener_thread") && branchCounts["branch_listener_thread"] >= 7);
            Assert.True(branchCounts.ContainsKey("branch_broken_compact") && branchCounts["branch_broken_compact"] >= 5);
        }

        [Fact]
        public void Plan107_109_CrossSystem_SignalIntelligenceAndMoralEchoes_SynergyContract()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var signals = LoadDistressCatalog(DataDirectory);
            var chainData = MoralChoiceChainCatalogLoader.Load(DataDirectory, files, json);

            Assert.Equal(25, signals.Count);
            Assert.Equal(60, chainData.EchoQuests.Count);

            // Verify that deceptive and moral signals have narrative IDs and clear moral stakes
            var trapSignal = signals.FirstOrDefault(s => s.FrequencyId == "freq_distress_148_2");
            Assert.NotNull(trapSignal);
            Assert.True(trapSignal!.IsTrapOrDeception);
            Assert.Equal("bait_trap", trapSignal.OutcomeTypeStr);
            Assert.False(string.IsNullOrWhiteSpace(trapSignal.WarningText));
            Assert.False(string.IsNullOrWhiteSpace(trapSignal.NarrativeId));

            // Verify genuine rescue signals have survivor or community outcomes
            var communitySignal = signals.FirstOrDefault(s => s.FrequencyId == "freq_distress_217_4");
            Assert.NotNull(communitySignal);
            Assert.Equal("survivor_community", communitySignal!.OutcomeTypeStr);
            Assert.NotEmpty(communitySignal.RevealedItems);

            // Verify that moral echo quests span both early and long-term campaign delay windows (5 to 90 days)
            int minPacing = chainData.EchoQuests.Min(e => e.MinDaysAfter);
            int maxPacing = chainData.EchoQuests.Max(e => e.MinDaysAfter);
            Assert.True(minPacing >= 5, "Minimum delayed callback should be at least 5 days.");
            Assert.True(maxPacing <= 90, "Maximum delayed callback should resolve within 90 days of choice.");

            // Verify determinism: independent loads yield matching collections
            var signals2 = LoadDistressCatalog(DataDirectory);
            var chainData2 = MoralChoiceChainCatalogLoader.Load(DataDirectory, files, json);
            Assert.Equal(signals.Count, signals2.Count);
            Assert.Equal(chainData.EchoQuests.Count, chainData2.EchoQuests.Count);
        }
    }
}
