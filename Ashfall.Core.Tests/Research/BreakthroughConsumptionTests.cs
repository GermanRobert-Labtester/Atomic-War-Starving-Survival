// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Xunit;

namespace Ashfall.Core.Tests.Research
{
    public sealed class BreakthroughConsumptionTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            string dir = baseDir;
            for (int i = 0; i < 6; i++)
            {
                probe = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(probe)) return probe;
                var parent = Directory.GetParent(dir);
                if (parent == null) break;
                dir = parent.FullName;
            }
            return probe;
        }

        [Fact]
        public void AllBreakthroughItems_HaveActiveRecipeConsumers()
        {
            string dataDir = ResolveDataDir();
            string rkPath = Path.Combine(dataDir, "research_knowledge.json");
            string recipesPath = Path.Combine(dataDir, "recipes.json");
            string itemsPath = Path.Combine(dataDir, "items.json");

            Assert.True(File.Exists(rkPath), "research_knowledge.json must exist");
            Assert.True(File.Exists(recipesPath), "recipes.json must exist");
            Assert.True(File.Exists(itemsPath), "items.json must exist");

            using var rkDoc = JsonDocument.Parse(File.ReadAllText(rkPath));
            using var recipesDoc = JsonDocument.Parse(File.ReadAllText(recipesPath));
            using var itemsDoc = JsonDocument.Parse(File.ReadAllText(itemsPath));

            var allItems = new HashSet<string>(StringComparer.Ordinal);
            if (itemsDoc.RootElement.TryGetProperty("items", out var itemsArray))
            {
                foreach (var it in itemsArray.EnumerateArray())
                {
                    if (it.TryGetProperty("id", out var idProp))
                        allItems.Add(idProp.GetString()!);
                }
            }

            var breakthroughItems = new HashSet<string>(StringComparer.Ordinal);
            int totalBreakthroughNodes = 0;
            if (rkDoc.RootElement.TryGetProperty("knowledge_nodes", out var nodesArray))
            {
                foreach (var node in nodesArray.EnumerateArray())
                {
                    if (node.TryGetProperty("breakthrough_item", out var btProp))
                    {
                        string? bt = btProp.GetString();
                        if (!string.IsNullOrEmpty(bt))
                        {
                            breakthroughItems.Add(bt);
                            totalBreakthroughNodes++;
                        }
                    }
                }
            }

            Assert.Equal(32, totalBreakthroughNodes);
            Assert.Equal(26, breakthroughItems.Count);

            // Collect all ingredient item IDs from recipes.json
            var consumedItems = new HashSet<string>(StringComparer.Ordinal);
            if (recipesDoc.RootElement.TryGetProperty("recipes", out var recipesArray))
            {
                foreach (var r in recipesArray.EnumerateArray())
                {
                    if (r.TryGetProperty("ingredients", out var ingArray))
                    {
                        foreach (var ing in ingArray.EnumerateArray())
                        {
                            if (ing.TryGetProperty("itemId", out var itemProp))
                            {
                                string? iid = itemProp.GetString();
                                if (!string.IsNullOrEmpty(iid))
                                    consumedItems.Add(iid);
                            }
                        }
                    }
                }
            }

            var orphanedItems = new List<string>();
            foreach (var bt in breakthroughItems)
            {
                Assert.True(allItems.Contains(bt), $"Breakthrough item '{bt}' must exist in items.json");
                if (!consumedItems.Contains(bt))
                {
                    orphanedItems.Add(bt);
                }
            }

            Assert.Empty(orphanedItems);
        }
    }
}
