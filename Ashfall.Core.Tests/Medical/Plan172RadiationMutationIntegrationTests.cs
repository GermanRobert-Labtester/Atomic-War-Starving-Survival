// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 172: Radiation & Genetic Mutation Integration Tests
// Verifies mutation catalog loading, risk calculations, inheritance,
// stat modifier aggregation, and social stigma scaling.
// ============================================================================
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Medical;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests.Plan172RadiationMutation
{
    public sealed class Plan172RadiationMutationIntegrationTests
    {
        private static MutationSystem CreateSystem(int seed = 12345)
        {
            var inv = new Inventory.Inventory();
            return new MutationSystem(new SeededRng(seed), inv);
        }

        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(System.AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(System.AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return Path.GetFullPath(c);
            }
            return Path.GetFullPath(Path.Combine(System.AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsValidMutationsFromDataCatalog()
        {
            var sys = CreateSystem();
            string dataPath = ResolveDataPath("mutations.json");
            Assert.True(File.Exists(dataPath), $"mutations.json must exist at {dataPath}");

            string json = File.ReadAllText(dataPath);
            sys.LoadCatalog(json);

            // Verify mutations are loaded and can be queried
            var allMutations = sys.GetAllMutations();
            Assert.NotEmpty(allMutations);
            Assert.Contains(allMutations, m => m.mutation_id == "mutation_low_light_adaptation");
            Assert.Contains(allMutations, m => m.mutation_id == "mutation_keratinized_skin");
        }

        [Fact]
        public void CalculateMutationChance_ScalesWithDoseAndInstability()
        {
            var sys = CreateSystem();
            string survivorId = "survivor_alpha";

            float initialChance = sys.CalculateMutationChance(survivorId);
            Assert.Equal(0.0f, initialChance);

            sys.AddRadiationExposure(survivorId, 50.0f, 1);
            float midChance = sys.CalculateMutationChance(survivorId);
            Assert.True(midChance > initialChance);

            sys.AddRadiationExposure(survivorId, 150.0f, 2);
            float highChance = sys.CalculateMutationChance(survivorId);
            Assert.True(highChance > midChance);
            Assert.InRange(highChance, 0.0f, 0.90f);
        }

        [Fact]
        public void InheritMutations_PassesTraitsToDescendantsDeterministically()
        {
            var sys = CreateSystem(42);
            string parentId = "parent_eva";
            string childId = "child_noah";

            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mutation_keratinized_skin",
                display_name = "Keratinized Callus Plating",
                branch_id = "dermal",
                tier = 1,
                capability_tags = new List<string> { "capability_chitin_armor" },
                visual_tags = new List<string> { "scaled_arms" },
                instability_cost = 10.0f
            });

            var parentProf = sys.EnsureProfile(parentId);
            parentProf.activeMutationIds.Add("mutation_keratinized_skin");

            // Probability 1.0f guarantees inheritance
            var inherited = sys.InheritMutations(parentId, childId, 1.0f);
            Assert.Single(inherited);
            Assert.Equal("mutation_keratinized_skin", inherited[0]);

            var childProf = sys.GetProfile(childId);
            Assert.NotNull(childProf);
            Assert.Contains("mutation_keratinized_skin", childProf.activeMutationIds);
            Assert.True(childProf.geneticInstability >= 10.0f);
        }

        [Fact]
        public void GetStatModifiers_AggregatesActiveMutationBonusesAndPenalties()
        {
            var sys = CreateSystem();
            string survivorId = "survivor_heavy";

            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mut_armor",
                display_name = "Plated Skin",
                stat_modifiers = new Dictionary<string, float> { { "armor_rating", 12.0f } }
            });
            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mut_speed_pen",
                display_name = "Heavy Limbs",
                stat_modifiers = new Dictionary<string, float> { { "armor_rating", 3.0f }, { "speed", -0.15f } }
            });

            var prof = sys.EnsureProfile(survivorId);
            prof.activeMutationIds.Add("mut_armor");
            prof.activeMutationIds.Add("mut_speed_pen");

            var mods = sys.GetStatModifiers(survivorId);
            Assert.Equal(15.0f, mods["armor_rating"]);
            Assert.Equal(-0.15f, mods["speed"]);
        }

        [Fact]
        public void CalculateSocialStigmaPenalty_ScalesWithVisibleTags()
        {
            var sys = CreateSystem();
            string survivorId = "survivor_deviant";

            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mut_scales",
                display_name = "Reptilian Scales",
                visual_tags = new List<string> { "scaled_skin", "tail" }
            });
            sys.RegisterMutation(new MutationNode
            {
                mutation_id = "mut_eyes",
                display_name = "Glow Eyes",
                visual_tags = new List<string> { "luminescent_eyes" }
            });

            var prof = sys.EnsureProfile(survivorId);
            prof.activeMutationIds.Add("mut_scales");
            prof.activeMutationIds.Add("mut_eyes");

            var visibleTags = sys.GetVisibleTags(survivorId);
            Assert.Equal(3, visibleTags.Count);

            float penalty = sys.CalculateSocialStigmaPenalty(survivorId);
            // 3 tags * 0.05 = 0.15
            Assert.Equal(0.15f, penalty, 2);
        }

        [Fact]
        public void InstabilitySpike_EventTriggersOnMultipleRadAwayDetox()
        {
            var sys = CreateSystem();
            string survivorId = "survivor_purged";

            float spikedInstability = -1f;
            sys.OnInstabilitySpike += (sId, inst) =>
            {
                if (sId == survivorId) spikedInstability = inst;
            };

            sys.AddRadiationExposure(survivorId, 15.0f, 1);
            sys.AdministerRadAway(survivorId, 5.0f, 1);
            sys.AdministerRadAway(survivorId, 5.0f, 2);
            Assert.Equal(-1f, spikedInstability);

            // Third dose triggers chemical stress instability spike
            sys.AdministerRadAway(survivorId, 5.0f, 3);
            Assert.True(spikedInstability > 0f);
        }
    }
}
