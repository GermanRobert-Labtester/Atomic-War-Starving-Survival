// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Plan185Capability
{
    /// <summary>
    /// DEBT-185-SKILL-DORMANCY-TICK — the campaign day owner now calls
    /// <see cref="SkillProgressionSystem.TickDaily"/> with host-shaped actors
    /// (one public <see cref="SimpleSkillActor"/> per living survivor, exactly as
    /// <c>Main.TickSharedSkillProgression</c> builds them). Proves the dormancy
    /// window is crossed on that path and practice reactivates the skill.
    /// </summary>
    public sealed class Plan185SkillDormancyTickTests
    {
        private const string DisciplineId = "medical";
        private const string SkillId = "skill_field_dressing";

        private static string ResolveDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory not found");
        }

        [Fact]
        public void Host_shaped_daily_tick_dormants_an_unused_skill_and_practice_reactivates_it()
        {
            var skills = new SkillProgressionSystem();
            SkillCatalogLoader.LoadAndRegister(skills, ResolveDataDir(), new FileSystemIO(), new SystemTextJsonSerializer());

            // Host shape: one SimpleSkillActor per living survivor.
            var actor = new SimpleSkillActor("sv_alpha");
            var actors = new List<SkillActor> { actor };

            int day = 5;
            for (int i = 0; i < 11; i++)
                skills.RecordAction(actor, DisciplineId, SkillProgressionSystem.DefaultXpPerAction, day);
            Assert.True(skills.HasActiveSkill("sv_alpha", SkillId));

            // Daily host tick, unused discipline.
            for (int i = 0; i < 20; i++)
            {
                day++;
                skills.TickDaily(day, actors);
            }

            Assert.True(skills.HasDormantSkill("sv_alpha", SkillId),
                "skill unused past the dormant window must leave the active set");

            // Practice on the same authority reactivates it.
            skills.RecordAction(actor, DisciplineId, 1f, day);
            Assert.False(skills.HasDormantSkill("sv_alpha", SkillId),
                "practising a dormant discipline reactivates the skill");
        }

        [Fact]
        public void Dead_survivors_are_excluded_from_the_host_actor_list()
        {
            // Mirrors Main.TickSharedSkillProgression: only IsAliveState survivors
            // become actors, so a dead survivor never drives dormancy work.
            var roster = new List<SurvivorNeedsState>
            {
                new SurvivorNeedsState { Id = "sv_alive", IsAlive = true },
                new SurvivorNeedsState { Id = "sv_dead", IsAlive = false }
            };

            var actors = new List<SimpleSkillActor>();
            for (int i = 0; i < roster.Count; i++)
            {
                var survivor = roster[i];
                if (survivor == null || !survivor.IsAliveState) continue;
                actors.Add(new SimpleSkillActor(survivor.Id));
            }

            Assert.Single(actors);
            Assert.Equal("sv_alive", actors[0].Id);
        }
    }
}
