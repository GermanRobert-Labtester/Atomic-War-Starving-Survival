using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class UndergroundFungiCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void UndergroundFungi_LoadsAll24CanonicalSpecies()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "underground_fungi_flora.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new UndergroundFungiCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllSpecies.Count);

            // Test first species (Blue Phosphor Moss)
            var f1 = catalog.GetById("flora_01_blue_phosphor_sulfur_moss");
            Assert.NotNull(f1);
            Assert.Equal("Bryum phosphoreum tessarati", f1.scientific_name);
            Assert.Equal(0, f1.spore_hazard_rating);
            Assert.Contains("luciferase", f1.medicinal_extract);
            Assert.Contains("cyan luminescence", f1.botanist_field_notes);

            // Test edible crops
            var edible = catalog.GetEdibleCrops();
            Assert.True(edible.Count >= 10);

            // Test hazardous spore species
            var hazards = catalog.GetHazardousSporeSpecies(3);
            Assert.True(hazards.Count >= 4); // Cordyceps (3), Radon Coral (4), Ergot (5), Mercury Sponge (4)

            // Test final species (Sonya's Apple Tree)
            var f24 = catalog.GetById("flora_24_the_century_seed_heirloom_apple_scion");
            Assert.NotNull(f24);
            Assert.Equal(52.0f, f24.edible_calories_per_100g);
            Assert.Equal(0, f24.spore_hazard_rating);
            Assert.Contains("walnut box on Day 3650", f24.botanist_field_notes);

            // Test tag search
            var medicine = catalog.GetByTag("medicine");
            Assert.True(medicine.Count >= 5);
        }

        [Fact]
        public void UndergroundFungi_AllEntriesHaveValidFieldsAndGrowthCycles()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "underground_fungi_flora.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new UndergroundFungiCatalog();
            catalog.Load(json, serializer);

            foreach (var f in catalog.AllSpecies)
            {
                Assert.False(string.IsNullOrWhiteSpace(f.species_id), "Missing species_id");
                Assert.False(string.IsNullOrWhiteSpace(f.scientific_name), $"Missing scientific_name on {f.species_id}");
                Assert.False(string.IsNullOrWhiteSpace(f.common_name), $"Missing common_name on {f.species_id}");
                Assert.False(string.IsNullOrWhiteSpace(f.habitat_substrate), $"Missing habitat on {f.species_id}");
                Assert.True(f.growth_cycle_days > 0, $"Invalid growth cycle on {f.species_id}");
                Assert.True(f.spore_hazard_rating >= 0 && f.spore_hazard_rating <= 5, $"Invalid hazard rating on {f.species_id}");
                Assert.True(f.edible_calories_per_100g >= 0f, $"Invalid calories on {f.species_id}");
                Assert.False(string.IsNullOrWhiteSpace(f.medicinal_extract), $"Missing extract on {f.species_id}");
                Assert.False(string.IsNullOrWhiteSpace(f.botanist_field_notes), $"Missing field notes on {f.species_id}");
                Assert.True(f.botanist_field_notes.Length > 30, $"Field notes too brief on {f.species_id}");
                Assert.NotNull(f.tags);
                Assert.True(f.tags.Length > 0, $"Tags empty on {f.species_id}");
            }
        }
    }
}
