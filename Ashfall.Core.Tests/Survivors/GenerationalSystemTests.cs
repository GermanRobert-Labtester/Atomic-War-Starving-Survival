// SPDX-License-Identifier: MIT
// ============================================================================
// Unit Tests: GenerationalSystemTests (Plan 178)
// ============================================================================
using System.Collections.Generic;
using Xunit;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class GenerationalSystemTests
    {
        private static GenerationalSystem CreateSystem(int seed = 42)
        {
            var inv = new Inventory.Inventory();
            var sys = new GenerationalSystem(new SeededRng(seed), inv);
            sys.RegisterTrait(new DevelopmentTraitDef
            {
                trait_id = "development_trait_resilient",
                display_name = "Hardened Resilience",
                min_trauma = 0f,
                max_trauma = 50f,
                min_education = 10f,
                weight = 1.0f,
                exclusive_group = "resilience"
            });
            sys.RegisterTrait(new DevelopmentTraitDef
            {
                trait_id = "development_trait_hypervigilant",
                display_name = "Hypervigilant",
                min_trauma = 40f,
                max_trauma = 100f,
                min_education = 0f,
                weight = 1.0f,
                exclusive_group = "trauma"
            });
            sys.RegisterTrait(new DevelopmentTraitDef
            {
                trait_id = "development_trait_malnourished_growth",
                display_name = "Stunted Physique",
                min_trauma = 0f,
                max_trauma = 100f,
                min_education = 0f,
                weight = 1.0f,
                exclusive_group = "physique"
            });
            return sys;
        }

        [Fact]
        public void GrowthTick_Is_Idempotent_On_Same_Day()
        {
            var sys = CreateSystem();
            var child = sys.EnsureChild("child_1", 1, DevelopmentPhase.YoungChild);
            float initialProgress = child.developmentProgress;

            sys.GrowthTick(2);
            float progressAfterTick1 = child.developmentProgress;
            Assert.True(progressAfterTick1 > initialProgress);

            // Same day tick should not advance growth further
            sys.GrowthTick(2);
            Assert.Equal(progressAfterTick1, child.developmentProgress);
        }

        [Fact]
        public void Child_Advances_Phase_At_Configured_Thresholds()
        {
            var sys = CreateSystem();
            var child = sys.EnsureChild("child_grow", 1, DevelopmentPhase.YoungChild);
            child.developmentProgress = 48.0f;

            sys.GrowthTick(2); // Growth adds >= 2.5f -> progress reaches >= 50.0f
            Assert.True(child.developmentProgress >= 50.0f);
            Assert.Equal(DevelopmentPhase.OlderChild, child.developmentPhase);
        }

        [Fact]
        public void Schoolhouse_Accumulates_Education_XP_With_Teacher()
        {
            var inv = new Inventory.Inventory();
            inv.AddById("school_primer", 1);
            var sys = new GenerationalSystem(new SeededRng(42), inv);

            var child = sys.EnsureChild("student_1", 1, DevelopmentPhase.OlderChild);
            sys.AssignTeacher("student_1", "teacher_elena", "first_aid");

            float xpBefore = child.educationXp;
            sys.GrowthTick(2);
            Assert.True(child.educationXp > xpBefore);
            Assert.True(child.educationXp >= 8.0f); // 5 base + 3 tool bonus
        }

        [Fact]
        public void TraumaLoad_Increases_From_Severe_Witnessed_Events()
        {
            var sys = CreateSystem();
            var child = sys.EnsureChild("trauma_victim", 1, DevelopmentPhase.YoungChild);
            float traumaBefore = child.traumaLoad;

            sys.RecordFormativeEvent("trauma_victim", "raid_breach", 3.0f, "raider_boss", 2);
            Assert.True(child.traumaLoad > traumaBefore);
            Assert.Contains(child.formativeEventIds, id => id.StartsWith("formative_trauma_victim_2"));
        }

        [Fact]
        public void Adulthood_Occurs_Exactly_Once_With_Deterministic_Traits()
        {
            var sys = CreateSystem(999);
            var child = sys.EnsureChild("future_adult", 1, DevelopmentPhase.Adolescent);
            child.developmentProgress = 98.0f;
            child.educationXp = 25.0f;
            child.traumaLoad = 10.0f;

            bool transitionFired = false;
            sys.OnAdulthoodReached += (id, phase, traits) =>
            {
                transitionFired = true;
            };

            sys.GrowthTick(2);
            Assert.True(transitionFired);
            Assert.Equal(DevelopmentPhase.AdultTransitioned, child.developmentPhase);
            Assert.True(child.adulthoodProcessed);
            Assert.NotEmpty(child.acquiredDevelopmentTraitIds);

            // Ticking subsequent days does not re-process adulthood
            transitionFired = false;
            sys.GrowthTick(3);
            Assert.False(transitionFired);
            Assert.Equal(1, sys.State.totalAdulthoodTransitions);
        }

        [Fact]
        public void State_RoundTrip_Preserves_Partial_Development()
        {
            var sys = CreateSystem();
            var child = sys.EnsureChild("save_child", 1, DevelopmentPhase.YoungChild);
            child.developmentProgress = 37.5f;
            child.traumaLoad = 18.0f;
            child.educationXp = 12.0f;
            sys.AssignGuardian("save_child", "guardian_sarah");

            var state = sys.CaptureState();
            var json = System.Text.Json.JsonSerializer.Serialize(state);
            var restoredState = System.Text.Json.JsonSerializer.Deserialize<GenerationalState>(json);

            Assert.NotNull(restoredState);
            var restoredSys = CreateSystem();
            restoredSys.RestoreState(restoredState!);

            var restoredChild = restoredSys.GetChild("save_child");
            Assert.NotNull(restoredChild);
            Assert.Equal(37.5f, restoredChild!.developmentProgress);
            Assert.Equal(18.0f, restoredChild.traumaLoad);
            Assert.Equal(12.0f, restoredChild.educationXp);
            Assert.Equal("guardian_sarah", restoredChild.assignedGuardianId);
        }
    }
}
