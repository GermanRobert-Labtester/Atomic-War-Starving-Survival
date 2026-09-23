// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class SurvivorIdentityObservabilityTests
    {
        [Fact]
        public void UnenrichedSurvivor_ReturnsDefaultObservabilitySlate()
        {
            var service = new SurvivorEnrichmentService();
            var slate = service.GetObservabilitySlate("survivor_unknown_99");

            Assert.Equal("survivor_unknown_99", slate.SurvivorId);
            Assert.False(slate.IsEnriched);
            Assert.Equal("Unspecified", slate.ProfessionLabel);
            Assert.Equal("Undeclared", slate.BeliefProfileLabel);
            Assert.Equal("None", slate.KeepsakeItemLabel);
            Assert.Empty(slate.KeepsakeItemTags);
            Assert.Equal("General Labor", slate.PrimaryDutyAffinity);
            Assert.Equal(0, slate.DutyComfortBonusPermille);
            Assert.Empty(slate.IdeologicalTensionBeliefId);
            Assert.Equal("None", slate.FrictionOpponentLabel);
            Assert.Empty(slate.CorePersonalityTraits);
        }

        [Fact]
        public void EnrichedSurvivor_ResolvesKeepsakeItemTagsFromCatalog()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_clockmaker",
                pre_war_profession_id = "machinist",
                personal_keepsake_item_id = "item_silver_pocketwatch",
                belief_profile_id = "atheist_rationalist",
                phantom_background_id = "machinist"
            });

            catalog.AddItemTags(new ExpansionItemTags
            {
                item_id = "item_silver_pocketwatch",
                tags = new List<string> { "personal_keepsake_candidate", "heirloom", "memory_token" }
            });

            var service = new SurvivorEnrichmentService(catalog);
            var slate = service.GetObservabilitySlate("survivor_clockmaker");

            Assert.True(slate.IsEnriched);
            Assert.Equal("Industrial Machinist", slate.ProfessionLabel);
            Assert.Equal("Item Silver Pocketwatch", slate.KeepsakeItemLabel);
            Assert.Equal(3, slate.KeepsakeItemTags.Count);
            Assert.Contains("personal_keepsake_candidate", slate.KeepsakeItemTags);
            Assert.Contains("heirloom", slate.KeepsakeItemTags);
            Assert.Contains("memory_token", slate.KeepsakeItemTags);
        }

        [Fact]
        public void EnrichedSurvivor_ResolvesDutyAffinityAndComfortBonus()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_medic",
                pre_war_profession_id = "nurse",
                belief_profile_id = "pacifist",
                phantom_background_id = "nurse"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var slate = service.GetObservabilitySlate("survivor_medic");

            Assert.Equal("Medical Ward", slate.PrimaryDutyAffinity);
            Assert.Equal(150, slate.DutyComfortBonusPermille); // +15% comfort
        }

        [Fact]
        public void EnrichedSurvivor_ResolvesIdeologicalTensionFrictionBelief()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_soldier",
                pre_war_profession_id = "soldier",
                belief_profile_id = "military_discipline",
                phantom_background_id = "former_soldier"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var slate = service.GetObservabilitySlate("survivor_soldier");

            Assert.Equal("Military Discipline", slate.BeliefProfileLabel);
            Assert.Equal("pacifist", slate.IdeologicalTensionBeliefId);
            Assert.Equal("Nonviolent Pacifism", slate.FrictionOpponentLabel);
        }

        [Fact]
        public void EnrichedSurvivor_SynthesizesDeterministicPersonalityTraitsWithoutRng()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_farmer_faith",
                pre_war_profession_id = "farmer",
                belief_profile_id = "religious_faith",
                phantom_background_id = "child_refugee"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var slate1 = service.GetObservabilitySlate("survivor_farmer_faith");
            var slate2 = service.GetObservabilitySlate("survivor_farmer_faith");

            Assert.Equal(3, slate1.CorePersonalityTraits.Count);
            Assert.Contains("Patient", slate1.CorePersonalityTraits);     // farmer
            Assert.Contains("Devout", slate1.CorePersonalityTraits);      // religious_faith
            Assert.Contains("Wary", slate1.CorePersonalityTraits);        // child_refugee

            // Deterministic consistency across calls
            Assert.Equal(slate1.CorePersonalityTraits, slate2.CorePersonalityTraits);
        }

        [Fact]
        public void EnrichedSurvivor_ObservabilitySlate_ProjectsTruthfulFields()
        {
            var catalog = new ExpansionEnrichmentCatalog();
            catalog.AddSurvivorFields(new ExpansionSurvivorFields
            {
                survivor_id = "survivor_theologian",
                pre_war_profession_id = "teacher",
                belief_profile_id = "religious_faith",
                personal_keepsake_item_id = "item_scripture_page",
                philosophical_stance = "Covenant of Grace",
                manifesto_law_code = "LEX-DIVINA-01",
                phantom_background_id = "teacher"
            });

            var service = new SurvivorEnrichmentService(catalog);
            var slate = service.GetObservabilitySlate("survivor_theologian");

            Assert.Equal("Secondary Educator", slate.ProfessionLabel);
            Assert.Equal("Spiritual Faith", slate.BeliefProfileLabel);
            Assert.Equal("Archive & Instruction", slate.PrimaryDutyAffinity);
            Assert.Equal("Covenant of Grace", slate.PhilosophicalStance);
            Assert.Equal("LEX-DIVINA-01", slate.ManifestoLawCode);
            Assert.Equal("atheist_rationalist", slate.IdeologicalTensionBeliefId);
            Assert.Equal("Material Rationalism", slate.FrictionOpponentLabel);
        }
    }
}
