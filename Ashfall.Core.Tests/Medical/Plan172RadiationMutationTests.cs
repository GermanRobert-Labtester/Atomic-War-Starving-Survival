// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 172: Radiation Mutation & Genetic Instability Invariant Tests
// Pure deterministic simulation: exposure accumulation, instability spikes,
// mutation eligibility/exclusivity/parents, capability projections, gene therapy,
// and save round-trip.
// ============================================================================

using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Medical;
using Ashfall.Core.Random;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class Plan172RadiationMutationTests
    {
        private static MutationCatalog CreateTestCatalog()
        {
            return new MutationCatalog
            {
                schema_version = 1,
                mutations = new List<MutationNode>
                {
                    new MutationNode
                    {
                        mutation_id = "mut_eyes_glow",
                        display_name = "Tapetal Luminescence",
                        branch_id = "sensory",
                        tier = 1,
                        required_exposure = 50.0f,
                        instability_cost = 15.0f,
                        parent_mutation_ids = new List<string>(),
                        exclusive_mutation_ids = new List<string> { "mut_eyes_blind" },
                        capability_tags = new List<string> { "capability_night_vision" },
                        visual_tags = new List<string> { "eye_glow" },
                        stat_modifiers = new Dictionary<string, float> { { "perception", 1.0f } }
                    },
                    new MutationNode
                    {
                        mutation_id = "mut_eyes_blind",
                        display_name = "Melted Irises",
                        branch_id = "sensory",
                        tier = 1,
                        required_exposure = 60.0f,
                        instability_cost = 20.0f,
                        parent_mutation_ids = new List<string>(),
                        exclusive_mutation_ids = new List<string> { "mut_eyes_glow" },
                        capability_tags = new List<string>(),
                        visual_tags = new List<string> { "scarred_eyes" },
                        stat_modifiers = new Dictionary<string, float> { { "perception", -1.0f } }
                    },
                    new MutationNode
                    {
                        mutation_id = "mut_echo_hearing",
                        display_name = "Ossified Canals",
                        branch_id = "sensory",
                        tier = 2,
                        required_exposure = 120.0f,
                        instability_cost = 25.0f,
                        parent_mutation_ids = new List<string> { "mut_eyes_glow" },
                        exclusive_mutation_ids = new List<string>(),
                        capability_tags = new List<string> { "capability_sonar" },
                        visual_tags = new List<string> { "large_ears" },
                        stat_modifiers = new Dictionary<string, float>()
                    },
                    new MutationNode
                    {
                        mutation_id = "mut_chitin_skin",
                        display_name = "Keratinized Callus Plating",
                        branch_id = "dermal",
                        tier = 1,
                        required_exposure = 80.0f,
                        instability_cost = 20.0f,
                        parent_mutation_ids = new List<string>(),
                        exclusive_mutation_ids = new List<string>(),
                        capability_tags = new List<string> { "capability_chitin_armor" },
                        visual_tags = new List<string> { "scaled_skin" },
                        stat_modifiers = new Dictionary<string, float> { { "armor_rating", 10.0f } }
                    }
                }
            };
        }

        private static MutationSystem CreateSystem(ISeededRng rng, Ashfall.Core.Inventory.Inventory? inv = null)
        {
            var inventory = inv ?? new Ashfall.Core.Inventory.Inventory();
            var system = new MutationSystem(rng, inventory);
            var catalog = CreateTestCatalog();
            foreach (var m in catalog.mutations)
            {
                system.RegisterMutation(m);
            }
            return system;
        }

        [Fact]
        public void ExposureAccrual_And_InstabilitySpike_Thresholds()
        {
            var system = CreateSystem(new SeededRng(100));

            // Below 20 mSv: no instability spike
            system.AddRadiationExposure("surv_1", 15.0f, 1);
            var prof = system.GetProfile("surv_1");
            Assert.NotNull(prof);
            Assert.Equal(15.0f, prof!.cumulativeRadDose);
            Assert.Equal(0f, prof.geneticInstability);

            // Dose > 20 mSv: spike = (25 - 20) * 0.25 = 1.25
            system.AddRadiationExposure("surv_1", 25.0f, 1);
            Assert.Equal(40.0f, prof.cumulativeRadDose);
            Assert.Equal(1.25f, prof.geneticInstability);
            Assert.Equal(1.25f, prof.lifetimePeakInstability);
        }

        [Fact]
        public void MutationChance_CalculatesFromDoseAndInstability()
        {
            var system = CreateSystem(new SeededRng(100));
            system.AddRadiationExposure("surv_1", 100.0f, 1);
            // dose = 100, instability = (100 - 20) * 0.25 = 20.0
            // baseChance = (100 * 0.0015) + (20 * 0.004) = 0.15 + 0.08 = 0.23
            float chance = system.CalculateMutationChance("surv_1");
            Assert.True(chance >= 0.20f && chance <= 0.25f);
        }

        [Fact]
        public void TryMutateSurvivor_RespectsRequiredExposure_And_Cooldown()
        {
            var rng = new SeededRng(42);
            var system = CreateSystem(rng);

            // Dose too low (10 mSv < 50 mSv min)
            system.AddRadiationExposure("surv_1", 10.0f, 1);
            bool mutatedLow = system.TryMutateSurvivor("surv_1", 1);
            Assert.False(mutatedLow);

            // Now expose heavily (100 mSv)
            system.AddRadiationExposure("surv_1", 90.0f, 2);
            bool mutatedDay2 = system.TryMutateSurvivor("surv_1", 2);
            // Cooldown: same day cannot mutate again
            bool mutatedAgainSameDay = system.TryMutateSurvivor("surv_1", 2);
            Assert.False(mutatedAgainSameDay);
        }

        [Fact]
        public void ExclusiveMutations_CannotBothBeAcquired()
        {
            var rng = new SeededRng(100);
            var system = CreateSystem(rng);
            var prof = system.EnsureProfile("surv_test");
            prof.cumulativeRadDose = 200.0f;
            prof.activeMutationIds.Add("mut_eyes_glow");

            // Attempt to mutate on day 3
            system.TryMutateSurvivor("surv_test", 3);

            // Cannot acquire mut_eyes_blind
            Assert.DoesNotContain("mut_eyes_blind", prof.activeMutationIds);
        }

        [Fact]
        public void ParentMutation_PrerequisiteIsEnforced()
        {
            var rng = new SeededRng(100);
            var system = CreateSystem(rng);
            var prof = system.EnsureProfile("surv_test");
            prof.cumulativeRadDose = 300.0f;
            // No mut_eyes_glow parent active

            system.TryMutateSurvivor("surv_test", 1);

            // mut_echo_hearing cannot be acquired without parent
            Assert.DoesNotContain("mut_echo_hearing", prof.activeMutationIds);
        }

        [Fact]
        public void Projections_CapabilityTags_StatModifiers_SocialStigma()
        {
            var system = CreateSystem(new SeededRng(100));
            var prof = system.EnsureProfile("surv_1");
            prof.activeMutationIds.Add("mut_eyes_glow");
            prof.activeMutationIds.Add("mut_chitin_skin");

            var caps = system.GetCapabilityTags("surv_1");
            Assert.Contains("capability_night_vision", caps);
            Assert.Contains("capability_chitin_armor", caps);

            var stats = system.GetStatModifiers("surv_1");
            Assert.Equal(1.0f, stats["perception"]);
            Assert.Equal(10.0f, stats["armor_rating"]);

            var vis = system.GetVisibleTags("surv_1");
            Assert.Equal(2, vis.Count);
            float stigma = system.CalculateSocialStigmaPenalty("surv_1");
            Assert.Equal(0.10f, stigma);
        }

        [Fact]
        public void GeneTherapy_ExcisesMutation_ConsumesVial_ReducesInstability()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            inv.Add(new Ashfall.Core.Inventory.ItemDefinition { id = "gene_therapy_retroviral_vial", displayName = "Retroviral Vial" }, 1);

            var system = CreateSystem(new SeededRng(100), inv);
            var prof = system.EnsureProfile("surv_1");
            prof.activeMutationIds.Add("mut_chitin_skin");
            prof.geneticInstability = 25.0f;

            var res = system.PerformGeneTherapy("surv_1", "mut_chitin_skin", 2);
            Assert.True(res.Success);
            Assert.DoesNotContain("mut_chitin_skin", prof.activeMutationIds);
            Assert.Equal(10.0f, prof.geneticInstability);
            Assert.Equal(0, inv.CountById("gene_therapy_retroviral_vial"));
            Assert.Equal(1, system.State.totalGeneTherapies);
        }

        [Fact]
        public void GeneTherapy_FailsIfVialMissing()
        {
            var inv = new Ashfall.Core.Inventory.Inventory(); // Empty
            var system = CreateSystem(new SeededRng(100), inv);
            var prof = system.EnsureProfile("surv_1");
            prof.activeMutationIds.Add("mut_chitin_skin");

            var res = system.PerformGeneTherapy("surv_1", "mut_chitin_skin", 2);
            Assert.False(res.Success);
            Assert.Equal("missing_retroviral_vial", res.FailureCode);
            Assert.Contains("mut_chitin_skin", prof.activeMutationIds);
        }

        [Fact]
        public void RadAway_ReducesCumulativeDose_And_SpikesInstabilityOnRepeatedUse()
        {
            var system = CreateSystem(new SeededRng(100));
            var prof = system.EnsureProfile("surv_1");
            prof.cumulativeRadDose = 100.0f;

            system.AdministerRadAway("surv_1", 30.0f, 1);
            Assert.Equal(70.0f, prof.cumulativeRadDose);
            Assert.Equal(1, prof.radAwayDosesAdministered);

            system.AdministerRadAway("surv_1", 30.0f, 2);
            system.AdministerRadAway("surv_1", 30.0f, 3); // 3rd dose triggers chemical stress (+8.0 instability)
            Assert.Equal(10.0f, prof.cumulativeRadDose);
            Assert.Equal(3, prof.radAwayDosesAdministered);
            Assert.Equal(8.0f, prof.geneticInstability);
        }

        [Fact]
        public void SaveRestore_RoundTrip_PreservesAllProfilesAndTotals()
        {
            var system = CreateSystem(new SeededRng(100));
            var prof = system.EnsureProfile("surv_save");
            prof.cumulativeRadDose = 150.0f;
            prof.geneticInstability = 32.5f;
            prof.lifetimePeakInstability = 45.0f;
            prof.activeMutationIds.Add("mut_eyes_glow");
            prof.lastMutationDay = 5;
            prof.geneTherapiesReceived = 2;
            system.State.totalMutationsAcquired = 3;
            system.State.totalGeneTherapies = 2;

            var snapshot = system.CaptureState();

            var freshSystem = CreateSystem(new SeededRng(200));
            freshSystem.RestoreState(snapshot);

            var restoredProf = freshSystem.GetProfile("surv_save");
            Assert.NotNull(restoredProf);
            Assert.Equal(150.0f, restoredProf!.cumulativeRadDose);
            Assert.Equal(32.5f, restoredProf.geneticInstability);
            Assert.Equal(45.0f, restoredProf.lifetimePeakInstability);
            Assert.Contains("mut_eyes_glow", restoredProf.activeMutationIds);
            Assert.Equal(5, restoredProf.lastMutationDay);
            Assert.Equal(2, restoredProf.geneTherapiesReceived);
            Assert.Equal(3, freshSystem.State.totalMutationsAcquired);
            Assert.Equal(2, freshSystem.State.totalGeneTherapies);
        }
    }
}
