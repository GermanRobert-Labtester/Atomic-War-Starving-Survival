// SPDX-License-Identifier: MIT
// Alpha feature F3 — storm sealing: the shipped insulation catalog carries a
// storm-sealing retrofit whose costs all resolve, and the thermal system loads
// it into its one insulation authority.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests.StormSealing
{
    public sealed class ThermalStormSealingCatalogTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found");
        }

        [Fact]
        public void ShippedCatalog_Defines_Storm_Sealing_With_Resolvable_Costs()
        {
            string dataDir = FindDataDir();
            string json = File.ReadAllText(Path.Combine(dataDir, "shelter_insulation_catalog.json"));

            using var doc = JsonDocument.Parse(json);
            var entry = doc.RootElement.GetProperty("insulations").EnumerateArray()
                .FirstOrDefault(e => e.GetProperty("insulation_id").GetString() == "insul_storm_sealing");

            Assert.True(entry.ValueKind != JsonValueKind.Undefined,
                "insul_storm_sealing must be authored in shelter_insulation_catalog.json");
            Assert.Equal("storm", entry.GetProperty("wall_material_tag").GetString());
            Assert.True(entry.GetProperty("air_leak_factor").GetDouble() <= 0.10,
                "storm sealing must meaningfully reduce air leaks");
            Assert.True(entry.GetProperty("max_upgrade_level").GetInt32() >= 1);

            var costs = entry.GetProperty("retrofit_item_costs").EnumerateArray().ToList();
            Assert.NotEmpty(costs);

            var itemIds = new HashSet<string>();
            using (var itemsDoc = JsonDocument.Parse(File.ReadAllText(Path.Combine(dataDir, "items.json"))))
            {
                foreach (var element in itemsDoc.RootElement.GetProperty("items").EnumerateArray())
                {
                    if (element.TryGetProperty("id", out var id)) itemIds.Add(id.GetString() ?? string.Empty);
                }
            }

            foreach (var cost in costs)
            {
                string id = cost.GetProperty("item_id").GetString() ?? string.Empty;
                Assert.True(itemIds.Contains(id), $"storm sealing cost '{id}' does not resolve in items.json");
                Assert.True(cost.GetProperty("amount").GetInt32() > 0);
            }
        }
    }
}
