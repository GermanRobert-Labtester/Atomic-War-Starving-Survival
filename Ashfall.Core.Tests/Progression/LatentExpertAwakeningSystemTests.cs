// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Progression
{
    public sealed class LatentExpertAwakeningSystemTests
    {
        private static string ResolveDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        private static SkillProgressionSystem CreateLoadedSkills()
        {
            var skills = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(skills, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());
            return skills;
        }

        [Fact]
        public void InitialState_Has12RegisteredAwakeningDefinitions()
        {
            var system = new LatentExpertAwakeningSystem();
            Assert.NotNull(system.GetDefinition("trait_miracle_worker"));
            Assert.NotNull(system.GetDefinition("trait_alchemist"));
            Assert.NotNull(system.GetDefinition("trait_grease_monkey"));
            Assert.NotNull(system.GetDefinition("trait_grid_walker"));
            Assert.NotNull(system.GetDefinition("trait_iron_chef"));
            Assert.NotNull(system.GetDefinition("trait_armorer"));
            Assert.NotNull(system.GetDefinition("trait_tinkerer"));
            Assert.NotNull(system.GetDefinition("trait_wasteland_scout"));
            Assert.NotNull(system.GetDefinition("trait_demolitions_expert"));
            Assert.NotNull(system.GetDefinition("trait_supply_chain_master"));
            Assert.NotNull(system.GetDefinition("trait_forge_master"));
            Assert.NotNull(system.GetDefinition("trait_sanitization_expert"));
        }

        [Fact]
        public void RecordProgress_IncrementsProgressAndAwakensAtThreshold()
        {
            var skills = CreateLoadedSkills();
            var system = new LatentExpertAwakeningSystem(skills);

            var actor = new SimpleSkillActor("surv_elena", "medical");

            bool awakenedFired = false;
            system.OnTraitAwakened += (survId, traitId, skillId, day) =>
            {
                if (survId == "surv_elena" && traitId == "trait_miracle_worker")
                    awakenedFired = true;
            };

            // trait_miracle_worker requires 1 emergency surgery
            Assert.False(system.IsAwakened("surv_elena", "trait_miracle_worker"));
            bool awakened = system.RecordProgress("surv_elena", "trait_miracle_worker", 1, 12, "Emergency thoracotomy under blackout", actor);

            Assert.True(awakened);
            Assert.True(awakenedFired);
            Assert.True(system.IsAwakened("surv_elena", "trait_miracle_worker"));
            Assert.True(skills.HasActiveSkill("surv_elena", "skill_miracle_worker"));
        }

        [Fact]
        public void MultiStepProgress_AwakensOnlyAfterFullThreshold()
        {
            var skills = CreateLoadedSkills();
            var system = new LatentExpertAwakeningSystem(skills);

            var actor = new SimpleSkillActor("surv_silas", "science");

            // trait_alchemist requires 5 reagent syntheses
            for (int i = 1; i <= 4; i++)
            {
                bool awakenedStep = system.RecordProgress("surv_silas", "trait_alchemist", 1, i, $"Reagent batch {i}", actor);
                Assert.False(awakenedStep);
                Assert.False(system.IsAwakened("surv_silas", "trait_alchemist"));
                Assert.Equal(i, system.GetProgress("surv_silas", "trait_alchemist"));
            }

            // 5th step triggers awakening
            bool finalAwakened = system.RecordProgress("surv_silas", "trait_alchemist", 1, 5, "Final reagent synthesis", actor);
            Assert.True(finalAwakened);
            Assert.True(system.IsAwakened("surv_silas", "trait_alchemist"));
            Assert.Equal(5, system.GetProgress("surv_silas", "trait_alchemist"));
            Assert.True(skills.HasActiveSkill("surv_silas", "skill_alchemist"));
        }

        [Fact]
        public void SaveAndRestore_PreservesProgressAndAwakenedStatus()
        {
            var skills = CreateLoadedSkills();
            var system = new LatentExpertAwakeningSystem(skills);
            var actor = new SimpleSkillActor("surv_tariq", "crafting");

            system.RecordProgress("surv_tariq", "trait_grease_monkey", 2, 8, "Generator repair", actor);

            var saved = system.CaptureState();
            var restored = new LatentExpertAwakeningSystem(skills);
            restored.RestoreState(saved);

            Assert.Equal(2, restored.GetProgress("surv_tariq", "trait_grease_monkey"));
            Assert.False(restored.IsAwakened("surv_tariq", "trait_grease_monkey"));

            // 3rd step triggers awakening on restored instance
            bool awakened = restored.RecordProgress("surv_tariq", "trait_grease_monkey", 1, 9, "Overhaul", actor);
            Assert.True(awakened);
            Assert.True(restored.IsAwakened("surv_tariq", "trait_grease_monkey"));
        }
    }
}
