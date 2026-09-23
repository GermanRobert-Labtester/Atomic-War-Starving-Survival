// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 174: Procedural Survivor Backstories & Origin Mechanics — Integration Tests
// Verifies catalog loading, template assignment, custom assignment, mechanical
// effect projection, secret revelation, and save/restore persistence.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Plan174SurvivorBackstories
{
    public sealed class Plan174SurvivorBackstoriesIntegrationTests
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
        public void LoadCatalog_LoadsOccupationsExperiencesAndTemplates()
        {
            var system = new BackstorySystem();
            string path = ResolveDataPath("backstory_templates.json");
            Assert.True(File.Exists(path), $"backstory_templates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var occupations = system.GetAllOccupations();
            Assert.True(occupations.Count >= 10, $"Expected >= 10 occupations, got {occupations.Count}");
            Assert.NotNull(system.GetOccupation("occ_doctor"));
            Assert.NotNull(system.GetOccupation("occ_soldier"));

            var experiences = system.GetAllExperiences();
            Assert.True(experiences.Count >= 6, $"Expected >= 6 experiences, got {experiences.Count}");
            Assert.NotNull(system.GetExperience("exp_combat_veteran"));

            var templates = system.GetAllTemplates();
            Assert.True(templates.Count >= 4, $"Expected >= 4 templates, got {templates.Count}");
            Assert.NotNull(system.GetTemplate("bst_military_medic"));
        }

        [Fact]
        public void AssignFromTemplate_PopulatesSurvivorBackstoryFields()
        {
            var system = new BackstorySystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("backstory_templates.json")));

            SurvivorBackstory? assigned = null;
            system.OnBackstoryAssigned += b => assigned = b;

            var backstory = system.AssignFromTemplate("survivor_101", "bst_military_medic", day: 3);

            Assert.NotNull(backstory);
            Assert.NotNull(assigned);
            Assert.Equal("survivor_101", backstory.SurvivorId);
            Assert.Equal("occ_doctor", backstory.OccupationId);
            Assert.Contains("exp_combat_veteran", backstory.LifeExperienceIds);
            Assert.Contains("exp_first_aid_trained", backstory.LifeExperienceIds);
            Assert.NotEmpty(backstory.PreWarLife);
            Assert.NotEmpty(backstory.DefiningMoment);
            Assert.Contains("carries_guilt_over_one_loss", backstory.Secrets);
        }

        [Fact]
        public void ProjectEffects_CalculatesCompoundSkillBonusesAndTraits()
        {
            var system = new BackstorySystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("backstory_templates.json")));

            system.AssignFromTemplate("survivor_102", "bst_military_medic", day: 1);
            var projection = system.ProjectEffects("survivor_102");

            Assert.Equal("survivor_102", projection.SurvivorId);
            Assert.Equal("Doctor", projection.OccupationLabel);

            // occ_doctor: medical +15; exp_first_aid_trained: medical +5 -> total medical bonus 20
            Assert.True(projection.SkillBonuses.TryGetValue("medical", out int medBonus));
            Assert.Equal(20, medBonus);

            // exp_combat_veteran: combat +8
            Assert.True(projection.SkillBonuses.TryGetValue("combat", out int combatBonus));
            Assert.Equal(8, combatBonus);

            // occ_doctor penalty: combat -5
            Assert.True(projection.SkillPenalties.TryGetValue("combat", out int combatPenalty));
            Assert.Equal(5, combatPenalty);

            // Traits from occupation and experiences
            Assert.Contains("trait_empathetic", projection.StartingTraits);
            Assert.Contains("trait_vigilant", projection.StartingTraits);

            // Starting items from doctor occupation
            Assert.Contains("medical_kit", projection.StartingItemIds);
        }

        [Fact]
        public void AssignCustom_SupportsProceduralCustomBackstory()
        {
            var system = new BackstorySystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("backstory_templates.json")));

            var backstory = system.AssignCustom(
                "survivor_103",
                "occ_mechanic",
                new[] { "exp_wilderness_survivor" },
                "Ran a repair garage on Route 9.",
                "Kept a generator running through a blizzard.",
                "Tools are meant to be used.",
                day: 4);

            Assert.Equal("survivor_103", backstory.SurvivorId);
            Assert.Equal("occ_mechanic", backstory.OccupationId);
            Assert.Single(backstory.LifeExperienceIds);

            var projection = system.ProjectEffects("survivor_103");
            Assert.Equal("Mechanic", projection.OccupationLabel);
            Assert.Contains("wrench", projection.StartingItemIds);
            Assert.Contains("trait_hardy", projection.StartingTraits); // from wilderness survivor
        }

        [Fact]
        public void RevealSecret_TracksSecretRevelationAndFiresEvent()
        {
            var system = new BackstorySystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("backstory_templates.json")));

            system.AssignFromTemplate("survivor_104", "bst_schoolteacher", day: 2);

            string? revealedId = null;
            system.OnSecretRevealed += (sId, secret) => revealedId = secret;

            bool success = system.RevealSecret("survivor_104", "knows_something_about_a_missing_child");
            Assert.True(success);
            Assert.Equal("knows_something_about_a_missing_child", revealedId);

            // Duplicate reveal should fail gracefully
            bool secondAttempt = system.RevealSecret("survivor_104", "knows_something_about_a_missing_child");
            Assert.False(secondAttempt);
        }

        [Fact]
        public void SaveRestore_PreservesBackstoriesAndRevealedSecrets()
        {
            var system = new BackstorySystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("backstory_templates.json")));

            system.AssignFromTemplate("survivor_105", "bst_plague_merchant", day: 5);
            system.RevealSecret("survivor_105", "has_pre_war_contacts_still_alive");

            var state = system.CaptureState();

            var restored = new BackstorySystem();
            restored.LoadCatalog(File.ReadAllText(ResolveDataPath("backstory_templates.json")));
            restored.RestoreState(state);

            Assert.Equal(1, restored.BackstoryCount);
            var backstory = restored.GetBackstory("survivor_105");
            Assert.NotNull(backstory);
            Assert.Equal("occ_merchant", backstory!.OccupationId);
            Assert.Contains("has_pre_war_contacts_still_alive", backstory.RevealedSecrets);

            var projection = restored.ProjectEffects("survivor_105");
            Assert.Equal("Merchant", projection.OccupationLabel);
            Assert.Contains("barter_goods", projection.StartingItemIds);
        }
    }
}
