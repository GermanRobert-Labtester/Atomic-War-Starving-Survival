using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Plan 106 test suite: verifies the expansion of dose_items.json from 5 -> 9 -> 15 items,
    /// backward compatibility with baseline items, schema adherence, category validity,
    /// global item namespace collision safety, medical realism, and canonical ItemCatalog integration.
    /// </summary>
    public class DoseItemExpansionTests
    {
        private static string FindDataDir()
        {
            string search = Directory.GetCurrentDirectory();
            for (int i = 0; i < 6; i++)
            {
                string candidate = Path.Combine(search, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                string? parent = Directory.GetParent(search)?.FullName;
                if (parent == null) break;
                search = parent;
            }
            return string.Empty;
        }

        [Fact]
        public void DoseItems_CatalogLoadsExactlyFifteenItems()
        {
            string dataDir = FindDataDir();
            Assert.False(string.IsNullOrEmpty(dataDir), "StreamingAssets/Data directory must exist");

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.NotNull(catalog);
            Assert.Equal(15, catalog.items.Count);
        }

        [Fact]
        public void DoseItems_PreservesOriginalFiveAndPlan27Items()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var itemsById = new Dictionary<string, DoseItemDef>(StringComparer.Ordinal);
            foreach (var item in catalog.items)
            {
                itemsById[item.id] = item;
            }

            // Original 5 compatibility anchors
            Assert.True(itemsById.ContainsKey("item_dose_ledger"));
            Assert.Equal("The Dose Ledger", itemsById["item_dose_ledger"].name);
            Assert.Equal("story", itemsById["item_dose_ledger"].category);

            Assert.True(itemsById.ContainsKey("item_calibration_key"));
            Assert.Equal("Dosimeter Calibration Key", itemsById["item_calibration_key"].name);
            Assert.Equal("tool", itemsById["item_calibration_key"].category);

            Assert.True(itemsById.ContainsKey("item_dosimeter_tag"));
            Assert.Equal("Dosimeter Tag", itemsById["item_dosimeter_tag"].name);
            Assert.Equal("tool", itemsById["item_dosimeter_tag"].category);

            Assert.True(itemsById.ContainsKey("item_palliative_morphine"));
            Assert.Equal("Palliative Morphine Tray", itemsById["item_palliative_morphine"].name);
            Assert.Equal("medical", itemsById["item_palliative_morphine"].category);

            Assert.True(itemsById.ContainsKey("item_cohort_first_board"));
            Assert.Equal("The Children's Baseline Board", itemsById["item_cohort_first_board"].name);
            Assert.Equal("story", itemsById["item_cohort_first_board"].category);

            // Plan 27 landed items
            Assert.True(itemsById.ContainsKey("item_calibrated_dosimeter"));
            Assert.True(itemsById.ContainsKey("item_forged_clean_bill_chit"));
            Assert.True(itemsById.ContainsKey("item_chelation_decorporation_course"));
            Assert.True(itemsById.ContainsKey("item_shielded_badge_case"));
        }

        [Fact]
        public void DoseItems_NewSixItemsPresentWithExpectedMetadata()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var itemsById = new Dictionary<string, DoseItemDef>(StringComparer.Ordinal);
            foreach (var item in catalog.items)
            {
                itemsById[item.id] = item;
            }

            // 10. Pocket Dosimeter (Measurement)
            Assert.True(itemsById.ContainsKey("item_pocket_dosimeter"));
            Assert.Equal("Pocket Dosimeter", itemsById["item_pocket_dosimeter"].name);
            Assert.Equal("tool", itemsById["item_pocket_dosimeter"].category);
            Assert.True(itemsById["item_pocket_dosimeter"].weightKg > 0);

            // 11. Radiation Survey Meter (Measurement / Field Survey)
            Assert.True(itemsById.ContainsKey("item_radiation_survey_meter"));
            Assert.Equal("Radiation Survey Meter", itemsById["item_radiation_survey_meter"].name);
            Assert.Equal("tool", itemsById["item_radiation_survey_meter"].category);
            Assert.True(itemsById["item_radiation_survey_meter"].weightKg > 1.0f);

            // 12. Dose Register Book (Documentation / Bureaucracy)
            Assert.True(itemsById.ContainsKey("item_dose_register_book"));
            Assert.Equal("Dose Register Book", itemsById["item_dose_register_book"].name);
            Assert.Equal("story", itemsById["item_dose_register_book"].category);
            Assert.Equal(0f, itemsById["item_dose_register_book"].tradeValue);

            // 13. Cohort Baseline Card (Cohort / Documentation)
            Assert.True(itemsById.ContainsKey("item_cohort_baseline_card"));
            Assert.Equal("Cohort Baseline Card", itemsById["item_cohort_baseline_card"].name);
            Assert.Equal("story", itemsById["item_cohort_baseline_card"].category);
            Assert.Equal(0f, itemsById["item_cohort_baseline_card"].tradeValue);

            // 14. Examiner Shielding Apron (Protection)
            Assert.True(itemsById.ContainsKey("item_shielding_apron"));
            Assert.Equal("Examiner Shielding Apron", itemsById["item_shielding_apron"].name);
            Assert.Equal("protective", itemsById["item_shielding_apron"].category);
            Assert.True(itemsById["item_shielding_apron"].weightKg >= 1.5f);

            // 15. Potassium Iodide Pack (Medical / Thyroid Prophylaxis)
            Assert.True(itemsById.ContainsKey("item_potassium_iodide_pack"));
            Assert.Equal("Potassium Iodide Pack", itemsById["item_potassium_iodide_pack"].name);
            Assert.Equal("medical", itemsById["item_potassium_iodide_pack"].category);
            Assert.True(itemsById["item_potassium_iodide_pack"].tradeValue > 0);
        }

        [Fact]
        public void DoseItems_AllIdsUniqueAndFollowItemPrefix()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var item in catalog.items)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.id), "Item id must not be empty");
                Assert.StartsWith("item_", item.id);
                Assert.True(seenIds.Add(item.id), $"Duplicate item id found in dose_items.json: {item.id}");
            }
        }

        [Fact]
        public void DoseItems_NamesAndDescriptionsNonEmpty()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            foreach (var item in catalog.items)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.name), $"Item {item.id} has empty name");
                Assert.False(string.IsNullOrWhiteSpace(item.description), $"Item {item.id} has empty description");
                Assert.True(item.description.Length >= 20, $"Item {item.id} description is too short: {item.description}");
            }
        }

        [Fact]
        public void DoseItems_WeightsAndTradeValuesWithinValidRanges()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            foreach (var item in catalog.items)
            {
                Assert.True(item.weightKg > 0f && item.weightKg <= 2.5f,
                    $"Item {item.id} has out-of-range weight: {item.weightKg}");
                Assert.True(item.tradeValue >= 0f && item.tradeValue <= 100f,
                    $"Item {item.id} has out-of-range tradeValue: {item.tradeValue}");
                Assert.False(float.IsNaN(item.weightKg) || float.IsInfinity(item.weightKg));
                Assert.False(float.IsNaN(item.tradeValue) || float.IsInfinity(item.tradeValue));
            }
        }

        [Fact]
        public void DoseItems_CategoriesMatchValidGrammar()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var validCategories = new HashSet<string>(StringComparer.Ordinal)
            {
                "story", "tool", "medical", "protective", "consumable"
            };

            foreach (var item in catalog.items)
            {
                Assert.True(validCategories.Contains(item.category),
                    $"Item {item.id} has unrecognized category '{item.category}'");
            }
        }

        private sealed class ItemIdProbe
        {
            public string id { get; set; } = string.Empty;
        }

        [Fact]
        public void DoseItems_GlobalItemNamespaceCollisionSafety()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            string primaryPath = fileIO.Combine(dataDir, "items.json");
            Assert.True(fileIO.FileExists(primaryPath), "items.json must exist");

            var rawPrimary = fileIO.ReadAllText(primaryPath);
            var primaryItems = CatalogLocator.LoadWrappedList<ItemIdProbe>(rawPrimary, SystemTextJsonSerializer.Options);
            Assert.NotNull(primaryItems);

            var primaryIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var p in primaryItems)
            {
                if (!string.IsNullOrEmpty(p.id)) primaryIds.Add(p.id);
            }

            var catalog = DoseContentCatalogLoader.Load(
                dataDir, fileIO, new SystemTextJsonSerializer());

            // No dose item should duplicate a primary items.json ID
            foreach (var item in catalog.items)
            {
                Assert.False(primaryIds.Contains(item.id),
                    $"Dose item '{item.id}' collides with primary items.json definition");
            }
        }

        [Fact]
        public void DoseItems_MedicalRealismSanityChecks()
        {
            string dataDir = FindDataDir();
            var catalog = DoseContentCatalogLoader.Load(
                dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            var itemsById = new Dictionary<string, DoseItemDef>(StringComparer.Ordinal);
            foreach (var item in catalog.items) itemsById[item.id] = item;

            // Potassium iodide must specify thyroid protection and not universal cure
            var ki = itemsById["item_potassium_iodide_pack"];
            Assert.Contains("thyroid", ki.description, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("cure", ki.description, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("immune", ki.description, StringComparison.OrdinalIgnoreCase);

            // Shielding apron must specify localized/torso protection and not total radiation immunity
            var apron = itemsById["item_shielding_apron"];
            Assert.Contains("torso", apron.description, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("immune", apron.description, StringComparison.OrdinalIgnoreCase);
            Assert.DoesNotContain("total protection", apron.description, StringComparison.OrdinalIgnoreCase);

            // General check: no item claims to cure radiation or give total immunity
            foreach (var item in catalog.items)
            {
                Assert.DoesNotContain("cure radiation", item.description, StringComparison.OrdinalIgnoreCase);
                Assert.DoesNotContain("immune to radiation", item.description, StringComparison.OrdinalIgnoreCase);
            }
        }

        [Fact]
        public void DoseItems_LoadedIntoCanonicalItemCatalog()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();

            var result = ItemCatalogLoader.LoadCatalogWithResult(dataDir, fileIO, serializer);
            Assert.False(result.HasErrors, $"ItemCatalog load failed with errors: {string.Join("; ", result.Messages)}");

            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);
            Assert.NotNull(catalog);

            // All 15 dose items should be registered into the ItemCatalog via SecondaryItemFiles
            var doseCatalog = DoseContentCatalogLoader.Load(dataDir, fileIO, serializer);
            Assert.Equal(15, doseCatalog.items.Count);
            foreach (var item in doseCatalog.items)
            {
                Assert.True(catalog.Contains(item.id),
                    $"Dose item '{item.id}' was not registered in the canonical ItemCatalog");
            }
        }
    }
}
