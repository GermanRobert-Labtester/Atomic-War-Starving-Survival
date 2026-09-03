using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// F1–F4 data integrity — the micro-location catalog is validated as a
    /// schema fixture set: every referenced item, journal namespace, and
    /// expedition destination must resolve against the real data authority.
    /// A deserialized-but-unconsumable field fails here before it ships.
    /// </summary>
    public class MicroLocationCatalogFixtureTests
    {
        private static string DataDir()
        {
            // Walk up to the repo root (the csproj sits in Ashfall.Core.Tests/).
            var dir = new DirectoryInfo(AppContext.BaseDirectory);
            while (dir != null && !File.Exists(Path.Combine(dir.FullName, "Ashfall.csproj")))
                dir = dir.Parent!;
            return Path.Combine(dir!.FullName, "Assets", "StreamingAssets", "Data");
        }

        private static List<EncounterDefinition> LoadMicroLocations()
        {
            string dataDir = DataDir();
            var loader = NarrativeEncounterCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.FindAll(e => e.id.StartsWith("micro_", StringComparison.Ordinal));
        }

        [Fact]
        public void MicroLocationCatalog_LoadsWithAllFixtureEncounters()
        {
            var micros = LoadMicroLocations();
            Assert.True(micros.Count >= 24, $"expected the authored micro-location set, got {micros.Count}");

            foreach (var fixture in new[]
            {
                "micro_crashed_truck", "micro_roadside_memorial", "micro_shrine",
                "micro_frozen_bus", "micro_rail_siding", "micro_observation_post",
                "micro_supply_drop"
            })
            {
                Assert.NotNull(micros.Find(m => m.id == fixture));
            }
        }

        [Fact]
        public void EveryItemGrant_ReferencesKnownItem()
        {
            var items = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "items.json")));
            var itemIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var el in items.RootElement.GetProperty("items").EnumerateArray())
            {
                var id = el.GetProperty("id").GetString();
                if (!string.IsNullOrEmpty(id)) itemIds.Add(id!);
            }

            foreach (var enc in LoadMicroLocations())
            {
                foreach (var choice in enc.choices)
                {
                    if (choice.grantItemQuantity != 0)
                        Assert.True(itemIds.Contains(choice.grantItemId),
                            $"encounter {enc.id} choice {choice.choiceId} references unknown item {choice.grantItemId}");
                    if (choice.grantItemQuantity == 0 && !string.IsNullOrEmpty(choice.grantItemId))
                        Assert.Fail($"encounter {enc.id} choice {choice.choiceId} grants zero of item {choice.grantItemId} (drop the id or set a quantity)");
                }
            }
        }

        [Fact]
        public void EveryJournalUnlock_UsesMicroKnowledgeNamespace()
        {
            foreach (var enc in LoadMicroLocations())
            {
                foreach (var choice in enc.choices)
                {
                    if (string.IsNullOrEmpty(choice.journalUnlockId)) continue;
                    Assert.True(choice.journalUnlockId.StartsWith("micro_", StringComparison.Ordinal),
                        $"encounter {enc.id} choice {choice.choiceId} journal key {choice.journalUnlockId} is outside the micro_ knowledge namespace");
                }
            }
        }

        [Fact]
        public void EveryLocationDiscovery_ReferencesKnownExpeditionDestination()
        {
            var destinations = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "expeditions.json")));
            var destIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var el in destinations.RootElement.GetProperty("expeditions").EnumerateArray())
            {
                var id = el.GetProperty("id").GetString();
                if (!string.IsNullOrEmpty(id)) destIds.Add(id!);
            }

            foreach (var enc in LoadMicroLocations())
            {
                foreach (var choice in enc.choices)
                {
                    if (string.IsNullOrEmpty(choice.discoverLocationId)) continue;
                    Assert.True(destIds.Contains(choice.discoverLocationId),
                        $"encounter {enc.id} choice {choice.choiceId} discovers unknown location {choice.discoverLocationId}");
                }
            }
        }

        [Fact]
        public void ClueGatedDestinations_AreMarkedRequiresDiscovery()
        {
            var doc = System.Text.Json.JsonDocument.Parse(
                File.ReadAllText(Path.Combine(DataDir(), "expeditions.json")));
            var flagged = new HashSet<string>(StringComparer.Ordinal);
            foreach (var el in doc.RootElement.GetProperty("expeditions").EnumerateArray())
            {
                if (el.TryGetProperty("requiresDiscovery", out var flag) && flag.GetBoolean())
                {
                    var id = el.GetProperty("id").GetString();
                    if (id != null) flagged.Add(id);
                }
            }

            Assert.Contains("rural_gas_station", flagged);
            Assert.Contains("government_bunker", flagged);
        }

        [Fact]
        public void AuthoredFixtures_CoverEachEffectAndCombinedChoice()
        {
            var micros = LoadMicroLocations();
            bool hasGrant = false, hasNegativeGrant = false, hasJournal = false,
                 hasDiscovery = false, hasDepletion = false, hasCombinedJournalLocation = false,
                 hasFlag = false;

            foreach (var enc in micros)
            {
                foreach (var c in enc.choices)
                {
                    hasGrant |= c.grantItemQuantity > 0;
                    hasNegativeGrant |= c.grantItemQuantity < 0;
                    hasJournal |= !string.IsNullOrEmpty(c.journalUnlockId);
                    hasDiscovery |= !string.IsNullOrEmpty(c.discoverLocationId);
                    hasDepletion |= c.depletesOnResolve;
                    hasFlag |= !string.IsNullOrEmpty(c.setWorldFlag);
                    hasCombinedJournalLocation |=
                        !string.IsNullOrEmpty(c.journalUnlockId)
                        && !string.IsNullOrEmpty(c.discoverLocationId);
                }
            }

            Assert.True(hasGrant, "at least one runtime choice must grant an item");
            Assert.True(hasNegativeGrant, "at least one runtime choice must consume an item (offering)");
            Assert.True(hasJournal, "at least one runtime choice must unlock journal knowledge");
            Assert.True(hasDiscovery, "at least one runtime choice must discover a location");
            Assert.True(hasDepletion, "at least one runtime choice must deplete its encounter");
            Assert.True(hasFlag, "at least one runtime choice must set a world flag");
            Assert.True(hasCombinedJournalLocation,
                "the observation post must combine journal + location effects in one resolution");
        }
    }
}
