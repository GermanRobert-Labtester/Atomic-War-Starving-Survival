// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Audio;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class Plan67CassetteSetsTests : CatalogTestBase
    {
        [Fact]
        public void CassetteSets_LoadsExactlyTwelveAuthoredSets()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            Assert.NotNull(sets);
            Assert.Equal(12, sets.Count);
        }

        [Fact]
        public void CassetteSets_EverySetHasValidMetadataAndPartCount()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            foreach (var set in sets)
            {
                Assert.False(string.IsNullOrWhiteSpace(set.set_id), "Set ID cannot be empty");
                Assert.False(string.IsNullOrWhiteSpace(set.set_title), $"Set title cannot be empty for {set.set_id}");
                Assert.True(set.total_parts >= 3 && set.total_parts <= 6, $"Set {set.set_id} has unexpected total_parts: {set.total_parts}");
                Assert.Equal(set.total_parts, set.parts.Count);
                Assert.False(string.IsNullOrWhiteSpace(set.hidden_cache_location), $"Cache location missing for {set.set_id}");
                Assert.NotEmpty(set.hidden_cache_items);
            }
        }

        [Fact]
        public void CassetteSets_AllFortyEightPartsHaveUniqueIdsAndTranscripts()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            var seenItemIds = new HashSet<string>(StringComparer.Ordinal);
            int totalParts = 0;

            foreach (var set in sets)
            {
                for (int i = 0; i < set.parts.Count; i++)
                {
                    var part = set.parts[i];
                    totalParts++;
                    Assert.Equal(i + 1, part.part);
                    Assert.StartsWith("cassette_", part.item_id);
                    Assert.True(seenItemIds.Add(part.item_id), $"Duplicate part item_id: {part.item_id}");
                    Assert.False(string.IsNullOrWhiteSpace(part.title), $"Part title missing for {part.item_id}");
                    Assert.False(string.IsNullOrWhiteSpace(part.description), $"Part description missing for {part.item_id}");
                    Assert.True(part.description.Length >= 20, $"Description too short for {part.item_id}: {part.description}");
                }
            }

            Assert.Equal(48, totalParts);
        }

        [Fact]
        public void CassetteSets_AllPartItemIdsExistInItemsJson()
        {
            var sets = CassetteSetCatalogLoader.Load(DataDirectory);
            string itemsPath = Path.Combine(DataDirectory, "items.json");
            Assert.True(File.Exists(itemsPath), "items.json must exist");

            string itemsJson = File.ReadAllText(itemsPath);
            foreach (var set in sets)
            {
                foreach (var part in set.parts)
                {
                    Assert.Contains($"\"id\": \"{part.item_id}\"", itemsJson);
                }
            }
        }
    }
}
