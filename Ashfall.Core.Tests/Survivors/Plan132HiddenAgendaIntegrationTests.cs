// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan132HiddenAgendaIntegrationTests
    {
        [Fact]
        public void MultiDayInvestigation_AccumulatesClues_And_ExposesAgenda()
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda(
                survivorId: "surv_infiltrator_01",
                type: AgendaType.Sabotage,
                targetFaction: "faction_hostile",
                targetSurvivor: null,
                startDay: 1,
                notes: "Tampering with oxygen scrubbers"
            );

            Assert.NotNull(agenda);
            Assert.Single(system.GetAllAgendas());
            Assert.Empty(system.GetAllClues());

            // Day 1 investigation (progress gain 30)
            var clue1 = system.InvestigateAgenda("surv_infiltrator_01", investigatorSkill: 30f, currentDay: 1);
            Assert.NotNull(clue1);
            Assert.False(agenda.IsDiscovered);
            Assert.Equal(30f, agenda.DiscoveryProgress);

            var agendaClues = system.GetCluesForAgenda(agenda.AgendaId);
            Assert.Single(agendaClues);
            Assert.Equal(clue1.ClueId, agendaClues[0].ClueId);

            // Day 2 tick
            system.TickDay(2);
            Assert.Equal(30f, agenda.DiscoveryProgress);

            // Day 3 investigation pushes progress to 70% >= 60%
            var clue2 = system.InvestigateAgenda("surv_infiltrator_01", investigatorSkill: 40f, currentDay: 3);
            Assert.NotNull(clue2);
            Assert.True(agenda.IsDiscovered);
            Assert.Equal(70f, agenda.DiscoveryProgress);
            Assert.Equal(2, system.GetCluesForAgenda(agenda.AgendaId).Count);
            Assert.Equal(2, system.GetAllClues().Count);
        }

        [Theory]
        [InlineData(AgendaResolution.Reconciled)]
        [InlineData(AgendaResolution.Expelled)]
        [InlineData(AgendaResolution.Exploited)]
        [InlineData(AgendaResolution.Betrayed)]
        public void ConfrontationBranches_ResolveAgendaAndClearFromActive(AgendaResolution resolution)
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda("surv_agent_02", AgendaType.ResourceTheft, startDay: 2);

            // Investigate to expose (40 + 30 = 70%)
            system.InvestigateAgenda("surv_agent_02", 40f, 2);
            system.InvestigateAgenda("surv_agent_02", 30f, 3);
            Assert.True(agenda.IsDiscovered);

            bool success = system.ConfrontSurvivor(agenda.AgendaId, resolution, currentDay: 4);
            Assert.True(success);
            Assert.True(agenda.IsResolved);
            Assert.Equal(resolution, agenda.Resolution);
            Assert.Empty(system.GetActiveAgendas());
            Assert.Null(system.GetAgendaForSurvivor("surv_agent_02"));
        }

        [Fact]
        public void PassiveSlipUp_AccumulatesProgress_AfterTenDays()
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda("surv_sleeper_03", AgendaType.SecretProtection, startDay: 1);

            // Days 1 through 10 should not cause slip-up (10 - 1 = 9 <= 10)
            for (int day = 1; day <= 10; day++)
            {
                system.TickDay(day);
            }
            Assert.Equal(0f, agenda.DiscoveryProgress);

            // Day 11: 11 - 1 = 10 <= 10 (no slip-up yet)
            system.TickDay(11);
            Assert.Equal(0f, agenda.DiscoveryProgress);

            // Days 12, 13, 14: each > 10 days since start, +1.0f each
            system.TickDay(12);
            system.TickDay(13);
            system.TickDay(14);
            Assert.Equal(3f, agenda.DiscoveryProgress);
        }

        [Fact]
        public void StateSerialization_RoundTrips_ActiveAndResolvedAgendas()
        {
            var source = new HiddenAgendaSystem();
            var a1 = source.AssignAgenda("surv_alpha", AgendaType.FactionLoyalty, "faction_syndicate", null, 1, "Leaking patrol routes");
            var a2 = source.AssignAgenda("surv_beta", AgendaType.Sabotage, null, null, 1, "Water poisoned");

            source.InvestigateAgenda("surv_alpha", 40f, 2);
            source.InvestigateAgenda("surv_alpha", 30f, 2);
            source.ConfrontSurvivor(a1.AgendaId, AgendaResolution.Reconciled, 3);

            source.InvestigateAgenda("surv_beta", 25f, 4);

            var state = source.CaptureState();
            Assert.NotNull(state);

            var restored = new HiddenAgendaSystem();
            restored.RestoreState(state);

            Assert.Single(restored.GetActiveAgendas());
            Assert.Equal(2, restored.GetAllAgendas().Count);

            var restoredBeta = restored.GetAgendaForSurvivor("surv_beta");
            Assert.NotNull(restoredBeta);
            Assert.Equal("surv_beta", restoredBeta.SurvivorId);
            Assert.Equal(25f, restoredBeta.DiscoveryProgress);
            Assert.Equal(AgendaType.Sabotage, restoredBeta.Type);

            Assert.Null(restored.GetAgendaForSurvivor("surv_alpha"));
            Assert.Equal(3, restored.GetAllClues().Count);
        }
    }
}
