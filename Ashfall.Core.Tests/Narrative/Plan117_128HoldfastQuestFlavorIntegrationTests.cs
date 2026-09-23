#nullable enable
// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Flags;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    public sealed class Plan117_128HoldfastQuestFlavorIntegrationTests
    {
        private static string ResolveDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private sealed class FlavorDataDto
        {
            public int schema_version { get; set; }
            public Dictionary<string, FactionEntryDto> factions { get; set; } = new Dictionary<string, FactionEntryDto>();
            public Dictionary<string, string> items { get; set; } = new Dictionary<string, string>();
        }

        private sealed class FactionEntryDto
        {
            public string register { get; set; } = string.Empty;
            public string voice { get; set; } = string.Empty;
            public string rejected { get; set; } = string.Empty;
            public string sold { get; set; } = string.Empty;
        }

        [Fact]
        public void HoldfastCatalog_LoadsAll24Quests_AndValidatesIntegrity()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var loader = new HoldfastCatalogLoader(io, serializer);

            var catalog = loader.Load(dir);

            Assert.NotNull(catalog);
            Assert.True(catalog.Quests.Count >= 20, $"Expected >= 20 quests, found {catalog.Quests.Count}");
            Assert.Equal(24, catalog.Quests.Count);

            var questIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var q in catalog.Quests)
            {
                Assert.False(string.IsNullOrWhiteSpace(q.id), "Quest ID cannot be empty");
                Assert.StartsWith("quest_holdfast_", q.id, StringComparison.OrdinalIgnoreCase);
                Assert.False(string.IsNullOrWhiteSpace(q.display_name), $"Quest '{q.id}' missing display_name");
                Assert.False(string.IsNullOrWhiteSpace(q.briefing), $"Quest '{q.id}' missing briefing");
                Assert.True(q.StageCount > 0, $"Quest '{q.id}' must have at least one stage");
                Assert.True(q.min_day >= 0, $"Quest '{q.id}' min_day must be non-negative");

                Assert.True(questIds.Add(q.id), $"Duplicate quest ID detected: '{q.id}'");
            }

            // Verify prereqs resolve if specified
            foreach (var q in catalog.Quests)
            {
                if (!string.IsNullOrEmpty(q.prereq_quest_id))
                {
                    Assert.Contains(q.prereq_quest_id, questIds);
                }
            }
        }

        [Fact]
        public void HoldfastFlavor_LoadsAll8Factions_AndValidatesRegisters()
        {
            string dir = ResolveDataDir();
            string flavorPath = Path.Combine(dir, "holdfast_flavor.json");
            Assert.True(File.Exists(flavorPath));

            string json = File.ReadAllText(flavorPath);
            var serializer = new SystemTextJsonSerializer();
            var flavor = serializer.Deserialize<FlavorDataDto>(json);

            Assert.NotNull(flavor);
            Assert.Equal(1, flavor.schema_version);
            Assert.Equal(8, flavor.factions.Count);

            string[] expectedFactions =
            {
                "faction_the_office",
                "faction_the_cutters",
                "faction_the_fleet",
                "faction_black_flotilla",
                "faction_supply_corps",
                "faction_railway_guild",
                "faction_hydro_barons",
                "faction_ordnance_foundry"
            };

            foreach (var fId in expectedFactions)
            {
                Assert.True(flavor.factions.ContainsKey(fId), $"Missing flavor faction '{fId}'");
                var entry = flavor.factions[fId];
                Assert.False(string.IsNullOrWhiteSpace(entry.register), $"Faction '{fId}' missing register");
                Assert.False(string.IsNullOrWhiteSpace(entry.voice), $"Faction '{fId}' missing voice");
                Assert.False(string.IsNullOrWhiteSpace(entry.rejected), $"Faction '{fId}' missing rejected");
                Assert.False(string.IsNullOrWhiteSpace(entry.sold), $"Faction '{fId}' missing sold");
            }
        }

        [Fact]
        public void HoldfastQuestSystem_ExecutesExpeditionFlow()
        {
            string dir = ResolveDataDir();
            var io = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var loader = new HoldfastCatalogLoader(io, serializer);
            var catalog = loader.Load(dir);

            var system = new HoldfastQuestSystem();
            system.BindCatalog(catalog.Quests);

            string questId = HoldfastQuestSystem.Sheet;
            var def = system.GetDef(questId);
            Assert.NotNull(def);

            bool started = system.TryStart(questId, 90);
            Assert.True(started);
            Assert.True(system.IsStarted(questId));

            // Complete stages
            int guard = 0;
            while (!system.IsCompleted(questId) && guard++ < 20)
            {
                system.Advance(questId);
            }

            Assert.True(system.IsCompleted(questId));

            // State persistence roundtrip
            var saveState = system.CaptureState();
            Assert.Contains(saveState.quests, q => q.questId == questId && q.completed);

            var restoredSystem = new HoldfastQuestSystem();
            restoredSystem.BindCatalog(catalog.Quests);
            restoredSystem.RestoreState(saveState);
            Assert.True(restoredSystem.IsCompleted(questId));
        }
    }
}
