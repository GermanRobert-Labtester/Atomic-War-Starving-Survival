// SPDX-License-Identifier: MIT
// ============================================================================
// Unit Tests: MutationSystemTests (Plan 180)
// ============================================================================
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Medical;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests.Medical
{
    public sealed class MutationSystemTests
    {
        private static MutationSystem CreateSystem(int seed = 42)
        {
            var inv = new Inventory.Inventory();
            var sys = new MutationSystem(new SeededRng(seed), inv);
            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mutation_low_light_adaptation",
                display_name = "Tapetal Luminescence",
                branch_id = "sensory",
                tier = 1,
                parent_mutation_ids = new List<string>(),
                exclusive_mutation_ids = new List<string> { "mutation_photophobia" },
                capability_tags = new List<string> { "capability_low_light_vision" },
                required_exposure = 20.0f,
                instability_cost = 15.0f
            });
            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mutation_photophobia",
                display_name = "Acute Photophobia",
                branch_id = "sensory",
                tier = 1,
                parent_mutation_ids = new List<string>(),
                exclusive_mutation_ids = new List<string> { "mutation_low_light_adaptation" },
                capability_tags = new List<string>(),
                required_exposure = 20.0f,
                instability_cost = 20.0f
            });
            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mutation_heightened_hearing",
                display_name = "Tympanic Hyper-Resonance",
                branch_id = "sensory",
                tier = 2,
                parent_mutation_ids = new List<string> { "mutation_low_light_adaptation" },
                exclusive_mutation_ids = new List<string>(),
                capability_tags = new List<string> { "capability_enhanced_hearing" },
                required_exposure = 60.0f,
                instability_cost = 25.0f
            });
            return sys;
        }

        [Fact]
        public void MutationRisk_Scales_NonLinearly_With_Cumulative_Dose()
        {
            var sys = CreateSystem();
            sys.AddRadiationExposure("survivor_rad", 10.0f, 1);
            float lowChance = sys.CalculateMutationChance("survivor_rad");

            sys.AddRadiationExposure("survivor_rad", 80.0f, 2);
            float highChance = sys.CalculateMutationChance("survivor_rad");

            Assert.True(highChance > lowChance);
            var prof = sys.GetProfile("survivor_rad");
            Assert.True(prof!.geneticInstability > 0f);
        }

        [Fact]
        public void MutateSurvivor_Enforces_Parent_Requirements_And_Exclusivity()
        {
            var sys = CreateSystem(777);
            sys.AddRadiationExposure("survivor_mut", 150.0f, 1);

            // Tier 2 (heightened hearing) cannot be acquired without Tier 1 (low light)
            var prof = sys.GetProfile("survivor_mut");
            Assert.Empty(prof!.activeMutationIds);

            // Directly grant low light
            prof.activeMutationIds.Add("mutation_low_light_adaptation");

            // Photophobia is exclusive with low light, so it should not be chosen
            sys.TryMutateSurvivor("survivor_mut", 2);
            Assert.DoesNotContain("mutation_photophobia", prof.activeMutationIds);
        }

        [Fact]
        public void RadAway_Overuse_Increases_Genetic_Instability()
        {
            var sys = CreateSystem();
            sys.AddRadiationExposure("survivor_detox", 50.0f, 1);
            var prof = sys.GetProfile("survivor_detox");
            float instBefore = prof!.geneticInstability;

            sys.AdministerRadAway("survivor_detox", 10f, 2);
            sys.AdministerRadAway("survivor_detox", 10f, 3);
            sys.AdministerRadAway("survivor_detox", 10f, 4);

            Assert.True(prof.geneticInstability > instBefore);
            Assert.Equal(3, prof.radAwayDosesAdministered);
        }

        [Fact]
        public void GeneTherapy_Excises_Target_Mutation_And_Removes_Effects()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("gene_therapy_retroviral_vial", 1);
            var sys = new MutationSystem(new SeededRng(42), inv);
            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mutation_low_light_adaptation",
                capability_tags = new List<string> { "capability_low_light_vision" }
            });

            var prof = sys.EnsureProfile("survivor_patient");
            prof.activeMutationIds.Add("mutation_low_light_adaptation");
            prof.geneticInstability = 40.0f;

            var res = sys.PerformGeneTherapy("survivor_patient", "mutation_low_light_adaptation", 5);
            Assert.True(res.Success);
            Assert.DoesNotContain("mutation_low_light_adaptation", prof.activeMutationIds);
            Assert.Equal(25.0f, prof.geneticInstability); // 40 - 15 reduction
            Assert.Equal(0, inv.CountById("gene_therapy_retroviral_vial"));
        }

        [Fact]
        public void CapabilityTags_Project_Active_Mutations()
        {
            var sys = CreateSystem();
            var prof = sys.EnsureProfile("survivor_tags");
            prof.activeMutationIds.Add("mutation_low_light_adaptation");
            prof.activeMutationIds.Add("mutation_heightened_hearing");

            var caps = sys.GetCapabilityTags("survivor_tags");
            Assert.Contains("capability_low_light_vision", caps);
            Assert.Contains("capability_enhanced_hearing", caps);
        }

        [Fact]
        public void State_RoundTrip_Preserves_Active_Mutations_And_Instability()
        {
            var sys = CreateSystem();
            var prof = sys.EnsureProfile("save_survivor");
            prof.activeMutationIds.Add("mutation_low_light_adaptation");
            prof.geneticInstability = 33.5f;
            prof.cumulativeRadDose = 120.0f;

            var state = sys.CaptureState();
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            var restoredState = System.Text.Json.JsonSerializer.Deserialize<MutationState>(json);

            Assert.NotNull(restoredState);
            var restoredSys = CreateSystem();
            restoredSys.RestoreState(restoredState!);

            var restoredProf = restoredSys.GetProfile("save_survivor");
            Assert.NotNull(restoredProf);
            Assert.Contains("mutation_low_light_adaptation", restoredProf!.activeMutationIds);
            Assert.Equal(33.5f, restoredProf.geneticInstability);
            Assert.Equal(120.0f, restoredProf.cumulativeRadDose);
        }
    }
}
