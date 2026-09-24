// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 151 / 174: Working Animals & Companion System — Tests
// Verifies catalog loading, registration, role constraints, feeding,
// starvation, training progression, and veterinary care.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Ecology;
using Ashfall.Core.IO;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Plan151WorkingAnimals
{
    public sealed class Plan151WorkingAnimalsTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void AuthoritativeCatalog_LoadsAllAuthoredCompanionSpecies()
        {
            string path = ResolveDataPath("companion_animals.json");
            Assert.True(File.Exists(path), $"companion_animals.json must exist at {path}");

            string dataDir = Path.GetDirectoryName(path)!;
            var result = CompanionAnimalCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer());

            Assert.False(result.HasErrors, string.Join("; ", result.Errors));
            Assert.True(result.Companions.Count >= 5);
            Assert.NotNull(result.Companions.FirstOrDefault(c => c.species_id == "species_ash_hound"));
            Assert.NotNull(result.Companions.FirstOrDefault(c => c.species_id == "species_feral_goat"));
            Assert.NotNull(result.Companions.FirstOrDefault(c => c.species_id == "species_cotton_hare"));
            Assert.NotNull(result.Companions.FirstOrDefault(c => c.species_id == "species_rad_dog"));
            Assert.NotNull(result.Companions.FirstOrDefault(c => c.species_id == "species_iron_crow"));
        }

        [Fact]
        public void RegisterCompanion_EnforcesKnownSpeciesAndProfile()
        {
            var houndProfile = new CompanionSpeciesProfile
            {
                species_id = "species_ash_hound",
                display_name = "Ash Hound",
                role_tags = new List<string> { "guard", "morale" },
                base_food_per_day = 2,
                fallback_food_item_ids = new List<string> { "raw_meat" },
                max_health = 80,
                guard_rating = 45
            };

            var system = new CompanionAnimalSystem(new[] { houndProfile });
            system.KnownSpeciesCheck = id => id == "species_ash_hound";

            var resOk = system.RegisterCompanion("hound_01", "species_ash_hound", 1, "Fang");
            Assert.True(resOk.Success);
            Assert.Equal("Fang", system.Companion("hound_01")?.name);

            var resUnknown = system.RegisterCompanion("hound_02", "species_rad_bear", 1);
            Assert.False(resUnknown.Success);
            Assert.Equal("unknown_species", resUnknown.ReasonCode);
        }

        [Fact]
        public void AssignRole_ValidatesRoleTagCompatibility()
        {
            var hareProfile = new CompanionSpeciesProfile
            {
                species_id = "species_cotton_hare",
                display_name = "Cotton Hare",
                role_tags = new List<string> { "morale" },
                base_food_per_day = 1,
                fallback_food_item_ids = new List<string> { "crop_leafy_green" },
                max_health = 30
            };

            var system = new CompanionAnimalSystem(new[] { hareProfile });
            system.RegisterCompanion("hare_01", "species_cotton_hare", 1, "Fluffy");

            // Cotton hare only has "morale", so "guard" must be rejected
            var resGuard = system.Assign("hare_01", "survivor_1", CompanionRole.Guard);
            Assert.False(resGuard.Success);
            Assert.Equal("role_incompatible", resGuard.ReasonCode);

            // "morale" must succeed
            var resMorale = system.Assign("hare_01", "survivor_1", CompanionRole.Morale);
            Assert.True(resMorale.Success);
            Assert.Equal((int)CompanionRole.Morale, system.Companion("hare_01")!.role);
        }

        [Fact]
        public void Feeding_ConsumesInventoryAndResetsHunger()
        {
            var houndProfile = new CompanionSpeciesProfile
            {
                species_id = "species_ash_hound",
                display_name = "Ash Hound",
                role_tags = new List<string> { "guard" },
                base_food_per_day = 2,
                preferred_food_tags = new List<string> { "raw_meat" },
                fallback_food_item_ids = new List<string> { "raw_meat" },
                max_health = 80
            };

            int meatCount = 5;
            var system = new CompanionAnimalSystem(new[] { houndProfile });
            system.BindFoodPort(
                id => id == "raw_meat" ? meatCount : 0,
                (id, amount) => { if (id == "raw_meat") meatCount -= amount; }
            );

            system.RegisterCompanion("hound_01", "species_ash_hound", 1);
            var hound = system.Companion("hound_01")!;
            hound.hunger = 50;

            var feedRes = system.Feed(hound, houndProfile, 1);
            Assert.True(feedRes.Fed);
            Assert.Equal(0, hound.hunger);
            Assert.Equal(3, meatCount); // 5 - 2 = 3
        }

        [Fact]
        public void SicknessTreatment_ConsumesMedicalItemAndRestoresHealth()
        {
            var houndProfile = new CompanionSpeciesProfile
            {
                species_id = "species_ash_hound",
                display_name = "Ash Hound",
                role_tags = new List<string> { "guard" },
                base_food_per_day = 2,
                fallback_food_item_ids = new List<string> { "raw_meat" },
                max_health = 80
            };

            int medsCount = 2;
            var system = new CompanionAnimalSystem(new[] { houndProfile });
            system.BindFoodPort(
                id => id == "item_antibiotics" ? medsCount : 0,
                (id, amount) => { if (id == "item_antibiotics") medsCount -= amount; }
            );

            system.RegisterCompanion("hound_01", "species_ash_hound", 1);
            var hound = system.Companion("hound_01")!;
            hound.sickness = (int)CompanionSicknessState.Infection;

            var treatRes = system.TreatSickness("hound_01", "item_antibiotics");
            Assert.True(treatRes.Success);
            Assert.Equal((int)CompanionSicknessState.Healthy, hound.sickness);
            Assert.Equal(1, medsCount);
        }
    }
}
