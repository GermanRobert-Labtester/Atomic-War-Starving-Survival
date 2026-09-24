// SPDX-License-Identifier: MIT
// Plan 174 host integration tests: verifies BackstorySystem, catalog loading,
// procedural template and custom origin mechanics, mechanical projection,
// secret revelation, save round-trips, and host wiring.

using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan174BackstoryHostIntegrationTests
    {
        private static string RepoRoot()
        {
            string? dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (File.Exists(Path.Combine(dir, "Ashfall.csproj")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string DataDir() => Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static string ReadRepoFile(params string[] parts)
            => File.ReadAllText(Path.Combine(new[] { RepoRoot() }.Concat(parts).ToArray()));

        private static BackstorySystem CreateSystemWithCatalog()
        {
            var system = new BackstorySystem();
            string json = File.ReadAllText(Path.Combine(DataDir(), "backstory_templates.json"));
            system.LoadCatalog(json);
            return system;
        }

        [Fact]
        public void Backstory_CatalogLoading_LoadsAuthoredTemplatesAndOccupations()
        {
            var system = CreateSystemWithCatalog();
            var occs = system.GetAllOccupations();
            var exps = system.GetAllExperiences();
            var tmpls = system.GetAllTemplates();

            Assert.NotEmpty(occs);
            Assert.True(occs.Count >= 8, $"Expected >= 8 occupations, got {occs.Count}");
            Assert.True(exps.Count >= 5, $"Expected >= 5 experiences, got {exps.Count}");
            Assert.True(tmpls.Count >= 4, $"Expected >= 4 templates, got {tmpls.Count}");

            var doctor = system.GetOccupation("occ_doctor");
            Assert.NotNull(doctor);
            Assert.Contains(doctor!.skill_bonuses, b => b.skill_id == "medical" && b.bonus > 0);
            Assert.Contains(doctor.skill_penalties, p => p.skill_id == "combat" && p.penalty > 0);
        }

        [Fact]
        public void Backstory_TemplateAssignment_AssignsOccupationAndSecrets()
        {
            var system = CreateSystemWithCatalog();
            bool eventFired = false;
            system.OnBackstoryAssigned += b =>
            {
                if (b.SurvivorId == "survivor_01") eventFired = true;
            };

            var backstory = system.AssignFromTemplate("survivor_01", "bst_military_medic", day: 1);

            Assert.NotNull(backstory);
            Assert.True(eventFired);
            Assert.Equal("survivor_01", backstory.SurvivorId);
            Assert.Equal("occ_doctor", backstory.OccupationId);
            Assert.NotEmpty(backstory.Secrets);
            Assert.Empty(backstory.RevealedSecrets);
        }

        [Fact]
        public void Backstory_MechanicalProjection_AppliesSkillBonusesAndTraits()
        {
            var system = CreateSystemWithCatalog();
            system.AssignFromTemplate("survivor_02", "bst_military_medic", day: 1);

            var projection = system.ProjectEffects("survivor_02");

            Assert.NotNull(projection);
            Assert.Equal("Doctor", projection.OccupationLabel);
            Assert.True(projection.SkillBonuses.ContainsKey("medical"), "Projection must contain medical skill bonus.");
            Assert.True(projection.SkillBonuses["medical"] >= 15, "Medical bonus must be >= 15.");
            Assert.NotEmpty(projection.StartingTraits);
            Assert.NotEmpty(projection.StartingItemIds);
        }

        [Fact]
        public void Backstory_SecretRevelation_IsIdempotentAndEmitsEvent()
        {
            var system = CreateSystemWithCatalog();
            var backstory = system.AssignFromTemplate("survivor_03", "bst_military_medic", day: 1);
            string secret = backstory.Secrets[0];

            string? revealedSecret = null;
            system.OnSecretRevealed += (survivorId, sec) =>
            {
                if (survivorId == "survivor_03") revealedSecret = sec;
            };

            bool firstReveal = system.RevealSecret("survivor_03", secret);
            Assert.True(firstReveal);
            Assert.Equal(secret, revealedSecret);

            // Idempotent rejection on duplicate
            bool secondReveal = system.RevealSecret("survivor_03", secret);
            Assert.False(secondReveal);

            var updated = system.GetBackstory("survivor_03");
            Assert.NotNull(updated);
            Assert.Contains(secret, updated!.RevealedSecrets);
        }

        [Fact]
        public void Backstory_CustomAssignment_StacksAdditiveExperienceBonuses()
        {
            var system = CreateSystemWithCatalog();
            system.AssignCustom(
                "survivor_custom",
                "occ_engineer",
                new[] { "exp_wilderness_survivor" },
                preWarLife: "Machinist in rail repair yards.",
                definingMoment: "Bypassed emergency seals during the initial strike.",
                reasonForSurvival: "Rebuilt ventilation manifolds.",
                day: 5);

            var proj = system.ProjectEffects("survivor_custom");

            Assert.NotNull(proj);
            Assert.Equal("Engineer", proj.OccupationLabel);
            Assert.True(proj.SkillBonuses.ContainsKey("technical"));
            Assert.Single(proj.ExperienceLabels);
        }

        [Fact]
        public void Backstory_SaveRestore_RoundtripsDeterministically()
        {
            var system = CreateSystemWithCatalog();
            var b1 = system.AssignFromTemplate("surv_a", "bst_military_medic", 1);
            system.RevealSecret("surv_a", b1.Secrets[0]);
            system.AssignFromTemplate("surv_b", "bst_rural_engineer", 2);

            var state = system.CaptureState();
            Assert.NotNull(state);
            Assert.Equal(2, state.Backstories.Count);

            var restoredSystem = CreateSystemWithCatalog();
            restoredSystem.RestoreState(state);

            Assert.Equal(2, restoredSystem.BackstoryCount);
            var restoredA = restoredSystem.GetBackstory("surv_a");
            Assert.NotNull(restoredA);
            Assert.Equal("occ_doctor", restoredA!.OccupationId);
            Assert.Single(restoredA.RevealedSecrets);
            Assert.Equal(b1.Secrets[0], restoredA.RevealedSecrets[0]);
        }

        [Fact]
        public void Backstory_Census_ExposesTruthfulCounts()
        {
            var system = CreateSystemWithCatalog();
            var b1 = system.AssignFromTemplate("surv_x", "bst_military_medic", 1);
            system.RevealSecret("surv_x", b1.Secrets[0]);

            var census = system.GetCensus();

            Assert.Equal(1, census.TotalBackstories);
            Assert.Equal(1, census.TotalRevealedSecrets);
            Assert.True(census.TotalOccupations >= 8);
            Assert.True(census.TotalExperiences >= 5);
            Assert.True(census.TotalTemplates >= 4);
        }

        [Fact]
        public void Backstory_SaveSectionRegistry_IsRegistered()
        {
            Assert.Contains("backstory", SaveSectionRegistry.SectionKeys);
            Assert.Equal("backstory_save.json", SaveSectionRegistry.FileNameFor("backstory"));

            var all = SaveSectionRegistry.All;
            var entry = all.FirstOrDefault(s => s.SectionKey == "backstory");
            Assert.NotNull(entry);
            Assert.Equal("SaveBackstory", entry!.SaveMethod);
            Assert.Equal("SetupBackstory", entry.SetupMethod);
            Assert.Equal("survivors", entry.Owner);
            Assert.Equal(SaveSectionRegistry.ExpandedShelterLifecycleGroup, entry.LifecycleGroup);
        }

        [Fact]
        public void Backstory_HostWiring_PresenceVerifiedInSource()
        {
            string hostSession = ReadRepoFile("src", "Host", "BackstoryHostSession.cs");
            Assert.Contains("class BackstoryHostSession", hostSession);
            Assert.Contains("class BackstorySaveStore", hostSession);
            Assert.Contains("backstory_save.json", hostSession);

            string hostCli = ReadRepoFile("src", "Host", "HostCli.Backstory.cs");
            Assert.Contains("HostCliBackstory", hostCli);
            Assert.Contains("RunSelfTest", hostCli);

            string mainBackstory = ReadRepoFile("src", "Main.Backstory.cs");
            Assert.Contains("SetupBackstory()", mainBackstory);
            Assert.Contains("SaveBackstory()", mainBackstory);

            string campaignOwners = ReadRepoFile("src", "Main.CampaignOwners.cs");
            Assert.Contains("BackstoryDayOwner", campaignOwners);
            Assert.Contains("backstory_ticked", campaignOwners);

            string detailPanel = ReadRepoFile("src", "UI", "SurvivorDetailPanel.cs");
            Assert.Contains("BackstoryProvider", detailPanel);
        }

        [Fact]
        public void Backstory_LiveAssignmentAndLifecycleReset_PresentInSource()
        {
            // Plan 174 live wiring: the catalog must be applied to the real roster,
            // deterministically, and the session must participate in the campaign
            // lifecycle reset so a new campaign cannot inherit the previous run.
            string mainBackstory = ReadRepoFile("src", "Main.Backstory.cs");
            Assert.Contains("AssignMissingBackstories", mainBackstory);
            Assert.Contains("StableHash.Of", mainBackstory);

            string gameFlow = ReadRepoFile("src", "Main.GameFlow.cs");
            Assert.Contains("AssignMissingBackstories()", gameFlow);

            string lifecycle = ReadRepoFile("src", "Main.Lifecycle.cs");
            Assert.Contains("late_wave_integrations", lifecycle);
            Assert.Contains("ResetLateWaveIntegrationSessions", lifecycle);
            Assert.Contains("ResetBackstory()", lifecycle);
            Assert.Contains("ResetMetaProgression()", lifecycle);
        }
    }
}
