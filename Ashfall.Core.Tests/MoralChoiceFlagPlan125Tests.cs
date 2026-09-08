using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.MoralChoice;

namespace Ashfall.Core.Tests
{
    public sealed class MoralChoiceFlagPlan125Tests : CatalogTestBase
    {
        private static readonly IFileIO s_files = new FileSystemIO();
        private static readonly IJsonSerializer s_json = new SystemTextJsonSerializer();

        private static readonly string[] s_newFlags =
        {
            "flag_spared_raider",
            "flag_executed_prisoner",
            "flag_shared_rations",
            "flag_hoarded_medicine",
            "flag_sheltered_refugee",
            "flag_expelled_survivor",
            "flag_repaired_infrastructure",
            "flag_sabotaged_rival",
            "flag_broke_treaty",
            "flag_honored_debt",
            "flag_ignored_distress",
            "flag_responded_distress",
            "flag_forged_record",
            "flag_preserved_archive",
            "flag_chosen_faction_side"
        };

        [Fact]
        public void CatalogContainsExactlyTwentyFiveUniqueHistoricalFlags()
        {
            var definitions = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json);
            Assert.Equal(25, definitions.Flags.Count);
            Assert.Equal(25, definitions.Flags.Select(f => f.Id).Distinct(StringComparer.Ordinal).Count());
            Assert.All(definitions.Flags, flag =>
            {
                Assert.StartsWith("flag_", flag.Id, StringComparison.Ordinal);
                Assert.False(string.IsNullOrWhiteSpace(flag.DisplayName));
            });
            Assert.All(s_newFlags, id => Assert.Contains(definitions.Flags, flag => flag.Id == id));
        }

        [Fact]
        public void CatalogIdsAreSynchronizedWithStaticIds()
        {
            var catalogIds = MoralChoiceFlagCatalogLoader.Load(DataDirectory, s_files, s_json)
                .Flags.Select(f => f.Id).ToHashSet(StringComparer.Ordinal);
            var staticIds = MoralChoiceIds.AllFlags.ToHashSet(StringComparer.Ordinal);

            Assert.Contains(MoralChoiceIds.FlagMessengerKept, staticIds);
            Assert.True(catalogIds.IsSubsetOf(staticIds));
            Assert.Equal(26, MoralChoiceIds.AllFlags.Length);
        }

        [Fact]
        public void EveryPlan125FlagHasARealMoralChoiceProducer()
        {
            var quests = new List<MoralChoiceQuestDefinition>();
            quests.AddRange(MoralChoiceCatalogLoader.LoadStubs(DataDirectory, s_files, s_json));
            quests.AddRange(MoralChoiceBranchQuestCatalogLoader.Load(DataDirectory, s_files, s_json));
            quests.AddRange(MoralChoiceExpansionQuestCatalogLoader.Load(DataDirectory, s_files, s_json));

            var producers = quests
                .SelectMany(q => q.Choices.Select((choice, index) => new { q.Id, index, choice.SetFlag }))
                .Where(p => !string.IsNullOrWhiteSpace(p.SetFlag))
                .GroupBy(p => p.SetFlag, StringComparer.Ordinal)
                .ToDictionary(g => g.Key, g => g.ToList(), StringComparer.Ordinal);

            foreach (string flag in s_newFlags)
            {
                Assert.True(producers.TryGetValue(flag, out var matches), $"No producer for {flag}");
                Assert.NotEmpty(matches!);
            }
        }

        [Fact]
        public void ConfiguredProducerWritesFlagOnlyAfterResolutionAndIsIdempotent()
        {
            var quest = new MoralChoiceQuestDefinition
            {
                Id = "quest_moral_plan125_write",
                Choices = new List<MoralChoiceOption>
                {
                    new MoralChoiceOption { Label = "Commit", SetFlag = "flag_shared_rations" },
                    new MoralChoiceOption { Label = "Decline" }
                }
            };
            var system = new MoralChoiceSystem(new SeededRng(125));
            system.RegisterQuest(quest);
            Assert.False(system.HasFlag("flag_shared_rations"));

            system.Resolve(quest, 0, string.Empty, 12);

            Assert.True(system.HasFlag("flag_shared_rations"));
            Assert.Single(system.State.activeFlags, id => id == "flag_shared_rations");

            system.Resolve(quest, 1, string.Empty, 99);

            Assert.Single(system.State.activeFlags, id => id == "flag_shared_rations");
        }

        [Fact]
        public void OldStateLeavesNewFlagsUnsetAndRoundTripsExistingHistory()
        {
            var oldState = new MoralChoiceState();
            oldState.activeFlags.Add(MoralChoiceIds.FlagBrokenPact);

            var restored = new MoralChoiceSystem(new SeededRng(125));
            restored.RestoreState(oldState);

            Assert.True(restored.HasFlag(MoralChoiceIds.FlagBrokenPact));
            Assert.False(restored.HasFlag("flag_preserved_archive"));
            Assert.False(restored.HasFlag("flag_broke_treaty"));
        }

        [Fact]
        public void RaiderResolutionKeepsSameIncidentChoicesMutuallyExclusive()
        {
            var raider = MoralChoiceCatalogLoader.Load(DataDirectory, s_files, s_json)
                .Single(q => q.Id == MoralChoiceIds.ShareRaider);
            var system = new MoralChoiceSystem(new SeededRng(125));
            system.RegisterQuest(raider);

            system.Resolve(raider, 0, raider.LocationId, 1);

            Assert.True(system.HasFlag("flag_spared_raider"));
            Assert.False(system.HasFlag("flag_executed_prisoner"));
            Assert.False(system.HasFlag("flag_shared_rations"));
        }
    }
}
