// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class HobbySystemTests
    {
        [Fact]
        public void GetOrCreateProgress_InitializesWithNoviceMastery()
        {
            var system = new HobbySystem();
            var prog = system.GetOrCreateProgress("dweller_1", "hobby_woodcarving");

            Assert.NotNull(prog);
            Assert.Equal("dweller_1", prog.SurvivorId);
            Assert.Equal("hobby_woodcarving", prog.HobbyId);
            Assert.Equal(0f, prog.Proficiency);
            Assert.Equal(HobbyMastery.Novice, prog.Mastery);
        }

        [Fact]
        public void ConductSession_IncreasesProficiency_AndGrantsMorale()
        {
            var system = new HobbySystem();
            HobbySessionResult? session = null;
            system.OnSessionCompleted += s => session = s;

            var result = system.ConductSession("dweller_1", "hobby_woodcarving", currentDay: 2);

            Assert.NotNull(result);
            Assert.Equal(session, result);
            Assert.True(result.ProficiencyGained > 0f);
            Assert.True(result.MoraleGained > 0f);

            var prog = system.GetOrCreateProgress("dweller_1", "hobby_woodcarving");
            Assert.Equal(1, prog.SessionsCompleted);
            Assert.Equal(2, prog.LastSessionDay);
        }

        [Fact]
        public void ConductSession_PromotesMastery_WhenProficiencyCrossesThreshold()
        {
            var system = new HobbySystem();
            var prog = system.GetOrCreateProgress("dweller_2", "hobby_reading");
            prog.Proficiency = 24f;

            HobbyMastery? achieved = null;
            system.OnMasteryAchieved += (p, m) => achieved = m;

            system.ConductSession("dweller_2", "hobby_reading", currentDay: 3);

            Assert.Equal(HobbyMastery.Apprentice, prog.Mastery);
            Assert.Equal(HobbyMastery.Apprentice, achieved);
        }

        [Fact]
        public void GetSharedHobbyAffinityBonus_CalculatesBonusFromCommonInterests()
        {
            var system = new HobbySystem();

            // Set up common hobby
            var pA = system.GetOrCreateProgress("survivor_a", "hobby_chess");
            pA.Proficiency = 30f;
            var pB = system.GetOrCreateProgress("survivor_b", "hobby_chess");
            pB.Proficiency = 35f;

            float bonus = system.GetSharedHobbyAffinityBonus("survivor_a", "survivor_b");
            Assert.Equal(5.0f, bonus);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new HobbySystem();
            var p = system1.GetOrCreateProgress("dweller_3", "hobby_painting");
            p.Proficiency = 55f;
            p.Mastery = HobbyMastery.Journeyman;
            p.SessionsCompleted = 10;

            var state = system1.CaptureState();
            Assert.Single(state.ProgressRecords);

            var system2 = new HobbySystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ProgressCount);
            var restored = system2.GetOrCreateProgress("dweller_3", "hobby_painting");
            Assert.Equal(55f, restored.Proficiency);
            Assert.Equal(HobbyMastery.Journeyman, restored.Mastery);
            Assert.Equal(10, restored.SessionsCompleted);
        }
    }
}
