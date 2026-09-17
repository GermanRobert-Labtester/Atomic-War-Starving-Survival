// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Shelter;

namespace Ashfall.Core.Tests.Shelter
{
    public class TrophyPipelineTests : CatalogTestBase
    {
        private static string DataDir => DataDirectory;

        [Fact]
        public void TrophySystem_InitialCatalog_ContainsEightBaselineTrophies()
        {
            var system = new TrophySystem();
            Assert.True(system.Catalog.Count >= 8);

            var expectedSpecies = new[] { "wolf", "deer", "boar", "fox", "slag_beetle", "molerat", "ash_crow", "pheasant" };
            foreach (var sp in expectedSpecies)
            {
                var trophy = system.GetTrophyForSpecies(sp);
                Assert.NotNull(trophy);
                Assert.StartsWith("trophy_", trophy!.Id);
                Assert.StartsWith("item_decor_trophy_", trophy.TrophyItemId);
                Assert.StartsWith("recipe_trophy_", trophy.RecipeId);
                Assert.True(trophy.LocalizedMoraleDelta > 0f);
            }
        }

        [Theory]
        [InlineData("wolf", "recipe_trophy_wolf_head", 3.0f)]
        [InlineData("deer", "recipe_trophy_deer_antlers", 2.0f)]
        [InlineData("boar", "recipe_trophy_boar_tusks", 3.0f)]
        [InlineData("fox", "recipe_trophy_fox_pelt", 2.0f)]
        [InlineData("slag_beetle", "recipe_trophy_beetle_carapace", 2.0f)]
        [InlineData("molerat", "recipe_trophy_molerat_skull", 1.0f)]
        [InlineData("ash_crow", "recipe_trophy_crow_feathers", 1.0f)]
        [InlineData("pheasant", "recipe_trophy_pheasant_plume", 1.0f)]
        public void TrophySystem_SpeciesMapping_ReturnsExpectedRecipeAndMorale(string speciesId, string expectedRecipe, float expectedMorale)
        {
            var system = new TrophySystem();
            Assert.Equal(expectedRecipe, system.GetTrophyRecipeForSpecies(speciesId));

            var trophy = system.GetTrophyForSpecies(speciesId);
            Assert.NotNull(trophy);
            Assert.Equal(expectedMorale, trophy!.LocalizedMoraleDelta);
        }

        [Fact]
        public void TrophySystem_InvalidSpecies_ReturnsEmptyRecipe()
        {
            var system = new TrophySystem();
            Assert.Equal(string.Empty, system.GetTrophyRecipeForSpecies("rabbit"));
            Assert.Equal(string.Empty, system.GetTrophyRecipeForSpecies("unknown_creature"));
            Assert.Null(system.GetTrophyForSpecies("rabbit"));
        }

        [Fact]
        public void TrophySystem_RecordQuarryPreserved_ExactlyOnceAward()
        {
            var system = new TrophySystem();
            int eventsFired = 0;
            TrophyDefinition? readyDef = null;
            TrophyAwardRecord? readyRec = null;

            system.OnTrophyReady += (d, r) =>
            {
                eventsFired++;
                readyDef = d;
                readyRec = r;
            };

            // First award
            var record = system.RecordQuarryPreserved("wolf", day: 12);
            Assert.NotNull(record);
            Assert.Equal(1, eventsFired);
            Assert.Equal("trophy_wolf_head", record!.TrophyId);
            Assert.Equal("wolf", record.SpeciesId);
            Assert.Equal(12, record.DayAwarded);
            Assert.True(system.IsAwarded("trophy_wolf_head"));
            Assert.Contains("recipe_trophy_wolf_head", system.UnlockedRecipeIds);

            // Second award for the same species must return null and NOT fire OnTrophyReady again
            var duplicate = system.RecordQuarryPreserved("wolf", day: 15);
            Assert.Null(duplicate);
            Assert.Equal(1, eventsFired);
        }

        [Fact]
        public void TrophySystem_SaveAndRestore_RoundTripsLedgerWithoutDuplicateEvents()
        {
            var original = new TrophySystem();
            original.RecordQuarryPreserved("deer", day: 8);
            original.RecordQuarryPreserved("boar", day: 14);

            var saved = original.CaptureState();
            Assert.Equal(2, saved.awardedTrophyIds.Count);
            Assert.Contains("trophy_deer_antlers", saved.awardedTrophyIds);
            Assert.Contains("trophy_boar_tusks", saved.awardedTrophyIds);

            var restored = new TrophySystem();
            int restoreEvents = 0;
            restored.OnTrophyReady += (_, _) => restoreEvents++;

            restored.RestoreState(saved);
            Assert.Equal(0, restoreEvents); // No events fired on restore
            Assert.True(restored.IsAwarded("trophy_deer_antlers"));
            Assert.True(restored.IsAwarded("trophy_boar_tusks"));
            Assert.False(restored.IsAwarded("trophy_wolf_head"));

            // Re-evaluating previously awarded species returns null
            Assert.Null(restored.RecordQuarryPreserved("deer", day: 20));
            Assert.Equal(0, restoreEvents);
        }

        [Fact]
        public void TrophySystem_OldSave_StartsNeutral()
        {
            var system = new TrophySystem();
            system.RestoreState(null);
            Assert.Empty(system.AwardedTrophyIds);
            Assert.Empty(system.UnlockedRecipeIds);

            system.RestoreState(new TrophySaveState());
            Assert.Empty(system.AwardedTrophyIds);
            Assert.Empty(system.UnlockedRecipeIds);
        }

        [Fact]
        public void ShelterDecorSystem_TrophyMorale_LocalizedToAssignedRoom()
        {
            var decor = new ShelterDecorSystem();
            decor.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_trophy_wolf_head",
                LocalizedMoraleDelta = 3.0f,
                Category = "trophy"
            });
            decor.RegisterItemModifier(new ShelterDecorItemModifier
            {
                ItemId = "item_decor_trophy_deer_antlers",
                LocalizedMoraleDelta = 2.0f,
                Category = "trophy"
            });

            Assert.True(ShelterDecorSystem.IsTrophyItem("item_decor_trophy_wolf_head"));
            Assert.Equal(3.0f, decor.GetTrophyMoraleModifier("item_decor_trophy_wolf_head"));

            // Assign wolf trophy to crafting_room
            Assert.True(decor.Assign("crafting_room", "trophy_mount_1", "item_decor_trophy_wolf_head", dayInstalled: 10));

            // Crafting room receives +3.0 morale
            Assert.Equal(3.0f, decor.GetRoomMoraleDelta("crafting_room"));
            // Other rooms receive 0.0 morale (strictly localized!)
            Assert.Equal(0.0f, decor.GetRoomMoraleDelta("sleeping_quarters"));
            Assert.Equal(0.0f, decor.GetRoomMoraleDelta("common_room"));

            // Assign deer antlers to common_room
            Assert.True(decor.Assign("common_room", "trophy_mount_1", "item_decor_trophy_deer_antlers", dayInstalled: 11));
            Assert.Equal(2.0f, decor.GetRoomMoraleDelta("common_room"));
            Assert.Equal(3.0f, decor.GetRoomMoraleDelta("crafting_room"));

            // Remove wolf trophy from crafting_room -> reverses morale
            Assert.True(decor.Remove("crafting_room", "trophy_mount_1"));
            Assert.Equal(0.0f, decor.GetRoomMoraleDelta("crafting_room"));
            Assert.Equal(2.0f, decor.GetRoomMoraleDelta("common_room"));
        }

        [Fact]
        public void ShelterDecorSystem_GetTrophySlots_ReturnsDesignatedAndOccupiedSlots()
        {
            var decor = new ShelterDecorSystem();
            var slots = decor.GetTrophySlots("workshop_1");
            Assert.Contains("trophy_mount_1", slots);
            Assert.Contains("trophy_mount_2", slots);

            // Assign custom trophy slot
            decor.Assign("workshop_1", "custom_trophy_pedestal", "item_decor_trophy_fox_pelt", 5);
            var updatedSlots = decor.GetTrophySlots("workshop_1");
            Assert.Contains("custom_trophy_pedestal", updatedSlots);
        }

        [Fact]
        public void TrappingBridge_GetTrophyRecipeForSpecies_MatchesCatalog()
        {
            var trapping = new WildlifeTrappingSystem(new SeededRng(42));
            Assert.Equal("recipe_trophy_wolf_head", trapping.GetTrophyRecipeForSpecies("wolf"));
            Assert.Equal("recipe_trophy_deer_antlers", trapping.GetTrophyRecipeForSpecies("deer"));
            Assert.Equal("recipe_trophy_boar_tusks", trapping.GetTrophyRecipeForSpecies("boar"));
            Assert.Equal("recipe_trophy_fox_pelt", trapping.GetTrophyRecipeForSpecies("fox"));
            Assert.Equal("recipe_trophy_beetle_carapace", trapping.GetTrophyRecipeForSpecies("slag_beetle"));
            Assert.Equal("recipe_trophy_molerat_skull", trapping.GetTrophyRecipeForSpecies("molerat"));
            Assert.Equal("recipe_trophy_crow_feathers", trapping.GetTrophyRecipeForSpecies("ash_crow"));
            Assert.Equal("recipe_trophy_pheasant_plume", trapping.GetTrophyRecipeForSpecies("pheasant"));
            Assert.Equal(string.Empty, trapping.GetTrophyRecipeForSpecies("rabbit"));
        }

        [Fact]
        public void TrophiesJson_DataAuthority_LoadsAndValidates()
        {
            string path = Path.Combine(DataDir, "trophies.json");
            Assert.True(File.Exists(path), $"trophies.json not found at {path}");

            string json = File.ReadAllText(path);
            var system = new TrophySystem();
            Assert.True(system.LoadCatalogFromJson(json));
            Assert.True(system.Catalog.Count >= 8);

            foreach (var trophy in system.Catalog)
            {
                Assert.False(string.IsNullOrWhiteSpace(trophy.Id));
                Assert.False(string.IsNullOrWhiteSpace(trophy.SpeciesId));
                Assert.False(string.IsNullOrWhiteSpace(trophy.TrophyItemId));
                Assert.False(string.IsNullOrWhiteSpace(trophy.RecipeId));
                Assert.False(string.IsNullOrWhiteSpace(trophy.DisplayName));
                Assert.True(trophy.LocalizedMoraleDelta > 0f);
            }
        }
    }
}
