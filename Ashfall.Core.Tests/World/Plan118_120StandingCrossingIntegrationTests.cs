// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan118_120StandingCrossingIntegrationTests
    {
        private static string DataDirectory
        {
            get
            {
                string start = Directory.GetCurrentDirectory();
                if (CatalogLocator.TryFindDataDirectory(start, out string found))
                    return found;
                if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                    return found;
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
            }
        }

        private static readonly string[] s_plan118QuestIds = new[]
        {
            "quest_record_the_survey_nail",
            "quest_record_the_overlay_pigment",
            "quest_record_the_second_count",
            "quest_record_the_lamp_keepers_oath",
            "quest_record_the_boundary_dispute",
            "quest_record_the_missing_plate",
            "quest_record_the_cold_survey",
            "quest_record_the_lamp_oil_ledger",
            "quest_record_the_rejected_survey",
            "quest_record_the_last_sector"
        };

        private static readonly string[] s_allEightCrossingFactionIds = new[]
        {
            CrossingIds.FactionScale,
            CrossingIds.FactionUnderwrite,
            CrossingIds.FactionCompact,
            CrossingIds.FactionLamplighters,
            CrossingIds.FactionGranaryWardens,
            CrossingIds.FactionWaterCommittee,
            CrossingIds.FactionQuarantinePost,
            CrossingIds.FactionSmugglersCourt
        };

        [Fact]
        public void Plan118_StandingRecordQuests_LoadsAllAuthoredQuestsAndVerifiesPlan118TenExpansionQuests()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new StandingRecordCatalogLoader(files, json);
            var catalog = loader.Load(DataDirectory);

            Assert.NotNull(catalog);
            Assert.True(catalog.Quests.Count >= 32, $"Expected >= 32 quests in Standing Record, found {catalog.Quests.Count}");

            var questMap = catalog.Quests.ToDictionary(q => q.id, StringComparer.Ordinal);

            foreach (string expectedId in s_plan118QuestIds)
            {
                Assert.True(questMap.ContainsKey(expectedId), $"Standing Record quest catalog missing Plan 118 quest '{expectedId}'");
                var quest = questMap[expectedId];

                Assert.NotNull(quest);
                Assert.False(string.IsNullOrWhiteSpace(quest.display_name), $"Quest '{expectedId}' missing display_name");
                Assert.False(string.IsNullOrWhiteSpace(quest.type), $"Quest '{expectedId}' missing type");
                Assert.False(string.IsNullOrWhiteSpace(quest.briefing), $"Quest '{expectedId}' missing briefing");
                Assert.True(quest.min_day >= 0, $"Quest '{expectedId}' invalid min_day {quest.min_day}");
                Assert.False(string.IsNullOrWhiteSpace(quest.target_location_id), $"Quest '{expectedId}' missing target_location_id");

                // Verify stages
                Assert.NotNull(quest.stages);
                Assert.NotEmpty(quest.stages);
                Assert.True(quest.StageCount >= 2, $"Quest '{expectedId}' has fewer than 2 stages");
                foreach (var stage in quest.stages)
                {
                    Assert.False(string.IsNullOrWhiteSpace(stage.id), $"Stage in quest '{expectedId}' has empty id");
                    Assert.False(string.IsNullOrWhiteSpace(stage.text), $"Stage '{stage.id}' in quest '{expectedId}' has empty text");
                }

                // Verify choices
                Assert.NotNull(quest.choices);
                Assert.NotEmpty(quest.choices);
                Assert.True(quest.choices.Length >= 2, $"Quest '{expectedId}' must have at least 2 choices");
                foreach (var choice in quest.choices)
                {
                    Assert.False(string.IsNullOrWhiteSpace(choice.id), $"Choice in quest '{expectedId}' has empty id");
                    Assert.False(string.IsNullOrWhiteSpace(choice.text), $"Choice '{choice.id}' in quest '{expectedId}' has empty text");
                    Assert.False(string.IsNullOrWhiteSpace(choice.set_flag), $"Choice '{choice.id}' in quest '{expectedId}' has empty set_flag");
                }

                // Verify mutations
                Assert.False(string.IsNullOrWhiteSpace(quest.complete_mutation), $"Quest '{expectedId}' missing complete_mutation");
                Assert.False(string.IsNullOrWhiteSpace(quest.fail_mutation), $"Quest '{expectedId}' missing fail_mutation");
            }
        }

        [Fact]
        public void Plan120_CrossingFactions_LoadsExactEightFactionsWithDistinctTradeProfiles()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new CrossingCatalogLoader(files, json);
            var catalog = loader.Load(DataDirectory);

            Assert.NotNull(catalog);
            Assert.Equal(8, catalog.Factions.Count);

            var factionMap = catalog.Factions.ToDictionary(f => f.id, StringComparer.Ordinal);
            var seenQuotes = new HashSet<string>(StringComparer.Ordinal);
            var seenWantsSignatures = new HashSet<string>(StringComparer.Ordinal);

            foreach (string expectedId in s_allEightCrossingFactionIds)
            {
                Assert.True(factionMap.ContainsKey(expectedId), $"Crossing catalog missing faction '{expectedId}'");
                var faction = factionMap[expectedId];

                Assert.NotNull(faction);
                Assert.False(string.IsNullOrWhiteSpace(faction.display_name), $"Faction '{expectedId}' missing display_name");
                Assert.True(faction.is_active, $"Faction '{expectedId}' should be active");
                Assert.False(string.IsNullOrWhiteSpace(faction.alignment), $"Faction '{expectedId}' missing alignment");
                Assert.False(string.IsNullOrWhiteSpace(faction.home_region), $"Faction '{expectedId}' missing home_region");
                Assert.Equal(CrossingIds.Region, faction.home_region);

                // Wants and Offers
                Assert.NotNull(faction.wants);
                Assert.NotEmpty(faction.wants);
                Assert.NotNull(faction.offers);
                Assert.NotEmpty(faction.offers);

                // Verify distinct trade signatures
                string wantsSig = string.Join(",", faction.wants.OrderBy(w => w, StringComparer.Ordinal));
                Assert.True(seenWantsSignatures.Add(wantsSig), $"Duplicate wants signature detected for faction '{expectedId}': {wantsSig}");

                // Signature Quote & Access Rule
                Assert.False(string.IsNullOrWhiteSpace(faction.signature_quote), $"Faction '{expectedId}' missing signature_quote");
                Assert.True(seenQuotes.Add(faction.signature_quote), $"Duplicate signature quote detected for faction '{expectedId}'");
                Assert.False(string.IsNullOrWhiteSpace(faction.access_rule), $"Faction '{expectedId}' missing access_rule");
            }
        }

        [Fact]
        public void Plan118_120_CrossDomainCoherence_BorderTerritoryAndLamplighterLinkages()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var crossingLoader = new CrossingCatalogLoader(files, json);
            var crossing = crossingLoader.Load(DataDirectory);

            var standingLoader = new StandingRecordCatalogLoader(files, json);
            var standing = standingLoader.Load(DataDirectory);

            // Crossing governs settlement factions (Rule 5)
            var lamplighters = crossing.GetFaction(CrossingIds.FactionLamplighters);
            Assert.NotNull(lamplighters);
            Assert.Contains("street_lighting", lamplighters!.offers);
            Assert.Contains("fuel_stores", lamplighters.wants);

            // Standing Record governs territorial sector lamps and oil ledgers (Rule 5)
            var oathQuest = standing.GetQuest("quest_record_the_lamp_keepers_oath");
            Assert.NotNull(oathQuest);
            Assert.Contains("keeper's oath", oathQuest!.briefing, StringComparison.OrdinalIgnoreCase);

            var oilLedgerQuest = standing.GetQuest("quest_record_the_lamp_oil_ledger");
            Assert.NotNull(oilLedgerQuest);
            Assert.Contains("oil ledger", oilLedgerQuest!.briefing, StringComparison.OrdinalIgnoreCase);

            // Boundary dispute quest links territory disputes to arbitration
            var disputeQuest = standing.GetQuest("quest_record_the_boundary_dispute");
            Assert.NotNull(disputeQuest);
            Assert.Contains("boundary", disputeQuest!.briefing, StringComparison.OrdinalIgnoreCase);

            // Ensure neither catalog leaks domain authority to the other
            Assert.All(crossing.Factions, f => Assert.StartsWith("faction_", f.id));
            Assert.All(standing.Quests, q => Assert.StartsWith("quest_record_", q.id));
        }

        [Fact]
        public void Plan118_120_DeterministicQuestChoiceResolutionAndFactionTrust()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var standingLoader = new StandingRecordCatalogLoader(files, json);
            var standing = standingLoader.Load(DataDirectory);

            var crossingLoader = new CrossingCatalogLoader(files, json);
            var crossing = crossingLoader.Load(DataDirectory);

            // Simulate deterministic choice selection across 2 seeded runs
            int seed = 42817;
            var rng1 = new SeededRng(seed);
            var rng2 = new SeededRng(seed);

            var choicesRun1 = new List<string>();
            var choicesRun2 = new List<string>();

            foreach (string qId in s_plan118QuestIds)
            {
                var quest = standing.GetQuest(qId);
                Assert.NotNull(quest);

                int pickIndex1 = rng1.Next(0, quest!.choices.Length);
                int pickIndex2 = rng2.Next(0, quest.choices.Length);

                Assert.Equal(pickIndex1, pickIndex2);
                choicesRun1.Add(quest.choices[pickIndex1].set_flag);
                choicesRun2.Add(quest.choices[pickIndex2].set_flag);
            }

            // Both runs must yield bitwise identical choice flags
            Assert.Equal(choicesRun1, choicesRun2);

            // Simulate faction trust adjustment deterministically
            var trustRun1 = new Dictionary<string, float>(StringComparer.Ordinal);
            var trustRun2 = new Dictionary<string, float>(StringComparer.Ordinal);

            foreach (var f in crossing.Factions)
            {
                float delta1 = (float)(rng1.NextDouble() * 20.0 - 10.0);
                float delta2 = (float)(rng2.NextDouble() * 20.0 - 10.0);

                Assert.Equal(delta1, delta2);
                trustRun1[f.id] = f.trust + delta1;
                trustRun2[f.id] = f.trust + delta2;
            }

            Assert.Equal(trustRun1, trustRun2);
        }
    }
}
