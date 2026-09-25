// SPDX-License-Identifier: MIT
// Alpha feature F2 — Door visitor barters: the resolution reports what the
// visitor leaves behind, and the shipped catalog's barter ids all resolve.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.YearOfAsh;
using Xunit;

namespace Ashfall.Core.Tests.DoorBarter
{
    public sealed class DoorEncounterBarterTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        private static DoorEncounterEntry EntryWith(EncounterChoice choice)
        {
            var entry = new DoorEncounterEntry
            {
                encounterId = "test_encounter",
                visitorName = "Test Visitor",
                description = "A knock at the hatch.",
            };
            entry.choices.Add(choice);
            return entry;
        }

        [Fact]
        public void BarterGrant_Is_Reported_By_Resolution()
        {
            var system = new DoorEncounterSystem();
            var entry = EntryWith(new EncounterChoice
            {
                choiceId = "choice_barter",
                text = "Trade.",
                requiredItemId = "dried_rations",
                requiredItemQuantity = 4,
                grantItemId = "thermal_blanket",
                grantItemQuantity = 1,
            });

            var result = system.ResolveChoice(entry, entry.choices[0], new List<SurvivorOccupantSnapshot>());

            Assert.Equal("thermal_blanket", result.grantItemId);
            Assert.Equal(1, result.grantItemQuantity);
        }

        [Fact]
        public void Choice_Without_Grant_Reports_Empty_Grant()
        {
            var system = new DoorEncounterSystem();
            var entry = EntryWith(new EncounterChoice
            {
                choiceId = "choice_no_barter",
                text = "Turn them away.",
            });

            var result = system.ResolveChoice(entry, entry.choices[0], new List<SurvivorOccupantSnapshot>());

            Assert.Equal(string.Empty, result.grantItemId);
            Assert.Equal(0, result.grantItemQuantity);
        }

        [Fact]
        public void ShippedCatalog_Barters_All_Resolve_Against_ItemsJson()
        {
            string dataDir = FindDataDir();
            var files = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var entries = DoorEncounterCatalogLoader.Load(dataDir, files, serializer);
            Assert.NotEmpty(entries);

            var itemIds = new HashSet<string>();
            using (var doc = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, "items.json"))))
            {
                foreach (var element in doc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (element.TryGetProperty("id", out var id)) itemIds.Add(id.GetString() ?? string.Empty);
                }
            }

            var barters = entries
                .SelectMany(e => e.choices.Select(c => (e.encounterId, c)))
                .Where(pair => !string.IsNullOrEmpty(pair.c.grantItemId))
                .ToList();

            Assert.True(barters.Count >= 10, $"expected at least 10 authored barters, found {barters.Count}");

            foreach (var (encounterId, choice) in barters)
            {
                Assert.True(choice.grantItemQuantity > 0,
                    $"{encounterId}/{choice.choiceId}: grant without quantity");
                Assert.True(itemIds.Contains(choice.grantItemId),
                    $"{encounterId}/{choice.choiceId}: grant '{choice.grantItemId}' does not resolve in items.json");
                if (!string.IsNullOrEmpty(choice.requiredItemId))
                {
                    Assert.True(itemIds.Contains(choice.requiredItemId),
                        $"{encounterId}/{choice.choiceId}: requirement '{choice.requiredItemId}' does not resolve in items.json");
                }
            }
        }

        [Fact]
        public void Shipped_Catalog_Carries_Counter_Offers_That_Pay_More_For_More()
        {
            // Alpha feature G2 — the counter-offer is a real risk/reward choice:
            // pay a strictly larger requirement for a strictly larger grant on
            // the same item, through the same consume/grant pipeline.
            string dataDir;
            if (!CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out dataDir!))
                dataDir = System.AppContext.BaseDirectory;
            if (!CatalogLocator.TryFindDataDirectory(dataDir, out dataDir!))
                throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");

            var loaderFiles = new FileSystemIO();
            var loaderSerializer = new SystemTextJsonSerializer();
            var entries = DoorEncounterCatalogLoader.Load(dataDir, loaderFiles, loaderSerializer);
            int pairs = 0;

            foreach (var encounter in entries)
            {
                var barters = encounter.choices
                    .Where(c => !string.IsNullOrEmpty(c.grantItemId) && !string.IsNullOrEmpty(c.requiredItemId))
                    .ToList();

                foreach (var counter in barters)
                {
                    var baseChoice = barters.FirstOrDefault(c =>
                        c != counter &&
                        c.grantItemId == counter.grantItemId &&
                        c.requiredItemId == counter.requiredItemId &&
                        c.requiredItemQuantity < counter.requiredItemQuantity &&
                        c.grantItemQuantity < counter.grantItemQuantity);
                    if (baseChoice == null) continue;

                    pairs++;
                    Assert.True(counter.requiredItemQuantity > baseChoice.requiredItemQuantity);
                    Assert.True(counter.grantItemQuantity > baseChoice.grantItemQuantity);
                    Assert.True(counter.factionStandingDelta >= baseChoice.factionStandingDelta,
                        $"{encounter.encounterId}/{counter.choiceId}: paying more must not reduce standing");
                }
            }

            Assert.True(pairs >= 5, $"expected at least 5 counter-offer pairs, found {pairs}");
        }
    }
}
