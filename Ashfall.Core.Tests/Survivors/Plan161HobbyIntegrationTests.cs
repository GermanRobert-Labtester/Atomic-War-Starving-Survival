// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Plan161Hobby
{
    public sealed class Plan161HobbyIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename);
        }

        [Fact]
        public void CatalogIntegrity_HobbyDefinitionsJson_LoadsAllCategoriesAndFacilities()
        {
            string path = ResolveDataPath("hobby_definitions.json");
            Assert.True(File.Exists(path), $"Catalog missing at: {path}");

            string json = File.ReadAllText(path);
            var system = new HobbySystem();
            system.LoadCatalog(json);

            Assert.True(system.AuthoredHobbies.Count >= 10, "Expected at least 10 hobbies in catalog.");

            var categories = system.AuthoredHobbies.Select(h => h.Category).Distinct().ToList();
            Assert.Contains(HobbyCategory.Creative, categories);
            Assert.Contains(HobbyCategory.Intellectual, categories);
            Assert.Contains(HobbyCategory.Physical, categories);
            Assert.Contains(HobbyCategory.Social, categories);
            Assert.Contains(HobbyCategory.Crafting, categories);
            Assert.Contains(HobbyCategory.Collecting, categories);

            var reading = system.AuthoredHobbies.FirstOrDefault(h => h.HobbyId == "hobby_reading");
            Assert.NotNull(reading);
            Assert.Equal("library", reading.RequiredFacility);
            Assert.Equal(5.0f, reading.BaseMoraleBonus);
        }

        [Fact]
        public void ConductSession_DeterministicProficiencyAndMoraleBonus()
        {
            string path = ResolveDataPath("hobby_definitions.json");
            var system = new HobbySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            HobbySessionResult? sessionFromSeam = null;
            system.OnSessionCompletedSeam = r => sessionFromSeam = r;

            var rng1 = new SeededRng(16101);
            var result1 = system.ConductSession("dweller_art", "hobby_painting", currentDay: 1, rng: rng1);

            Assert.NotNull(result1);
            Assert.NotNull(sessionFromSeam);
            Assert.Equal(result1, sessionFromSeam);
            Assert.True(result1.ProficiencyGained > 0f);
            Assert.True(result1.MoraleGained >= 8.0f); // Painting base morale bonus

            // Seeded replay verification:
            var systemReplay = new HobbySystem();
            if (File.Exists(path)) systemReplay.LoadCatalog(File.ReadAllText(path));
            var rng2 = new SeededRng(16101);
            var result2 = systemReplay.ConductSession("dweller_art", "hobby_painting", currentDay: 1, rng: rng2);

            Assert.Equal(result1.ProficiencyGained, result2.ProficiencyGained);
            Assert.Equal(result1.MoraleGained, result2.MoraleGained);
        }

        [Fact]
        public void MasteryProgression_ThresholdsAndSeamNotification()
        {
            var system = new HobbySystem();
            var prog = system.GetOrCreateProgress("dweller_cadet", "hobby_woodcarving");
            prog.Proficiency = 24.5f;

            HobbyMastery? promotedMastery = null;
            system.OnMasteryAchievedSeam = (p, m) => promotedMastery = m;

            // Session pushes proficiency over 25 -> Apprentice
            system.ConductSession("dweller_cadet", "hobby_woodcarving", currentDay: 5);

            Assert.Equal(HobbyMastery.Apprentice, prog.Mastery);
            Assert.Equal(HobbyMastery.Apprentice, promotedMastery);

            // Test resolve static helper
            Assert.Equal(HobbyMastery.Novice, HobbySystem.ResolveMastery(15f));
            Assert.Equal(HobbyMastery.Apprentice, HobbySystem.ResolveMastery(35f));
            Assert.Equal(HobbyMastery.Journeyman, HobbySystem.ResolveMastery(60f));
            Assert.Equal(HobbyMastery.Master, HobbySystem.ResolveMastery(85f));
        }

        [Fact]
        public void FacilityGating_ValidatesRequiredRooms()
        {
            string path = ResolveDataPath("hobby_definitions.json");
            var system = new HobbySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            var facilities = new[] { "library", "workshop" };

            Assert.True(system.CanConductSession("hobby_reading", facilities));
            Assert.True(system.CanConductSession("hobby_woodcarving", facilities));
            Assert.False(system.CanConductSession("hobby_painting", facilities)); // Needs studio
            Assert.True(system.CanConductSession("hobby_coin_collecting", facilities)); // Needs none
        }

        [Fact]
        public void GroupSession_MultiSurvivorBonusAndSharedAffinity()
        {
            string path = ResolveDataPath("hobby_definitions.json");
            var system = new HobbySystem();
            if (File.Exists(path)) system.LoadCatalog(File.ReadAllText(path));

            // Solo session
            var soloResult = system.ConductSession("dweller_host", "hobby_chess", currentDay: 10);

            // Group session with 2 companions
            var groupResult = system.ConductSession("dweller_companion1", "hobby_chess", currentDay: 10, coParticipantIds: new[] { "dweller_host", "dweller_companion2" });

            Assert.True(groupResult.MoraleGained > soloResult.MoraleGained, "Group sessions should yield higher morale bonus.");

            // Advance proficiency to >= 20 to unlock affinity bonus
            var pHost = system.GetOrCreateProgress("dweller_host", "hobby_chess");
            pHost.Proficiency = 30f;
            var pComp = system.GetOrCreateProgress("dweller_companion1", "hobby_chess");
            pComp.Proficiency = 25f;

            float affinity = system.GetSharedHobbyAffinityBonus("dweller_host", "dweller_companion1");
            Assert.Equal(5.0f, affinity);
        }

        [Fact]
        public void CaptureAndRestoreState_PreservesFullHobbyProgressAndAuthoredCatalog()
        {
            string path = ResolveDataPath("hobby_definitions.json");
            var system1 = new HobbySystem();
            if (File.Exists(path)) system1.LoadCatalog(File.ReadAllText(path));

            var p1 = system1.GetOrCreateProgress("dweller_x", "hobby_calisthenics");
            p1.Proficiency = 78f;
            p1.Mastery = HobbyMastery.Master;
            p1.SessionsCompleted = 15;
            p1.LastSessionDay = 12;

            var p2 = system1.GetOrCreateProgress("dweller_y", "hobby_storytelling");
            p2.Proficiency = 42f;
            p2.Mastery = HobbyMastery.Apprentice;
            p2.SessionsCompleted = 6;
            p2.LastSessionDay = 11;

            var state = system1.CaptureState();
            Assert.Equal(1, state.SchemaVersion);
            Assert.Equal(2, state.ProgressRecords.Count);
            Assert.True(state.AuthoredHobbies.Count >= 10);

            var system2 = new HobbySystem();
            system2.RestoreState(state);

            Assert.Equal(2, system2.ProgressCount);
            Assert.True(system2.AuthoredHobbies.Count >= 10);

            var restoredP1 = system2.GetOrCreateProgress("dweller_x", "hobby_calisthenics");
            Assert.Equal(78f, restoredP1.Proficiency);
            Assert.Equal(HobbyMastery.Master, restoredP1.Mastery);
            Assert.Equal(15, restoredP1.SessionsCompleted);
            Assert.Equal(12, restoredP1.LastSessionDay);
        }
    }
}
