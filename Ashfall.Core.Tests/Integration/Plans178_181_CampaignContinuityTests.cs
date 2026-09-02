// SPDX-License-Identifier: MIT
// ============================================================================
// Flagship Integration Test: Plans 178-181 Campaign Continuity
// Subsystems: Childhood Rearing, Prisoner Management, Mutation Trees, Stealth
// ============================================================================
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Ashfall.Core.Factions;
using Ashfall.Core.Medical;
using Ashfall.Core.Combat;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests.Integration
{
    public sealed class Plans178_181_CampaignContinuityTests
    {
        [Fact]
        public void MultiSystem_CampaignContinuity_Plans178_181()
        {
            var rng = new SeededRng(178181);
            var inv = new Inventory.Inventory();

            // Stock starting supplies
            inv.AddById("school_primer", 1);
            inv.AddById("ration_hardtack", 20);
            inv.AddById("clean_water", 20);
            inv.AddById("gene_therapy_retroviral_vial", 1);

            // 1. Initialize systems
            var generational = new GenerationalSystem(rng, inv);
            var prisoners = new PrisonerSystem(rng, inv);
            var mutations = new MutationSystem(rng, inv);
            var stealth = new StealthSystem(rng, inv);

            // Register authored definitions
            generational.RegisterTrait(new DevelopmentTraitDef
            {
                trait_id = "development_trait_field_medic_instinct",
                display_name = "Field Medic Instinct",
                min_trauma = 0f,
                max_trauma = 40f,
                min_education = 15f,
                weight = 1.0f
            });

            prisoners.RegisterTactic(new InterrogationTacticDef
            {
                tactic_id = "interrogation_conversation",
                display_name = "Rapport & Conversation",
                base_compliance_delta = 15f,
                trust_delta = 20f,
                fear_delta = -5f,
                intel_chance = 0.80f,
                false_intel_chance = 0.0f,
                severity = "Humane"
            });

            mutations.RegisterMutation(new MutationNode
            {
                mutation_id = "mutation_low_light_adaptation",
                display_name = "Tapetal Luminescence",
                capability_tags = new List<string> { "capability_low_light_vision" },
                required_exposure = 50.0f,
                instability_cost = 15.0f
            });

            stealth.RegisterCamouflageGear(new CamouflageGearDef
            {
                camo_id = "camo_ash_cloak",
                camo_rating = 0.5f,
                night_modifier = 0.2f
            });

            // ── STEP 1: Night Stealth Sortie — Rescue Orphan & Capture Raider ──
            stealth.EnsurePartyStealth("sortie_night");
            stealth.SetTravelMode("sortie_night", StealthTravelMode.NightOps);
            stealth.EquipCamoGear("sortie_night", "camo_ash_cloak");

            bool bypassed = stealth.BypassEncounter("sortie_night", "fog", isNight: true, "ruins");
            Assert.True(bypassed);

            // ── STEP 2: Welcome Child to Shelter & Schoolhouse ──
            var orphan = generational.EnsureChild("orphan_leo", 1, DevelopmentPhase.YoungChild);
            generational.AssignGuardian("orphan_leo", "guardian_sarah");
            generational.AssignTeacher("orphan_leo", "teacher_elena", "first_aid");

            // ── STEP 3: Detain & Interrogate Captured Raider ──
            Assert.True(prisoners.TakePrisoner("raider_scout", "faction_iron_crows", 1));
            var interrResult = prisoners.Interrogate("raider_scout", "interrogation_conversation", 1);
            Assert.True(interrResult.Success);
            Assert.True(interrResult.IntelDiscovered);

            // Child was not exposed to brutality -> trauma remains low
            Assert.Equal(0.0f, orphan.traumaLoad);

            // ── STEP 4: Chronic Radiation in Scavenger Induces Mutation ──
            mutations.AddRadiationExposure("scavenger_mira", 120.0f, 1);
            bool mutated = mutations.TryMutateSurvivor("scavenger_mira", 2);
            Assert.True(mutated);

            var caps = mutations.GetCapabilityTags("scavenger_mira");
            Assert.Contains("capability_low_light_vision", caps);

            // ── STEP 5: Gene Therapy Excises Mutation ──
            var geneRes = mutations.PerformGeneTherapy("scavenger_mira", "mutation_low_light_adaptation", 3);
            Assert.True(geneRes.Success);
            Assert.Empty(mutations.GetCapabilityTags("scavenger_mira"));

            // ── STEP 6: Multi-Day Growth into Adulthood ──
            for (int day = 2; day <= 20; day++)
            {
                generational.GrowthTick(day);
                prisoners.TickUpkeepAndEscape(day);
            }

            orphan.developmentProgress = 100.0f;
            generational.GrowthTick(21);
            Assert.Equal(DevelopmentPhase.AdultTransitioned, orphan.developmentPhase);
            Assert.True(orphan.adulthoodProcessed);
            Assert.NotEmpty(orphan.acquiredDevelopmentTraitIds);

            // ── STEP 7: Save Serialization & State Round-Trip ──
            var genJson = System.Text.Json.JsonSerializer.Serialize(generational.CaptureState());
            var prisJson = System.Text.Json.JsonSerializer.Serialize(prisoners.CaptureState());
            var mutJson = System.Text.Json.JsonSerializer.Serialize(mutations.CaptureState());
            var stlJson = System.Text.Json.JsonSerializer.Serialize(stealth.CaptureState());

            var restoredGen = System.Text.Json.JsonSerializer.Deserialize<GenerationalState>(genJson);
            var restoredPris = System.Text.Json.JsonSerializer.Deserialize<PrisonerState>(prisJson);
            var restoredMut = System.Text.Json.JsonSerializer.Deserialize<MutationState>(mutJson);
            var restoredStl = System.Text.Json.JsonSerializer.Deserialize<StealthState>(stlJson);

            Assert.NotNull(restoredGen);
            Assert.NotNull(restoredPris);
            Assert.NotNull(restoredMut);
            Assert.NotNull(restoredStl);

            Assert.Equal(1, restoredGen!.totalAdulthoodTransitions);
            Assert.NotEmpty(restoredPris!.extractedIntelRecords);
            Assert.Equal(1, restoredMut!.totalGeneTherapies);
            Assert.Equal(1, restoredStl!.totalBypasses);
        }
    }
}
