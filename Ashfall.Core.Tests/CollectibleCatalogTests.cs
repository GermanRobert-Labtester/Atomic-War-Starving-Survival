using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class CollectibleCatalogTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static CollectibleCatalog? LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return CollectibleCatalogLoader.Load(DataDir, fileIO, json);
        }

        [Fact]
        public void Catalog_Loads()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
        }

        [Fact]
        public void Catalog_Has40Entries()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(40, catalog.Count);
        }

        [Fact]
        public void Catalog_AllItemIdsResolve()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var items = ItemCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var itemIds = new HashSet<string>();
            foreach (var item in items) itemIds.Add(item.id);

            foreach (var kvp in catalog.ByItemId)
            {
                Assert.True(itemIds.Contains(kvp.Key),
                    $"Collectible {kvp.Key} references missing item");
            }
        }

        [Fact]
        public void Catalog_AllCategoriesPresent()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var expected = new HashSet<string>
            {
                "vinyl", "photograph", "poster", "book", "magazine",
                "technical_manual", "military_document", "personal_letter",
                "badge", "patch", "toy", "religious_object",
                "sports_memorabilia", "cultural_artifact", "newspaper", "map"
            };
            var found = new HashSet<string>();
            foreach (var d in catalog.ByItemId.Values)
                found.Add(d.category);

            foreach (var cat in expected)
            {
                Assert.True(found.Contains(cat), $"Missing category: {cat}");
            }
        }

        [Fact]
        public void Catalog_CategoryDistribution()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var counts = new Dictionary<string, int>();
            foreach (var d in catalog.ByItemId.Values)
            {
                counts.TryGetValue(d.category, out int c);
                counts[d.category] = c + 1;
            }

            Assert.Equal(3, counts["vinyl"]);
            Assert.Equal(2, counts["photograph"]);
            Assert.Equal(3, counts["poster"]);
            Assert.Equal(2, counts["book"]);
            Assert.Equal(2, counts["magazine"]);
            Assert.Equal(5, counts["technical_manual"]);
            Assert.Equal(3, counts["military_document"]);
            Assert.Equal(3, counts["personal_letter"]);
            Assert.Equal(2, counts["badge"]);
            Assert.Equal(2, counts["patch"]);
            Assert.Equal(2, counts["toy"]);
            Assert.Equal(2, counts["religious_object"]);
            Assert.Equal(2, counts["sports_memorabilia"]);
            Assert.Equal(2, counts["cultural_artifact"]);
            Assert.Equal(2, counts["newspaper"]);
            Assert.Equal(3, counts["map"]);
        }

        [Fact]
        public void Catalog_ValidRarityValues()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var valid = new HashSet<string> { "common", "uncommon", "rare", "unique" };
            foreach (var d in catalog.ByItemId.Values)
            {
                Assert.True(valid.Contains(d.rarity),
                    $"Invalid rarity '{d.rarity}' on {d.item_id}");
            }
        }

        [Fact]
        public void Catalog_ValidEffectTypes()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var valid = new HashSet<string>
            {
                "none", "morale", "knowledge", "recipe",
                "location_clue", "faction_info", "journal_unlock"
            };
            foreach (var d in catalog.ByItemId.Values)
            {
                Assert.True(valid.Contains(d.effect_type),
                    $"Invalid effect_type '{d.effect_type}' on {d.item_id}");
            }
        }

        [Fact]
        public void Catalog_NonNoneEffectsHaveTarget()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            foreach (var d in catalog.ByItemId.Values)
            {
                if (d.effect_type != "none" && d.effect_type != "morale")
                {
                    Assert.False(string.IsNullOrEmpty(d.effect_target),
                        $"Non-none/non-morale effect on {d.item_id} has empty effect_target");
                }
            }
        }

        [Fact]
        public void Catalog_UniqueCount()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            int uniqueCount = 0;
            foreach (var d in catalog.ByItemId.Values)
            {
                if (d.unique) uniqueCount++;
            }
            Assert.True(uniqueCount >= 2 && uniqueCount <= 8,
                $"Expected 2-8 unique collectibles, got {uniqueCount}");
        }

        [Fact]
        public void Catalog_ItemIdsAreCollectible()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            foreach (var kvp in catalog.ByItemId)
            {
                Assert.StartsWith("item_collectible_", kvp.Key);
            }
        }

        [Fact]
        public void Catalog_NoDuplicateItemIds()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var seen = new HashSet<string>();
            foreach (var kvp in catalog.ByItemId)
            {
                Assert.True(seen.Add(kvp.Key), $"Duplicate item_id: {kvp.Key}");
            }
        }

        [Fact]
        public void Catalog_GetByItemId()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var def = catalog.GetByItemId("item_collectible_family_portrait");
            Assert.NotNull(def);
            Assert.Equal("photograph", def.category);
            Assert.Equal("common", def.rarity);
            Assert.Equal("morale", def.effect_type);
        }

        [Fact]
        public void Catalog_IsCollectible()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.True(catalog.IsCollectible("item_collectible_family_portrait"));
            Assert.False(catalog.IsCollectible("nonexistent_item"));
        }

        [Fact]
        public void Catalog_MissingFile_ReturnsNull()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = CollectibleCatalogLoader.Load("/nonexistent/path", fileIO, json);
            Assert.Null(result);
        }
    }
}
