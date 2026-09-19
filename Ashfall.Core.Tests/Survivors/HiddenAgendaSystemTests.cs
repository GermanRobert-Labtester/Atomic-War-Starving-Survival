// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class HiddenAgendaSystemTests
    {
        [Fact]
        public void AssignAgenda_SetsInitialValues_And_TriggersEvent()
        {
            var system = new HiddenAgendaSystem();
            SurvivorHiddenAgenda? emitted = null;
            system.OnAgendaAssigned += a => emitted = a;

            var agenda = system.AssignAgenda(
                survivorId: "surv_01",
                type: AgendaType.ResourceTheft,
                targetFaction: "faction_raiders",
                startDay: 5,
                notes: "Hoarding meds"
            );

            Assert.NotNull(agenda);
            Assert.Equal("surv_01", agenda.SurvivorId);
            Assert.Equal(AgendaType.ResourceTheft, agenda.Type);
            Assert.Equal("faction_raiders", agenda.TargetFactionId);
            Assert.Equal(0f, agenda.DiscoveryProgress);
            Assert.False(agenda.IsDiscovered);
            Assert.False(agenda.IsResolved);
            Assert.Equal(AgendaResolution.Pending, agenda.Resolution);
            Assert.Equal(emitted, agenda);
            Assert.Equal(1, system.ActiveAgendaCount);
        }

        [Fact]
        public void InvestigateAgenda_IncreasesProgress_CreatesClue_And_ExposesWhenOverThreshold()
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda("surv_02", AgendaType.Sabotage);

            AgendaClue? clueEvent = null;
            SurvivorHiddenAgenda? exposedEvent = null;
            system.OnClueDiscovered += c => clueEvent = c;
            system.OnAgendaExposed += a => exposedEvent = a;

            // First investigation with skill 25
            var clue1 = system.InvestigateAgenda("surv_02", investigatorSkill: 25f, currentDay: 2);
            Assert.NotNull(clue1);
            Assert.Equal(clue1, clueEvent);
            Assert.Equal(25f, agenda.DiscoveryProgress);
            Assert.False(agenda.IsDiscovered);
            Assert.Null(exposedEvent);

            // Second investigation with skill 40 -> total 65f >= 60f
            var clue2 = system.InvestigateAgenda("surv_02", investigatorSkill: 40f, currentDay: 3);
            Assert.NotNull(clue2);
            Assert.Equal(65f, agenda.DiscoveryProgress);
            Assert.True(agenda.IsDiscovered);
            Assert.Equal(agenda, exposedEvent);
            Assert.Equal(2, system.TotalCluesCount);
        }

        [Fact]
        public void ConfrontSurvivor_FailsIfProgressTooLow()
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda("surv_03", AgendaType.EscapePlan);
            system.InvestigateAgenda("surv_03", investigatorSkill: 20f, currentDay: 1); // 20% < 60%

            bool success = system.ConfrontSurvivor(agenda.AgendaId, AgendaResolution.Expelled, currentDay: 1);
            Assert.False(success);
            Assert.False(agenda.IsResolved);
            Assert.Equal(AgendaResolution.Pending, agenda.Resolution);
        }

        [Fact]
        public void ConfrontSurvivor_SucceedsIfEvidenceSufficient_And_ResolvesAgenda()
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda("surv_04", AgendaType.FactionLoyalty);
            system.InvestigateAgenda("surv_04", investigatorSkill: 40f, currentDay: 1);
            system.InvestigateAgenda("surv_04", investigatorSkill: 25f, currentDay: 2); // 65%

            SurvivorHiddenAgenda? resolvedAgenda = null;
            AgendaResolution? resolvedResolution = null;
            system.OnAgendaResolved += (a, r) =>
            {
                resolvedAgenda = a;
                resolvedResolution = r;
            };

            bool success = system.ConfrontSurvivor(agenda.AgendaId, AgendaResolution.Reconciled, currentDay: 3);
            Assert.True(success);
            Assert.True(agenda.IsConfronted);
            Assert.True(agenda.IsResolved);
            Assert.Equal(AgendaResolution.Reconciled, agenda.Resolution);
            Assert.Equal(agenda, resolvedAgenda);
            Assert.Equal(AgendaResolution.Reconciled, resolvedResolution);
            Assert.Equal(0, system.ActiveAgendaCount);
        }

        [Fact]
        public void TickDay_PassiveSlipUpIncreasesProgress_AfterTenDays()
        {
            var system = new HiddenAgendaSystem();
            var agenda = system.AssignAgenda("surv_05", AgendaType.SecretProtection, startDay: 1);

            // Day 5: not > 10 days since start
            system.TickDay(currentDay: 5);
            Assert.Equal(0f, agenda.DiscoveryProgress);

            // Day 12: > 10 days since start, increases by 1.0f
            system.TickDay(currentDay: 12);
            Assert.Equal(1f, agenda.DiscoveryProgress);
        }

        [Fact]
        public void CaptureState_And_RestoreState_PreservesAgendasAndClues()
        {
            var system1 = new HiddenAgendaSystem();
            var agenda = system1.AssignAgenda("surv_06", AgendaType.ResourceTheft, "faction_drifters", "surv_victim", 2, "Stole rations");
            system1.InvestigateAgenda("surv_06", 30f, 3);

            var state = system1.CaptureState();

            var system2 = new HiddenAgendaSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ActiveAgendaCount);
            Assert.Equal(1, system2.TotalCluesCount);

            var restoredAgenda = system2.GetAgendaForSurvivor("surv_06");
            Assert.NotNull(restoredAgenda);
            Assert.Equal("surv_06", restoredAgenda.SurvivorId);
            Assert.Equal(AgendaType.ResourceTheft, restoredAgenda.Type);
            Assert.Equal("faction_drifters", restoredAgenda.TargetFactionId);
            Assert.Equal(30f, restoredAgenda.DiscoveryProgress);
            Assert.Equal("Stole rations", restoredAgenda.Notes);
        }
    }
}
