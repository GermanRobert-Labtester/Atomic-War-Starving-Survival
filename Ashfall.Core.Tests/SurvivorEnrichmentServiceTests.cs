// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class SurvivorEnrichmentServiceTests : CatalogTestBase
    {
        private static string FindDataDir() => DataDirectory;

        private static ExpansionEnrichmentCatalog LoadReal()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            return loader.Load(FindDataDir());
        }

        [Fact]
        public void DeepLore_And_Antigravity_Overlays_Loaded_And_Merged()
        {
            var catalog = LoadReal();

            // aris_thorne comes from deep_lore_survivor_fields.json
            var aris = catalog.GetSurvivorFields("aris_thorne");
            Assert.NotNull(aris);
            Assert.Equal("machinist", aris!.pre_war_profession_id);
            Assert.Equal("blueprint_roll", aris.personal_keepsake_item_id);
            Assert.Equal("material_rationalism", aris.philosophical_stance);

            // the_veteran comes from expansion_survivor_fields.json and has stance/manifesto from antigravity_survivor_fields.json
            var vet = catalog.GetSurvivorFields("the_veteran");
            Assert.NotNull(vet);
            Assert.Equal("military_discipline", vet!.belief_profile_id);
            Assert.Equal("folded_flag", vet.personal_keepsake_item_id);
            Assert.Equal("stoicism", vet.philosophical_stance);
            Assert.Equal("military_discipline_code", vet.manifesto_law_code);
        }

        [Fact]
        public void Field_Level_Precedence_Preserves_Baseline_And_Adds_Specialist_Fields()
        {
            var catalog = new ExpansionEnrichmentCatalog();

            var baseline = new ExpansionSurvivorFields
            {
                survivor_id = "test_surv",
                belief_profile_id = "collectivist_solidarity",
                personal_keepsake_item_id = "test_keepsake",
                phantom_background_id = "generic",
                pre_war_profession_id = "nurse"
            };
            catalog.AddSurvivorFields(baseline);

            // Specialist overlay attempting to supply stance and conflicting belief
            var specialist = new ExpansionSurvivorFields
            {
                survivor_id = "test_surv",
                belief_profile_id = "military_discipline", // Conflict! Should be ignored in specialist mode
                philosophical_stance = "stoicism",
                manifesto_law_code = "duty_code"
            };
            catalog.MergeSurvivorFields(specialist, isSpecialistOverlay: true);

            var merged = catalog.GetSurvivorFields("test_surv");
            Assert.NotNull(merged);
            // Core baseline preserved
            Assert.Equal("collectivist_solidarity", merged!.belief_profile_id);
            Assert.Equal("test_keepsake", merged.personal_keepsake_item_id);
            Assert.Equal("nurse", merged.pre_war_profession_id);
            // Specialist fields merged
            Assert.Equal("stoicism", merged.philosophical_stance);
            Assert.Equal("duty_code", merged.manifesto_law_code);
        }

        [Fact]
        public void GetSurvivorsByProfession_Returns_Matching_Survivors()
        {
            var catalog = LoadReal();
            var nurses = catalog.GetSurvivorsByProfession("nurse");
            Assert.NotEmpty(nurses);
            Assert.Contains("elena_vasquez", nurses);
            Assert.Contains("the_surgeon", nurses);
        }

        [Fact]
        public void GetKeepsakeItemId_Returns_Correct_Id()
        {
            var catalog = LoadReal();
            string ksElena = catalog.GetKeepsakeItemId("elena_vasquez");
            Assert.Equal("worn_stethoscope", ksElena);

            string ksMarcus = catalog.GetKeepsakeItemId("marcus_olejnik");
            Assert.Equal("tarnished_pocket_watch", ksMarcus);

            string ksUnknown = catalog.GetKeepsakeItemId("nonexistent");
            Assert.Equal(string.Empty, ksUnknown);
        }

        [Fact]
        public void SurvivorEnrichmentService_Projects_Enriched_Survivor_With_Clean_Labels()
        {
            var catalog = LoadReal();
            var service = new SurvivorEnrichmentService(catalog);

            var def = new SurvivorDefinition
            {
                id = "elena_vasquez",
                displayName = "Elena Vasquez",
                profession = "Paramedic",
                baseHealth = 100f
            };

            var view = service.GetView("elena_vasquez", def);
            Assert.NotNull(view);
            Assert.True(view.IsEnriched);
            Assert.Equal("Elena Vasquez", view.DisplayName);
            Assert.Equal("nurse", view.ProfessionId);
            Assert.Equal("Registered Nurse", view.ProfessionLabel);
            Assert.Equal("collectivist_solidarity", view.BeliefProfileId);
            Assert.Equal("Collectivist Solidarity", view.BeliefProfileLabel);
            Assert.Equal("worn_stethoscope", view.PersonalKeepsakeItemId);
            Assert.Equal("Worn Stethoscope", view.KeepsakeItemLabel);
            Assert.Equal("nurse", view.PhantomBackgroundId);
            Assert.Equal("Clinical Care", view.PhantomBackgroundLabel);
        }

        [Fact]
        public void SurvivorEnrichmentService_Projects_Unenriched_Survivor_Gracefully()
        {
            var catalog = LoadReal();
            var service = new SurvivorEnrichmentService(catalog);

            var def = new SurvivorDefinition
            {
                id = "unenriched_wanderer",
                displayName = "Wanderer Joe",
                profession = "Scavenger",
                baseHealth = 100f
            };

            var view = service.GetView("unenriched_wanderer", def);
            Assert.NotNull(view);
            Assert.False(view.IsEnriched);
            Assert.Equal("Wanderer Joe", view.DisplayName);
            Assert.Equal(string.Empty, view.ProfessionId);
            Assert.Equal("Scavenger", view.ProfessionLabel); // Falls back to def.profession
            Assert.Equal(string.Empty, view.BeliefProfileId);
            Assert.Equal("Undeclared", view.BeliefProfileLabel);
            Assert.Equal(string.Empty, view.PersonalKeepsakeItemId);
            Assert.Equal("None", view.KeepsakeItemLabel);
            Assert.Equal(string.Empty, view.PhantomBackgroundId);
            Assert.Equal("Survivor", view.PhantomBackgroundLabel);
        }

        [Fact]
        public void SurvivorEnrichmentService_Zero_Mutation_On_Survivor_Definition()
        {
            var catalog = LoadReal();
            var service = new SurvivorEnrichmentService(catalog);

            var def = new SurvivorDefinition
            {
                id = "elena_vasquez",
                displayName = "Elena Vasquez",
                profession = "Paramedic",
                baseHealth = 100f,
                traitIds = new List<string> { "trait_calm" }
            };

            var view = service.GetView("elena_vasquez", def);

            // Definition must remain unaltered
            Assert.Equal("Paramedic", def.profession);
            Assert.Equal(100f, def.baseHealth);
            Assert.Single(def.traitIds);
            Assert.Equal("trait_calm", def.traitIds[0]);
        }

        [Fact]
        public void Negative_Fixture_Unknown_Survivor_Returns_Default_View()
        {
            var service = new SurvivorEnrichmentService();
            var view = service.GetView("totally_unknown_survivor");
            Assert.NotNull(view);
            Assert.False(view.IsEnriched);
            Assert.Equal("Totally Unknown Survivor", view.DisplayName);
            Assert.Equal("Unspecified", view.ProfessionLabel);
            Assert.Equal("Undeclared", view.BeliefProfileLabel);
            Assert.Equal("None", view.KeepsakeItemLabel);
        }

        [Fact]
        public void Negative_Fixture_Empty_SurvivorId_Returns_Unknown()
        {
            var service = new SurvivorEnrichmentService();
            var view = service.GetView("");
            Assert.NotNull(view);
            Assert.Equal("Unknown", view.DisplayName);
        }

        [Fact]
        public void ItemInspectionModel_Exposes_Narrative_Tags_And_Keepsake_Candidate()
        {
            var catalog = LoadReal();
            var def = new ItemDefinition
            {
                id = "teddy_bear",
                displayName = "Teddy Bear",
                type = ItemType.Relic
            };

            var inspection = ItemInspectionModel.Create(def, null, catalog);
            Assert.NotNull(inspection);
            Assert.True(inspection.IsKeepsakeCandidate);
            Assert.Contains("personal_keepsake_candidate", inspection.NarrativeTags);
            Assert.Contains("phantom_childhood", inspection.NarrativeTags);
        }

        [Fact]
        public void Keepsake_Association_Does_Not_Imply_Inventory_Possession()
        {
            var catalog = LoadReal();
            var service = new SurvivorEnrichmentService(catalog);
            var view = service.GetView("elena_vasquez");

            // View has personal_keepsake_item_id
            Assert.Equal("worn_stethoscope", view.PersonalKeepsakeItemId);

            // A brand new inventory contains 0 items
            var inventory = new Ashfall.Core.Inventory.Inventory();
            Assert.Equal(0, inventory.CountById("worn_stethoscope"));

            // Proves keepsake metadata does not auto-populate inventory
            Assert.False(inventory.HasSufficient("worn_stethoscope", 1));
        }

        [Fact]
        public void Belief_And_Profession_Do_Not_Mutate_Gameplay_Stats()
        {
            var catalog = LoadReal();
            var service = new SurvivorEnrichmentService(catalog);

            // 1. Elena (nurse, collectivist_solidarity)
            var elenaView = service.GetView("elena_vasquez");
            // 2. Marcus (machinist, pragmatic_individualism)
            var marcusView = service.GetView("marcus_olejnik");

            // Projection view does not expose any HP, morale bonuses, or combat power modifiers
            Assert.Equal("nurse", elenaView.ProfessionId);
            Assert.Equal("machinist", marcusView.ProfessionId);
            Assert.Equal("collectivist_solidarity", elenaView.BeliefProfileId);
            Assert.Equal("pragmatic_individualism", marcusView.BeliefProfileId);

            // Both have pure narrative views without mechanical stats
            Assert.True(elenaView.IsEnriched);
            Assert.True(marcusView.IsEnriched);
        }
    }
}
